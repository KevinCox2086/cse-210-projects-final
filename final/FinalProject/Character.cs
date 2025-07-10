using System;

namespace AdventureGame.Character
{
    public abstract class Character
    {
        public string Name { get; protected set; }
        public int Health { get; protected set; }
        public int MaxHealth { get; protected set; }

        protected static readonly Random _random = new Random();
        
        public abstract int CalculateDamage(out bool isCritical);

        public Character(string name, int maxHealth)
        {
            Name = name;
            MaxHealth = maxHealth;
            Health = maxHealth;
        }

        public abstract string GetDescription();

        public bool IsAlive => Health > 0;

        public virtual void TakeDamage(int damage)
        {
            Health -= damage;
            if (Health < 0) Health = 0;
        }
    }
}