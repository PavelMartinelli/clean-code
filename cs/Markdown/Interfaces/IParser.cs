namespace Markdown.Interfaces;

public interface IParser
{
    IEnumerable<Token> Parse(string markdown);
}