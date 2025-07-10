using AdventureGame.Character;

namespace AdventureGame.Items
{
    public class Potion : Item
    {
        public int HealAmount { get; private set; }

        public Potion(string name, string description, string discoveryMessage, int healAmount) 
            : base(name, description, discoveryMessage)
        {
            HealAmount = healAmount;
        }

        public override void Use(Player player)
        {
            player.Heal(HealAmount);
            player.RemoveFromInventory(this);
        }
    }
}