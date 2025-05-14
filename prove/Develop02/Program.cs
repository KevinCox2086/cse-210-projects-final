using System;
using System.IO; // Include if needed, although Journal class handles file IO directly

public class Program
{
    // --- Main Entry Point ---
    static void Main(string[] args)
    {
        // Create instances of our core classes
        Journal myJournal = new Journal();
        PromptGenerator promptGenerator = new PromptGenerator();
        bool keepRunning = true;

        Console.WriteLine("Welcome to the Journal Program!");

        // --- Main Application Loop ---
        while (keepRunning)
        {
            DisplayMenu(); // Show menu options
            Console.Write("What would you like to do? ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1": // Write a new entry
                    HandleWriteNewEntry(myJournal, promptGenerator);
                    break;
                case "2": // Display the journal
                    myJournal.DisplayAll();
                    break;
                case "3": // Load the journal from a file
                    HandleLoadJournal(myJournal);
                    break;
                case "4": // Save the journal to a file
                    HandleSaveJournal(myJournal);
                    break;
                case "5": // Quit
                    keepRunning = false;
                    break;
                default: // Handle invalid input
                    Console.WriteLine("Invalid choice. Please enter a number from 1 to 5.");
                    break;
            }
            // Optional: Add a small pause or prompt before showing the menu again
            // Console.WriteLine("\nPress Enter to continue...");
            // Console.ReadLine();
        }

        Console.WriteLine("Thank you for using the Journal Program. Goodbye!");
    }

    // --- Helper Methods for Menu Actions ---

    /// <summary>
    /// Displays the main menu options to the console.
    /// </summary>
    static void DisplayMenu()
    {
        Console.WriteLine("\nPlease select one of the following choices:");
        Console.WriteLine("1. Write");
        Console.WriteLine("2. Display");
        Console.WriteLine("3. Load");
        Console.WriteLine("4. Save - Please end filename with (.json) ex: journal.json");
        Console.WriteLine("5. Quit");
    }

    /// <summary>
    /// Handles the process of writing a new journal entry.
    /// </summary>
    /// <param name="journal">The Journal instance to add the entry to.</param>
    /// <param name="prompter">The PromptGenerator instance to get a prompt from.</param>
    static void HandleWriteNewEntry(Journal journal, PromptGenerator prompter)
    {
        string prompt = prompter.GetRandomPrompt();
        Console.WriteLine($"\n{prompt}"); // Display the prompt
        Console.Write("> "); // Prompt for user input
        string response = Console.ReadLine();

        // Get the current date (using Knoxville, TN time as per context if needed, otherwise local machine time)
        // For simplicity, using local machine time here. Adjust if specific timezone is required.
        // Use DateTimeOffset for timezone awareness if necessary: DateTimeOffset.Now or DateTimeOffset.UtcNow
        string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); // Example format with time

        // Create the new entry
        Entry newEntry = new Entry(date, prompt, response);

        // Add it to the journal
        journal.AddEntry(newEntry);
        Console.WriteLine("Entry recorded successfully!");
    }

    /// <summary>
    /// Handles loading the journal from a user-specified file.
    /// </summary>
    /// <param name="journal">The Journal instance to load data into.</param>
    static void HandleLoadJournal(Journal journal)
    {
        Console.Write("\nEnter the filename to load the journal from: ");
        string filename = Console.ReadLine();
        // Basic validation - check if filename is not empty
        if (!string.IsNullOrWhiteSpace(filename))
        {
            journal.LoadFromFile(filename);
        }
        else
        {
            Console.WriteLine("Invalid filename provided.");
        }
    }

    /// <summary>
    /// Handles saving the journal to a user-specified file.
    /// </summary>
    /// <param name="journal">The Journal instance containing data to save.</param>
    static void HandleSaveJournal(Journal journal)
    {
        Console.Write("\nEnter the filename to save the journal to: ");
        string filename = Console.ReadLine();
        // Basic validation - check if filename is not empty
        if (!string.IsNullOrWhiteSpace(filename))
        {
             journal.SaveToFile(filename);
        }
         else
        {
            Console.WriteLine("Invalid filename provided.");
        }
    }
}