using FluentAssertions;
using Markdown;
using Markdown.Tags;

namespace MarkdownTests.InteractionTests;

[TestFixture]
public class TagInteractionTests
{
    private Md mdRenderer;

    [OneTimeSetUp]
    public void Setup()
    {
        var supportedTags = new List<ITag>
            { new HeaderTag(), new ItalicTag(), new StrongTag() };
        mdRenderer = new Md(supportedTags);
    }

    [Test]
    public void WhenStrongWithItalicInside_ConvertsCorrectly()
    {
        var text = "Внутри __двойного выделения _одинарное_ тоже__ работает";
        var expected = "Внутри <strong>двойного выделения <em>одинарное</em> тоже</strong> работает";

        var result = mdRenderer.Render(text);

        result.Should().Be(expected);
    }

    [Test]
    public void WhenItalicWithStrongInside_StrongNotConverted()
    {
        var text = "Но не наоборот — внутри _одинарного __двойное__ не_ работает";
        var expected = "Но не наоборот — внутри <em>одинарного __двойное__ не</em> работает";

        var result = mdRenderer.Render(text);

        result.Should().Be(expected);
    }

    [Test]
    public void WhenTagsIntersected_NotConverted()
    {
        var text = "В случае __пересечения _двойных__ и одинарных_ подчерков";
        var expected = "В случае __пересечения _двойных__ и одинарных_ подчерков";

        var result = mdRenderer.Render(text);

        result.Should().Be(expected);
    }

    [Test]
    public void WhenHeaderWithFormatting_ConvertsCorrectly()
    {
        var text = "# Заголовок __с _разными_ символами__";
        var expected = "<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>";

        var result = mdRenderer.Render(text);

        result.Should().Be(expected);
    }

    [Test]
    public void WhenEscapedItalicInStrong_ConvertsCorrectly()
    {
        var text = @"__Текст с \_экранированным\_ курсивом__";
        var expected = "<strong>Текст с _экранированным_ курсивом</strong>";

        var result = mdRenderer.Render(text);

        result.Should().Be(expected);
    }

    [Test]
    public void WhenUnpairedUnderscores_NotConverted()
    {
        var text = "__Непарные_ символы в рамках одного абзаца";
        var expected = "__Непарные_ символы в рамках одного абзаца";

        var result = mdRenderer.Render(text);

        result.Should().Be(expected);
    }
}