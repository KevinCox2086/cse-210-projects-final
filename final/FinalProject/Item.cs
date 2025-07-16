using System.Collections.Generic;
using System.Linq;

namespace AdventureGame.Items
{
    public abstract class Item
    {
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public string DetailedDescription { get; protected set; }
        public string DiscoveryMessage { get; protected set; }
        public List<string> Keywords { get; protected set; }
        public bool IsSecret { get; protected set; } = false;

        public Item(string name, string description, string detailedDescription, string discoveryMessage)
        {
            Name = name;
            Description = description;
            DetailedDescription = detailedDescription;
            DiscoveryMessage = discoveryMessage;
            Keywords = new List<string> { name.ToLower() };
        }

        public virtual string Use(Character.Player player)
        {
            return $"You can't seem to use the {Name} in any meaningful way.";
        }
    }
}