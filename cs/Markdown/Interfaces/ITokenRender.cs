global using Markdown.Tokens;
namespace Markdown.Interfaces;

public interface ITokenRender
{
    string Render(Token token);
}