using System;

/*
* IMPROVEMENT 1: This includes robust input handling.
* The new private helper method, `PromptForInteger`, uses `int.TryParse`
* within a loop to ensure that the user enters a valid number when one
* is required. This prevents the program from crashing if the user
* accidentally types text instead of a number.
*
* IMPROVEMENT 2: This adds a Level and Rank system.
* A new method, `GetRankFromScore`, determines the player's title based
* on their score. `DisplayPlayerInfo` shows this rank, and `RecordEvent`
* now detects and celebrates when the player levels up.
*/


class Program
{
    static void Main(string[] args)
    {
        GoalManager goalManager = new GoalManager();
        goalManager.Start();
    }
}