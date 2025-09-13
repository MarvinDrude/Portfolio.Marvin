using Portfolio.Marvin.Enums;
using Portfolio.Marvin.Models.Technologies;
using Portfolio.Marvin.Providers.Interfaces;

namespace Portfolio.Marvin.Providers;

public sealed class TechnologyProvider : ITechnologyProvider
{
   public Technology? GetTechnology(TechnologyKind kind)
   {
      return _all.GetValueOrDefault(kind);
   }

   public IEnumerable<Technology> GetAllTechnologies()
   {
      return _all.Values;
   }

   private static readonly Dictionary<TechnologyKind, Technology> _all = new Dictionary<TechnologyKind, Technology>()
   {
      [TechnologyKind.CSharp] = new (TechnologyKind.CSharp, "C#", new TechnologyLogo("logo_csharp", "#8a71e3", true)),
      [TechnologyKind.Net] = new (TechnologyKind.Net, ".NET", new TechnologyLogo("logo_net", "#512cd4", true)),
      [TechnologyKind.NodeJs] = new (TechnologyKind.NodeJs, "Node.js", new TechnologyLogo("logo_nodejs", "#6fa660", true)),
      [TechnologyKind.JavaScript] = new (TechnologyKind.JavaScript, "JavaScript", new TechnologyLogo("logo_js", "#f7df1e", false)),
      [TechnologyKind.Css] = new (TechnologyKind.Css, "CSS", new TechnologyLogo("logo_js", "#1572b6", true)),
      [TechnologyKind.Html5] = new (TechnologyKind.Html5, "HTML5", new TechnologyLogo("logo_html", "#e34f26", true)),
      [TechnologyKind.MongoDb] = new (TechnologyKind.MongoDb, "MongoDB", new TechnologyLogo("logo_mongodb", "#47a248", true)),
      [TechnologyKind.MySql] = new (TechnologyKind.MySql, "MySQL", new TechnologyLogo("logo_mysql", "#4479a1", true)),
      [TechnologyKind.PostgreSql] = new (TechnologyKind.PostgreSql, "PostgreSQL", new TechnologyLogo("logo_postgre", "#346692", true)),
      [TechnologyKind.Clickhouse] = new (TechnologyKind.Clickhouse, "Clickhouse", new TechnologyLogo("logo_clickhouse", "#ffcc01", false)),
      [TechnologyKind.PowerShell] = new (TechnologyKind.PowerShell, "PowerShell", new TechnologyLogo("logo_powershell", "#2571bd", true)),
      [TechnologyKind.SharePoint] = new (TechnologyKind.SharePoint, "SharePoint", new TechnologyLogo("logo_sharepoint", "#037e83", true)),
      [TechnologyKind.Teams] = new (TechnologyKind.Teams, "Teams", new TechnologyLogo("logo_teams", "#474fb8", true)),
      [TechnologyKind.SqlServer] = new (TechnologyKind.SqlServer, "SQL Server", new TechnologyLogo("logo_sql_server", "#cc2927", true)),
      [TechnologyKind.React] = new (TechnologyKind.React, "React", new TechnologyLogo("logo_react", "#61dafb", true))
   };
}