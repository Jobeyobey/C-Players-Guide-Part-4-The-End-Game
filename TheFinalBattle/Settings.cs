﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TheFinalBattleComponents.MenuHelpers;
using static TheFinalBattleComponents.ConsoleHelpers;

// This class contains the settings for different aspects of the game, for easy balancing and testing
namespace TheFinalBattleSettings
{
    public static class Settings
    {
        // Game Settings
        public static int NumRounds { get; } = 5;
        public static int Delay { get; } = 400; // Introduces slight delay between computer actions just to make game easier to read

        // Standard Attack Settings
        public static int PunchDamage { get; } = 1;
        public static int PunchAccuracy { get; } = 100;
        public static int PileOnDamage { get; } = 1;
        public static int PileOnAccuracy { get; } = 40;
        public static int QuickShotDamage { get; } = 3;
        public static int QuickShotAccuracy { get; } = 50;
        public static int CannonOfConsolasDamage { get; } = 1;
        public static int CannonOfConsolasAccuracy { get; } = 95;
        public static int BoneCrunchDamage { get; } = 1;
        public static int BoneCrunchAccuracy { get; } = 100;
        public static int BiteDamage { get; } = 1;
        public static int BiteAccuracy { get; } = 100;
        public static int UnravelingDamage { get; } = 6;
        public static int UnravelingAccuracy { get; } = 100;

        // Gear Attack Settings
        public static int SwordDamage { get; } = 2;
        public static int SwordAccuracy { get; } = 90;
        public static int DaggerDamage { get; } = 1;
        public static int DaggerAccuracy { get; } = 90;
        public static int BowDamage { get; } = 3;
        public static int BowAccuracy { get; } = 50;
        public static int TomeDamage { get; } = 1;
        public static int CurseDamage { get; } = 2;
        public static int TomeAccuracy { get; } = 90;

        // Item Settings
        public static int HealthPotionStrength { get; } = 8;
        public static int BombDamage { get; } = 4;

        // Game Setup
        public static int SetupGame()
        {
            ConsoleHelpWriteLine("Choose game mode:", ConsoleColor.Yellow);
            ConsoleHelpWriteLine("1 - Human vs Human", ConsoleColor.White);
            ConsoleHelpWriteLine("2 - Human vs Computer", ConsoleColor.White);
            ConsoleHelpWriteLine("3 - Computer vs Computer", ConsoleColor.White);

            // Ensure player choice is valid
            int playerChoice;
            while (true)
            {
                if (int.TryParse(ConsoleHelpReadLine(ConsoleColor.Cyan), out playerChoice))
                {
                    if (playerChoice > 0 && playerChoice < 4)
                    {
                        break;
                    }
                }

                ConsoleHelpWriteLine("Please pick a valid number from the menu.", ConsoleColor.Red);
            }

            return playerChoice;
        }
    }
}
