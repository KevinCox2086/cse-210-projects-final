// ScriptureInfo.cs

public class ScriptureInfo
{
    public string Book { get; }
    public int Chapter { get; }
    public int StartVerse { get; }
    public int? EndVerse { get; }
    public string Text { get; }

    // Constructor for single verse
    public ScriptureInfo(string book, int chapter, int verse, string text)
    {
        Book = book;
        Chapter = chapter;
        StartVerse = verse;
        EndVerse = null;
        Text = text;
    }

    // Constructor for verse range
    public ScriptureInfo(string book, int chapter, int startVerse, int endVerse, string text)
    {
        Book = book;
        Chapter = chapter;
        StartVerse = startVerse;
        EndVerse = endVerse;
        Text = text;
    }
}