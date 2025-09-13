using Portfolio.Marvin.Enums;

namespace Portfolio.Marvin.Models.Technologies;

public record Technology(
   TechnologyKind Kind,
   string Name,
   TechnologyLogo Logo);