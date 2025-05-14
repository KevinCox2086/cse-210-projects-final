public class Entry
{
    // Properties must be public for JsonSerializer to see them by default
    public string Date { get; private set; } // Changed to use private set for deserialization
    public string PromptText { get; private set; }
    public string EntryText { get; private set; }

    // Constructor used for creating new entries in the program
    // AND potentially by JsonSerializer if parameter names match properties
    public Entry(string date, string promptText, string entryText)
    {
        Date = date;
        PromptText = promptText;
        EntryText = entryText;
    }

    // --- Potentially needed for JSON Deserialization ---
    // If the constructor above doesn't work automatically with System.Text.Json,
    // you might need ONE of the following:
    // 1. Add a parameterless constructor (less ideal if you want immutability)
    //    private Entry() {} // Make private if only for deserializer
    // 2. Add the [JsonConstructor] attribute (requires using System.Text.Json.Serialization;)
    //    [JsonConstructor]
    //    public Entry(string date, string promptText, string entryText) { ... }
    // 3. Make properties have public setters (least ideal for immutability)
    //    public string Date { get; set; } // etc.

    // Let's try WITHOUT changes first, but keep these in mind.
    // I've slightly modified properties above to use 'private set' which is often
    // compatible with serializers while maintaining external immutability.

    public override string ToString()
    {
        // (Keep your existing ToString method)
        return $"Date: {Date}{Environment.NewLine}Prompt: {PromptText}{Environment.NewLine}Entry: {EntryText}{Environment.NewLine}----------";
    }
}