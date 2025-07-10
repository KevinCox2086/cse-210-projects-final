using System;

namespace AdventureGame
{
    public static class UI
    {
        public static void DrawHealthBar(string name, int currentHealth, int maxHealth)
        {
            if (currentHealth < 0) currentHealth = 0;

            Console.Write($"{name,-12} [");

            decimal healthPercentage = (decimal)currentHealth / maxHealth;

            if (healthPercentage > 0.6m)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else if (healthPercentage > 0.3m)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }

            const int barWidth = 20;
            int filledBlocks = (int)(healthPercentage * barWidth);

            for (int i = 0; i < barWidth; i++)
            {
                if (i < filledBlocks)
                {
                    Console.Write("█");
                }
                else
                {
                    Console.Write("░");
                }
            }

            Console.ResetColor();
            Console.WriteLine($"] {currentHealth}/{maxHealth}");
        }
    }
}