// ScriptureLibrary.cs
// This is part of the shwoing creativity and exceeding requirements
// where I implemented a Scripture Library that can return random scriptures.
using System;
using System.Collections.Generic;

public class ScriptureLibrary
{
    private List<ScriptureInfo> _scriptureEntries;
    private Random _randomGenerator;

    public ScriptureLibrary()
    {
        _scriptureEntries = new List<ScriptureInfo>();
        _randomGenerator = new Random();
        LoadScriptures();
    }

    // Loads scriptures
    private void LoadScriptures()
    {
        _scriptureEntries.Add(new ScriptureInfo("John", 3, 16, "For God so loved the world that he gave his only begotten Son, that whosoever believeth in him should not perish, but have eternal life."));
        _scriptureEntries.Add(new ScriptureInfo("Proverbs", 3, 5, 6, "Trust in the Lord with all thine heart; and lean not unto thine own understanding. In all thy ways acknowledge him, and he shall direct thy paths."));
        _scriptureEntries.Add(new ScriptureInfo("Alma", 32, 21, "And now as I said concerning faith—faith is not to have a perfect knowledge of things; therefore if ye have faith ye hope for things which are not seen, which are true."));
        _scriptureEntries.Add(new ScriptureInfo("Isaiah", 1, 18, "Come now, and let us reason together, saith the Lord: though your sins be as scarlet, they shall be as white as snow; though they be red like crimson, they shall be as wool."));
        _scriptureEntries.Add(new ScriptureInfo("Moroni", 10, 4, 5, "And when ye shall receive these things, I would exhort you that ye would ask God, the Eternal Father, in the name of Christ, if these things are not true; and if ye shall ask with a sincere heart, with real intent, having faith in Christ, he will manifest the truth of it unto you, by the power of the Holy Ghost. And by the power of the Holy Ghost ye may know the truth of all things."));
        _scriptureEntries.Add(new ScriptureInfo("Philippians", 4, 13, "I can do all things through Christ which strengtheneth me."));
        _scriptureEntries.Add(new ScriptureInfo("Joshua", 1, 9, "Have not I commanded thee? Be strong and of a good courage; be not afraid, neither be thou dismayed: for the Lord thy God is with thee whithersoever thou goest."));
        _scriptureEntries.Add(new ScriptureInfo("Psalms", 23, 1, 3, "The Lord is my shepherd; I shall not want. He maketh me to lie down in green pastures: he leadeth me beside the still waters. He restoreth my soul: he leadeth me in the paths of righteousness for his name's sake."));
        _scriptureEntries.Add(new ScriptureInfo("Matthew", 11, 28, 30, "Come unto me, all ye that labour and are heavy laden, and I will give you rest. Take my yoke upon you, and learn of me; for I am meek and lowly in heart: and ye shall find rest unto your souls. For my yoke is easy, and my burden is light."));
        _scriptureEntries.Add(new ScriptureInfo("Romans", 8, 38, 39, "For I am persuaded, that neither death, nor life, nor angels, nor principalities, nor powers, nor things present, nor things to come, Nor height, nor depth, nor any other creature, shall be able to separate us from the love of God, which is in Christ Jesus our Lord."));
        _scriptureEntries.Add(new ScriptureInfo("1 Nephi", 3, 7, "And it came to pass that I, Nephi, said unto my father: I will go and do the things which the Lord hath commanded, for I know that the Lord giveth no commandments unto the children of men, save he shall prepare a way for them that they may accomplish the thing which he commandeth them."));
        _scriptureEntries.Add(new ScriptureInfo("Ether", 12, 27, "And if men come unto me I will show unto them their weakness. I give unto men weakness that they may be humble; and my grace is sufficient for all men that humble themselves before me; for if they humble themselves before me, and have faith in me, then will I make weak things become strong unto them."));
        // Add more scriptures here
   }

    public Scripture GetRandomScripture()
    {
        if (_scriptureEntries.Count == 0)
        {
            Console.WriteLine("Warning: Scripture library is empty. Using a default scripture.");
            Reference defaultRef = new Reference("Error", 0, 0);
            return new Scripture(defaultRef, "No scriptures available in the library.");
        }

        int randomIndex = _randomGenerator.Next(_scriptureEntries.Count);
        ScriptureInfo selectedEntry = _scriptureEntries[randomIndex];

        Reference reference;
        if (selectedEntry.EndVerse.HasValue)
        {
            reference = new Reference(selectedEntry.Book, selectedEntry.Chapter, selectedEntry.StartVerse, selectedEntry.EndVerse.Value);
        }
        else
        {
            reference = new Reference(selectedEntry.Book, selectedEntry.Chapter, selectedEntry.StartVerse);
        }

        return new Scripture(reference, selectedEntry.Text);
    }
}