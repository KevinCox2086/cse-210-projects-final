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
                "You stand in the center of a bustling town square. A large, ornate fountain bubbles cheerfully, its basin glittering with tossed coins.", 
                "You stand in the center of a bustling town square. A large, ornate fountain bubbles cheerfully.",
                "You see something shiny in the bottom of the fountain.",
                "",
                0, 0);

            var armory = new Location(
                "Armory", 
                "The air is thick with the smell of old leather and sharpening stones. Racks of dusty armor and shields line the walls.",
                "The air is thick with the smell of old leather and sharpening stones. Racks of dusty armor and shields line the walls. It looks like anything useful has already been taken.",
                "Your eye is drawn to a glint of steel from a sword rack in the corner.",
                "",
                0, 1);
                
            var alchemistShop = new Location(
                "Alchemist's Shop", 
                "Strange, sweet-smelling smoke hangs in the air. Shelves are crammed with bizarre ingredients in jars and beakers of bubbling, colorful liquids.",
                "Strange, sweet-smelling smoke hangs in the air. The shelves are filled with bizarre ingredients, but nothing else seems immediately useful.",
                "You notice a single potion on the counter that seems to glow with a soft, inviting light.",
                "",
                1, 0);
                
            var darkCaveEntrance = new Location(
                "Dark Cave Entrance", 
                "A chilling wind howls from the mouth of a dark cave, carrying with it the foul stench of its inhabitant. The way forward is blocked by a heavy stone door, sealed with a large, rusty lock.",
                "A chilling wind howls from the mouth of a dark cave. The heavy stone door stands open, leading into darkness.",
                "",
                "",
                -1, 0);
                
            var caveInterior = new Location(
                "Cave Interior", 
                "The air is damp and heavy with the smell of wet earth and mildew. The only light is the faint path back to the entrance. A narrow passage continues deeper into the darkness.",
                "The air is damp and heavy with the smell of wet earth and mildew. A narrow passage continues deeper into the darkness.",
                "",
                "",
                -2, 0);

            var dragonLair = new Location(
                "Dragon's Lair",
                "You enter a massive cavern. The air shimmers with intense heat, and the floor is littered with the scorched bones of previous adventurers. In the center of the room, sleeping atop a vast hoard of gold, is a Large Red Dragon.",
                "The massive cavern is now quiet, the great dragon defeated. Its hoard of gold glitters in the dim light.",
                "A beautiful amulet rests just beyond the dragon's reach.",
                "The Large Red Dragon stirs and speaks: \"So, another morsel has wandered into my hoard. Your bones will look fine scattered amongst the others.\"",
                -3, 0);

            AllLocations.Add(townSquare);
            AllLocations.Add(armory);
            AllLocations.Add(alchemistShop);
            AllLocations.Add(darkCaveEntrance);
            AllLocations.Add(caveInterior);
            AllLocations.Add(dragonLair);

            var sword = new Weapon("Iron Sword", "A simple but sturdy iron sword.", "A standard-issue sword. It's seen better days but is still sharp.", "Leaning against a dusty rack, you spot a sturdy Iron Sword and take it.", 5, 10);
            var dragonslayerBlade = new Weapon("Dragonslayer Blade", "A legendary blade that hums with power.", "An ancient blade, surprisingly light and perfectly balanced. It glows with a faint, silver light.", "The defeated goblin scout was carrying a magnificent blade. This will be perfect for fighting the dragon!", 15, 20);
            var healthPotion = new Potion("Health Potion", "A swirling red liquid.", "A small vial filled with a shimmering red liquid that seems to move on its own.", "On a cluttered shelf, a swirling red Health Potion catches your eye. You pocket it.", 25, new List<string> { "potion", "health potion" });
            var caveKey = new Key("Rusty Key", "An old, rusty key.", "A heavy, ornate key, covered in rust. It feels important.", "Tucked away in a shadowy corner of the fountain's basin, you find a Rusty Key.", 101, new List<string> { "key", "rusty key" });
            var luckyCoin = new LuckyCoin("Lucky Coin", "A small, worn coin that feels warm to the touch.", "A tarnished silver coin with a four-leaf clover on one side. It feels strangely warm.", "After drinking from the fountain, you notice a loose stone at the bottom. Prying it open reveals a hidden Lucky Coin!");
            var amulet = new Treasure("Ancient Amulet", "A beautiful, glowing amulet.", "A heavy gold amulet set with a single, flawless ruby that seems to absorb the light around it.", "With the dragon vanquished, you cautiously approach its hoard and claim the beautiful Ancient Amulet. This must be the treasure!");

            var goblinGuard = new Enemy("Goblin Guard", "A short, brutish creature with dull, hateful eyes. It wields a crude club and wears mismatched pieces of stolen armor.", 30, 1, 5, new List<string> { "goblin", "guard", "goblin guard" });
            var goblinScout = new Enemy("Goblin Scout", "This goblin is leaner and quicker than its friend, with eyes that dart around nervously. It carries a sharp, curved scimitar.", 40, 4, 8, new List<string> { "goblin", "scout", "goblin scout" });
            goblinScout.Loot.Add(dragonslayerBlade);
            goblinScout.DefeatMessage = "As the goblin scout falls, the magnificent blade it was carrying clatters to the floor.";
            goblinScout.WoundedDescription = "You'll never get the boss's treasure!";
            
            var largeRedDragon = new Enemy("Large Red Dragon", "It's a magnificent and terrifying beast, easily the size of a house. Its scales are like red-hot metal, and smoke curls from its nostrils even in its sleep.", 85, 10, 20, new List<string> { "dragon", "red dragon", "large dragon", "large red dragon" });
            largeRedDragon.WoundedDescription = "Insolent gnat! I will burn you to ash!";

            townSquare.Items.Add(caveKey);
            townSquare.Items.Add(luckyCoin);
            armory.Items.Add(sword);
            alchemistShop.Items.Add(healthPotion);
            darkCaveEntrance.Characters.Add(goblinGuard);
            caveInterior.Characters.Add(goblinScout);
            dragonLair.Characters.Add(largeRedDragon);
            dragonLair.Items.Add(amulet);

            townSquare.AddExit("north", new Exit(armory));
            townSquare.AddExit("east", new Exit(alchemistShop));
            townSquare.AddExit("west", new Exit(darkCaveEntrance));
            armory.AddExit("south", new Exit(townSquare));
            alchemistShop.AddExit("west", new Exit(townSquare));
            darkCaveEntrance.AddExit("east", new Exit(townSquare));
            darkCaveEntrance.AddExit("forward", new Exit(caveInterior, isLocked: true, requiredKeyId: 101));
            caveInterior.AddExit("out", new Exit(darkCaveEntrance));
            caveInterior.AddExit("forward", new Exit(dragonLair));
            dragonLair.AddExit("out", new Exit(caveInterior));
            
            StartingLocation = townSquare;
        }
    }
}