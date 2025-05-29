// Program.cs
using System;

public class Program
{
    public static void Main(string[] args)
    {
        ScriptureLibrary library = new ScriptureLibrary();
        string playAgainInput = "yes";

        while (playAgainInput.ToLower() == "yes" || playAgainInput.ToLower() == "y")
        {
            Scripture scripture = library.GetRandomScripture();
            int wordsToHidePerTurn = 3;
            string currentScriptureInput = "";

            Console.Clear();
            Console.WriteLine("A new scripture has been selected for you!");
            Console.WriteLine("Press any key to begin...");
            Console.ReadKey();

            while (currentScriptureInput.ToLower() != "quit" && !scripture.IsCompletelyHidden())
            {
                Console.Clear();
                Console.WriteLine("Memorize the following scripture:");
                Console.WriteLine("--------------------------------");
                Console.WriteLine(scripture.GetDisplayText());
                Console.WriteLine("--------------------------------");
                Console.WriteLine("\nPress Enter to hide more words, or type 'quit' to stop this scripture.");
                currentScriptureInput = Console.ReadLine();

                if (currentScriptureInput.ToLower() != "quit")
                {
                    scripture.HideRandomWords(wordsToHidePerTurn);
                }
            }

            Console.Clear();
            Console.WriteLine("Final state of this scripture:");
            Console.WriteLine("--------------------------------");
            Console.WriteLine(scripture.GetDisplayText());
            Console.WriteLine("--------------------------------");

            if (scripture.IsCompletelyHidden())
            {
                Console.WriteLine("\nWell done! You have memorized this scripture.");
            }
            else
            {
                Console.WriteLine("\nYou chose to stop working on this scripture.");
            }

            Console.WriteLine("\nWould you like to try another scripture? (yes/no)");
            playAgainInput = Console.ReadLine();
        }

        Console.WriteLine("\nThank you for using the Scripture Memorizer! Goodbye.");
    }
}