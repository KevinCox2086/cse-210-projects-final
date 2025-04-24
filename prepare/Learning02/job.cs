// Job.cs

using System;

public class Job
{
    // Member variables (fields)
    public string _company;
    public string _jobTitle;
    public int _startYear;
    public int _endYear;

    // Constructor (optional, might be added later)

    // Behaviors (Methods)
    public void Display()
    {
        // Displays the job details in the specified format
        Console.WriteLine($"{_jobTitle} ({_company}) {_startYear}-{_endYear}");
    }
}