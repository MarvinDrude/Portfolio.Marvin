using Portfolio.Marvin.Enums;

namespace Portfolio.Marvin.Models.Blogs;

public sealed class BlogPageMeta
{
   public required string Title { get; set; }
   
   public required string RelativeUrl { get; set; }
   
   public required string Description { get; set; }

   public List<string> Tags { get; set; } = [];

   public List<TechnologyKind> Technologies { get; set; } = [];
   
   public DateTimeOffset Date { get; set; }
   
   public required string Image { get; set; }
}