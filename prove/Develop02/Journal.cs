using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json; // <--- Add this for JSON serialization
// using System.Text.Json.Serialization; // <-- Might need this later for attributes like [JsonConstructor]

public class Journal
{
    // --- Member Variables (Private Fields) ---
    private List<Entry> _entries;

    // --- Constructor ---
    public Journal()
    {
        _entries = new List<Entry>();
    }

    // --- Methods ---

    /// <summary>
    /// Adds a new Entry object to the journal's list.
    /// </summary>
    /// <param name="newEntry">The Entry object to add.</param>
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

    /// <summary>
    /// Displays all entries currently stored in the journal to the console.
    /// </summary>
    public void DisplayAll()
    {
        if (_entries == null || _entries.Count == 0) // Added null check for safety
        {
            Console.WriteLine("\nJournal is empty.");
            return;
        }

        Console.WriteLine("\n--- Journal Entries ---");
        foreach (Entry entry in _entries)
        {
            Console.WriteLine(entry); // Relies on Entry.ToString()
        }
        Console.WriteLine("--- End of Journal ---\n");
    }

    // --- Updated SaveToFile using JSON ---
    /// <summary>
    /// Saves the current list of entries to a file in JSON format.
    /// </summary>
    /// <param name="filename">The path/name of the file to save to (e.g., journal.json).</param>
    public void SaveToFile(string filename)
    {
        Console.WriteLine($"Saving journal to {filename} as JSON...");

        try
        {
            // Configure options for readability (indented JSON)
            var options = new JsonSerializerOptions
            {
                WriteIndented = true // Makes the JSON file human-readable
            };

            // Serialize the entire list of entries to a JSON string
            string jsonString = JsonSerializer.Serialize(_entries, options);

            // Write the JSON string to the specified file (overwrites if exists)
            File.WriteAllText(filename, jsonString);

            Console.WriteLine("Journal saved successfully as JSON.");
        }
        catch (IOException ex) // Catch file access errors
        {
            Console.WriteLine($"Error saving journal: {ex.Message}");
        }
        catch (JsonException ex) // Catch errors during JSON serialization
        {
            Console.WriteLine($"Error serializing journal to JSON: {ex.Message}");
        }
        catch (Exception ex) // Catch other unexpected errors
        {
            Console.WriteLine($"An unexpected error occurred during saving: {ex.Message}");
        }
    }

    // --- Updated LoadFromFile using JSON ---
    /// <summary>
    /// Loads entries from a specified JSON file, replacing any current entries.
    /// </summary>
    /// <param name="filename">The path/name of the JSON file to load from.</param>
    public void LoadFromFile(string filename)
    {
        Console.WriteLine($"Loading journal from JSON file: {filename}...");

        if (!File.Exists(filename))
        {
            Console.WriteLine("File not found. Starting with an empty journal.");
            _entries = new List<Entry>(); // Ensure journal is empty if file doesn't exist
            return;
        }

        try
        {
            // Read the entire JSON file content
            string jsonString = File.ReadAllText(filename);

            // Check for empty file content before deserializing
            if (string.IsNullOrWhiteSpace(jsonString))
            {
                Console.WriteLine("File is empty. Starting with an empty journal.");
                _entries = new List<Entry>();
                return;
            }

            // Deserialize the JSON string back into a List<Entry>
            // Note: Uses the public properties (Date, PromptText, EntryText) and
            // expects a matching constructor or parameterless constructor + setters.
            // Our Entry class with matching constructor parameters should work.
            List<Entry> loadedEntries = JsonSerializer.Deserialize<List<Entry>>(jsonString);

            // Replace the current list, handle potential null result from Deserialize
            _entries = loadedEntries ?? new List<Entry>();

            Console.WriteLine("Journal loaded successfully from JSON.");

        }
        catch (IOException ex) // Catch file access errors
        {
            Console.WriteLine($"Error loading journal file: {ex.Message}");
            // Decide strategy: keep old _entries or clear them? Let's keep old ones on load failure.
        }
        catch (JsonException ex) // Catch errors during JSON deserialization (invalid format)
        {
            Console.WriteLine($"Error parsing JSON file: {ex.Message}");
            Console.WriteLine("Please ensure the file contains valid JSON. The journal was not loaded.");
             // Keep old _entries on load failure.
        }
         catch (Exception ex) // Catch other unexpected errors
        {
             Console.WriteLine($"An unexpected error occurred during loading: {ex.Message}");
             // Keep old _entries on load failure.
        }
    }
}