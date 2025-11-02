using Markdown.Interfaces;
using Markdown.Parsers;

namespace Markdown.Handlers;

public class EscapeHandler : ITokenHandler
{
    public bool CanHandle(char currentChar, ParserContext context)
    {
        return currentChar == '\\' && !context.IsEscaped;
    }
    
    public Token Handle(char currentChar, ParserContext context)
    {
        context.Position++;

        if (context.Position >= context.SourceText.Length)
            return new Token(TokenType.Text, "\\");
        
        var escapedChar = context.SourceText[context.Position];
            
        var escapedText = new Token(TokenType.Text, escapedChar.ToString());
        context.Position++;
        return escapedText;
    }
}
