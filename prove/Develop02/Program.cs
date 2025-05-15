using System;
using System.IO;

public class Program
{
    static void Main(string[] args)
    {
        Journal myJournal = new Journal();
        PromptGenerator promptGenerator = new PromptGenerator();
        bool keepRunning = true;

        Console.WriteLine("Welcome to the Journal Program!");

        // Main loop for the program
        while (keepRunning)
        {
            DisplayMenu();
            Console.Write("What would you like to do? ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    HandleWriteNewEntry(myJournal, promptGenerator);
                    break;
                case "2":
                    myJournal.DisplayAll();
                    break;
                case "3":
                    HandleLoadJournal(myJournal);
                    break;
                case "4":
                    HandleSaveJournal(myJournal);
                    break;
                case "5":
                    keepRunning = false; // Exit the loop
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please enter a number from 1 to 5.");
                    break;
            }
        }
        Console.WriteLine("Thank you for using the Journal Program. Goodbye!");
    }

    // Shows the menu options
    static void DisplayMenu()
    {
        Console.WriteLine("\nPlease select one of the following choices:");
        Console.WriteLine("1. Write");
        Console.WriteLine("2. Display");
        Console.WriteLine("3. Load");
        Console.WriteLine("4. Save");
        Console.WriteLine("5. Quit");
    }

    // Handles writing a new journal entry
    static void HandleWriteNewEntry(Journal journal, PromptGenerator prompter)
    {
        string prompt = prompter.GetRandomPrompt();
        Console.WriteLine($"\n{prompt}");
        Console.Write("> ");
        string response = Console.ReadLine();
        string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); // Get current date and time
        Entry newEntry = new Entry(date, prompt, response);
        journal.AddEntry(newEntry);
        Console.WriteLine("Entry recorded successfully!");
    }

    // Handles loading the journal from a file
    static void HandleLoadJournal(Journal journal)
    {
        Console.Write("\nEnter the filename of the JSON journal to load (e.g., myjournal.json): ");
        string filename = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(filename))
        {
            journal.LoadFromFile(filename);
        }
        else
        {
            Console.WriteLine("Invalid filename provided. Load cancelled.");
        }
    }

    // Handles saving the journal to a file
    static void HandleSaveJournal(Journal journal)
    {
        Console.Write("\nEnter the filename to save the journal to (e.g., myjournal.json): ");
        string desiredFilename = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(desiredFilename))
        {
            string finalFilename = desiredFilename;
            // Ensure the file ends with .json
            if (!string.Equals(Path.GetExtension(desiredFilename), ".json", StringComparison.OrdinalIgnoreCase))
            {
                finalFilename = Path.ChangeExtension(desiredFilename, ".json");
                Console.WriteLine($"Saving as: {finalFilename}");
            }
            journal.SaveToFile(finalFilename);
        }
        else
        {
            Console.WriteLine("Invalid filename provided. Save cancelled.");
        }
    }
}