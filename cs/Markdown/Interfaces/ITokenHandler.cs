using Markdown.Parsers;

namespace Markdown.Interfaces;

public interface ITokenHandler
{
    bool CanHandle(char currentChar, char next, ParserContext context);
    public void Handle(ParserContext context);
}