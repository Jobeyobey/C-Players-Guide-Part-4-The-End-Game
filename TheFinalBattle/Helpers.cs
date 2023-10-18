using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheFinalBattleSettings;

namespace TheFinalBattleComponents
{
    public class Helpers
    {
        // Pick an integer between 0 and 'max'.
        // Used for having human/computer pick an action from action/attack/item menus
        public static int PickFromMenu(int max, bool isHuman)
        {
            int choice;

            if (isHuman) // If player is human, prompt them to pick an option
            {
                // Ensure player choice is valid
                while (true)
                {
                    if (int.TryParse(ConsoleHelpReadLine(ConsoleColor.Cyan), out choice))
                    {
                        if (choice >= 0 && choice <= max)
                        {
                            break;
                        }
                    }

                    ConsoleHelpWriteLine("Please pick a valid number from the menu.", ConsoleColor.Red);
                }
            }
            else // If player is not human, pick a random option
            {
                // TODO
                // CREATE WHOLE NEW COMPUTER METHOD TO EXECUTE HERE
                Thread.Sleep(Settings.Delay);

                Random random = new Random();
                choice = random.Next(max) + 1;
            }

            return choice;
        }

        public static void MakeHeroParty(TheFinalBattle game)
        {
            // Get player to input name for MainCharacter
            ConsoleHelpWriteLine("What will the True Programmer's name be?", ConsoleColor.Yellow);
            string? name = ConsoleHelpReadLine(ConsoleColor.Cyan);

            while (true) // Check name is valid
            {
                if (name == null || name == "")
                {
                    ConsoleHelpWriteLine("Please input a valid name.", ConsoleColor.Red);
                    name = ConsoleHelpReadLine(ConsoleColor.Cyan);
                }
                else
                {
                    break;
                }
            }

            // Add extra hero characters here
            game.Player1.Party.Add(new MainCharacter(name));

            // Add party items here
            game.Player1.Items.Add(ItemType.HealthPotion);
            game.Player1.Items.Add(ItemType.HealthPotion);
            game.Player1.Items.Add(ItemType.HealthPotion);
        }

        // Depending on current round, make relevant monster party.
        // Add rounds/monsters here as required
        public static void MakeMonsterParty(TheFinalBattle game, int round)
        {
            game.Player2.Party.Clear(); // Make sure Monster party list is clear

            if (round == 1)
            {
                // Add monsters here
                game.Player2.Party.Add(new Skeleton("SKELETON ONE"));

                // Add items here
                game.Player2.Items.Add(ItemType.HealthPotion);
            }
            else if (round == 2)
            {
                // Add monsters here
                game.Player2.Party.Add(new Skeleton("SKELETON ONE"));
                game.Player2.Party.Add(new Skeleton("SKELETON TWO"));

                // Add items here
                game.Player2.Items.Add(ItemType.HealthPotion);
            }
            else if (round == 3)
            {
                // Add monsters here
                game.Player2.Party.Add(new Skeleton("SKELETON ONE"));
                game.Player2.Party.Add(new Skeleton("SKELETON TWO"));
                game.Player2.Party.Add(new TheUncodedOne("THE UNCODED ONE"));

                // Add items here
                game.Player2.Items.Add(ItemType.HealthPotion);
            }
            // New rounds go here by adding extra "else if's"
            // Number of rounds must be updated in Settings.cs
        }

        public static void ConsoleHelpWrite(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
        }

        public static void ConsoleHelpWriteLine(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
        }

        public static string ConsoleHelpReadLine(ConsoleColor color)
        {
            Console.ForegroundColor = color;
            string? response = Console.ReadLine();
            return response;
        }

        public static void DisplayGameStatus(TheFinalBattle game, Character activeChar)
        {
            Console.WriteLine();

            ConsoleHelpWriteLine("============================================= BATTLE =============================================", ConsoleColor.White);
            foreach (Character character in game.Player1.Party)
            {
                if (character == activeChar)
                    ConsoleHelpWriteLine($"{character.Name, -5} ( {character.CurrentHp}/{character.MaxHp} )", ConsoleColor.Yellow);
                else
                    ConsoleHelpWriteLine($"{character.Name, -5} ( {character.CurrentHp}/{character.MaxHp} )", ConsoleColor.Gray);
            }

            ConsoleHelpWriteLine("----------------------------------------------- VS -----------------------------------------------", ConsoleColor.White);
            foreach (Character character in game.Player2.Party)
            {
                if (character == activeChar)
                    ConsoleHelpWriteLine($"{character.Name, 90} ( {character.CurrentHp}/{character.MaxHp} )", ConsoleColor.Yellow);
                else
                    ConsoleHelpWriteLine($"{character.Name, 90} ( {character.CurrentHp}/{character.MaxHp} )", ConsoleColor.Gray);
            }
            ConsoleHelpWriteLine("==================================================================================================", ConsoleColor.White);
            Console.WriteLine();
        }
    }
}
