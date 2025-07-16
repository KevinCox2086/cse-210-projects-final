namespace AdventureGame.Items
{
    public class Treasure : Item
    {
        public Treasure(string name, string description, string detailedDescription, string discoveryMessage) 
            : base(name, description, detailedDescription, discoveryMessage)
        {
        }

        public override string Use(Character.Player player)
        {
            return $"You gaze at the {Name}. It seems to hum with ancient power.";
        }
    }
}