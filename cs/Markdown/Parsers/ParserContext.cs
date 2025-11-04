using System.Text;
using Markdown.Interfaces;

namespace Markdown.Parsers;

public class ParserContext
{
    public Stack<Token> Stack { get; } = new();
    public StringBuilder Buffer { get; } = new();
    public List<Token> Tokens { get; } = [];
    public List<int> IntersectedIndexes { get; } = [];
    public string MarkdownText { get; init; } = "";
    public int CurrentIndex { get; set; }
    public required IParser Parser { get; init; }
}