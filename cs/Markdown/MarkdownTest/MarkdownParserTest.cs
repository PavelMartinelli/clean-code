using FluentAssertions;
using Markdown.Handlers;
using Markdown.Interfaces;
using Markdown.Parsers;
using NUnit.Framework;
using System.Diagnostics;

namespace Markdown.MarkdownTest;

[TestFixture]
public class MarkdownParserTests
{
    private MarkdownParser parser;

    [SetUp]
    public void Setup()
    {
        parser = new MarkdownParser(new List<ITokenHandler>
        {
            new EscapeHandler(),
            new HeaderHandler(),
            new StrongHandler(),
            new ItalicHandler(),
            new NewLineHandler(),
        });
    }

    [Test]
    public void Parse_WhenItalicTag_ShouldReturnCorrectTokens()
    {
        var input = "Это _курсив_ текст";
        var expected = new List<Token>
        {
            new(TokenType.Text, "Это "),
            new(TokenType.Italics,
                children: new List<Token> { new(TokenType.Text, "курсив") }),
            new(TokenType.Text, " текст")
        };
        CompareTokens(expected, parser.Parse(input).ToList());
    }

    [Test]
    public void Parse_WhenStrongTag_ShouldReturnCorrectTokens()
    {
        var input = "Это __полужирный__ текст";
        var expected = new List<Token>
        {
            new(TokenType.Text, "Это "),
            new(TokenType.Strong,
                children: new List<Token>
                    { new(TokenType.Text, "полужирный") }),
            new(TokenType.Text, " текст")
        };
        CompareTokens(expected, parser.Parse(input).ToList());
    }

    [Test]
    public void Parse_WhenHeaderTag_ShouldReturnCorrectTokens()
    {
        var input = "# Заголовок";
        var expected = new List<Token>
        {
            new(TokenType.Header,
                children: new List<Token> { new(TokenType.Text, "Заголовок") })
        };
        CompareTokens(expected, parser.Parse(input).ToList());
    }

    [Test]
    public void Parse_WhenNestedItalicAndStrongTags_ShouldReturnCorrectTokens()
    {
        var input = "Это __жирный _и курсивный_ текст__";
        var expected = new List<Token>
        {
            new(TokenType.Text, "Это "),
            new(TokenType.Strong, children: new List<Token>
            {
                new(TokenType.Text, "жирный "),
                new(TokenType.Italics,
                    children: new List<Token>
                        { new(TokenType.Text, "и курсивный") }),
                new(TokenType.Text, " текст")
            })
        };
        CompareTokens(expected, parser.Parse(input).ToList());
    }

    [Test]
    public void Parse_WhenMultipleTokensInLine_ShouldReturnCorrectTokens()
    {
        var input = "Это _курсив_,а это __жирный__ текст.";
        var expected = new List<Token>
        {
            new(TokenType.Text, "Это "),
            new(TokenType.Italics,
                children: new List<Token> { new(TokenType.Text, "курсив") }),
            new(TokenType.Text, ",а это "),
            new(TokenType.Strong,
                children: new List<Token> { new(TokenType.Text, "жирный") }),
            new(TokenType.Text, " текст.")
        };
        CompareTokens(expected, parser.Parse(input).ToList());
    }

    [Test]
    public void Parse_WhenBoundedTagsInOneWord_ShouldReturnCorrectTokens()
    {
        var input = "en_d._ ,mi__dd__le";
        var expected = new List<Token>
        {
            new(TokenType.Text, "en"),
            new(TokenType.Italics,
                children: new List<Token> { new(TokenType.Text, "d.") }),
            new(TokenType.Text, " ,mi"),
            new(TokenType.Strong,
                children: new List<Token> { new(TokenType.Text, "dd") }),
            new(TokenType.Text, "le")
        };
        CompareTokens(expected, parser.Parse(input).ToList());
    }

    [Test]
    public void Parse_WhenEscapedTags_ShouldReturnPlainText()
    {
        var input = @"Экранированный \_символ\_";
        var expected = new List<Token>
            { new(TokenType.Text, "Экранированный _символ_") };
        CompareTokens(expected, parser.Parse(input).ToList());
    }

    [Test]
    public void Parse_WhenItalicInStrong_ShouldReturnCorrectTokens()
    {
        var input = "Это __двойное _и одинарное_ выделение__";
        var expected = new List<Token>
        {
            new(TokenType.Text, "Это "),
            new(TokenType.Strong, children: new List<Token>
            {
                new(TokenType.Text, "двойное "),
                new(TokenType.Italics,
                    children: new List<Token>
                        { new(TokenType.Text, "и одинарное") }),
                new(TokenType.Text, " выделение")
            })
        };
        CompareTokens(expected, parser.Parse(input).ToList());
    }

