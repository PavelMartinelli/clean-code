using Markdown.Handlers;
using Markdown.Interfaces;
using Markdown.Parsers;
using Markdown.Renders;
using Markdown.TokenHandlers;

namespace Markdown;

public class Md
{
    private static readonly IParser parser;
    private static readonly IRender renderer;

    static Md()
    {
        var handlers = new List<ITokenHandler>
        {
            new EscapeHandler(),
            new HeaderHandler(),
            new StrongHandler(),
            new ItalicHandler(),
            new NewLineHandler(),
        };

        parser = new MarkdownParser(handlers);
        renderer = new HtmlRenderer();
    }

    public string Render(string markdown)
    {
        if (string.IsNullOrEmpty(markdown))
            return string.Empty;

        var tokens = parser.Parse(markdown);
        return renderer.Render(tokens);
    }
}