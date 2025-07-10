using System;
using System.Collections.Generic;
using System.Linq;
using AdventureGame.Core;
using AdventureGame.Items;
using AdventureGame.Character;

namespace AdventureGame
{
    public static class Map
    {
        public static void Draw(List<Location> allLocations, Location currentLocation, Player player)
        {
            if (!allLocations.Any()) return;

            int minX = allLocations.Min(loc => loc.X);
            int maxX = allLocations.Max(loc => loc.X);
            int minY = allLocations.Min(loc => loc.Y);
            int maxY = allLocations.Max(loc => loc.Y);

            Console.WriteLine("\n--- World Map ---");
            Console.WriteLine(" [YOU] = Your Location | [ ! ] = Item of Interest");

            for (int y = maxY; y >= minY; y--)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    Location here = allLocations.FirstOrDefault(loc => loc.X == x && loc.Y == y);

                    if (here == null)
                    {
                        Console.Write("     ");
                    }
                    else if (here == currentLocation)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("[YOU]");
                        Console.ResetColor();
                    }
                    else
                    {
                        bool hasInterest = false;

                        if (here.Items.Any(item => item is Weapon || item is Potion || item is Key))
                        {
                            hasInterest = true;
                        }
                        else if (here.Items.Any(item => item is Treasure))
                        {
                            bool hasKey = player.Inventory.OfType<Key>().Any();
                            bool hasWeapon = player.Inventory.OfType<Weapon>().Any();
                            bool hasPotion = player.Inventory.OfType<Potion>().Any();

                            if (hasKey && hasWeapon && hasPotion)
                            {
                                hasInterest = true;
                            }
                        }

                        if (hasInterest)
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write("[ ! ]");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.Write("[   ]");
                        }
                    }
                }
                Console.WriteLine();
                Console.WriteLine();
            }
            Console.WriteLine("-----------------");
        }
    }
}