    [Test]
    public void Parse_WhenHeaderWithTags_ShouldReturnCorrectTokens()
    {
        var input = "# Заголовок __с _разными_ символами__";
        var expected = new List<Token>
        {
            new(TokenType.Header, children: new List<Token>
            {
                new(TokenType.Text, "Заголовок "),
                new(TokenType.Strong, children: new List<Token>
                {
                    new(TokenType.Text, "с "),
                    new(TokenType.Italics,
                        children: new List<Token>
                            { new(TokenType.Text, "разными") }),
                    new(TokenType.Text, " символами")
                })
            })
        };
        CompareTokens(expected, parser.Parse(input).ToList());
    }

    [Test]
    public void Parse_WhenHeaderWithoutSpace_ShouldNotBeHeader()
    {
        var input = "#Заголовок без пробела";
        var expected = new List<Token>
            { new(TokenType.Text, "#Заголовок без пробела") };
        CompareTokens(expected, parser.Parse(input).ToList());
    }


    [Test]
    public void Parse_WhenMultipleHeaders_ShouldReturnCorrectTokens()
    {
        var input = "# Заголовок 1\n# Заголовок 2";
        var expected = new List<Token>
        {
            new(TokenType.Header,
                children: new List<Token>
                    { new(TokenType.Text, "Заголовок 1") }),
            new(TokenType.Text, "\n"),
            new(TokenType.Header,
                children: new List<Token>
                    { new(TokenType.Text, "Заголовок 2") })
        };
        CompareTokens(expected, parser.Parse(input).ToList());
    }

    [Test]
    public void Parse_WhenEmptyItalic_ShouldNotReturnTags()
    {
        var input = "Если пустая _______ строка";
        var expected = new List<Token>
            { new(TokenType.Text, "Если пустая _______ строка") };
        CompareTokens(expected, parser.Parse(input).ToList());
    }

    [Test]
    public void Parse_WhenUnderscoresInNumbers_ShouldNotReturnTags()
    {
        var input = "Текст с цифрами_12_3 не должен выделяться";
        var expected = new List<Token>
        {
            new(TokenType.Text, "Текст с цифрами_12_3 не должен выделяться")
        };
        CompareTokens(expected, parser.Parse(input).ToList());
    }

