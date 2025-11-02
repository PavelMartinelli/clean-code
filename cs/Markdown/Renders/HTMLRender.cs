using System.Text;
using Markdown.Interfaces;
using Markdown.TokenRenders;

namespace Markdown.Renders;


// Renderers/HtmlRenderer.cs
public class HtmlRenderer : IRender
{
    private readonly SyntaxTreeConstructor treeConstructor;
    
    public HtmlRenderer(SyntaxTreeConstructor treeConstructor)
    {
        this.treeConstructor = treeConstructor;
    }
    
    public string Render(IEnumerable<Token> tokens)
    {
        var structuredTokens = treeConstructor.Construct(tokens);
        var result = new StringBuilder();
        
        foreach (var token in structuredTokens)
        {
            var converter = RenderFactory.GetRender(token.Type);
            result.Append(converter.Render(token));
        }
        
        return result.ToString();
    }
}