using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TheFinalBattleComponents;
using TheFinalBattleSettings;
using static System.Net.Mime.MediaTypeNames;

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
    public abstract class Attack : IAction
    {
        public abstract string Name { get; init; }
        public Character ActiveChar { get; init; }
        public Character TargetChar { get; init; }
        public abstract int Damage { get; init; }
        public Attack() { }
        public abstract void Execute(TheFinalBattle game);
    }

    public class Punch : Attack
    {
        public override string Name { get; init; } = "PUNCH";
        public override int Damage { get; init; } = Settings.PunchDamage;
        public Punch(Character activeChar, Character targetChar)
        {
            ActiveChar = activeChar;
            TargetChar = targetChar;
        }
        public override void Execute(TheFinalBattle game)
        {
            AttackHelper.DoDamage(this);
        }
    }

    public class BoneCrunch : Attack
    {
        public override string Name { get; init; } = "BONECRUNCH";
        public override int Damage { get; init; }

        public BoneCrunch(Character activeChar, Character targetChar)
        {
            ActiveChar = activeChar;
            TargetChar = targetChar;

            // Randomly set damage. '+1' to BoneCrunchDamage because allowing 0-1 damage would require "random.Next(2);"
            Random random = new ();
            Damage = random.Next(Settings.BoneCrunchDamage + 1);
        }

        public override void Execute(TheFinalBattle game)
        {
            AttackHelper.DoDamage(this);
        }
    }

    public class Unraveling : Attack
    {
        public override string Name { get; init; } = "UNRAVELING";
        public override int Damage { get; init; } = Settings.UnravelingDamage;

        public Unraveling(Character activeChar, Character targetChar)
        {
            ActiveChar = activeChar;
            TargetChar = targetChar;

            // Randomly set damage. '+1' to UnravelingDamage because allowing 0-1 damage would require "random.Next(2);"
            Random random = new();
            Damage = random.Next(Settings.UnravelingDamage + 1);
        }

        public override void Execute(TheFinalBattle game)
        {
            AttackHelper.DoDamage(this);
        }
    }

    // Attack helper will contain methods to help calculate attacks for all attack types
    internal static class AttackHelper
    {
        public static void DoDamage(Attack attack)
        {
            // Temporary variables for easier readability
            string attackName = attack.Name;
            Character activeChar = attack.ActiveChar;
            Character targetChar = attack.TargetChar;

            // Announce attack and damage
            Console.WriteLine($"{activeChar.Name} did {attackName} on {targetChar.Name}");
            Console.WriteLine($"{attackName} dealt {attack.Damage} to {targetChar.Name}");

            // Damage target and report new health status
            targetChar.AlterHp(-attack.Damage);
            Console.WriteLine($"{targetChar.Name} has {attack.TargetChar.CurrentHp}/{attack.TargetChar.MaxHp} HP");
        }
    }

    public enum ActionType { Nothing, Attack } // Available options to characters
    public enum AttackType { Punch, BoneCrunch, Unraveling } // All available attacks in the game
}