// Scripture.cs
using System;
using System.Collections.Generic;
using System.Linq;

public class Scripture
{
    private Reference _reference;
    private List<Word> _words;
    private Random _randomGenerator;

    // Constructor
    public Scripture(Reference reference, string text)
    {
        _reference = reference;
        _words = new List<Word>();
        _randomGenerator = new Random();

        // Split text into words
        string[] wordArray = text.Split(new char[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string wordText in wordArray)
        {
            _words.Add(new Word(wordText));
        }
    }

    // Hides random words
    public void HideRandomWords(int numberToHide)
    {
        List<int> visibleWordIndices = new List<int>();
        for (int i = 0; i < _words.Count; i++)
        {
            if (!_words[i].IsHidden())
            {
                visibleWordIndices.Add(i);
            }
        }

        int wordsToActuallyHide = Math.Min(numberToHide, visibleWordIndices.Count);

        for (int i = 0; i < wordsToActuallyHide; i++)
        {
            if (visibleWordIndices.Count == 0)
                break;

            int randomIndexInList = _randomGenerator.Next(visibleWordIndices.Count);
            int wordIndexToHide = visibleWordIndices[randomIndexInList];

            _words[wordIndexToHide].Hide();
            visibleWordIndices.RemoveAt(randomIndexInList);
        }
    }

    // Gets display text
    public string GetDisplayText()
    {
        string displayText = _reference.GetFormattedReference() + " ";
        foreach (Word word in _words)
        {
            displayText += word.GetDisplayText() + " ";
        }
        return displayText.TrimEnd();
    }

    // Checks if all words are hidden
    public bool IsCompletelyHidden()
    {
        foreach (Word word in _words)
        {
            if (!word.IsHidden())
            {
                return false;
            }
        }
        return true;
    }
}