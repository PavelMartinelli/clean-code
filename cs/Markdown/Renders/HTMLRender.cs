using System.Text;
using Markdown.Interfaces;
using Markdown.TokenRenders;

namespace Markdown.Renders;

public class HtmlRenderer : IRender
{
    public string Render(IEnumerable<Token> tokens)
    {
        throw new NotImplementedException();
    }
}