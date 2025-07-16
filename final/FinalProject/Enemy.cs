using System;
using System.Collections.Generic;
using AdventureGame.Items;

namespace AdventureGame.Character
{
    public class Enemy : Character
    {
        private readonly int _minDamage;
        private readonly int _maxDamage;
        
        public List<Item> Loot { get; private set; } = new List<Item>();
        public string DefeatMessage { get; set; } = "";
        public string WoundedDescription { get; set; }

        public Enemy(string name, string detailedDescription, int maxHealth, int minDamage, int maxDamage, List<string> keywords) 
            : base(name, detailedDescription, maxHealth)
        {
            _minDamage = minDamage;
            _maxDamage = maxDamage;
            Keywords.AddRange(keywords);
            WoundedDescription = "Ugh... you'll pay for that!";
        }

        public override int CalculateDamage(out bool isCritical)
        {
            int damage = _random.Next(_minDamage, _maxDamage + 1);

            if (_random.Next(1, 101) <= 20)
            {
                isCritical = true;
                return damage * 2;
            }

            isCritical = false;
            return damage;
        }

        public override string GetDescription()
        {
            double healthPercent = (double)Health / MaxHealth;
            if (healthPercent < 0.5 && !string.IsNullOrEmpty(WoundedDescription))
            {
                return WoundedDescription;
            }

            if (this.Name == "Goblin Guard") return "No entry! Boss says so!";
            if (this.Name == "Goblin Scout") return "You're not supposed to be here! I'll get you!";
            if (this.Name == "Large Red Dragon") return "My hoard is MINE! I will melt the flesh from your bones!";
            
            return $"A menacing {Name} stands here, looking for a fight.";
        }
    }
}