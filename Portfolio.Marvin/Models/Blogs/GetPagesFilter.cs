namespace Portfolio.Marvin.Models.Blogs;

public sealed class GetPagesFilter
{
   public string? Tag { get; set; }

   public int PageIndex { get; set; } = 0;
   
   public int PageSize { get; set; } = 10;
}