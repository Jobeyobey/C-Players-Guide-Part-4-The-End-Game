using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheFinalBattleComponents;
using static System.Collections.Specialized.BitVector32;

namespace TheFinalBattleComponents
{
    // TheFinalBattle will track the status of the whole game and run it
    public class TheFinalBattle
    {
        public Player Player1 { get; private set; }
        public Player Player2 { get; private set; }

        public TheFinalBattle() // Constructor
        {
            Player1 = new Player(false);
            Player2 = new Player(false);
        }

        public void Run() // Run the game
        {
            MakeHeroParty();
            MakeMonsterParty();
            StartRound();
        }

        public void StartRound() // A round consists of multiple turns, ending when a party has no characters left.
        {
            int turnNumber = 0;
            bool player1Turn = true;

            while (true) // Round Loop
            {
                Thread.Sleep(1000);

                TakeTurn(player1Turn, turnNumber);

                player1Turn = !player1Turn; // Flip who's turn it is

                // Increment turn number if this is the end of Player2's turn
                if (!player1Turn)
                    turnNumber++;

                Console.WriteLine(); // Create space to differentiate turns
            }
        }
        public void MakeHeroParty()
        {
            // Get player to input name for MainCharacter
            Console.WriteLine("What will the True Programmer's name be? ");
            string? name = Console.ReadLine();

            while (true) // Check name is valid
            {
                if (name == null || name == "")
                {
                    Console.WriteLine("Please input a valid name. ");
                    name = Console.ReadLine();
                }
                else
                {
                    break;
                }
            }

            // Create and add TOG to party
            Player1.Party.Add(new MainCharacter(20, name));
        }

        public void MakeMonsterParty()
        {
            Player2.Party.Add(new Skeleton(10));
        }

        public void TakeTurn(bool player1Turn, int turnNumber)
        {
            IAction action; // Prepare memory for action

            // Get current player to pick action for active character
            if (player1Turn)
            {
                int characterIndex = turnNumber % Player1.Party.Count;
                action = GetAction(this, Player1.Party[characterIndex], Player1.isHuman);
            }
            else
            {
                int characterIndex = turnNumber % Player2.Party.Count;
                action = GetAction(this, Player2.Party[characterIndex], Player2.isHuman);
            }

            action.Execute(this); // Execute chosen action
        }

        public IAction GetAction(TheFinalBattle game, Character character, bool isHuman)
        {
            Console.WriteLine($"It is {character.Name}'s turn...");
            Action chosenAction = PickAction(isHuman);

            // Resolve chosen action
            if (chosenAction == Action.Nothing) return new NothingAction(character);
            if (chosenAction == Action.Attack)
            {
                ListAttacks(character);

                IAction attack;

                if (isHuman)
                {
                    attack = HumanAttack(character);
                }
                else
                {
                    attack = ComputerAttack(character);
                }

                return attack;
            }

            // Code should not be able to get to here, but if it somehow does, do 'nothing' action.
            return new NothingAction(character);
        }

        // List actions character can take
        public static Action PickAction(bool isHuman)
        {
            // Print list of available actions. Also add actions to easily accessible list.
            List<string> actionList = new List<string>();
            foreach (Action action in Enum.GetValues(typeof(Action)))
            {
                Console.WriteLine($"- {action}");

                string tempAction = action.ToString().ToLower(); // Ensure actions are lower case to easier match user inputs later on
                actionList.Add(tempAction);
            }

            string? chosenAction; // Prepare chosenAction string for player/computer to fill in below

            if (isHuman)
            {
                chosenAction = Console.ReadLine();

                // Prevent chosenAction being null
                while (chosenAction == null)
                {
                    Console.WriteLine("Please input a valid action.");
                    chosenAction = Console.ReadLine();
                }

                // Ensure chosenAction is valid
                while (true)
                {
                    chosenAction = chosenAction.ToLower();
                    if (actionList.Contains(chosenAction)) break;

                    // If code reached here, input was invalid
                    Console.WriteLine("That is not a valid action. Pick an action from the list.");
                    chosenAction = Console.ReadLine();
                }
            }
            else // If computer player, pick random action
            {
                Thread.Sleep(1500); // Delay computer player choice for easier reading

                // Pick a random action for character from available list
                Random random = new Random();
                int randomIndex = random.Next(actionList.Count);
                chosenAction = actionList[randomIndex];
            }

            // Player or computer should have by now input a valid action. Resolve action.
            if (chosenAction == "attack") return Action.Attack;
            else return Action.Nothing;
        }

        // List character's available attacks
        public static void ListAttacks(Character character)
        {
            Console.WriteLine("Available Attacks:");
            foreach (Attack attack in character.attackList)
            {
                Console.WriteLine($"- {attack}");
            }
        }

        // Allow human player to pick character attack
        // TODO add validation that attack picked is available to character
        public static IAction HumanAttack(Character character)
        {
            string? chosenAttack = Console.ReadLine();
            while (true)
            {
                if (chosenAttack == "punch") return new Punch(character);
                if (chosenAttack == "bonecrunch") return new BoneCrunch(character);

                // If code reached here, chosenAction was invalid
                Console.WriteLine("Action not recognised. Please pick from the list by typing in the name of the action you'd like to take.");
                chosenAttack = Console.ReadLine();
            }
        }

        // Allow computer player to pick character action
        public static IAction ComputerAttack(Character character)
        {
            // Get list of available Attacks
            List<string> availableAttacks = new List<string>();
            foreach(Attack action in character.attackList)
            {
                availableAttacks.Add(action.ToString());
            }

            // Pick a random attack for character (from available list)
            Random random = new Random();
            int randomIndex = random.Next(availableAttacks.Count);
            string randomAttack = availableAttacks[randomIndex];

            Thread.Sleep(1500); // Delay computer choice a moment

            if (randomAttack == "punch") return new Punch(character);
            if (randomAttack == "bonecrunch") return new BoneCrunch(character);
            else return new Punch(character);
        }
    }
}
