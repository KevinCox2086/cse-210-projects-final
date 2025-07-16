using System.Collections.Generic;
using AdventureGame.Character;

namespace AdventureGame.Items
{
    public class Key : Item
    {
        public int KeyId { get; private set; }

        public Key(string name, string description, string detailedDescription, string discoveryMessage, int keyId, List<string> keywords) 
            : base(name, description, detailedDescription, discoveryMessage)
        {
            KeyId = keyId;
            Keywords.AddRange(keywords);
        }

        public override string Use(Player player)
        {
            return "This key might be useful for a lock.";
        }
    }
}