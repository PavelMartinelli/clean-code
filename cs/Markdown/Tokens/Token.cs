namespace Markdown.Tokens;

public class Token
{
    public TokenType Type { get; init; }
    public string Content { get; set; }
    public List<Token> Children { get; init; }
    
    public Token(TokenType type, string content = "", List<Token>? children = null)
    {
        Type = type;
        Content = content;
        Children = children ?? [];
    }
}