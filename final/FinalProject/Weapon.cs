using AdventureGame.Character;

namespace AdventureGame.Items
{
    public class Weapon : Item
    {
        public int MinDamage { get; private set; }
        public int MaxDamage { get; private set; }

        public Weapon(string name, string description, string discoveryMessage, int minDamage, int maxDamage) 
            : base(name, description, discoveryMessage)
        {
            MinDamage = minDamage;
            MaxDamage = maxDamage;
        }

        public override void Use(Player player)
        {
            System.Console.WriteLine($"You brandish the {Name}. It feels powerful in your hand.");
        }
    }
}