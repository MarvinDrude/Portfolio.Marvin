using Portfolio.Marvin.Models.Projects;

namespace Portfolio.Marvin.Providers.Interfaces;

public interface IProjectProvider
{
   public IEnumerable<Project> GetAllProjects();
}