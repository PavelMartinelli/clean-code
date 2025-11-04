using Markdown.Interfaces;
using Markdown.Parsers;

namespace Markdown.Handlers;

public class EscapeHandler : ITokenHandler
{
    public bool CanHandle(char current, char next, ParserContext context)
        => current == '\\';

    public void Handle(ParserContext context)
    {
        if (context.CurrentIndex + 1 < context.MarkdownText.Length)
        {
            var next = context.MarkdownText[context.CurrentIndex + 1];
            if (next is '_' or '#' or '\\')
            {
                context.Buffer.Append(next);
                context.CurrentIndex += 2;
                return;
            }
        }

        context.Buffer.Append('\\');
        context.CurrentIndex++;
    }
}