using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown;

internal class TokenFactory
{
    private readonly List<(string mdTag, Func<int, string, IToken> factory)> tokenTemplates;
    private readonly string sourceText;

    public TokenFactory(string sourceText, IEnumerable<ITag> tags)
    {
        this.sourceText = sourceText;
        tokenTemplates = new List<(string, Func<int, string, IToken>)>();
        
        tokenTemplates.Add(("\\", (pos, text) => new EscapeToken()));
        tokenTemplates.Add(("\n", (pos, text) => new NewlineToken()));
        
        var sortedTags = tags.OrderByDescending(tag => tag.MdTag.Length).ThenBy(tag => tag.MdTag);
        foreach (var tag in sortedTags)
        {
            tokenTemplates.Add((tag.MdTag, (pos, text) => 
            {
                var leftChar = text.ElementAtOrDefault(pos - 1);
                var rightChar = text.ElementAtOrDefault(pos + tag.MdTag.Length);
                return new TagToken(tag, leftChar, rightChar);
            }));
        }
    }

    public bool TryCreateToken(int position, out IToken token)
    {
        token = null;
        
        foreach (var (mdTag, factory) in tokenTemplates)
        {
            if (position + mdTag.Length > sourceText.Length ||
                sourceText.Substring(position, mdTag.Length) != mdTag) 
                continue;
            
            token = factory(position, sourceText);
            return true;
        }

        return false;
    }
}