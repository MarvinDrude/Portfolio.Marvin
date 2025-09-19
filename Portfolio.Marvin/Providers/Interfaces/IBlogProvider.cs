using Portfolio.Marvin.Models.Blogs;

namespace Portfolio.Marvin.Providers.Interfaces;

public interface IBlogProvider
{
   public ValueTask<BlogPage?> GetPage(string relativeUrl, CancellationToken ct = default);

   public ValueTask<List<BlogPage>> GetPages(GetPagesFilter filter, CancellationToken ct = default);
   
   public Task Reload(CancellationToken ct = default);
}