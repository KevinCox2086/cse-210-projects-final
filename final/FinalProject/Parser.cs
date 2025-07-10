using System;

namespace AdventureGame.Core
{
    public static class Parser
    {
        public static string[] Parse(string input)
        {
            return input.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        }
    }
}