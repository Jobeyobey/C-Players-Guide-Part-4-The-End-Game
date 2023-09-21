using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheFinalBattleComponents;

namespace TheFinalBattleComponents
{
    public interface ICommand
    {
        void Execute(TheFinalBattle game);
    }

    public class Command : ICommand
    {
        Character ActiveCharacter { get; }

        public void Execute(TheFinalBattle game)
        {
            Console.WriteLine($"It is {ActiveCharacter.Name}'s turn...");
            ActiveCharacter.SkipTurn();
        }

        public Command(Character character)
        {
            ActiveCharacter = character;
        }
    }
}
