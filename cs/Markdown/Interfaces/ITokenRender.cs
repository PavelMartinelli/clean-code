global using Markdown.Tokens;
using System.Text;

namespace Markdown.Interfaces;

public interface ITokenRender
{
    void Render(Token token, StringBuilder result);
}