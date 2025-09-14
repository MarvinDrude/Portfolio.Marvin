using Portfolio.Marvin.Enums;
using Portfolio.Marvin.Models.Experiences;
using Portfolio.Marvin.Providers.Interfaces;

namespace Portfolio.Marvin.Providers;

public sealed class ExperienceProvider : IExperienceProvider
{
   public IEnumerable<Experience> GetAllExperiences()
   {
      return _experiences.OrderByDescending(x => x.EndedAt);
   }

   private readonly List<Experience> _experiences = [
      new ()
      {
         CompanyName = "CONNEXT Communication GmbH",
         CompanyImageUrl = "images/companies/connext-fill.webp",
         JobTitle = "Senior Full Stack Developer",
         StartedAt = new DateTimeOffset(2025, 3, 1, 0, 0, 0, TimeSpan.Zero),
         EndedAt = DateTimeOffset.MaxValue,
         ImageUrls = [],
         Technologies = [
            TechnologyKind.Net, TechnologyKind.CSharp, TechnologyKind.Blazor,
            TechnologyKind.SqlServer, TechnologyKind.PostgreSql, TechnologyKind.Html5,
            TechnologyKind.Css, TechnologyKind.JavaScript
         ],
         Descriptions = [
            "Design and implement new features in the Vivendi BI product branch, enabling faster and more accurate data-driven decision-making for customers.",
            "Translate complex business requirements into scalable, maintainable C# code, reducing long-term technical debt and improving delivery speed.",
            "Improve system performance and reliability in BI workflows, optimizing data handling and boosting user efficiency.",
            "Collaborate with cross-functional teams to align BI product development with business goals, ensuring maximum customer value.",
            "Enhance the maintainability of the codebase, accelerating onboarding of new developers and reducing bug-fix turnaround times.",
            "Build custom C# source generators to automate repetitive patterns and enforce consistency across the codebase, reducing manual effort and improving developer productivity."
         ]
      },
      new ()
      {
         CompanyName = "Drude, Grossert GbR",
         CompanyImageUrl = "brand.webp",
         JobTitle = "Senior Full Stack Developer",
         StartedAt = new DateTimeOffset(2022, 8, 1, 0, 0, 0, TimeSpan.Zero),
         EndedAt = DateTimeOffset.MaxValue,
         ImageUrls = [],
         Technologies = [
            TechnologyKind.Net, TechnologyKind.CSharp, TechnologyKind.Blazor,
            TechnologyKind.MongoDb,  TechnologyKind.Clickhouse, TechnologyKind.PostgreSql, 
            TechnologyKind.Html5, TechnologyKind.Css, TechnologyKind.JavaScript,
            TechnologyKind.NodeJs
         ],
         Descriptions = [
            "Co-found and manage a small software company focused on building innovative, self-driven projects with real-world usage.",
            "Design and develop full-stack applications (frontend & backend), ensuring scalability, maintainability, and user-focused design.",
            "Deliver tailored software solutions for external clients, aligning technical implementations with their business needs.",
            "Champion end-to-end product development, from concept and architecture to deployment and maintenance.",
            "Build products that we actively use ourselves, ensuring high quality, reliability, and continuous improvement based on real-world feedback."
         ],
      },
      new ()
      {
         CompanyName = "Fellowmind Germany GmbH",
         CompanyImageUrl = "images/companies/fellowmind.webp",
         JobTitle = "Senior Full Stack Developer",
         StartedAt = new DateTimeOffset(2024, 4, 1, 0, 0, 0, TimeSpan.Zero),
         EndedAt = new DateTimeOffset(2025, 2, 1, 0, 0, 0, TimeSpan.Zero),
         ImageUrls = [],
         Technologies = [
            TechnologyKind.Net, TechnologyKind.CSharp, TechnologyKind.NodeJs,
            TechnologyKind.React, TechnologyKind.SqlServer, TechnologyKind.Html5,
            TechnologyKind.Css, TechnologyKind.JavaScript, TechnologyKind.SharePoint,
            TechnologyKind.Teams, TechnologyKind.TypeScript
         ],
         Descriptions = [
            "Designed, developed, and enhanced software products, ensuring scalability and long-term maintainability.",
            "Planned and implemented cloud-native architectures within Microsoft Azure, improving reliability and cost efficiency.",
            "Built and delivered modern web applications using ASP.NET and the latest .NET 8/9 features.",
            "Designed and developed reusable frontend components in React and native JavaScript, accelerating UI delivery.",
            "Conducted comprehensive testing to ensure software quality, stability, and compliance with business requirements.",
            "Migrated legacy codebases into modern environments, reducing technical debt and extending product lifecycle."
         ]
      },
      new ()
      {
         CompanyName = "Fellowmind Germany GmbH",
         CompanyImageUrl = "images/companies/fellowmind.webp",
         JobTitle = "Full Stack Developer",
         StartedAt = new DateTimeOffset(2017, 8, 1, 0, 0, 0, TimeSpan.Zero),
         EndedAt = new DateTimeOffset(2022, 8, 1, 0, 0, 0, TimeSpan.Zero),
         ImageUrls = [],
         Technologies = [
            TechnologyKind.Net, TechnologyKind.CSharp, TechnologyKind.NodeJs,
            TechnologyKind.React, TechnologyKind.Html5, TechnologyKind.PowerShell,
            TechnologyKind.Css, TechnologyKind.JavaScript, TechnologyKind.SharePoint,
            TechnologyKind.Teams
         ],
         Descriptions = [
            "Designed and implemented cloud architectures using Microsoft Azure to support scalable and secure solutions.",
            "Built and developed frontend components in React and native JavaScript, improving usability and performance.",
            "Developed and optimized software solutions to meet evolving customer and business needs.",
            "Performed extensive testing to ensure product quality, stability, and reliability.",
            "Modernized legacy codebases, migrating them into current environments to reduce technical debt.",
            "Designed and implemented web applications with ASP.NET and .NET Framework / .NET 5/6."
         ]
      },
   ];
}