using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class Journal
{
    private List<Entry> _entries; // List to hold all journal entries

    public Journal()
    {
        _entries = new List<Entry>();
    }

    // Adds an entry to the journal
    public void AddEntry(Entry newEntry)
    {
        if (newEntry != null)
        {
            _entries.Add(newEntry);
        }
        else
        {
            Console.WriteLine("Attempted to add a null entry.");
        }
    }

    // Shows all entries in the journal
    public void DisplayAll()
    {
        if (_entries == null || _entries.Count == 0)
        {
            Console.WriteLine("\nJournal is empty.");
            return;
        }
        Console.WriteLine("\n--- Journal Entries ---");
        foreach (Entry entry in _entries)
        {
            Console.WriteLine(entry);
        }
        Console.WriteLine("--- End of Journal ---\n");
    }

    // Saves the journal to a JSON file
    public void SaveToFile(string filename)
    {
        Console.WriteLine($"Saving journal to {filename} as JSON...");
        try
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(_entries, options);
            File.WriteAllText(filename, jsonString);
            Console.WriteLine("Journal saved successfully as JSON.");
        }
        catch (Exception ex) // Basic error handling
        {
            Console.WriteLine($"Error saving journal: {ex.Message}");
        }
    }

    // Loads the journal from a JSON file
    public void LoadFromFile(string filename)
    {
        Console.WriteLine($"Loading journal from JSON file: {filename}...");
        if (!File.Exists(filename))
        {
            Console.WriteLine("File not found. Starting with an empty journal.");
            _entries = new List<Entry>();
            return;
        }
        try
        {
            string jsonString = File.ReadAllText(filename);
            if (string.IsNullOrWhiteSpace(jsonString))
            {
                Console.WriteLine("File is empty. Starting with an empty journal.");
                _entries = new List<Entry>();
                return;
            }
            List<Entry> loadedEntries = JsonSerializer.Deserialize<List<Entry>>(jsonString);
            _entries = loadedEntries ?? new List<Entry>();
            Console.WriteLine("Journal loaded successfully from JSON.");
        }
        catch (Exception ex) // Basic error handling
        {
            Console.WriteLine($"Error loading journal: {ex.Message}");
        }
    }
}