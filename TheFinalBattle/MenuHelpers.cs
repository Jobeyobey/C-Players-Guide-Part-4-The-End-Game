using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TheFinalBattleSettings;
using static TheFinalBattleComponents.ConsoleHelpers;

namespace TheFinalBattleComponents
{
    public class MenuHelpers
    {
        // Pick an integer between 0 and 'max'.
        // Used for having human/computer pick an action from action/attack/item menus
        public static int PickFromMenu(int max, bool isHuman)
        {
            int choice;

            if (isHuman) // If player is human, prompt them to pick an option
            {
                // Ensure player choice is valid
                while (true)
                {
                    if (int.TryParse(ConsoleHelpReadLine(ConsoleColor.Cyan), out choice))
                    {
                        if (choice >= 0 && choice <= max)
                        {
                            break;
                        }
                    }

                    ConsoleHelpWriteLine("Please pick a valid number from the menu.", ConsoleColor.Red);
                }
            }
            else // If player is not human, pick a random option
            {
                Thread.Sleep(Settings.Delay);

                Random random = new Random();
                choice = random.Next(max) + 1;
            }

            return choice;
        }

        // List actions character can take
        public static ActionType PickAction(bool isHuman)
        {
            // Print list of actions with index numbers for player to pick from
            int index = 0;
            foreach (ActionType action in Enum.GetValues(typeof(ActionType)))
            {
                index++;
                ConsoleHelpWriteLine($"{index} - {action}", ConsoleColor.White);
            }

            int actionIndex = PickFromMenu(index, isHuman);

            ActionType chosenAction = (ActionType)actionIndex - 1; // '-1' because array is zero-indexed

            return chosenAction;
        }

        public static ActionType ComputerAction(TheFinalBattle game, Player activePlayer, Character activeChar)
        {
            // Print menu to console despite player being computer (Just nice to see what options are available when watching)
            int index = 0;
            foreach (ActionType action in Enum.GetValues(typeof(ActionType)))
            {
                index++;
                ConsoleHelpWriteLine($"{index} - {action}", ConsoleColor.White);
            }

            Thread.Sleep(Settings.Delay);

            // Declare chance variables
            int attackChance = 100;
            int itemChance = 0;
            int equipChance = 0;

            // Check if party has items to use
            if (activePlayer.Items.Count != 0)
            {
                // Check whether items in inventory are offensive or defensive
                bool hasDefensiveItem = false;
                bool hasOffensiveItem = false;

                foreach (ItemType item in activePlayer.Items)
                {
                    if (item == ItemType.HealthPotion)
                        hasDefensiveItem = true;
                    // TODO add offensive items
                }

                // Check if available defensive items are useful
                if (hasDefensiveItem)
                {
                    foreach (Character character in activePlayer.Party)
                    {
                        if (character.CurrentHp <= character.MaxHp / 2)
                        {
                            itemChance = 25; // 25% chance of using an item if healing is needed
                            attackChance -= 25; // Compensate by removing 25% from attack chance
                        }
                    }
                }

                //TODO Check if available offensive items are useful
            }

            // Check if character needs gear and has it available
            if (activeChar.Equipped == null && activePlayer.Gear.Count != 0)
            {
                equipChance = 50; // 50% chance of equipping gear if it's available and character doesn't have gear
                attackChance -= 50; // Compensate by removing 50% from attack chance
            }

            // Prioritise healing over equipping if healing is needed, as per challenge requirements
            if (itemChance != 0)
            {
                itemChance = 50;
                attackChance = 50;
                equipChance = 0;
            }

            // Calculate what action to take
            Random random = new Random();
            int randomInt = random.Next(100);

            if (randomInt < attackChance)
                return ActionType.Attack;
            else if (randomInt < attackChance + itemChance)
                return ActionType.UseItem;
            else if (randomInt < attackChance + itemChance + equipChance)
                return ActionType.Equip;
            else
                return ActionType.Nothing;
        }

        public static IAction PickAttack(TheFinalBattle game, Character activeChar, Player activePlayer)
        {
            ConsoleHelpWriteLine("Pick an attack.", ConsoleColor.Yellow);

            // Print list of attacks with index numbers for player to pick from
            ConsoleHelpWriteLine($"0 - Pick another action", ConsoleColor.White);
            int index = 0;
            foreach (AttackType attack in activeChar.attackList)
            {
                index++;

                if (attack == AttackType.Weapon) // Print weapon name
                {
                    ConsoleHelpWriteLine($"{index} - {activeChar.Equipped.AttackName}", ConsoleColor.White);
                }
                else
                {
                    ConsoleHelpWriteLine($"{index} - {attack}", ConsoleColor.White);
                }
            }

            // Choose attack from list
            AttackType chosenAttack;

            if (!activePlayer.isHuman && activeChar.Equipped != null) // If computer and character is equipped, use equipped weapon
                chosenAttack = AttackType.Weapon;
            else
            {
                int chosenAttackIndex = PickFromMenu(index, activePlayer.isHuman);
                chosenAttack = activeChar.attackList[chosenAttackIndex - 1]; // '-1' because array is zero-indexed
            }

            // Calculate target player/party
            Player targetPlayer;
            if (activePlayer == game.Player1)
                targetPlayer = game.Player2;
            else
                targetPlayer = game.Player1;

            // Pick target. Returns null if user wants to pick another action
            Character target = PickTarget(targetPlayer.Party, activePlayer.isHuman);
            if (target == null)
                return null;

            // Add all possible attacks here
            if (chosenAttack == AttackType.Weapon)     return new WeaponAttack(activeChar, target);
            if (chosenAttack == AttackType.BoneCrunch) return new BoneCrunch(activeChar, target);
            if (chosenAttack == AttackType.Unraveling) return new Unraveling(activeChar, target);
            else return new Punch(activeChar, target);
        }

