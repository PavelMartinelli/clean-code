using System.Text;

namespace Markdown.Parsers;

public class ParserContext
{
    public string SourceText { get; set; }
    public int Position { get; set; }
    public StringBuilder CurrentText { get; set; }
    public Stack<Token> TokenStack { get; set; }
    public List<Token> ResultTokens { get; set; }
    public bool IsEscaped { get; set; }

    public ParserContext()
    {
        CurrentText = new StringBuilder();
        TokenStack = new Stack<Token>();
        ResultTokens = new List<Token>();
    }

    public void FlushCurrentText()
    {
        if (CurrentText.Length <= 0)
            return;
        ResultTokens.Add(new Token(TokenType.Text, CurrentText.ToString()));
        CurrentText.Clear();
    }
}