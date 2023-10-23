﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TheFinalBattleComponents;
using static TheFinalBattleComponents.MenuHelpers;
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
            // If Character already has gear equipped, move it to Player inventory
            if (ActiveChar.Equipped != null)
            {
                ActivePlayer.Gear.Add(ActiveChar.Equipped);
                ActiveChar.attackList.Remove(AttackType.Weapon);
                ActiveChar.Equipped = null;
            }

            // Equip character with gear, remove it from inventory
            ActiveChar.Equipped = Gear;
            ActiveChar.attackList.Add(AttackType.Weapon);
            ActivePlayer.Gear.Remove(Gear);
        }
    }

    // ATTACKS
    public abstract class Attack : IAction
    {
        public abstract string AttackName { get; init; }
        public Character ActiveChar { get; init; }
        public Character TargetChar { get; init; }
        public abstract int Damage { get; init; }
        public abstract int Accuracy { get; init; }
        public Attack() { }
        public abstract void Execute(TheFinalBattle game); // Method to execute action
    }

    public class Punch : Attack
    {
        public override string AttackName { get; init; } = "Punch";
        public override int Damage { get; init; } = Settings.PunchDamage;
        public override int Accuracy { get; init; } = Settings.PunchAccuracy;
        public Punch(Character activeChar, Character targetChar)
        {
            ActiveChar = activeChar;
            TargetChar = targetChar;
        }
        public override void Execute(TheFinalBattle game)
        {
            ActionHelper.DoAttack(this);
        }
    }

    public class WeaponAttack : Attack
    {
        public override string AttackName { get; init; }
        public override int Damage { get; init; }
        public override int Accuracy { get; init; }
        public WeaponAttack(Character activeChar, Character targetChar)
        {
            ActiveChar = activeChar;
            TargetChar = targetChar;
            AttackName = activeChar.Equipped.AttackName;
            Damage = activeChar.Equipped.Damage;
            Accuracy = activeChar.Equipped.Accuracy;
        }

        public override void Execute(TheFinalBattle game)
        {
            ActionHelper.DoAttack(this);
        }
    }

    public class BoneCrunch : Attack
    {
        public override string AttackName { get; init; } = "BoneCrunch";
        public override int Damage { get; init; } = Settings.BoneCrunchDamage;
        public override int Accuracy { get; init; } = Settings.BoneCrunchAccuracy;

        public BoneCrunch(Character activeChar, Character targetChar)
        {
            ActiveChar = activeChar;
            TargetChar = targetChar;

            // Randomly set damage. '+1' to BoneCrunchDamage because allowing 0-1 damage would require "random.Next(2);"
            Random random = new ();
            Damage = random.Next(Damage + 1);
        }

        public override void Execute(TheFinalBattle game)
        {
            ActionHelper.DoAttack(this);
        }
    }

    public class Unraveling : Attack
    {
        public override string AttackName { get; init; } = "Unraveling";
        public override int Damage { get; init; } = Settings.UnravelingDamage;
        public override int Accuracy { get; init; } = Settings.UnravelingAccuracy;

        public Unraveling(Character activeChar, Character targetChar)
        {
            ActiveChar = activeChar;
            TargetChar = targetChar;

            // Randomly set damage. '+1' to UnravelingDamage because allowing 0-1 damage would require "random.Next(2);"
            Random random = new();
            Damage = random.Next(Damage + 1);
        }

        public override void Execute(TheFinalBattle game)
        {
            ActionHelper.DoAttack(this);
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
        public static void DoAttack(Attack attack)
        {
            // Check if attack hit or misses
            bool hit = CheckHit(attack);

            // Temporary variables for easier readability
            string attackName = attack.AttackName;
            Character activeChar = attack.ActiveChar;
            Character targetChar = attack.TargetChar;

            // Announce attack and result
            Thread.Sleep(Settings.Delay / 2);
            ConsoleHelpWriteLine($"{activeChar.Name} did {attackName} on {targetChar.Name}", ConsoleColor.Gray);

            Thread.Sleep(Settings.Delay / 2);
            if (hit)
                ConsoleHelpWriteLine($"{attackName} dealt {attack.Damage} to {targetChar.Name}", ConsoleColor.Gray);
            else
                ConsoleHelpWriteLine($"{activeChar.Name} missed {targetChar.Name}!", ConsoleColor.Gray);

            // Update and report status of target
            Thread.Sleep(Settings.Delay / 2);

            if (hit)
                targetChar.AlterHp(-attack.Damage);

            ConsoleHelpWriteLine($"{targetChar.Name} has {attack.TargetChar.CurrentHp}/{attack.TargetChar.MaxHp} HP", ConsoleColor.Gray);
            Thread.Sleep(Settings.Delay / 2);
        }

        // Check if an attack hits or misses based on accuracy
        public static bool CheckHit(Attack attack)
        {
            bool hit;

            Random random = new Random();
            int fire = random.Next(0, 100);

            hit = fire < attack.Accuracy ? true : false;

            return hit;
        }
    }

    public enum ActionType { Nothing, Attack, UseItem, Equip } // Available actions to all characters
    public enum AttackType { Punch, Weapon, BoneCrunch, Unraveling } // All available attacks in the game. Remember to add new attacks to "PickAttack" method.
    public enum ItemType { HealthPotion } // All available items in the game.
}