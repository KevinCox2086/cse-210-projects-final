using System;
using System.Collections.Generic;
using System.IO;


public class GoalManager
{
    private List<Goal> _goals;
    private int _score;

    public GoalManager()
    {
        _goals = new List<Goal>();
        _score = 0;
    }

    private int PromptForInteger(string prompt)
    {
        int number;
        Console.Write(prompt);
        while (!int.TryParse(Console.ReadLine(), out number))
        {
            Console.WriteLine("Invalid input. Please enter a valid number.");
            Console.Write(prompt);
        }
        return number;
    }

    
    private string GetRankFromScore(int score)
    {
        if (score < 1000) return "Level 1: Novice Adventurer";
        if (score < 2500) return "Level 2: Apprentice";
        if (score < 5000) return "Level 3: Journeyman";
        if (score < 7500) return "Level 4: Master";
        return "Level 5: Grandmaster";
    }

    public void Start()
    {
        bool running = true;
        while (running)
        {
            DisplayPlayerInfo();

            Console.WriteLine("Menu Options:");
            Console.WriteLine("  1. Create New Goal");
            Console.WriteLine("  2. List Goals");
            Console.WriteLine("  3. Save Goals");
            Console.WriteLine("  4. Load Goals");
            Console.WriteLine("  5. Record Event");
            Console.WriteLine("  6. Quit");
            Console.Write("Select a choice from the menu: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    CreateGoal();
                    break;
                case "2":
                    ListGoalDetails();
                    break;
                case "3":
                    SaveGoals();
                    break;
                case "4":
                    LoadGoals();
                    break;
                case "5":
                    RecordEvent();
                    break;
                case "6":
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    public void DisplayPlayerInfo()
    {
        Console.WriteLine($"\nYou have {_score} points.");
        Console.WriteLine($"Your current rank is: {GetRankFromScore(_score)}\n");
    }

    public void ListGoalDetails()
    {
        Console.WriteLine("The goals are:");
        if (_goals.Count == 0)
        {
            Console.WriteLine("You have no goals yet.");
        }
        else
        {
            for (int i = 0; i < _goals.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {_goals[i].GetDetailsString()}");
            }
        }
    }

    public void CreateGoal()
    {
        Console.WriteLine("The types of Goals are:");
        Console.WriteLine("  1. Simple Goal");
        Console.WriteLine("  2. Eternal Goal");
        Console.WriteLine("  3. Checklist Goal");
        Console.Write("Which type of goal would you like to create? ");
        string typeChoice = Console.ReadLine();

        Console.Write("What is the name of your goal? ");
        string name = Console.ReadLine();
        Console.Write("What is a short description of it? ");
        string description = Console.ReadLine();
        
        int points = PromptForInteger("What is the amount of points associated with this goal? ");

        switch (typeChoice)
        {
            case "1":
                _goals.Add(new SimpleGoal(name, description, points));
                break;
            case "2":
                _goals.Add(new EternalGoal(name, description, points));
                break;
            case "3":
                int target = PromptForInteger("How many times does this goal need to be accomplished for a bonus? ");
                int bonus = PromptForInteger("What is the bonus for accomplishing it that many times? ");
                _goals.Add(new ChecklistGoal(name, description, points, target, bonus));
                break;
            default:
                Console.WriteLine("Invalid goal type.");
                break;
        }
    }

    public void RecordEvent()
    {
        if (_goals.Count == 0)
        {
            Console.WriteLine("You have no goals to record.");
            return;
        }

        Console.WriteLine("The goals are:");
        for (int i = 0; i < _goals.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {_goals[i].GetDetailsString()}");
        }
        
        int goalChoice = PromptForInteger("Which goal did you accomplish? ") - 1;

        if (goalChoice >= 0 && goalChoice < _goals.Count)
        {
            string previousRank = GetRankFromScore(_score);

            int pointsEarned = _goals[goalChoice].RecordEvent();
            _score += pointsEarned;
            
            string newRank = GetRankFromScore(_score);

            if (previousRank != newRank)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n*********************************");
                Console.WriteLine("    CONGRATULATIONS! LEVEL UP!   ");
                Console.WriteLine($" You have achieved the rank of: {newRank}");
                Console.WriteLine("*********************************\n");
                Console.ResetColor();
            }

            Console.WriteLine($"You now have {_score} points.");
        }
        else
        {
            Console.WriteLine("Invalid goal selection.");
        }
    }

    public void SaveGoals()
    {
        Console.Write("What is the filename for the goal file? ");
        string filename = Console.ReadLine();

        using (StreamWriter outputFile = new StreamWriter(filename))
        {
            outputFile.WriteLine(_score);
            foreach (Goal goal in _goals)
            {
                outputFile.WriteLine(goal.GetStringRepresentation());
            }
        }
        Console.WriteLine("Goals saved successfully.");
    }

    public void LoadGoals()
    {
        Console.Write("What is the filename for the goal file? ");
        string filename = Console.ReadLine();
        
        if (!File.Exists(filename))
        {
            Console.WriteLine("File not found.");
            return;
        }

        string[] lines = File.ReadAllLines(filename);
        
        _goals.Clear();

        _score = int.Parse(lines[0]);

        for (int i = 1; i < lines.Length; i++)
        {
            string[] parts = lines[i].Split(':');
            string goalType = parts[0];
            string name = parts[1];
            string description = parts[2];
            int points = int.Parse(parts[3]);

            switch (goalType)
            {
                case "SimpleGoal":
                    bool isComplete = bool.Parse(parts[4]);
                    _goals.Add(new SimpleGoal(name, description, points, isComplete));
                    break;
                case "EternalGoal":
                    _goals.Add(new EternalGoal(name, description, points));
                    break;
                case "ChecklistGoal":
                    int bonus = int.Parse(parts[4]);
                    int target = int.Parse(parts[5]);
                    int completed = int.Parse(parts[6]);
                    _goals.Add(new ChecklistGoal(name, description, points, target, bonus, completed));
                    break;
            }
        }
        Console.WriteLine("Goals loaded successfully.");
    }
}