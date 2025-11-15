using Markdown.Tags;
using Markdown.TokenHandlers;
using Markdown.Tokens;

namespace Markdown;

public class Md
{
    private readonly IReadOnlyList<ITag> supportedTags;
    private readonly MarkdownParser parser;

    public Md(List<ITag> supportedTags)
    {
        this.supportedTags = supportedTags;
        parser = new MarkdownParser(
        [
            new EscapeTokenHandler(), 
            new NewlineTokenHandler(),
            new TagTokenHandler()
        ],  new NestingHandler());
    }

    public string Render(string sourceText)
    {
        var lexer = new Lexer(sourceText, supportedTags);
        var tokens = lexer.Tokenize();
        
        var parsedTokens = parser.Parse(tokens);

        return parsedTokens.ConvertToHtml();
    }
}