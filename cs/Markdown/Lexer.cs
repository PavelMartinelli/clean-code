using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown;

internal class Lexer
{
    private readonly TokenFactory tokenFactory;
    private readonly string sourceText;

    public Lexer(string text, IEnumerable<ITag> tags)
    {
        this.sourceText = text;
        this.tokenFactory = new TokenFactory(text, tags);
    }

    public LinkedList<IToken> Tokenize()
    {
        var tokens = new LinkedList<IToken>();
        var lastTokenIndex = 0;
        
        for (var i = 0; i < sourceText.Length;)
        {
            if (tokenFactory.TryCreateToken(i, out var token))
            {
                if (i - lastTokenIndex > 0)
                    tokens.AddLast(new TextToken(sourceText[lastTokenIndex..i]));

                tokens.AddLast(token);
                lastTokenIndex = i + token.Length;
                i = lastTokenIndex;
            }
            else
                i++;
        }
        
        if (sourceText.Length - lastTokenIndex > 0)
            tokens.AddLast(new TextToken(sourceText[lastTokenIndex..]));
        
        tokens.AddLast(new EOFToken());
        return tokens;
    }
}