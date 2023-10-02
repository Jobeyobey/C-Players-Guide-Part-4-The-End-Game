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
    }
}
