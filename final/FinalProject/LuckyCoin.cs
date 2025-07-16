using System.Collections.Generic;

namespace AdventureGame.Items
{
    public class LuckyCoin : Item
    {
        public LuckyCoin(string name, string description, string detailedDescription, string discoveryMessage) 
            : base(name, description, detailedDescription, discoveryMessage)
        {
            Keywords.Add("coin");
            IsSecret = true;
        }

        public override string Use(Character.Player player)
        {
            return "You flip the lucky coin in the air. It feels auspicious.";
        }
    }
}