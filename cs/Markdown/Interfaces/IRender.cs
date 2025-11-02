namespace Markdown.Interfaces;

public interface IRender
{
    string Render(IEnumerable<Token> tokens);
}