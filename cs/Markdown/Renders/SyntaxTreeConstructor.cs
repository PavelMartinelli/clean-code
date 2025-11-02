namespace Markdown.Renders;

public class SyntaxTreeConstructor
{
    private readonly Dictionary<TokenType, TokenType[]> nestingRules = new()
    {
        [TokenType.Strong] = [TokenType.Italics, TokenType.Text],
        [TokenType.Italics] = [TokenType.Text],
        [TokenType.Header] = [TokenType.Strong, TokenType.Italics, TokenType.Text
        ]
    };

    public List<Token> Construct(IEnumerable<Token> flatTokens)
    {
        var structuredTokens = new List<Token>();
        var stack = new Stack<Token>();
        
        foreach (var token in flatTokens)
        {
            if (IsContainerToken(token))
            {
                ProcessContainerToken(token, stack, structuredTokens);
            }
            else
            {
                ProcessLeafToken(token, stack, structuredTokens);
            }
        }
        
        ProcessRemainingStack(stack, structuredTokens);
        return structuredTokens;
    }
    
    private void ProcessContainerToken(Token token, Stack<Token> stack, List<Token> result)
    {
        if (HasIntersectingTags(stack, token))
        {
            HandleTagIntersection(stack, token, result);
            return;
        }
        
        if (stack.Count > 0 && !CanNest(stack.Peek(), token))
        {
            var parent = stack.Pop();
            result.Add(parent);
        }
        
        stack.Push(token);
    }
    
    private void ProcessLeafToken(Token token, Stack<Token> stack, List<Token> result)
    {
        if (stack.Count > 0)
        {
            stack.Peek().Children.Add(token);
        }
        else
        {
            result.Add(token);
        }
    }
    
    private void ProcessRemainingStack(Stack<Token> stack, List<Token> result)
    {
        while (stack.Count > 0)
        {
            var token = stack.Pop();
            ConvertToText(token, result);
        }
    }
    
    private bool IsContainerToken(Token token)
    {
        return token.Type is TokenType.Strong or TokenType.Italics or TokenType.Header;
    }
    
    private bool CanNest(Token parent, Token child)
    {
        return nestingRules.TryGetValue(parent.Type, out var allowedChildren) && 
               allowedChildren.Contains(child.Type);
    }
    
    private bool HasIntersectingTags(Stack<Token> stack, Token newToken)
    {
        // TODO: Реализовать проверку пересечения тегов
        return false;
    }
    
    private void HandleTagIntersection(Stack<Token> stack, Token newToken, List<Token> result)
    {
        // TODO: Реализовать обработку пересекающихся тегов
        ConvertToText(newToken, result);
    }
    
    private void ConvertToText(Token token, List<Token> result)
    {
        var textContent = GetOriginalMarkdown(token);
        result.Add(new Token(TokenType.Text, textContent));
    }
    
    private string GetOriginalMarkdown(Token token)
    {
        return token.Type switch
        {
            TokenType.Italics => "_" + token.Content + "_",
            TokenType.Strong => "__" + token.Content + "__",
            TokenType.Header => "# " + token.Content,
            _ => token.Content
        };
    }
}
