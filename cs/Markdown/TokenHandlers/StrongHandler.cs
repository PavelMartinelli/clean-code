using Markdown.Interfaces;
using Markdown.Parsers;
using Markdown.TokenHandlers;

namespace Markdown.Handlers;

public class StrongHandler : BoundaryTokenHandler
{
    protected override string Delimiter => "__";
    protected override TokenType TokenType => TokenType.Strong;

    public override bool CanHandle(char current, char next,
        ParserContext context)
        => current == '_' && next == '_';

    protected override bool HasValidNesting(ParserContext context)
    {
        if (context.Stack.Count <= 0) return true;
        var topToken = context.Stack.Peek();
        return topToken.Type != TokenType.Italics;
    }
}