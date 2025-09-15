using Portfolio.Marvin.Models.Projects;
using Portfolio.Marvin.Providers.Interfaces;

namespace Portfolio.Marvin.Providers;

public sealed class ProjectProvider : IProjectProvider
{
   public IEnumerable<Project> GetAllProjects()
   {
      return _projects.OrderByDescending(x => x.StartedAt);
   }

   private readonly List<Project> _projects = [
      new ()
      {
         Name = "SiteSights.io",
         StartedAt = new DateTimeOffset(2023, 3, 1, 0, 0, 0, TimeSpan.Zero),
         ProjectUrl = "",
         ImageUrls = [
            ""
         ],
         Descriptions = [
            ""
         ]
      },
   ];
}