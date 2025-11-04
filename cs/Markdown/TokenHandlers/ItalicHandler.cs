using Markdown.Interfaces;
using Markdown.Parsers;
using Markdown.TokenHandlers;

namespace Markdown.Handlers;

public class ItalicHandler : BoundaryTokenHandler
{
    protected override string Delimiter => "_";
    protected override TokenType TokenType => TokenType.Italics;

    public override bool CanHandle(char current, char next,
        ParserContext context)
        => current == '_' && next != '_';

    protected override bool HasValidNesting(ParserContext context) => true;
}