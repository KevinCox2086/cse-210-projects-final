using System;
using System.Threading;

namespace AdventureGame
{
    public static class Sound
    {
        public static void PlayPlayerAttack()
        {
            Console.Beep(200, 100);
            Console.Beep(300, 100);
        }

        public static void PlayEnemyAttack()
        {
            Console.Beep(150, 100);
            Console.Beep(100, 100);
        }

        public static void PlayMiss()
        {
            Console.Beep(100, 200);
        }

        public static void PlayCriticalHit()
        {
            Console.Beep(600, 100);
            Console.Beep(800, 100);
            Console.Beep(1000, 150);
        }

        public static void PlayItemGet()
        {
            Console.Beep(400, 100);
            Console.Beep(600, 100);
        }

        public static void PlayUnlockDoor()
        {
            Console.Beep(300, 150);
            Console.Beep(500, 150);
            Console.Beep(700, 200);
        }
    }
}