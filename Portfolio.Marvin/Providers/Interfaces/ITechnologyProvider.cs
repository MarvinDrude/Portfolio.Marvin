using Portfolio.Marvin.Enums;
using Portfolio.Marvin.Models.Technologies;

namespace Portfolio.Marvin.Providers.Interfaces;

public interface ITechnologyProvider
{
   public Technology? GetTechnology(TechnologyKind kind);
   
   public IEnumerable<Technology> GetAllTechnologies();
}