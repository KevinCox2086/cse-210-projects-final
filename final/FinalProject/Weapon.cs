using AdventureGame.Character;

namespace AdventureGame.Items
{
    public class Weapon : Item
    {
        public int MinDamage { get; private set; }
        public int MaxDamage { get; private set; }

        public Weapon(string name, string description, string detailedDescription, string discoveryMessage, int minDamage, int maxDamage) 
            : base(name, description, detailedDescription, discoveryMessage)
        {
            MinDamage = minDamage;
            MaxDamage = maxDamage;
        }

        public override string Use(Player player)
        {
            if (this.Name == "Dragonslayer Blade")
            {
                return "The Dragonslayer Blade hums with a faint energy as you hold it aloft.";
            }
            return $"You practice a quick swing with the {Name}.";
        }
    }
}