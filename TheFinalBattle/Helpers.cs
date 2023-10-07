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
        // Used for having human/computer pick an item from action/attack menus etc.
        public static int PickFromMenu(int max, bool isHuman)
        {
            int choice;

            if (isHuman)
            {
                choice = Convert.ToInt32(Console.ReadLine());

                while (true)
                {
                    if (choice > 0 && choice <= max)
                    {
                        break;
                    }
                    Console.WriteLine("Please a number from the available choices.");
                    choice = Convert.ToInt32(Console.ReadLine());
                }
            }
            else
            {
                Thread.Sleep(Settings.Delay);

                Random random = new Random();
                choice = random.Next(max) + 1;
            }

            return choice - 1;
        }

        public static void MakeHeroParty(TheFinalBattle game)
        {
            // Get player to input name for MainCharacter
            Console.WriteLine("What will the True Programmer's name be? ");
            string? name = Console.ReadLine();

            while (true) // Check name is valid
            {
                if (name == null || name == "")
                {
                    Console.WriteLine("Please input a valid name. ");
                    name = Console.ReadLine();
                }
                else
                {
                    break;
                }
            }

            game.Player1.Party.Add(new MainCharacter(name));
            // Add extra hero characters here
        }

        // Depending on current round, make relevant monster party.
        // Add rounds/monsters here as required
        public static void MakeMonsterParty(TheFinalBattle game, int round)
        {
            game.Player2.Party.Clear(); // Make sure Monster party list is clear

            if (round == 1)
            {
                game.Player2.Party.Add(new Skeleton("SKELETON ONE"));
            }
            else if (round == 2)
            {
                game.Player2.Party.Add(new Skeleton("SKELETON ONE"));
                game.Player2.Party.Add(new Skeleton("SKELETON TWO"));
            }
            else if (round == 3)
            {
                game.Player2.Party.Add(new Skeleton("SKELETON ONE"));
                game.Player2.Party.Add(new Skeleton("SKELETON TWO"));
                game.Player2.Party.Add(new TheUncodedOne("THE UNCODED ONE"));
            }
            // New rounds go here by adding extra "else if's"
            // Number of rounds must be updated in Settings.cs
        }
    }
}
