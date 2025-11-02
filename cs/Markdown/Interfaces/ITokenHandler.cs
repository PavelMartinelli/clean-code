using Markdown.Parsers;

namespace Markdown.Interfaces;

public interface ITokenHandler
{
    bool CanHandle(char currentChar, ParserContext context);
    Token Handle(char currentChar, ParserContext context);
}