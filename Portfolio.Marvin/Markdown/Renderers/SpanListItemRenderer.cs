using Markdig.Helpers;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;

namespace Portfolio.Marvin.Markdown.Renderers;

public sealed class SpanListItemRenderer : HtmlObjectRenderer<ListBlock>
{
   protected override void Write(HtmlRenderer renderer, ListBlock listBlock)
   {
      renderer.EnsureLine();
      if (renderer.EnableHtmlForBlock)
      {
         if (listBlock.IsOrdered)
         {
            renderer.Write("<ol");
            if (listBlock.BulletType != '1')
            {
               renderer.Write(new StringSlice(" type=\""));
               renderer.Write(new StringSlice(listBlock.BulletType.ToString(), NewLine.None));
               renderer.Write(new StringSlice("\"", NewLine.None));
            }

            if (listBlock.OrderedStart is not null && listBlock.OrderedStart != "1")
            {
               renderer.Write(" start=\"");
               renderer.Write(new StringSlice(listBlock.OrderedStart, NewLine.None));
               renderer.Write(new StringSlice("\"", NewLine.None));
            }
         }
         else
         {
            renderer.Write("<ul");
         }

         renderer.WriteAttributes(listBlock);
         renderer.WriteLine('>');
      }

      foreach (var item in listBlock)
      {
         var listItem = (ListItemBlock)item;
         var previousImplicit = renderer.ImplicitParagraph;
         renderer.ImplicitParagraph = !listBlock.IsLoose;

         renderer.EnsureLine();
         if (renderer.EnableHtmlForBlock)
         {
            renderer.Write("<li");
            renderer.WriteAttributes(listItem);
            renderer.Write(new StringSlice("><span>", NewLine.None));
         }

         renderer.WriteChildren(listItem);

         if (renderer.EnableHtmlForBlock)
         {
            renderer.WriteLine("</span></li>");
         }

         renderer.EnsureLine();
         renderer.ImplicitParagraph = previousImplicit;
      }

      if (renderer.EnableHtmlForBlock)
      {
         renderer.WriteLine(listBlock.IsOrdered ? "</ol>" : "</ul>");
      }

      renderer.EnsureLine();
   }
}