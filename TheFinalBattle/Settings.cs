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
        public static int Delay { get; } = 500;

        // Attack Settings
        public static int PunchDamage { get; } = 3;
        public static int BoneCrunchDamage { get; } = 1;
        public static int UnravelingDamage { get; } = 2;

        // Game Setup
        public static int SetupGame()
        {
            Console.WriteLine("Choose game mode:");
            Console.WriteLine("1 - Human vs Human");
            Console.WriteLine("2 - Human vs Computer");
            Console.WriteLine("3 - Computer vs Computer");

            int playerChoice = Convert.ToInt32(Console.ReadLine());

            while (playerChoice != 1 && playerChoice != 2 && playerChoice != 3)
            {
                Console.WriteLine("Please pick one of the 3 options by pressing the correspending key.");
                playerChoice = Console.Read();
            }

            return playerChoice;
        }
    }
}
