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
    public class PunchAction : IAction
    {
        Character ActiveCharacter { get; } // Character activating command
        Character TargetCharacter { get; } // Character being targeted

        public PunchAction(Character character, Character target)
        {
            ActiveCharacter = character;
            TargetCharacter = target;
        }

        public void Execute(TheFinalBattle game)
        {
            Console.WriteLine($"{ActiveCharacter.Name} did PUNCH on {TargetCharacter.Name}");
        }
    }

    public class BoneCrunchAction : IAction
    {
        Character ActiveCharacter { get; } // Character activating command
        Character TargetCharacter { get; } // Character being targeted

        public BoneCrunchAction(Character character, Character target)
        {
            ActiveCharacter = character;
            TargetCharacter = target;
        }

        public void Execute(TheFinalBattle game)
        {
            Console.WriteLine($"{ActiveCharacter.Name} did BONECRUNCH on {TargetCharacter.Name}");
        }
    }

    public enum Action { Nothing, Attack } // Available options to characters
    public enum Attack { Punch, BoneCrunch } // All available attacks in the game
}