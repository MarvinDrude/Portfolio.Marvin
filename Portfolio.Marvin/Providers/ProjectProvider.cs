using Portfolio.Marvin.Enums;
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
         Name = "Repo: Me.Memory (WIP)",
         StartedAt = new DateTimeOffset(2025, 8, 9, 0, 0, 0, TimeSpan.Zero),
         ProjectUrl = "https://github.com/MarvinDrude/Me.Memory",
         Descriptions = [
            "Developed an internal C# library for advanced memory- and buffer-management, including span-based and pooled data structures.",
            "Structured code to optimize reuse and reduce GC / heap pressure, using buffer pooling and span etc.",
            "Maintained the library as a shared internal dependency across multiple projects to ensure consistent performance and reduce duplicated effort."
         ],
         Technologies = [
            TechnologyKind.Net, TechnologyKind.CSharp
         ],
      },
      new ()
      {
         Name = "Repo: CodeAnalytics.Engine (very WIP)",
         StartedAt = new DateTimeOffset(2025, 7, 19, 0, 0, 0, TimeSpan.Zero),
         ProjectUrl = "https://github.com/MarvinDrude/CodeAnalytics.Engine",
         Descriptions = [
            "Created CodeAnalytics.Engine, a tool that turns C# solutions into actionable, rich insights by parsing .sln / .csproj structures, source files, dependencies, and metrics.",
            "Developed a lightweight console collector that scans codebases and emits normalized metadata + metrics.",
            "Designed and implemented a Blazor-based Web Viewer enabling interactive exploration: source browser, file/folder navigation, symbol occurrence search, definitions lookup etc.",
            "Integrated features such as syntax highlighting, file/directory hierarchy visualisation, and navigable symbol definitions across the code base.",
            "Structured solution into modular components (Collector, Common, Contracts, Web Viewer) to separate concerns and maintain extensibility."
         ],
         Technologies = [
            TechnologyKind.Net, TechnologyKind.CSharp,
            TechnologyKind.Blazor, TechnologyKind.PostgreSql
         ],
      },
      new ()
      {
         Name = "Repo: CodeGen.Core (WIP)",
         StartedAt = new DateTimeOffset(2025, 6, 2, 0, 0, 0, TimeSpan.Zero),
         ProjectUrl = "https://github.com/MarvinDrude/CodeGen.Core",
         Descriptions = [
            "Designed and implemented a lightweight C# source generation library (CodeGen.Core), with utility structs and classes aimed for future .NET 10 support.",
            "Engineered a zero / low allocation text writer (“CodeTextWriter”) enabling efficient code generation with minimal heap overhead.",
            "Built an abstracted builder layer (“CodeBuilder”) atop the writer to support both stateful and immediate code building patterns, maintaining performance while offering flexible APIs.",
            "Supported advanced features like namespace & using declarations, nullable context, auto-generated code comments, method and class declarations with base classes/interfaces, parameters, indentation control etc., all with performance in mind.",
            "Developed unit testing (CodeGen.Core.Tests) and performance benchmarking tools (CodeGen.Core.Perfs) to ensure correctness and efficiency.",
            "Maintained code clarity and extensibility by separating concerns (common structs, core logic, performance, tests) across modules."
         ],
         Technologies = [
            TechnologyKind.Net, TechnologyKind.CSharp
         ],
      },
      new ()
      {
         Name = "SiteSights io",
         StartedAt = new DateTimeOffset(2023, 3, 2, 0, 0, 0, TimeSpan.Zero),
         ProjectUrl = "https://sitesights.io",
         ImageUrls = [
            "projects/sitesights/1.webp",
            "projects/sitesights/2.webp",
         ],
         Descriptions = [
            "Developed a privacy-friendly alternative to Google Analytics with a backend fully built on ASP.NET and .NET 9.",
            "Implemented ClickHouse as the database to enable highly efficient analytical queries.",
            "Designed and built custom interactive charts (e.g., line graphs) using JavaScript, CSS, and Canvas.",
            "Integrated the Paddle API to power the in-house subscription and billing system.",
            "Engineered a scalable C# backend leveraging modern ASP.NET practices.",
            "Established observability features including distributed tracing and structured logging.",
            "Enabled real-time communication between frontend and backend via HTTP and WebSockets."
         ],
         Technologies = [
            TechnologyKind.Net, TechnologyKind.CSharp, TechnologyKind.Clickhouse,
            TechnologyKind.SqlServer, TechnologyKind.PostgreSql, TechnologyKind.Html5,
            TechnologyKind.Css, TechnologyKind.JavaScript
         ],
      },
      new ()
      {
         Name = "Rewrote io",
         StartedAt = new DateTimeOffset(2023, 3, 1, 0, 0, 0, TimeSpan.Zero),
         ProjectUrl = "https://rewrote.io",
         ImageUrls = [
            "projects/rewrote/1.webp",
            "projects/rewrote/2.webp",
         ],
         Descriptions = [
            "Built both the frontend and backend of a full-stack web application from the ground up.",
            "Developed an AI-powered text editing platform with features such as template generation, sentence rewriting, and inline commenting.",
            "Overcame the complex challenge of integrating advanced AI functionalities seamlessly into the editing workflow.",
            "Delivered a modern, user-friendly web interface optimized for productivity and intuitive user experience.",
            "Engineered a robust backend architecture to support real-time editing and scalable AI processing."
         ],
         Technologies = [
            TechnologyKind.Net, TechnologyKind.CSharp, TechnologyKind.MongoDb, 
            TechnologyKind.Html5, TechnologyKind.Css, TechnologyKind.JavaScript
         ],
      },
      new ()
      {
         Name = "Gidd io",
         StartedAt = new DateTimeOffset(2021, 2, 1, 0, 0, 0, TimeSpan.Zero),
         ProjectUrl = "https://gidd.io",
         ImageUrls = [
            "projects/giddio/1.webp",
            "projects/giddio/2.webp",
         ],
         Descriptions = [
            "Built browser-based multiplayer mini-games with friends, running on a backend fully developed in ASP.NET and .NET 9.",
            "Implemented frontend–backend communication via HTTP and WebSockets for real-time gameplay.",
            "Engineered a scalable C# backend with dedicated WebSocket servers designed for horizontal scaling.",
            "Developed 3D games in the frontend using three.js.",
            "Designed and implemented game logic in the backend, including efficient processing and anti-cheat mechanisms.",
            "Created user interfaces and implemented client-side logic with JavaScript.",
            "Conducted performance and stress testing to ensure stability under high user loads.",
            "Analyzed and applied test-driven optimizations to improve reliability and performance."
         ],
         Technologies = [
            TechnologyKind.Net, TechnologyKind.CSharp, TechnologyKind.MongoDb, 
            TechnologyKind.Html5, TechnologyKind.Css, TechnologyKind.JavaScript
         ],
      },
      new ()
      {
         Name = "SpeedAutoClicker",
         StartedAt = new DateTimeOffset(2020, 5, 1, 0, 0, 0, TimeSpan.Zero),
         ProjectUrl = "https://speedautoclicker.net",
         ImageUrls = [
            "projects/speedautoclicker/1.webp",
            "projects/speedautoclicker/2.webp",
         ],
         Descriptions = [
            "Developed a desktop automation tool with a modern WPF-based user interface for Windows.",
            "Implemented Windows API integrations to capture and trigger low-level events such as mouse clicks.",
            "Built and maintained both the frontend and backend of the accompanying web application.",
            "Designed the website generation pipeline using a headless CMS powered by C#.",
            "Delivered a widely used tool capable of automating mouse interactions on Windows PCs with high performance and reliability."
         ],
         Technologies = [
            TechnologyKind.Net, TechnologyKind.CSharp,
            TechnologyKind.Html5, TechnologyKind.Css, TechnologyKind.JavaScript
         ],
      },
   ];
}