using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown.TokenHandlers;

internal class NewlineTokenHandler : ITokenHandler
{
    public void Handle(LinkedListNode<IToken> node, Stack<TagToken> stack)
    {
        if (node.Value is NewlineToken or EOFToken)
        {
            HandleNewlineToken(node, stack);
        }
    }

    private void HandleNewlineToken(LinkedListNode<IToken> node, Stack<TagToken> stack)
    {
        while (stack.Count > 0)
        {
            var openedTag = stack.Pop();
            
            if (openedTag.Tag is HeaderTag headerTag && headerTag.CanBeOpened(openedTag.Left, openedTag.Right))
            {
                openedTag.Status = TagStatus.Opened;
                node.List.AddBefore(node, new TagToken(openedTag.Tag, openedTag.Left, openedTag.Right) 
                    { Status = TagStatus.Closed });
            }
            else
                openedTag.Status = TagStatus.Broken;
        }
    }
}