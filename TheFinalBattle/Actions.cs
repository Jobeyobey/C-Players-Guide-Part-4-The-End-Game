using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TheFinalBattleComponents;
using static TheFinalBattleComponents.Helpers;
using static TheFinalBattleComponents.ConsoleHelpers;
using TheFinalBattleSettings;
using System.Collections;

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
            ConsoleHelpWriteLine($"{ActiveCharacter.Name} did NOTHING", ConsoleColor.Gray);
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
        public override string Name { get; init; } = "Punch";
        public override int Damage { get; init; } = Settings.PunchDamage;
        public Punch(Character activeChar, Character targetChar)
        {
            ActiveChar = activeChar;
            TargetChar = targetChar;
        }
        public override void Execute(TheFinalBattle game)
        {
            ActionHelper.DoDamage(this);
        }
    }

    public class BoneCrunch : Attack
    {
        public override string Name { get; init; } = "BoneCrunch";
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
            ActionHelper.DoDamage(this);
        }
    }

    public class Unraveling : Attack
    {
        public override string Name { get; init; } = "Unraveling";
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
            ActionHelper.DoDamage(this);
        }
    }

    // ITEMS
    public abstract class Item : IAction
    {
        public abstract string Name { get; init; }
        public Player ActivePlayer { get; init; }
        public Character ActiveChar { get; init; }
        public Character TargetChar { get; init; }
        public Item() { }
        public abstract void Execute(TheFinalBattle game); // To keep creation of new items flexible, all settings are added in "Execute" command
    }

    public class HealthPotion : Item
    {
        public override string Name { get; init; } = "Health Potion";

        public HealthPotion(Player activePlayer, Character activeChar, Character targetChar)
        {
            ActivePlayer = activePlayer;
            ActiveChar = activeChar;
            TargetChar = targetChar;
        }

        public override void Execute(TheFinalBattle game)
        {
            // Announce use of Item
            Thread.Sleep(Settings.Delay / 2);
            ConsoleHelpWriteLine($"{ActiveChar.Name} used {Name} on {TargetChar.Name}", ConsoleColor.Gray);

            // Resolve item
            Thread.Sleep(Settings.Delay / 2);
            TargetChar.AlterHp(10);
            ConsoleHelpWriteLine($"{TargetChar.Name} has {TargetChar.CurrentHp}/{TargetChar.MaxHp} HP", ConsoleColor.Gray);
            Thread.Sleep(Settings.Delay / 2);

            // Remove item from party inventory
            ActivePlayer.Items.Remove(ItemType.HealthPotion);
        }
    }

    // Action helper will contain methods to help calculate attacks for all Action types
    internal static class ActionHelper
    {
        public static void DoDamage(Attack attack)
        {
            // Temporary variables for easier readability
            string attackName = attack.Name;
            Character activeChar = attack.ActiveChar;
            Character targetChar = attack.TargetChar;

            // Announce attack and damage
            Thread.Sleep(Settings.Delay / 2);
            ConsoleHelpWriteLine($"{activeChar.Name} did {attackName} on {targetChar.Name}", ConsoleColor.Gray);

            Thread.Sleep(Settings.Delay / 2);
            ConsoleHelpWriteLine($"{attackName} dealt {attack.Damage} to {targetChar.Name}", ConsoleColor.Gray);

            // Damage target and report new health status
            Thread.Sleep(Settings.Delay / 2);
            targetChar.AlterHp(-attack.Damage);
            ConsoleHelpWriteLine($"{targetChar.Name} has {attack.TargetChar.CurrentHp}/{attack.TargetChar.MaxHp} HP", ConsoleColor.Gray);
            Thread.Sleep(Settings.Delay / 2);
        }
    }

    public enum ActionType { Nothing, Attack, UseItem } // Available actions to all characters
    public enum AttackType { Punch, BoneCrunch, Unraveling } // All available attacks in the game. Remember to add new attacks to "PickAttack" method.
    public enum ItemType { HealthPotion }
}