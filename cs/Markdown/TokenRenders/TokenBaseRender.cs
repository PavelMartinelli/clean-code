using System.Text;
using Markdown.Interfaces;

namespace Markdown.TokenRenders;

public abstract class TokenBaseRender: ITokenRender
{
    public string Render(Token token)
    {
        throw new NotImplementedException();
    }
    protected string RenderChildren(Token token)
    {
        var result = new StringBuilder();
        foreach (var child in token.Children)
        {
            var render = RenderFactory.GetRender(child.Type);
            result.Append(render.Render(child));
        }
        return result.ToString();
    }

    
}