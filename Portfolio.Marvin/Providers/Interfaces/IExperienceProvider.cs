using Portfolio.Marvin.Models.Experiences;

namespace Portfolio.Marvin.Providers.Interfaces;

public interface IExperienceProvider
{
   public IEnumerable<Experience> GetAllExperiences();
}