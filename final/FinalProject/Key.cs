using AdventureGame.Character;

namespace AdventureGame.Items
{
    public class Key : Item
    {
        public int KeyId { get; private set; }

        public Key(string name, string description, string discoveryMessage, int keyId) 
            : base(name, description, discoveryMessage)
        {
            KeyId = keyId;
        }

        public override void Use(Player player)
        {
            System.Console.WriteLine($"You hold up the {Name}. It might be useful for a lock.");
        }
    }
}