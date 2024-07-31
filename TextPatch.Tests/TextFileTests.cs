using System.Text;

namespace TextPatch.Tests;

public class TextFileTests
{
    private static string GetTextLineByLine(TextFile tf)
    {
        StringBuilder result = new();
        for (var i = 0; i < tf.LinesCount; i++)
            result.Append(tf.GetLine(i));
        return result.ToString();
    }

    [Theory]
    [InlineData("text", 1)]
    [InlineData("text\n", 1)]
    [InlineData("text\n\n", 2)]
    [InlineData("text\n\nmy", 3)]
    [InlineData("text\r\nmy", 2)]
    [InlineData("text\rmy", 2)]
    [InlineData("text\n\rmy", 3)]
    public void DifferentLineEndings(string text, int numLines)
    {
        var tf = TextFile.Read(text);
        var result = GetTextLineByLine(tf);

        Assert.Equal(text, result.ToString());
        Assert.Equal(tf.LinesCount, numLines);
    }

    [Theory]
    [InlineData("text", 0, 0, "A", "Atext")]
    [InlineData("text\n2nd\n3rd", 1, 3, "A\nB\n", "text\n2ndA\nB\n\n3rd")]
    [InlineData("text\n2nd\n3rd", 2, 3, "A", "text\n2nd\n3rdA")]
    public void InsertText(string text, int line, int pos, string insert, string result)
    {
        var tf = TextFile.Read(text);
        tf.InsertAt(line, pos, insert);
        Assert.Equal(result, GetTextLineByLine(tf));
    }

    [Theory]
    [InlineData("text", "\napp", "text\napp")]
    [InlineData("text\nmy", "\napp", "text\nmy\napp")]
    public void AppendText(string text, string insert, string result)
    {
        var tf = TextFile.Read(text);
        tf.Append(insert);
        Assert.Equal(result, GetTextLineByLine(tf));
    }


    [Theory]
    [InlineData("text", 0, 0, 1, "ext")]
    [InlineData("text", 0, 3, 1, "tex")]
    [InlineData("text\nline\none\ntwo", 1, 0, 5, "text\none\ntwo")]
    public void DeleteText(string text, int line, int pos, int count, string result)
    {
        var tf = TextFile.Read(text);
        tf.DeleteAt(line, pos, count);
        Assert.Equal(result, GetTextLineByLine(tf));
    }
}