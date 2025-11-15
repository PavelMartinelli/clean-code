namespace Markdown.Tags;

public class HeaderTag : ITag
{
    public string MdTag => "# ";
    public string HtmlOpenTag => "<h1>";
    public string HtmlCloseTag => "</h1>";

    public bool CanBeOpened(char left, char right)
    {
        return left is '\0' or '\n';
    }
    
    public bool CanBeClosed(char left, char right)
    {
        return true;
    }
}