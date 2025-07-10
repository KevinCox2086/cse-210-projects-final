using System;
using System.Collections.Generic;
using System.Linq;
using AdventureGame.Items;

namespace AdventureGame.Character
{
    public class Player : Character
    {
        private readonly List<Item> _inventory = new List<Item>();
        public IReadOnlyList<Item> Inventory => _inventory;

        public Player(string name, int maxHealth) : base(name, maxHealth) { }

        public override int CalculateDamage(out bool isCritical)
        {
            int minDamage;
            int maxDamage;
            var bestWeapon = Inventory.OfType<Weapon>().OrderByDescending(w => w.MaxDamage).FirstOrDefault();
            if (bestWeapon != null)
            {
                minDamage = bestWeapon.MinDamage;
                maxDamage = bestWeapon.MaxDamage;
            }
            else
            {
                minDamage = 1;
                maxDamage = 4;
            }
            int damage = _random.Next(minDamage, maxDamage + 1);
            if (_random.Next(1, 101) <= 10)
            {
                isCritical = true;
                return damage * 2;
            }
            isCritical = false;
            return damage;
        }

        public void AddToInventory(Item item)
        {
            _inventory.Add(item);
        }

        public void RemoveFromInventory(Item item)
        {
            _inventory.Remove(item);
        }

        public void Heal(int amount)
        {
            Health += amount;
            if (Health > MaxHealth) Health = MaxHealth;
        }

        public override string GetDescription()
        {
            return $"It's you, {Name}, a brave adventurer.";
        }
    }
}