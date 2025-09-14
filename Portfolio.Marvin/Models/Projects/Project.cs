using Portfolio.Marvin.Enums;

namespace Portfolio.Marvin.Models.Projects;

public sealed class Project
{
   public required string Name { get; set; }
   public string? ProjectUrl { get; set; }
   
   public List<string> Descriptions { get; set; } = [];
   
   public required DateTimeOffset StartedAt { get; set; }
   
   public List<TechnologyKind> Technologies { get; set; } = [];
   public List<string> ImageUrls { get; set; } = [];
}