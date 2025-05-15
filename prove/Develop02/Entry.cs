using System;

public class Entry
{
    // Fields to store entry data
    public string Date { get; private set; }
    public string PromptText { get; private set; }
    public string EntryText { get; private set; }

    // Constructor to create a new entry
    public Entry(string date, string promptText, string entryText)
    {
        Date = date;
        PromptText = promptText;
        EntryText = entryText;
    }

    // How the entry should be displayed as a string
    public override string ToString()
    {
        return $"Date: {Date}{Environment.NewLine}Prompt: {PromptText}{Environment.NewLine}Entry: {EntryText}{Environment.NewLine}----------";
    }
}