using System;
using System.Collections.Generic;

public class PromptGenerator
{
    private List<string> _prompts;
    private Random _random;

    public PromptGenerator()
    {
        _random = new Random();
        _prompts = new List<string> // List of journal prompts
        {
            "Who was the most interesting person I interacted with today?",
            "What was the best part of my day?",
            "How did I see the hand of the Lord in my life today?",
            "What was the strongest emotion I felt today?",
            "If I had one thing I could do over today, what would it be?",
            "What is something new I learned today?",
            "What am I grateful for right now?",
            "Describe a moment of peace or quiet you experienced today."
        };
    }

    // Gets a random prompt from the list
    public string GetRandomPrompt()
    {
        if (_prompts == null || _prompts.Count == 0)
        {
            return "No prompts available.";
        }
        int index = _random.Next(_prompts.Count);
        return _prompts[index];
    }
}