        public static IAction PickItem(TheFinalBattle game, Character character, Player activePlayer)
        {
            // Check player has items to use
            if (activePlayer.Items.Count == 0)
            {
                ConsoleHelpWriteLine("You have no items in your inventory.", ConsoleColor.Red);
                return null;
            }

            // If player is computer, use computer method
            if (!activePlayer.isHuman)
                return ComputerItem(game, character, activePlayer);

            // Prompt player for choice, listing available items
            ConsoleHelpWriteLine("Pick an item to use.", ConsoleColor.Yellow);
            ConsoleHelpWriteLine($"0 - Pick another action", ConsoleColor.White);
            int index = 0;
            foreach (ItemType item in activePlayer.Items)
            {
                index++;
                ConsoleHelpWriteLine($"{index} - {item}", ConsoleColor.White);
            }

            // Pick item to use. '0' is to pick another action
            int chosenIndex = PickFromMenu(index, activePlayer.isHuman);
            if (chosenIndex == 0)
                return null;

            ItemType chosenItem = activePlayer.Items[chosenIndex - 1]; // '-1' because array is zero-indexed

            // Choose target party depending on whether it's an offensive/defensive item
            Player targetPlayer;
            if (chosenItem == ItemType.HealthPotion)
            {
                targetPlayer = activePlayer == game.Player1 ? game.Player1 : game.Player2; // Target friendly party
            }
            else
            {
                targetPlayer = activePlayer == game.Player1 ? game.Player2 : game.Player1; // Target enemy party
            }

            // Pick target. '0' is to pick another action
            Character target = PickTarget(targetPlayer.Party, activePlayer.isHuman);
            if (target == null)
                return null;

            // Add all possible items here
            if (chosenItem == ItemType.HealthPotion) return new HealthPotion(activePlayer, character, target);
            else return null;
        }

        public static IAction ComputerItem(TheFinalBattle game, Character activeChar, Player activePlayer)
        {
            ItemType chosenItem;
            ConsoleHelpWriteLine("Pick an item to use...", ConsoleColor.Yellow);

            // Print list of items with index numbers for player to pick from
            int index = 0;
            foreach (ItemType item in activePlayer.Items)
            {
                index++;
                ConsoleHelpWriteLine($"{index} - {item}", ConsoleColor.White);
            }

            // Check what items are available
            bool hasHealthPotion = false;

            foreach (ItemType item in activePlayer.Items)
            {
                if (item == ItemType.HealthPotion)
                    hasHealthPotion = true;
            }

            // Check if health potion can/should be used
            if (hasHealthPotion)
            {
                bool needsHealing = false;
                Character damagedChar = null;

                // Check if any characters need healing and track character most in need
                foreach (Character partyMember in activePlayer.Party)
                {
                    if (partyMember.CurrentHp <= partyMember.MaxHp / 2)
                    {
                        needsHealing = true;

                        if (damagedChar == null)
                            damagedChar = partyMember;
                        else if (partyMember.CurrentHp / partyMember.MaxHp < damagedChar.CurrentHp / damagedChar.MaxHp)
                            damagedChar = partyMember;
                    }
                }

                // If a character needs healing, heal the one most in need
                if (needsHealing)
                {
                    return new HealthPotion(activePlayer, activeChar, damagedChar); 
                }
            }

            return null;
        }

        public static Character PickTarget(List<Character> targetParty, bool isHuman)
        {
            ConsoleHelpWriteLine("Pick a target...", ConsoleColor.Yellow);

            // List available target
            ConsoleHelpWriteLine($"0 - Pick another action", ConsoleColor.White);
            int index = 0;
            foreach (Character target in targetParty)
            {
                index++;
                ConsoleHelpWriteLine($"{index} - {target.Name}", ConsoleColor.White);
            }

            // Prompt user to pick target. If user returns '0', they want to go back and pick another action.
            int chosenTarget = PickFromMenu(index, isHuman);
            if (chosenTarget == 0)
                return null;

            return targetParty[chosenTarget - 1]; // '-1' because array is zero-indexed
        }

        public static IAction PickGear(TheFinalBattle game, Character activeChar, Player activePlayer)
        {
            ConsoleHelpWriteLine("Equip some gear...", ConsoleColor.Yellow);

            // Check player has items to use
            if (activePlayer.Gear.Count == 0)
            {
                ConsoleHelpWriteLine("You have no gear in your inventory.", ConsoleColor.Red);
                return null;
            }

            // Print list of items with index numbers for player to pick from
            ConsoleHelpWriteLine($"0 - Pick another action", ConsoleColor.White);
            int index = 0;
            foreach (Gear gear in activePlayer.Gear)
            {
                index++;
                ConsoleHelpWriteLine($"{index} - {gear.Name}", ConsoleColor.White);
            }

            // Pick item to use. '0' is to pick another action
            int chosenIndex = PickFromMenu(index, activePlayer.isHuman);
            if (chosenIndex == 0)
                return null;

            Gear chosenGear = activePlayer.Gear[chosenIndex - 1]; // '-1' because array is zero-indexed

            return new EquipGear(activePlayer, activeChar, chosenGear);
        }
    }
}