    [Test]
    public void Parse_WhenEscapingSymbols_ShouldNotReturnTags()
    {
        var input = @"Здесь сим\волы экранирования\ \должны остаться.\";
        var expected = new List<Token>
        {
            new(TokenType.Text,
                @"Здесь сим\волы экранирования\ \должны остаться.\")
        };
        CompareTokens(expected, parser.Parse(input).ToList());
    }

    [Test]
    public void Parse_WhenEscapedEscapeCharacter_ShouldReturnCorrectTokens()
    {
        var input = @"\\_вот это будет выделено тегом_";
        var expected = new List<Token>
        {
            new(TokenType.Text, @"\"),
            new(TokenType.Italics,
                children: new List<Token>
                    { new(TokenType.Text, "вот это будет выделено тегом") })
        };
        CompareTokens(expected, parser.Parse(input).ToList());
    }

    [Test]
    public void Parse_WhenTagInDifferentWords_ShouldNotReturnTags()
    {
        var input = "Это пер_вый в_торой пример.";
        var expected = new List<Token>
            { new(TokenType.Text, "Это пер_вый в_торой пример.") };
        CompareTokens(expected, parser.Parse(input).ToList());
    }

    [Test]
    public void Parse_WhenUnclosedTags_ShouldNotReturnTags()
    {
        var input = "_e __e";
        var expected = new List<Token> { new(TokenType.Text, "_e __e") };
        CompareTokens(expected, parser.Parse(input).ToList());
    }

    [Test]
    public void Parse_WhenTagsIntersection_ShouldNotReturnTags()
    {
        var input = "__пересечение _двойных__ и одинарных_";
        var expected = new List<Token>
            { new(TokenType.Text, "__пересечение _двойных__ и одинарных_") };
        CompareTokens(expected, parser.Parse(input).ToList());
    }

    [Test]
    public void Parse_WhenTagsIntersectionWithNewLines_ShouldNotReturnTags()
    {
        var input = "__s \n s__,_e \r\n e_";
        var expected = new List<Token>
            { new(TokenType.Text, "__s \n s__,_e \r\n e_") };
        CompareTokens(expected, parser.Parse(input).ToList());
    }

    [Test]
    public void Parse_WhenUnpairedUnderscores_ShouldNotReturnTags()
    {
        var input = "__Непарные_ символы не считаются выделением";
        var expected = new List<Token>
        {
            new(TokenType.Text, "__Непарные_ символы не считаются выделением")
        };
        CompareTokens(expected, parser.Parse(input).ToList());
    }


    [Test]
    public void Parse_WhenItalicInsideStrong_ShouldReturnCorrectTokens()
    {
        var input = "Внутри __двойного выделения _одинарное_ тоже__ работает.";
        var expected = new List<Token>
        {
            new(TokenType.Text, "Внутри "),
            new(TokenType.Strong, children: new List<Token>
            {
                new(TokenType.Text, "двойного выделения "),
                new(TokenType.Italics,
                    children: new List<Token>
                        { new(TokenType.Text, "одинарное") }),
                new(TokenType.Text, " тоже")
            }),
            new(TokenType.Text, " работает.")
        };
        CompareTokens(expected, parser.Parse(input).ToList());
    }

    [Test]
    public void Parse_WhenStrongInsideItalic_ShouldNotReturnNestedStrong()
    {
        var input =
            "Но не наоборот — внутри _одинарного __двойное__ не_ работает.";
        var expected = new List<Token>
        {
            new(TokenType.Text, "Но не наоборот — внутри "),
            new(TokenType.Italics,
                children: new List<Token>
                    { new(TokenType.Text, "одинарного __двойное__ не") }),
            new(TokenType.Text, " работает.")
        };
        CompareTokens(expected, parser.Parse(input).ToList());
    }

    [Test]
    public void Parse_WhenSpaceAfterOpeningUnderscore_ShouldNotReturnTags()
    {
        var input =
            "За подчерками, начинающими выделение, должен следовать непробельный символ. Иначе эти_ подчерки_ не считаются выделением";
        var expected = new List<Token> { new(TokenType.Text, input) };
        CompareTokens(expected, parser.Parse(input).ToList());
    }

    [Test]
    public void Parse_WhenSpaceBeforeClosingUnderscore_ShouldNotReturnTags()
    {
        var input =
            "Подчерки, заканчивающие выделение, должны следовать за непробельным символом. Иначе эти _подчерки _не считаются_ окончанием выделения";
        var expected = new List<Token>
        {
            new(TokenType.Text,
                "Подчерки, заканчивающие выделение, должны следовать за непробельным символом. Иначе эти _подчерки "),
            new(TokenType.Italics,
                children: new List<Token>
                    { new(TokenType.Text, "не считаются") }),
            new(TokenType.Text, " окончанием выделения")
        };
        CompareTokens(expected, parser.Parse(input).ToList());
    }

    [Test]
    public void Parse_WhenEmptyUnderscores_ShouldNotReturnTags()
    {
        var input = "____";
        var expected = new List<Token> { new(TokenType.Text, "____") };
        CompareTokens(expected, parser.Parse(input).ToList());
    }

    [Test]
    public void Parse_WhenPlainText_ShouldReturnSingleTextToken()
    {
        var input = "Обычный текст без разметки";
        var expected = new List<Token>
            { new(TokenType.Text, "Обычный текст без разметки") };
        CompareTokens(expected, parser.Parse(input).ToList());
    }

    [Test]
    public void Parse_WhenNewLine_ShouldReturnTextTokenWithNewLine()
    {
        var input = "Текст с\nпереносом строки";
        var expected = new List<Token>
            { new(TokenType.Text, "Текст с\nпереносом строки") };
        CompareTokens(expected, parser.Parse(input).ToList());
    }

    [Test]
    public void
        Parse_WhenEscapedEscapeCharacterInText_ShouldReturnCorrectTokens()
    {
        var input = @"Текст с\\\\экранированием";
        var expected = new List<Token>
            { new(TokenType.Text, @"Текст с\\экранированием") };
        CompareTokens(expected, parser.Parse(input).ToList());
    }

    [Test]
    public void Parse_WhenItalicAtWordBeginning_ShouldReturnCorrectTokens()
    {
        var input = "_нач_ало";
        var expected = new List<Token>
        {
            new(TokenType.Italics,
                children: new List<Token> { new(TokenType.Text, "нач") }),
            new(TokenType.Text, "ало")
        };
        CompareTokens(expected, parser.Parse(input).ToList());
    }


    [Test]
    public void Parse_Performance_ShouldBeLinear()
    {
        var largeText = string.Join("",
            Enumerable.Repeat(" _курсивом_ и __жирным__ и # Заголовок", 1000));

        var stopwatch = Stopwatch.StartNew();
        var tokens = parser.Parse(largeText).ToList();
        stopwatch.Stop();

        tokens.Should().NotBeEmpty();
        stopwatch.ElapsedMilliseconds.Should().BeLessThan(1000,
            "Парсинг должен выполняться за линейное время");
    }

    private static void CompareTokens(IReadOnlyList<Token> expected,
        IReadOnlyList<Token> actual)
    {
        actual.Should().HaveCount(expected.Count);

        for (var i = 0; i < expected.Count; i++)
        {
            var expectedToken = expected[i];
            var actualToken = actual[i];

            actualToken.Type.Should().Be(expectedToken.Type);
            actualToken.Content.Should().Be(expectedToken.Content);

            if (expectedToken.Children.Any())
            {
                actualToken.Children.Should().NotBeNull();
                CompareTokens(expectedToken.Children, actualToken.Children);
            }
            else
                actualToken.Children.Should().BeEmpty();
        }
    }
}