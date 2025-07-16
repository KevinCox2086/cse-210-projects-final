using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using AdventureGame.Character;
using AdventureGame.Core;
using AdventureGame.Items;

namespace AdventureGame
{
    public class GameEngine
    {
        private Player _player;
        private Location _currentLocation;
        private bool _isRunning = true;
        private readonly World _world;
        private string _lastActionMessage = "";
        private readonly Random _random = new Random();
        private bool _hasDrankFromFountain = false;

        public GameEngine()
        {
            _world = new World();
        }

        public void Run()
        {
            string playerName = ShowIntroScreen();
            _player = new Player(playerName, 100);
            _currentLocation = _world.StartingLocation;
            
            PerformTimedLoot();

            while (_isRunning)
            {
                RedrawGameScreen();
                string command = Console.ReadLine();
                if (string.IsNullOrEmpty(command))
                {
                    _lastActionMessage = "";
                    continue;
                }

                string[] commandParts = Parser.Parse(command);
                ProcessCommand(commandParts);
            }
        }

        private string ShowIntroScreen()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(@"
    ================================================================
    |                                                              |
    |                  Welcome to the C# Adventure!                |
    |                                                              |
    ================================================================
            ");
            Console.ResetColor();
            Console.WriteLine("\n\nA foul corruption has spread from the nearby caves, and the town needs a hero.");
            Console.WriteLine("Whispers in the tavern speak of goblins, a fearsome dragon, and a lost artifact of great power.");
            Console.WriteLine("\nYour goal is to venture into the caves, defeat the dragon, and recover the legendary Ancient Amulet.");
            Console.WriteLine("\n--- How to Play ---");
            Console.WriteLine("Type commands composed of a verb and sometimes a noun (e.g., 'go north', 'use potion').");
            Console.WriteLine("Items in new areas will be picked up automatically after a brief pause.");
            Console.WriteLine("Check your status and items at any time by typing 'inventory' or 'i'.");
            Console.WriteLine("The game will suggest possible actions at the bottom of the screen.");
            
            Console.WriteLine("\nWhat is your name, adventurer?");
            Console.Write("> ");
            string playerName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(playerName))
            {
                playerName = "Hero";
            }

            Console.WriteLine($"\nGood luck, {playerName}!");
            Console.WriteLine("\nPress any key to begin your quest...");
            Console.ReadKey();
            return playerName;
        }

        private void RedrawGameScreen()
        {
            Console.Clear();
            Map.Draw(_world.AllLocations, _currentLocation, _player);
            
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"--- {_currentLocation.Name} ---");
            Console.ResetColor();

            if (_currentLocation.HasBeenVisited)
            {
                Console.WriteLine(_currentLocation.DescriptionAfterLoot);
            }
            else
            {
                Console.WriteLine(_currentLocation.Description);
            }

