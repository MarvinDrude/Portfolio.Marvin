using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;

namespace Portfolio.Marvin.Markdown.Renderers;

public sealed class SpanListItemRenderer : HtmlObjectRenderer<ListItemBlock>
{
   protected override void Write(HtmlRenderer renderer, ListItemBlock obj)
   {
      renderer.Write("<li>");
      renderer.Write("<span>");
      
      renderer.WriteChildren(obj);

      renderer.Write("</span>");
      renderer.WriteLine("</li>");
   }
}