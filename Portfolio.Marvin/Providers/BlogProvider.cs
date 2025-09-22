using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using Markdig;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;
using Me.Memory.Serialization;
using Portfolio.Marvin.Markdown.Renderers;
using Portfolio.Marvin.Models.Blogs;
using Portfolio.Marvin.Providers.Interfaces;

namespace Portfolio.Marvin.Providers;

public sealed class BlogProvider : IBlogProvider
{
   private readonly IWebHostEnvironment _environment;

   private readonly ConcurrentDictionary<string, BlogPage> _pages = [];
   private readonly ConcurrentDictionary<string, bool> _tags = [];
   
   private string RootPath => Path.Combine(_environment.WebRootPath, "blogs-pages");
   
   public BlogProvider(IWebHostEnvironment environment)
   {
      _environment = environment;
   }

   public ValueTask<BlogPage?> GetPage(string relativeUrl, CancellationToken ct = default)
   {
      return ValueTask.FromResult(_pages.GetValueOrDefault(relativeUrl));
   }

   public ValueTask<List<BlogPage>> GetPages(GetPagesFilter filter, CancellationToken ct = default)
   {
      IEnumerable<BlogPage> query = _pages.Values;

      if (filter.Tag is not null)
      {
         query = query.Where(p => p.Meta.Tags.Contains(filter.Tag, StringComparer.InvariantCultureIgnoreCase));
      }

      // in memory pagination is not that bad as in db's
      return ValueTask.FromResult(
         query.OrderByDescending(p => p.Meta.Date)
            .Skip(filter.PageIndex * filter.PageSize)
            .Take(filter.PageSize)
            .ToList());
   }

   public ValueTask<List<string>> GetTags(CancellationToken ct = default)
   {
      return ValueTask.FromResult(_tags.Keys.ToList());
   }
   
   public async Task Reload(CancellationToken ct = default)
   {
      Clear();
      var allDirectories = Directory.GetDirectories(RootPath, "*", SearchOption.AllDirectories);

      foreach (var directory in allDirectories)
      {
         if (IsRelevantFolder(directory) is not 
             ({ Length: > 0 } metaPath, { Length: > 0 } contentPath))
         {
            continue;
         }

         var rawMetaJson = await File.ReadAllTextAsync(metaPath, Encoding.UTF8, ct);
         var meta = JsonSerializer.Deserialize<BlogPageMeta>(rawMetaJson, _jsonOptions);

         if (meta is null)
         {
            continue;
         }
         
         var rawContent = await File.ReadAllTextAsync(contentPath, Encoding.UTF8, ct);
         AddPage(meta, rawContent);
      }
   }

   private void AddPage(BlogPageMeta meta, string rawContent)
   {
      foreach (var tag in meta.Tags)
      {
         _tags[tag] = true;
      }

      using var writer = new StringWriter();
      var renderer = new HtmlRenderer(writer);
      
      _markdownPipeline.Setup(renderer);

      // TODO: fix list rendering bug
      
      var doc = Markdig.Markdown.Parse(rawContent, _markdownPipeline);
      renderer.Render(doc);
      writer.Flush();

      var markdown = writer.ToString();

      _pages[meta.RelativeUrl] = new BlogPage()
      {
         Meta = meta,
         Content = markdown,
         RawContent = rawContent
      };
   }

   private void Clear()
   {
      _pages.Clear();
      _tags.Clear();
   }
   
   private (string MetaPath, string ContentPath) IsRelevantFolder(string absoluteUrl)
   {
      var (metaPath, contentPath) = (GetMetaPath(absoluteUrl), GetContentPath(absoluteUrl));

      if (!FileExists(metaPath) || !FileExists(contentPath))
      {
         return (string.Empty, string.Empty);
      }
      
      return (metaPath, contentPath);
   }
   
   private bool FileExists(string absoluteUrl) => File.Exists(absoluteUrl);
   
   private string GetMetaPath(string absoluteUrl) => Path.Combine(absoluteUrl, "meta.json");
   private string GetContentPath(string absoluteUrl) => Path.Combine(absoluteUrl, "content.md");

   private static readonly MarkdownPipeline _markdownPipeline = new MarkdownPipelineBuilder()
      .UseAdvancedExtensions()
      .Build();
   
   private static readonly JsonSerializerOptions _jsonOptions = new ()
   {
      AllowTrailingCommas = true
   };
}