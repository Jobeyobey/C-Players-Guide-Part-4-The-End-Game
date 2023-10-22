using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheFinalBattleComponents;
using TheFinalBattleSettings;

namespace TheFinalBattleComponents
{
    internal class ConsoleHelpers
    {
        // Console.Write with custom colour
        public static void ConsoleHelpWrite(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
        }

        // Console.WriteLine with custom colour
        public static void ConsoleHelpWriteLine(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
        }

        // Console.ReadLine with custom colour
        public static string ConsoleHelpReadLine(ConsoleColor color)
        {
            Console.ForegroundColor = color;
            string? response = Console.ReadLine();
            return response;
        }

        // Display current names, health etc. of both parties. Active player is highlighted in yellow.
        public static void DisplayGameStatus(TheFinalBattle game, Character activeChar)
        {
            Console.WriteLine();

            Thread.Sleep(Settings.Delay / 3);
            ConsoleHelpWriteLine("============================================= BATTLE =============================================", ConsoleColor.White);
            foreach (Character character in game.Player1.Party)
            {
                Thread.Sleep(Settings.Delay / 3);
                string charStatus = GetCharacterStatus(character);

                if (character == activeChar)
                    ConsoleHelpWriteLine($"{charStatus}", ConsoleColor.Yellow);
                else
                    ConsoleHelpWriteLine($"{charStatus}", ConsoleColor.Gray);
            }

            Thread.Sleep(Settings.Delay / 3);
            ConsoleHelpWriteLine("----------------------------------------------- VS -----------------------------------------------", ConsoleColor.White);
            foreach (Character character in game.Player2.Party)
            {
                Thread.Sleep(Settings.Delay / 3);
                string charStatus = GetCharacterStatus(character);

                if (character == activeChar)
                    ConsoleHelpWriteLine($"{charStatus,98}", ConsoleColor.Yellow);
                else
                    ConsoleHelpWriteLine($"{charStatus,98}", ConsoleColor.Gray);
            }

            Thread.Sleep(Settings.Delay / 3);
            ConsoleHelpWriteLine("==================================================================================================", ConsoleColor.White);
            Console.WriteLine();
        }

        // Create a string for displaying character status
        public static string GetCharacterStatus(Character character)
        {
            string charStatus = $"{character.Name} ({character.CurrentHp}/{character.MaxHp})";

            // If character has gear equipped, add to string
            if (character.Equipped != null)
            {
                charStatus += $" Gear: {character.Equipped.Name}";
            }

            return charStatus;
        }
    }
}
