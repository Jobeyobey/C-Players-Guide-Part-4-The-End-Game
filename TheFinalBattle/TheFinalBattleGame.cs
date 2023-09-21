﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheFinalBattleComponents;

namespace TheFinalBattleComponents
{
    // TheFinalBattle will track the status of the whole game and run it
    public class TheFinalBattle
    {
        public Player Player1 { get; }
        public Player Player2 { get; }

        public TheFinalBattle(Player player1, Player player2) // Constructor
        {
            Player1 = player1;
            Player2 = player2;
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

            while (true)
            {
                Thread.Sleep(500);

                TakeTurn(player1Turn, turnNumber);

                player1Turn = !player1Turn;

                // Increment turn number if this is the end of player 2's turn
                if (!player1Turn)
                    turnNumber++;

                Console.WriteLine(); // Create space to differentiate turns
            }
        }
        public void MakeHeroParty()
        {
            Player1.Party.Add(new Skeleton(10, 10));
        }
        public void MakeMonsterParty()
        {
            Player2.Party.Add(new Skeleton(10, 10));
        }
        public void TakeTurn(bool player1Turn, int turnNumber)
        {
            if (player1Turn)
            {
                int characterIndex = turnNumber % Player1.Party.Count;
                ICommand command = GetCommand(this, Player1.Party[characterIndex]);
                command.Execute(this);
            }
            else // if player2's turn
            {
                int characterIndex = turnNumber % Player2.Party.Count;
                ICommand command = GetCommand(this, Player2.Party[characterIndex]);
                command.Execute(this);
            }
        }
        public ICommand GetCommand(TheFinalBattle game, Character character)
        {
            // TODO: List what actions can be taken and get input for what action to take
            Command command = new Command(character); // TODO: This should be a switch with multiple options for what command to create
            return command;
        }
    }
}
