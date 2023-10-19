using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TheFinalBattleComponents.Helpers;
using static TheFinalBattleComponents.ConsoleHelpers;

// This class contains the settings for different aspects of the game, for easy balancing and testing
namespace TheFinalBattleSettings
{
    public static class Settings
    {
        // Game Settings
        public static int NumRounds { get; } = 3;
        public static int Delay { get; } = 500; // Introduces slight delay between computer actions just to make game easier to read

        // Attack Settings
        public static int PunchDamage { get; } = 1;
        public static int BoneCrunchDamage { get; } = 1;
        public static int UnravelingDamage { get; } = 2;

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
