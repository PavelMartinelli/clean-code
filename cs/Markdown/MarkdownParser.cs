using Markdown.TokenHandlers;
using Markdown.Tokens;

namespace Markdown;

internal class MarkdownParser
{
    private readonly IEnumerable<ITokenHandler> handlers;
    private readonly NestingHandler nestingHandler;

    public MarkdownParser(IEnumerable<ITokenHandler> handlers, NestingHandler nestingHandler)
    {
        this.handlers = handlers;
        this.nestingHandler = nestingHandler;
    }

    public IEnumerable<IToken> Parse(IEnumerable<IToken> tokens)
    {
        var tokenList = new LinkedList<IToken>(tokens);
        var stack = new Stack<TagToken>();
        
        for (var node = tokenList.First; node != null; node = node.Next)
        {
            foreach (var handler in handlers)
                handler.Handle(node, stack);
        }
        
        nestingHandler.HandleNesting(tokenList);

        return tokenList;
    }
}