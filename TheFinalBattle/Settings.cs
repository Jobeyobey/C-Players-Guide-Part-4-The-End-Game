using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// This class contains the settings for different aspects of the game, for easy balancing and testing
namespace TheFinalBattleSettings
{
    public static class Settings
    {
        // Game Settings
        public static int NumRounds { get; } = 3;
        public static int Delay { get; } = 1000; // Introduces slight delay between computer actions just to make game easier to read

        // Attack Settings
        public static int PunchDamage { get; } = 1;
        public static int BoneCrunchDamage { get; } = 1;
        public static int UnravelingDamage { get; } = 2;

        // Game Setup
        public static int SetupGame()
        {
            Console.WriteLine("Choose game mode:");
            Console.WriteLine("1 - Human vs Human");
            Console.WriteLine("2 - Human vs Computer");
            Console.WriteLine("3 - Computer vs Computer");

            // Ensure player choice is valid
            int playerChoice;
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out playerChoice))
                {
                    if (playerChoice > 0 && playerChoice < 4)
                    {
                        break;
                    }
                }

                Console.WriteLine("Please pick a valid number from the menu.");
            }

            return playerChoice;
        }
    }
}
