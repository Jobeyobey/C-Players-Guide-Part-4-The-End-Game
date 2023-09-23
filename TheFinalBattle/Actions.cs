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

    // ATTACKS
    public class Punch : IAction
    {
        Character ActiveCharacter { get; } // Character activating command

        public Punch(Character character)
        {
            ActiveCharacter = character;
        }

        public void Execute(TheFinalBattle game)
        {
            Console.WriteLine($"{ActiveCharacter.Name} did PUNCH");
        }
    }

    public class BoneCrunch : IAction
    {
        Character ActiveCharacter { get; } // Character activating command

        public BoneCrunch(Character character)
        {
            ActiveCharacter = character;
        }

        public void Execute(TheFinalBattle game)
        {
            Console.WriteLine($"{ActiveCharacter.Name} did BONECRUNCH");
        }
    }

    public enum Action { Nothing, Attack } // Available options to characters
    public enum Attack { Punch, BoneCrunch } // All available attacks in the game
}