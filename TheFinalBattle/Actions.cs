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
    // All actions implement this interface
    public interface IAction
    {
        void Execute(TheFinalBattle game);
    }

    // This action does nothing. It effectively just passes the turn
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

    // EQUIP GEAR
    public class EquipGear : IAction
    {
        public Player ActivePlayer { get; init; }
        public Character ActiveChar { get; init; }
        public Gear Gear { get; init; }

        public EquipGear(Player activePlayer, Character activeChar, Gear gear) {
            ActivePlayer = activePlayer;
            ActiveChar = activeChar;
            Gear = gear;
        }

        public void Execute(TheFinalBattle game)
        {
            ActiveChar.Equipped = Gear;
            ActiveChar.attackList.Add(AttackType.Weapon);
            ActivePlayer.Gear.Remove(Gear);
        }
    }

    // ATTACKS
    public abstract class Attack : IAction
    {
        public abstract string Name { get; init; } // Used for displaying name of attack in menu, as well as in battle description. (e.g. Player1 {Punched} Player2)
        public Character ActiveChar { get; init; } // Character initiating the action
        public Character TargetChar { get; init; } // Target of action
        public abstract int Damage { get; init; } // Damage dealt by action
        public Attack() { }
        public abstract void Execute(TheFinalBattle game); // Method to execute action
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

    public class WeaponAttack : Attack
    {
        public override string Name { get; init; }
        public override int Damage { get; init; }
        public WeaponAttack(Character activeChar, Character targetChar)
        {
            ActiveChar = activeChar;
            TargetChar = targetChar;
            Name = activeChar.Equipped.Name;
            Damage = activeChar.Equipped.Damage;
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
        public abstract string Name { get; init; } // Used for displaying name of item in menu, as well as in battle description. (e.g. Char1 used {HealthPotion} on Char2)
        public Player ActivePlayer { get; init; } // Player using item
        public Character ActiveChar { get; init; } // Character using item
        public Character TargetChar { get; init; } // Targeted character
        public Item() { }
        public abstract void Execute(TheFinalBattle game); // Method to execute action. These will contain more code than attacks due to the flexible nature of items
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

    public enum ActionType { Nothing, Attack, UseItem, Equip } // Available actions to all characters
    public enum AttackType { Punch, Weapon, BoneCrunch, Unraveling } // All available attacks in the game. Remember to add new attacks to "PickAttack" method.
    public enum ItemType { HealthPotion } // All available items in the game.
}