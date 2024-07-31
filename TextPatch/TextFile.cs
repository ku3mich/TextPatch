using System.Text;

namespace TextPatch;

public class TextFile
{
    protected string _Text = "";
    protected List<(int offs, int length)> LinesIndex = new();

    public string Text
    {
        get => _Text;
        set
        {
            _Text = value;
            BuildLinesIndex();
        }
    }

    public int LinesCount => LinesIndex.Count;

    protected void CheckRangeLine(int n)
    {
        if (n >= LinesIndex.Count)
            throw new TextFileRangeException();
    }

    protected (int offs, int length) CheckRangeLinePos(int line, int pos)
    {
        CheckRangeLine(line);

        var addr = LinesIndex[line];
        if (pos > addr.length)
            throw new TextFileRangeException();

        return addr;
    }


    public string GetLine(int n)
    {
        CheckRangeLine(n);

        var idx = LinesIndex[n];
        return _Text.Substring(idx.offs, idx.length);
    }

    public void InsertAt(int line, int pos, string text)
    {
        var addr = CheckRangeLinePos(line, pos);
        var sb = new StringBuilder();
        if (addr.offs + pos > 0)
            sb.Append(_Text.Substring(0, addr.offs + pos));

        sb.Append(text);
        sb.Append(_Text.Substring(addr.offs + pos));
        Text = sb.ToString();
    }

    public void Append(string text)
    {
        if (LinesIndex.Count == 0)
        {
            Text = text;
            return;
        }

        var addr = LinesIndex[LinesIndex.Count - 1];
        InsertAt(LinesIndex.Count - 1, addr.length, text);
    }

    public void DeleteAt(int line, int pos, int count)
    {
        var addr = CheckRangeLinePos(line, pos);

        Text = string.Concat(
            _Text.Substring(0, addr.offs + pos),
            _Text.Substring(addr.offs + pos + count));
    }

    protected void BuildLinesIndex()
    {
        if (LinesIndex.Count > 0)
            LinesIndex.Clear();

        using var rdr = new StringReader(_Text);
        int offs = 0, l;
        while (true)
        {
            l = ScanForLine(rdr);
            if (l == 0)
                break;

            LinesIndex.Add((offs, l));
            offs += l;
        }
    }

    protected static int ScanForLine(TextReader rdr)
    {
        int l = 0;

        while (true)
        {
            int ch = rdr.Read();
            if (ch == -1)
                break;

            if (ch == '\r' || ch == '\n')
            {
                l++;
                if (ch == '\r' && rdr.Peek() == '\n')
                {
                    rdr.Read();
                    l++;
                }

                return l;
            }
            l++;
        }

        return l;
    }

    public static TextFile Read(string s)
    {
        using var rdr = new StringReader(s);
        return Read(rdr);
    }

    public static TextFile Read(Stream s)
    {
        using var rdr = new StreamReader(s);
        return Read(rdr);
    }

    public static TextFile Read(TextReader rdr)
    {
        var tf = new TextFile();
        tf.Text = rdr.ReadToEnd();
        return tf;
    }
}
