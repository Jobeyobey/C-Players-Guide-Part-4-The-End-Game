using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheFinalBattleComponents;

namespace TheFinalBattleComponents
{
    public interface IAction
    {
        void Execute(TheFinalBattle game);
    }

    public class NothingAction : IAction
    {
        Character ActiveCharacter { get; } // Character activating command

        public NothingAction(Character character) // Constructor
        {
            ActiveCharacter = character;
        }

        public void Execute(TheFinalBattle game)
        {
            Console.WriteLine($"{ActiveCharacter.Name} did NOTHING");
        }
    }

    public class TestAction : IAction
    {
        Character ActiveCharacter { get; } // Character activating command

        public TestAction(Character character)
        {
            ActiveCharacter = character;
        }

        public void Execute(TheFinalBattle game)
        {
            Console.WriteLine($"{ActiveCharacter.Name} did TESTACTION");
        }
    }

    public enum Action { Nothing, TestAction } // All available actions in the game
}