// Resume.cs

using System;
using System.Collections.Generic; // Required for using List<T>

public class Resume
{
    // Member variables (fields)
    public string _name; // Stores the person's name

    // Initializes the list right away to avoid null reference issues
    public List<Job> _jobs = new List<Job>();

    // Constructor (optional, could be added to set the name)
    // public Resume(string name)
    // {
    //     _name = name;
    //     _jobs = new List<Job>(); // Initialize list here too if using constructor
    // }

    // Behaviors (Methods)
    public void Display()
    {
        // Display the person's name
        Console.WriteLine($"Name: {_name}");

        // Display the "Jobs:" header
        Console.WriteLine("Jobs:");

        // Iterate through each job in the _jobs list
        foreach (Job job in _jobs)
        {
            // Call the Display() method of the Job class for each job
            job.Display();
        }
    }
}

