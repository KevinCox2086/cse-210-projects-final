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
        private readonly Player _player;
        private Location _currentLocation;
        private bool _isRunning = true;
        private readonly World _world;
        private string _lastActionMessage = "";

        public GameEngine()
        {
            _world = new World();
            _player = new Player("Hero", 100);
            _currentLocation = _world.StartingLocation;
        }

        public void Run()
        {
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

            if (_currentLocation.Items.Any())
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("\n(You notice something on the ground.)");
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
            string verbAndPreposition = commandParts.Length > 1 && commandParts[0] == "talk" && commandParts[1] == "to" ? "talk to" : verb;
            if (verbAndPreposition == "talk to") { Talk(string.Join(" ", commandParts.Skip(2))); return; }
            switch (verb)
            {
                case "quit": _isRunning = false; Console.WriteLine("Thanks for playing!"); break;
                case "look": _lastActionMessage = ""; break;
                case "go": Move(noun); break;
                case "use": Use(noun); break;
                case "attack": Attack(noun); break;
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
            Console.WriteLine($"\n--- COMBAT START: You are fighting the {enemy.Name}! ---");
            Console.ResetColor();
            UI.DrawHealthBar(_player.Name, _player.Health, _player.MaxHealth);
            UI.DrawHealthBar(enemy.Name, enemy.Health, enemy.MaxHealth);
            Thread.Sleep(4000);
            while (_player.IsAlive && enemy.IsAlive)
            {
                int playerDamage = _player.CalculateDamage(out bool playerCrit);
                if (playerCrit) DisplayCriticalHit();
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"\nYou attack the {enemy.Name} for {playerDamage} damage.");
                enemy.TakeDamage(playerDamage);
                Thread.Sleep(1000);
                UI.DrawHealthBar(enemy.Name, enemy.Health, enemy.MaxHealth);
                Thread.Sleep(3000);
                if (!enemy.IsAlive)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\nYou have defeated the {enemy.Name}!");
                    _currentLocation.Characters.Remove(enemy);
                    Console.ResetColor();
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    PerformTimedLoot();
                    break;
                }
                int enemyDamage = enemy.CalculateDamage(out bool enemyCrit);
                if (enemyCrit) DisplayCriticalHit();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nThe {enemy.Name} attacks you for {enemyDamage} damage.");
                _player.TakeDamage(enemyDamage);
                Thread.Sleep(1000);
                UI.DrawHealthBar(_player.Name, _player.Health, _player.MaxHealth);
                Thread.Sleep(3000);
                if (!_player.IsAlive)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("\nYou have been defeated. GAME OVER.");
                    _isRunning = false;
                    Console.ResetColor();
                    Console.WriteLine("\nPress any key to exit.");
                    Console.ReadKey();
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
            if (_currentLocation.Items.Any())
            {
                RedrawGameScreen();
                Thread.Sleep(5000);
                
                var itemsInLocation = new List<Item>(_currentLocation.Items);
                var lootMessages = new StringBuilder();
                foreach (var item in itemsInLocation)
                {
                    _player.AddToInventory(item);
                    _currentLocation.Items.Remove(item);
                    lootMessages.AppendLine(item.DiscoveryMessage);

                    if (item is Treasure)
                    {
                        _lastActionMessage = lootMessages.ToString().TrimEnd();
                        EndGame();
                        return;
                    }
                }
                _lastActionMessage = lootMessages.ToString().TrimEnd();
            }
        }

        private void Use(string itemName)
        {
            if (string.IsNullOrEmpty(itemName)) { _lastActionMessage = "Use what?"; return; }
            var itemToUse = _player.Inventory.FirstOrDefault(i => i.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));
            if (itemToUse == null) { _lastActionMessage = $"You don't have a '{itemName}'."; return; }

            if (itemToUse is Key key)
            {
                var lockedExit = _currentLocation.Exits.FirstOrDefault(e => e.Value.IsLocked && e.Value.RequiredKeyId == key.KeyId);

                if (lockedExit.Value != null)
                {
                    lockedExit.Value.IsLocked = false;
                    _lastActionMessage = $"You use the {key.Name}. With a loud *CLICK*, the way {lockedExit.Key} is now unlocked!";
                }
                else
                {
                    _lastActionMessage = "That key doesn't seem to work on anything here.";
                }
                return;
            }

            if (itemToUse is Potion)
            {
                itemToUse.Use(_player);
                _lastActionMessage = "You feel invigorated!";
            }
            else
            {
                itemToUse.Use(_player);
                _lastActionMessage = $"You use the {itemToUse.Name}.";
            }
        }

        private void DisplayPrompt()
        {
            var suggestions = new List<string>();
            if (_currentLocation.Exits.Any())
            {
                foreach (var exitKey in _currentLocation.Exits.Keys)
                {
                    suggestions.Add($"go {exitKey}");
                }
            }
            var firstEnemy = _currentLocation.Characters.OfType<Enemy>().FirstOrDefault();
            if (firstEnemy != null) { suggestions.Add($"attack {firstEnemy.Name.ToLower()}"); }
            else if (_currentLocation.Characters.Any(c => c is not Player)) { suggestions.Add($"talk to {_currentLocation.Characters.First(c => c is not Player).Name.ToLower()}"); }
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
            Console.WriteLine(@"
    >>-CRITICAL HIT!->>
            ");
            Console.ResetColor();
            Thread.Sleep(1500);
        }

        private void Talk(string characterName)
        {
            if (string.IsNullOrEmpty(characterName)) { _lastActionMessage = "Talk to whom?"; return; }
            var character = _currentLocation.Characters.FirstOrDefault(c => c.Name.Equals(characterName, StringComparison.OrdinalIgnoreCase) && c != _player);
            if (character != null)
            {
                _lastActionMessage = $"You talk to {character.Name}. They say: \"{character.GetDescription()}\"";
                if (character is Enemy) _lastActionMessage += "\nIt growls at you menacingly.";
            }
            else { _lastActionMessage = $"There is no one named '{characterName}' here."; }
        }

        private void ShowInventory()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n--- Inventory ---");
            UI.DrawHealthBar("Your Health", _player.Health, _player.MaxHealth);
            if (_player.Inventory.Any())
            {
                foreach (var item in _player.Inventory)
                {
                    Console.WriteLine($"- {item.Name}: {item.Description}");
                }
            }
            else
            {
                Console.WriteLine("Your inventory is empty.");
            }
            Console.ResetColor();
            Console.WriteLine("\nPress any key to return to the game.");
            Console.ReadKey();
        }

        private void ShowHelp()
        {
            Console.Clear();
            Console.WriteLine("\n--- Available Commands ---");
            Console.WriteLine("go [direction]    - Move to a new location (e.g., 'go north')");
            Console.WriteLine("look              - Redraws the screen, showing the current location.");
            Console.WriteLine("use [item]        - Use an item from your inventory");
            Console.WriteLine("talk to [char]    - Talk to a character (e.g., 'talk to goblin')");
            Console.WriteLine("attack [char]     - Attack a character (e.g., 'attack goblin')");
            Console.WriteLine("inventory (or i)  - Show your inventory and status");
            Console.WriteLine("help              - Show this help message");
            Console.WriteLine("quit              - Exit the game");
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
            var enemy = _currentLocation.Characters.OfType<Enemy>().FirstOrDefault(e => e.Name.Equals(enemyName, StringComparison.OrdinalIgnoreCase));
            if (enemy != null) { StartCombat(enemy); }
            else { _lastActionMessage = $"There is no one named '{enemyName}' to attack here."; }
        }
    }
}