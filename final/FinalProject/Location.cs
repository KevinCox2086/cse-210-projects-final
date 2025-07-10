using System.Collections.Generic;
using System.Linq;
using AdventureGame.Character;
using AdventureGame.Items;

namespace AdventureGame.Core
{
    public class Location
    {
        public string Name { get; }
        public string Description { get; }
        public string DescriptionAfterLoot { get; }
        public int X { get; }
        public int Y { get; }
        public bool HasBeenVisited { get; set; } = false;

        public List<Item> Items { get; } = new List<Item>();
        public List<Character.Character> Characters { get; } = new List<Character.Character>();
        public Dictionary<string, Exit> Exits { get; } = new Dictionary<string, Exit>();

        public Location(string name, string description, string descriptionAfterLoot, int x, int y)
        {
            Name = name;
            Description = description;
            DescriptionAfterLoot = descriptionAfterLoot;
            X = x;
            Y = y;
        }

        public void AddExit(string direction, Exit exit)
        {
            Exits[direction.ToLower()] = exit;
        }
    }
}