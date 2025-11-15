namespace Markdown.Tags;

public class MarkedListTag: ITag
{
    public  string MdTag => "* ";
    public string HtmlOpenTag => "<li>";
    public string HtmlCloseTag => "</li>";
    public bool CanBeOpened(char left, char right)
    {
        return left is '\0' or '\n';
    }

    public bool CanBeClosed(char left, char right)
    {
        return true;
    }
}