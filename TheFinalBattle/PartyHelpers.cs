using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TheFinalBattleComponents.ConsoleHelpers;

namespace TheFinalBattleComponents
{
    internal class PartyHelpers
    {
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
            game.Player1.Party.Add(new MainCharacter(name, new Sword()));
            game.Player1.Party.Add(new VinFletcher(new Bow()));
            game.Player1.Party.Add(new MylaraAndSkorin(new CannonOfConsolas()));

            // Add party items here
            game.Player1.Items.Add(ItemType.HealthPotion);
            game.Player1.Items.Add(ItemType.Bomb);
        }

        // Depending on current round, make relevant monster party.
        public static void MakeMonsterParty(TheFinalBattle game, int round)
        {
            // ROUND ONE
            if (round == 1)
            {
                // Add monsters here
                game.Player2.Party.Add(new Skeleton("SKELETON ONE", new Dagger()));

                // Add items here
                game.Player2.Items.Add(ItemType.HealthPotion);
                game.Player2.Items.Add(ItemType.Bomb);
            }

            // ROUND TWO
            else if (round == 2)
            {
                // Add monsters here
                game.Player2.Party.Add(new Skeleton("SKELETON ONE", null));
                game.Player2.Party.Add(new Skeleton("SKELETON TWO", null));

                // Add items here
                game.Player2.Items.Add(ItemType.HealthPotion);
                game.Player2.Items.Add(ItemType.Bomb);

                // Add gear here
                game.Player2.Gear.Add(new Dagger());
                game.Player2.Gear.Add(new Dagger());
            }

            // ROUND THREE
            else if (round == 3)
            {
                // Add monsters here
                game.Player2.Party.Add(new StoneAmarok("STONE AMAROK ONE", null));
                game.Player2.Party.Add(new StoneAmarok("STONE AMAROK TWO", null));

                // Add items here
                game.Player2.Items.Add(ItemType.Bomb);
            }

            // ROUND FOUR
            else if (round == 4)
            {
                // Add monsters here
                game.Player2.Party.Add(new Skeleton("SKELETON ONE", new Dagger()));
                game.Player2.Party.Add(new Skeleton("SKELETON TWO", new Dagger()));
                game.Player2.Party.Add(new TheUncodedOne("THE UNCODED ONE", null));

                // Add items here
                game.Player2.Items.Add(ItemType.HealthPotion);

                // Add gear here
            }
            // New rounds go here by adding extra "else if's"
            // Number of rounds must be updated in Settings.cs
        }

        public static void LootEnemyParty(TheFinalBattle game)
        {
            // Move all items to Player1's inventory
            foreach (ItemType item in game.Player2.Items)
            {
                game.Player1.Items.Add(item);
            }

            // Move all items to Player1's inventory
            foreach (Gear gear in game.Player2.Gear)
            {
                game.Player1.Gear.Add(gear);
            }

            // Clear Player2's inventory and gear
            game.Player2.Items.Clear();
            game.Player2.Gear.Clear();
        }
    }
}
