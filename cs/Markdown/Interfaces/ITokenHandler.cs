using Markdown.Parsers;

namespace Markdown.Interfaces;

public interface ITokenHandler
{
    bool CanHandle(char currentChar);
    Token Handle(char currentChar);
}