            if (_currentLocation.Items.Any(i => !i.IsSecret) && !string.IsNullOrEmpty(_currentLocation.ItemHint))
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine($"\n{_currentLocation.ItemHint}");
                Console.ResetColor();
            }

            if (_currentLocation.Characters.Any())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nCharacters present:");
                foreach (var character in _currentLocation.Characters) Console.WriteLine($"- {character.Name}");
                Console.ResetColor();
            }

            if (!string.IsNullOrEmpty(_lastActionMessage))
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"\n{_lastActionMessage}");
                Console.ResetColor();
                _lastActionMessage = ""; 
            }

            DisplayPrompt();
            Console.Write("> ");
        }

        private void ProcessCommand(string[] commandParts)
        {
            string verb = commandParts[0];
            string noun = commandParts.Length > 1 ? string.Join(" ", commandParts.Skip(1)) : "";
            string fullCommand = string.Join(" ", commandParts);

            if (fullCommand == "drink from fountain" || fullCommand == "drink fountain") { DrinkFromFountain(); return; }
            if (fullCommand.StartsWith("look at")) { LookAt(string.Join(" ", commandParts.Skip(2))); return; }

            string verbAndPreposition = commandParts.Length > 1 && commandParts[0] == "talk" && commandParts[1] == "to" ? "talk to" : verb;
            if (verbAndPreposition == "talk to") { Talk(string.Join(" ", commandParts.Skip(2))); return; }
            switch (verb)
            {
                case "quit": case "exit": _isRunning = false; Console.WriteLine("Thanks for playing!"); break;
                case "look": case "examine": _lastActionMessage = ""; break;
                case "go": case "move": Move(noun); break;
                case "use": Use(noun); break;
                case "attack": case "fight": Attack(noun); break;
                case "inventory": case "i": ShowInventory(); break;
                case "map": _lastActionMessage = "The map is always visible at the top of the screen."; break;
                case "help": ShowHelp(); break;
                default: _lastActionMessage = "I don't understand that command."; break;
            }
        }

        private void StartCombat(Enemy enemy)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"\n--- COMBAT START ---");
            Console.ResetColor();
            
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"The {enemy.Name} notices you and shouts, \"{enemy.GetDescription()}\"");
            Console.ResetColor();
            
            Thread.Sleep(2000);

            UI.DrawHealthBar(_player.Name, _player.Health, _player.MaxHealth);
            UI.DrawHealthBar(enemy.Name, enemy.Health, enemy.MaxHealth);
            Thread.Sleep(4000);
            
            var playerAttackVerbs = new List<string> { "You attack", "You swing at", "You lunge towards", "You strike" };
            var enemyAttackVerbs = new List<string> { $"The {enemy.Name} attacks", $"The {enemy.Name} lunges at", $"The {enemy.Name} swings at" };
            var missVerbs = new List<string> { "and miss", "but your attack goes wide", "but it dodges out of the way" };

            while (_player.IsAlive && enemy.IsAlive)
            {
                if (_random.Next(1, 101) <= 15)
                {
                    Console.WriteLine($"\nYou swing wildly {missVerbs[_random.Next(missVerbs.Count)]}!");
                    Sound.PlayMiss();
                }
                else
                {
                    int playerDamage = _player.CalculateDamage(out bool playerCrit);
                    string attackVerb = playerAttackVerbs[_random.Next(playerAttackVerbs.Count)];
                    
                    Console.ForegroundColor = ConsoleColor.White;
                    if (playerCrit)
                    {
                        DisplayCriticalHit();
                        Console.WriteLine($"A devastating blow! {attackVerb} the {enemy.Name} for {playerDamage} damage!");
                    }
                    else
                    {
                        double enemyHealthPercent = (double)enemy.Health / enemy.MaxHealth;
                        if (enemyHealthPercent > 0.7) { Console.WriteLine($"\n{attackVerb} the {enemy.Name}, but your blow barely scratches its tough hide, dealing {playerDamage} damage."); }
                        else if (enemyHealthPercent > 0.3) { Console.WriteLine($"\n{attackVerb} the {enemy.Name}, landing a solid blow for {playerDamage} damage."); }
                        else { Console.WriteLine($"\nThe {enemy.Name} stumbles, wounded. {attackVerb}, striking a weak spot for {playerDamage} damage!"); }
                        Sound.PlayPlayerAttack();
                    }
                    
                    enemy.TakeDamage(playerDamage);
                }
                
                Thread.Sleep(1000);
                UI.DrawHealthBar(enemy.Name, enemy.Health, enemy.MaxHealth);
                Thread.Sleep(3000);

                if (!enemy.IsAlive)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\nYou have defeated the {enemy.Name}!");
                    if (!string.IsNullOrEmpty(enemy.DefeatMessage)) { Console.ForegroundColor = ConsoleColor.Gray; Console.WriteLine(enemy.DefeatMessage); Console.ResetColor(); }
                    if (enemy.Loot.Any()) { _currentLocation.Items.AddRange(enemy.Loot); }
                    _currentLocation.Characters.Remove(enemy);
                    Console.ResetColor();
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    PerformTimedLoot();
                    break;
                }

                if (_random.Next(1, 101) <= 15)
                {
                    Console.WriteLine($"\nThe {enemy.Name} lunges and misses you!");
                    Sound.PlayMiss();
                }
                else
                {
                    int enemyDamage = enemy.CalculateDamage(out bool enemyCrit);
                    string attackVerb = enemyAttackVerbs[_random.Next(enemyAttackVerbs.Count)];

                    Console.ForegroundColor = ConsoleColor.Red;
                    if (enemyCrit)
                    {
                        DisplayCriticalHit();
                        Console.WriteLine($"A devastating blow! {attackVerb} you for {enemyDamage} damage!");
                    }
                    else
                    {
                        Console.WriteLine($"\n{attackVerb} you for {enemyDamage} damage.");
                        Sound.PlayEnemyAttack();
                    }

                    _player.TakeDamage(enemyDamage);
                }
                
                Thread.Sleep(1000);
                UI.DrawHealthBar(_player.Name, _player.Health, _player.MaxHealth);
                Thread.Sleep(3000);

                if (!_player.IsAlive)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("\nYour vision fades to black...");
                    Console.WriteLine("The legend of the Ancient Amulet remains a mystery, and the town's hope fades with you.");
                    _isRunning = false;
                    Console.ResetColor();
                    Console.WriteLine("\n--- GAME OVER ---");
                    Console.WriteLine("\nPress any key to exit.");
                    Console.ReadKey();
                    return;
                }
                Console.ResetColor();
            }
            Console.WriteLine("--- COMBAT END ---");
        }

        private void Move(string direction)
        {
            var enemy = _currentLocation.Characters.OfType<Enemy>().FirstOrDefault();
            if (enemy != null)
            {
                _lastActionMessage = $"The {enemy.Name} blocks your path! You can't leave.";
                return;
            }

            if (string.IsNullOrEmpty(direction))
            {
                _lastActionMessage = "Go where?";
                return;
            }

            if (_currentLocation.Exits.TryGetValue(direction, out Exit exit))
            {
                if (exit.IsLocked)
                {
                    _lastActionMessage = "The way is locked.";
                }
                else
                {
                    _currentLocation.HasBeenVisited = true;
                    _currentLocation = exit.Destination;
                    _lastActionMessage = "";
                    
                    if (!_currentLocation.HasBeenVisited && !string.IsNullOrEmpty(_currentLocation.EntryMessage))
                    {
                        RedrawGameScreen();
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine($"\n{_currentLocation.EntryMessage}");
                        Console.ResetColor();
                        Thread.Sleep(5000);
                    }
                    
                    PerformTimedLoot();
                }
            }
            else
            {
                _lastActionMessage = $"You can't go {direction}.";
            }
        }

        private void PerformTimedLoot()
        {
            if (_currentLocation.Items.Any(i => !i.IsSecret) && !_currentLocation.Characters.OfType<Enemy>().Any())
            {
                RedrawGameScreen();
                Thread.Sleep(5000);
                
                var itemsInLocation = _currentLocation.Items.Where(i => !i.IsSecret).ToList();
                var lootMessages = new StringBuilder();
                foreach (var item in itemsInLocation)
                {
                    var oldBestWeapon = _player.Inventory.OfType<Weapon>().OrderByDescending(w => w.MaxDamage).FirstOrDefault();
                    _player.AddToInventory(item);
                    _currentLocation.Items.Remove(item);
                    lootMessages.AppendLine(item.DiscoveryMessage);
                    Sound.PlayItemGet();
                    if (item is Weapon newWeapon)
                    {
                        if (oldBestWeapon != null)
                        {
                            if (newWeapon.MaxDamage > oldBestWeapon.MaxDamage) { lootMessages.AppendLine($"You equip the {newWeapon.Name}, discarding your old {oldBestWeapon.Name}. It's a clear upgrade!"); }
                            else { lootMessages.AppendLine($"You stow the {newWeapon.Name}, as your currently equipped weapon is better."); }
                        }
                    }
                    if (item is Treasure) { _lastActionMessage = lootMessages.ToString().TrimEnd(); EndGame(); return; }
                }
                _lastActionMessage = lootMessages.ToString().TrimEnd();
            }
        }

        private void Use(string itemName)
        {
            if (string.IsNullOrEmpty(itemName)) { _lastActionMessage = "Use what?"; return; }
            var itemToUse = _player.Inventory.FirstOrDefault(i => i.Keywords.Contains(itemName));
            if (itemToUse != null)
            {
                if (itemToUse is Key key)
                {
                    var lockedExit = _currentLocation.Exits.FirstOrDefault(e => e.Value.IsLocked && e.Value.RequiredKeyId == key.KeyId);
                    if (lockedExit.Value != null) { lockedExit.Value.IsLocked = false; _lastActionMessage = $"You use the {key.Name}. With a loud *CLICK*, the way {lockedExit.Key} is now unlocked!"; Sound.PlayUnlockDoor(); }
                    else { _lastActionMessage = "That key doesn't seem to work on anything here."; }
                    return;
                }
                _lastActionMessage = itemToUse.Use(_player);
            }
            else { _lastActionMessage = $"You don't have a '{itemName}' in your inventory."; }
        }

        private void DrinkFromFountain()
        {
            if (_currentLocation.Name != "Town Square") { _lastActionMessage = "There is no fountain here."; return; }
            if (_hasDrankFromFountain) { _lastActionMessage = "You drink from the fountain again, but nothing else happens."; return; }
            _hasDrankFromFountain = true;
            int healthRestored = _player.Heal(5);
            var message = new StringBuilder($"The water is cool and refreshing. You restore {healthRestored} health.");
            var luckyCoin = _currentLocation.Items.OfType<LuckyCoin>().FirstOrDefault();
            if (luckyCoin != null)
            {
                _player.AddToInventory(luckyCoin);
                _currentLocation.Items.Remove(luckyCoin);
                message.AppendLine($"\n{luckyCoin.DiscoveryMessage}");
                Sound.PlayItemGet();
            }
            _lastActionMessage = message.ToString();
        }

        private void LookAt(string targetName)
        {
            if (string.IsNullOrEmpty(targetName)) { _lastActionMessage = "Look at what?"; return; }

            var itemInInventory = _player.Inventory.FirstOrDefault(i => i.Keywords.Contains(targetName));
            if (itemInInventory != null) { _lastActionMessage = itemInInventory.DetailedDescription; return; }

            var itemInRoom = _currentLocation.Items.FirstOrDefault(i => i.Keywords.Contains(targetName));
            if (itemInRoom != null) { _lastActionMessage = itemInRoom.DetailedDescription; return; }

            var characterInRoom = _currentLocation.Characters.FirstOrDefault(c => c.Keywords.Contains(targetName));
            if (characterInRoom != null) { _lastActionMessage = characterInRoom.DetailedDescription; return; }
            
            _lastActionMessage = $"You don't see any '{targetName}' here.";
        }

        private void DisplayPrompt()
        {
            var suggestions = new List<string>();
            if (_currentLocation.Exits.Any()) { foreach (var exitKey in _currentLocation.Exits.Keys) { suggestions.Add($"go {exitKey}"); } }
            var firstEnemy = _currentLocation.Characters.OfType<Enemy>().FirstOrDefault();
            if (firstEnemy != null) { suggestions.Add($"attack {firstEnemy.Name.ToLower()}"); }
            else if (_currentLocation.Characters.Any(c => c is not Player)) { suggestions.Add($"talk to {_currentLocation.Characters.First(c => c is not Player).Name.ToLower()}"); }
            if (_currentLocation.Name == "Town Square" && !_hasDrankFromFountain) { suggestions.Add("drink fountain"); }
            suggestions.Add("help");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write($"\n(Try: {string.Join(", ", suggestions)})");
            Console.ResetColor();
            Console.WriteLine();
        }

        private void DisplayCriticalHit()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Sound.PlayCriticalHit();
            Console.WriteLine(@"
    >>-CRITICAL HIT!->>
            ");
            Console.ResetColor();
            Thread.Sleep(1500);
        }

        private void Talk(string characterName)
        {
            if (string.IsNullOrEmpty(characterName)) { _lastActionMessage = "Talk to whom?"; return; }
            var characterToTalkTo = _currentLocation.Characters.Where(c => c != _player).FirstOrDefault(c => c.Keywords.Contains(characterName));
            if (characterToTalkTo != null) { _lastActionMessage = $"You talk to {characterToTalkTo.Name}. They say: \"{characterToTalkTo.GetDescription()}\""; }
            else { _lastActionMessage = $"You don't see anyone named '{characterName}' here."; }
        }

        private void ShowInventory()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n--- Inventory ---");
            UI.DrawHealthBar(_player.Name, _player.Health, _player.MaxHealth);
            var bestWeapon = _player.Inventory.OfType<Weapon>().OrderByDescending(w => w.MaxDamage).FirstOrDefault();
            if (_player.Inventory.Any())
            {
                foreach (var item in _player.Inventory)
                {
                    string equippedText = (item == bestWeapon) ? " (Equipped)" : "";
                    Console.WriteLine($"- {item.Name}{equippedText}: {item.Description}");
                }
            }
            else { Console.WriteLine("Your inventory is empty."); }
            Console.ResetColor();
            Console.WriteLine("\nPress any key to return to the game.");
            Console.ReadKey();
        }

        private void ShowHelp()
        {
            Console.Clear();
            Console.WriteLine("\n--- Available Commands ---");
            Console.WriteLine("go / move [direction]    - Move to a new location (e.g., 'go north')");
            Console.WriteLine("look / examine           - Redraws the screen, showing the current location.");
            Console.WriteLine("look at [target]         - Get a detailed description of an item or character.");
            Console.WriteLine("use [item]               - Use an item from your inventory");
            Console.WriteLine("talk to [char]           - Talk to a character (e.g., 'talk to goblin')");
            Console.WriteLine("attack / fight [char]    - Attack a character (e.g., 'attack goblin')");
            Console.WriteLine("inventory / i            - Show your inventory and status");
            Console.WriteLine("help                     - Show this help message");
            Console.WriteLine("quit / exit              - Exit the game");
            Console.WriteLine("\nPress any key to return to the game.");
            Console.ReadKey();
        }

        private void EndGame()
        {
            RedrawGameScreen();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n*******************************************************************");
            Console.WriteLine("\nCongratulations!");
            Console.WriteLine("You have recovered the Ancient Amulet and cleared the goblin menace.");
            Console.WriteLine("Your adventure is complete!");
            Console.WriteLine("\n*******************************************************************");
            Console.ResetColor();
            _isRunning = false;
            Console.WriteLine("\nPress any key to exit.");
            Console.ReadKey();
        }

        private void Attack(string enemyName)
        {
            if (string.IsNullOrEmpty(enemyName)) { _lastActionMessage = "Attack whom?"; return; }
            var enemyToAttack = _currentLocation.Characters.OfType<Enemy>().FirstOrDefault(e => e.Keywords.Contains(enemyName));
            if (enemyToAttack != null)
            {
                if (enemyToAttack.Name == "Large Red Dragon" && _player.Health < 50 && _player.Inventory.OfType<Potion>().Any())
                {
                    _lastActionMessage = "The dragon looks incredibly powerful. This might be a good time to use a health potion before attacking.";
                    return;
                }
                StartCombat(enemyToAttack);
            }
            else { _lastActionMessage = $"You don't see anyone named '{enemyName}' here to attack."; }
        }
    }
}