using Markdown.Interfaces;

namespace Markdown.Parsers;

public class MarkdownParser : IParser
{
    private readonly List<ITokenHandler> handlers;

    public MarkdownParser(IEnumerable<ITokenHandler> handlers)
    {
        this.handlers = handlers.ToList();
    }

    public IEnumerable<Token> Parse(string markdown)
    {
        throw new NotImplementedException();
    }
}