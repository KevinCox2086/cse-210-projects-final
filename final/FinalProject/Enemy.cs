using System;

namespace AdventureGame.Character
{
    public class Enemy : Character
    {
        private readonly int _minDamage;
        private readonly int _maxDamage;

        public Enemy(string name, int maxHealth, int minDamage, int maxDamage) : base(name, maxHealth)
        {
            _minDamage = minDamage;
            _maxDamage = maxDamage;
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
            return $"A menacing {Name} stands here, looking for a fight.";
        }
    }
}