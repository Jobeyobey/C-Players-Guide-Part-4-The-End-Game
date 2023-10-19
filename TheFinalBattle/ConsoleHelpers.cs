using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheFinalBattleComponents;

namespace TheFinalBattleComponents
{
    internal class ConsoleHelpers
    {
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
                    ConsoleHelpWriteLine($"{character.Name,-5} ( {character.CurrentHp}/{character.MaxHp} )", ConsoleColor.Yellow);
                else
                    ConsoleHelpWriteLine($"{character.Name,-5} ( {character.CurrentHp}/{character.MaxHp} )", ConsoleColor.Gray);
            }

            ConsoleHelpWriteLine("----------------------------------------------- VS -----------------------------------------------", ConsoleColor.White);
            foreach (Character character in game.Player2.Party)
            {
                if (character == activeChar)
                    ConsoleHelpWriteLine($"{character.Name,90} ( {character.CurrentHp}/{character.MaxHp} )", ConsoleColor.Yellow);
                else
                    ConsoleHelpWriteLine($"{character.Name,90} ( {character.CurrentHp}/{character.MaxHp} )", ConsoleColor.Gray);
            }
            ConsoleHelpWriteLine("==================================================================================================", ConsoleColor.White);
            Console.WriteLine();
        }
    }
}
