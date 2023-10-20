﻿using System;
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
            game.Player1.Party.Add(new MainCharacter(name, null));

            // Add party items here
            game.Player1.Items.Add(ItemType.HealthPotion);
            game.Player1.Items.Add(ItemType.HealthPotion);
            game.Player1.Items.Add(ItemType.HealthPotion);
            game.Player1.Gear.Add(new Dagger());
        }

        // Depending on current round, make relevant monster party.
        public static void MakeMonsterParty(TheFinalBattle game, int round)
        {
            // Clear existing members/items
            game.Player2.Party.Clear();
            game.Player2.Items.Clear();

            // ROUND ONE
            if (round == 1)
            {
                // Add monsters here
                game.Player2.Party.Add(new Skeleton("SKELETON ONE", null));

                // Add items here
                game.Player2.Items.Add(ItemType.HealthPotion);
                game.Player2.Gear.Add(new Dagger());
            }

            // ROUND TWO
            else if (round == 2)
            {
                // Add monsters here
                game.Player2.Party.Add(new Skeleton("SKELETON ONE", null));
                game.Player2.Party.Add(new Skeleton("SKELETON TWO", null));

                // Add items here
                game.Player2.Items.Add(ItemType.HealthPotion);
            }

            // ROUND THREE
            else if (round == 3)
            {
                // Add monsters here
                game.Player2.Party.Add(new Skeleton("SKELETON ONE", null));
                game.Player2.Party.Add(new Skeleton("SKELETON TWO", null));
                game.Player2.Party.Add(new TheUncodedOne("THE UNCODED ONE", null));

                // Add items here
                game.Player2.Items.Add(ItemType.HealthPotion);
            }
            // New rounds go here by adding extra "else if's"
            // Number of rounds must be updated in Settings.cs
        }
    }
}
