namespace Portfolio.Marvin.Models.Blogs;

public sealed class BlogPage
{
   public required BlogPageMeta Meta { get; set; }
   
   public required string RawContent { get; set; }
   
   public required string Content { get; set; }
}