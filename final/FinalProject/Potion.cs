using System.Collections.Generic;
using AdventureGame.Character;

namespace AdventureGame.Items
{
    public class Potion : Item
    {
        public int HealAmount { get; private set; }

        public Potion(string name, string description, string detailedDescription, string discoveryMessage, int healAmount, List<string> keywords) 
            : base(name, description, detailedDescription, discoveryMessage)
        {
            HealAmount = healAmount;
            Keywords.AddRange(keywords);
        }

        public override string Use(Player player)
        {
            int healthRestored = player.Heal(HealAmount);
            player.RemoveFromInventory(this);
            return $"You drink the {Name} and restore {healthRestored} health. Your health is now {player.Health}/{player.MaxHealth}.";
        }
    }
}