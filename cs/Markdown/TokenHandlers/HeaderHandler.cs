using Markdown.Interfaces;
using Markdown.Parsers;

namespace Markdown.Handlers;

public class HeaderHandler : ITokenHandler
{
    public bool CanHandle(char currentChar, ParserContext context)
    {
        throw new NotImplementedException();
    }

    public Token Handle(char currentChar, ParserContext context)
    {
        throw new NotImplementedException();
    }
}