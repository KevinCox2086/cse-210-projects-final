namespace AdventureGame.Items
{
    public class Treasure : Item
    {
        public Treasure(string name, string description, string discoveryMessage) 
            : base(name, description, discoveryMessage)
        {
        }

        public override void Use(Character.Player player)
        {
            System.Console.WriteLine($"You gaze at the {Name}. It seems to hum with ancient power.");
        }
    }
}