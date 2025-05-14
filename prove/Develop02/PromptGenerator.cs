using System;
using System.Collections.Generic;

public class PromptGenerator
{
    // --- Member Variables ---
    private List<string> _prompts;
    private Random _random;

    // --- Constructor ---
    public PromptGenerator()
    {
        _random = new Random(); // Initialize the random number generator

        // Initialize the list of prompts
        _prompts = new List<string>
        {
            "Who was the most interesting person I interacted with today?",
            "What was the best part of my day?",
            "How did I see the hand of the Lord in my life today?",
            "What was the strongest emotion I felt today?",
            "If I had one thing I could do over today, what would it be?",
            "What is something new I learned today?",
            "What am I grateful for right now?",
            "Describe a moment of peace or quiet you experienced today."
            // Add more prompts as desired (at least 5 required)
        };
    }

    // --- Methods ---

    /// <summary>
    /// Gets a random prompt from the predefined list.
    /// </summary>
    /// <returns>A randomly selected prompt string.</returns>
    public string GetRandomPrompt()
    {
        if (_prompts == null || _prompts.Count == 0)
        {
            return "No prompts available."; // Fallback
        }

        int index = _random.Next(_prompts.Count); // Get a random index within the list bounds
        return _prompts[index];
    }
}