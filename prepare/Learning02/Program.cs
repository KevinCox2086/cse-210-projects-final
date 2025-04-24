// Program.cs

using System;
using System.Collections.Generic; // Make sure this is included at the top

class Program
{
    static void Main(string[] args)
    {
        // --- Previous code creating and displaying job1 and job2 ---
        Job job1 = new Job();
        job1._jobTitle = "Software Engineer";
        job1._company = "Microsoft";
        job1._startYear = 2019;
        job1._endYear = 2022;

        Job job2 = new Job();
        job2._jobTitle = "Web Developer";
        job2._company = "Google";
        job2._startYear = 2020;
        job2._endYear = 2023;

        Console.WriteLine("--- Job Details ---");
        job1.Display();
        job2.Display();

        // --- New code to test Resume class ---
        Console.WriteLine("\n--- Resume Test ---");

        // 1. Create a new instance of the Resume class
        Resume myResume = new Resume();

        // (Optional: Set the name - though not explicitly asked for in this step)
        myResume._name = "Allison Rose";

        // 2. Add the two jobs to the list of jobs in the resume object
        myResume._jobs.Add(job1);
        myResume._jobs.Add(job2);

        // 3. Verify accessing and displaying the first job title
        // Access the list: myResume._jobs
        // Access the first item (index 0): [0]
        // Access that job's title: ._jobTitle
        Console.WriteLine($"First job title in list: {myResume._jobs[0]._jobTitle}");

        // (Optional: Display the second job's company to test index 1)
        // Console.WriteLine($"Second job company in list: {myResume._jobs[1]._company}");

        // Keep the console window open (optional)
        // Console.WriteLine("\nPress any key to exit.");
        // Console.ReadKey();
    }
}