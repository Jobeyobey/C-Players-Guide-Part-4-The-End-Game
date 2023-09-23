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

            // Get current player to pick action
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
            ListActions(character);

            IAction action;

            if (isHuman)
            {
                action = HumanAction(character);
            }
            else
            {
                action = ComputerAction(character);
            }

            return action;
        }

        // List character's available actions
        public static void ListActions(Character character)
        {
            Console.WriteLine("Available Actions:");
            foreach (Action action in character.actionList)
            {
                Console.WriteLine($"- {action}");
            }
        }

        // Allow human player to pick character action
        // TODO add validation that action picked is available to character
        public static IAction HumanAction(Character character)
        {
            string chosenAction = Console.ReadLine();
            while (true)
            {
                if (chosenAction == "Nothing") return new NothingAction(character);

                // If code reached here, chosenAction was invalid
                Console.WriteLine("Action not recognised. Please pick from the list by typing in the name of the action you'd like to take.");
                chosenAction = Console.ReadLine();
            }
        }

        // Allow computer player to pick character action
        public static IAction ComputerAction(Character character)
        {
            // Get list of Action Enum values
            List<string> availableActions = new List<string>();
            foreach(Action action in character.actionList)
            {
                availableActions.Add(action.ToString());
            }

            // Pick a random action for character (from available list)
            Random random = new Random();
            int randomIndex = random.Next(availableActions.Count);
            string randomAction = availableActions[randomIndex];

            Thread.Sleep(1500); // Delay computer choice a moment

            if (randomAction == "Nothing") return new NothingAction(character);
            if (randomAction == "TestAction") return new TestAction(character);
            else return new NothingAction(character);
        }
    }
}
