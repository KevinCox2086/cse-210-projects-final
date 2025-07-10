namespace AdventureGame.Items
{
    public abstract class Item
    {
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public string DiscoveryMessage { get; protected set; }

        public Item(string name, string description, string discoveryMessage)
        {
            Name = name;
            Description = description;
            DiscoveryMessage = discoveryMessage;
        }

        public virtual void Use(Character.Player player)
        {
            System.Console.WriteLine($"You can't seem to use the {Name} in any meaningful way.");
        }
    }
}