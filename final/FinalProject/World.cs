using System.Collections.Generic;
using AdventureGame.Character;
using AdventureGame.Items;
using AdventureGame.Core;

namespace AdventureGame.Core
{
    public class World
    {
        public Location StartingLocation { get; private set; }
        public List<Location> AllLocations { get; private set; }
        
        public World()
        {
            AllLocations = new List<Location>();
            CreateWorld();
        }

        private void CreateWorld()
        {
            var townSquare = new Location(
                "Town Square", 
                "You stand in the center of a bustling town square. A large, ornate fountain bubbles cheerfully, its basin glittering with tossed coins... and something else that catches your eye.", 
                "You stand in the center of a bustling town square. A large, ornate fountain bubbles cheerfully.",
                0, 0);

            var armory = new Location(
                "Armory", 
                "The air is thick with the smell of old leather and sharpening stones. Racks of dusty armor and shields line the walls. Among the neglected equipment, a simple but well-made sword leans within reach.",
                "The air is thick with the smell of old leather and sharpening stones. Racks of dusty armor and shields line the walls. It looks like anything useful has already been taken.",
                0, 1);
                
            var alchemistShop = new Location(
                "Alchemist's Shop", 
                "Strange, sweet-smelling smoke hangs in the air. Shelves are crammed with bizarre ingredients in jars and beakers of bubbling, colorful liquids. One particular vial, filled with a swirling red fluid, seems to pulse with a faint light.",
                "Strange, sweet-smelling smoke hangs in the air. The shelves are filled with bizarre ingredients, but nothing else seems immediately useful.",
                1, 0);
                
            var darkCaveEntrance = new Location(
                "Dark Cave Entrance", 
                "A chilling wind howls from the mouth of a dark cave, carrying with it the foul stench of its inhabitant. The way forward is blocked by a heavy stone door, sealed with a large, rusty lock.",
                "A chilling wind howls from the mouth of a dark cave. The heavy stone door stands open, leading into darkness.",
                -1, 0);
                
            var caveInterior = new Location(
                "Cave Interior", 
                "The air is damp and heavy with the smell of wet earth and mildew. The only light is the faint path back to the entrance, but you can perceive a faint, soft glow coming from a small pedestal deeper within the darkness.",
                "The air is damp and heavy with the smell of wet earth and mildew. An empty pedestal sits in the center of the room, its glow now gone.",
                -2, 0);

            AllLocations.Add(townSquare);
            AllLocations.Add(armory);
            AllLocations.Add(alchemistShop);
            AllLocations.Add(darkCaveEntrance);
            AllLocations.Add(caveInterior);

            var sword = new Weapon("Iron Sword", "A simple but sturdy iron sword.", "Leaning against a dusty rack, you spot a sturdy Iron Sword and take it.", 5, 10);
            var healthPotion = new Potion("Health Potion", "A swirling red liquid.", "On a cluttered shelf, a swirling red Health Potion catches your eye. You pocket it.", 25);
            var caveKey = new Key("Rusty Key", "An old, rusty key.", "Tucked away in a shadowy corner of the fountain's basin, you find a Rusty Key.", 101);
            var amulet = new Treasure("Ancient Amulet", "A beautiful, glowing amulet.", "As the goblin falls, the faint glow from the pedestal intensifies. You approach and pick up the beautiful Ancient Amulet. This must be the treasure!");

            var goblinGuard = new Enemy("Goblin Guard", 30, 1, 5);
            var goblinScout = new Enemy("Goblin Scout", 25, 2, 6);

            townSquare.Items.Add(caveKey);
            armory.Items.Add(sword);
            alchemistShop.Items.Add(healthPotion);
            darkCaveEntrance.Characters.Add(goblinGuard);
            caveInterior.Characters.Add(goblinScout);
            caveInterior.Items.Add(amulet);

            townSquare.AddExit("north", new Exit(armory));
            townSquare.AddExit("east", new Exit(alchemistShop));
            townSquare.AddExit("west", new Exit(darkCaveEntrance));

            armory.AddExit("south", new Exit(townSquare));
            alchemistShop.AddExit("west", new Exit(townSquare));

            darkCaveEntrance.AddExit("east", new Exit(townSquare));
            darkCaveEntrance.AddExit("forward", new Exit(caveInterior, isLocked: true, requiredKeyId: 101));

            caveInterior.AddExit("out", new Exit(darkCaveEntrance));
            
            StartingLocation = townSquare;
        }
    }
}