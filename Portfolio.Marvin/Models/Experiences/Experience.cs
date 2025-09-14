using Portfolio.Marvin.Enums;

namespace Portfolio.Marvin.Models.Experiences;

public sealed class Experience
{
   public required string JobTitle { get; set; }
   
   public required string CompanyName { get; set; }
   public required string CompanyImageUrl { get; set; }

   public List<string> Descriptions { get; set; } = [];
   
   public required DateTimeOffset StartedAt { get; set; }
   public DateTimeOffset EndedAt { get; set; } = DateTimeOffset.MaxValue;
   
   public List<TechnologyKind> Technologies { get; set; } = [];
   public List<string> ImageUrls { get; set; } = [];
}