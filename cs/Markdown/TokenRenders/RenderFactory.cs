using Markdown.Interfaces;

namespace Markdown.TokenRenders;

public static class RenderFactory
{
    private static readonly Dictionary<TokenType, ITokenRender> Renders = new Dictionary<TokenType, ITokenRender>();
    
    static RenderFactory()
    {
        Renders[TokenType.Text] = new TextRender();
        Renders[TokenType.Italics] = new ItalicRender();
        Renders[TokenType.Strong] = new StrongRender();
        Renders[TokenType.Header] = new HeaderRender();
        Renders[TokenType.NewLine] = new NewLineRender();
    }
    
    public static ITokenRender GetRender(TokenType type)
    {
        return Renders.TryGetValue(type, out var converter) ? converter : Renders[TokenType.Text];
    }
}




