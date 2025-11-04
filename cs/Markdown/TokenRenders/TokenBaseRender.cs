using System.Text;
using Markdown.Interfaces;

namespace Markdown.TokenRenders;

public abstract class TokenBaseRender : ITokenRender
{
    public abstract void Render(Token token, StringBuilder result);
    
    protected void RenderChildren(Token token, StringBuilder result)
    {
        foreach (var child in token.Children)
        {
            var render = RenderFactory.GetRender(child.Type);
            render.Render(child, result);
        }
    }
}