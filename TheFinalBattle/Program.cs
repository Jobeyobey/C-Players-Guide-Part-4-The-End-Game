using TheFinalBattleComponents;

Console.Title = "The Final Battle";

// Temporary players for testing
Player player1 = new Player(false);
Player player2 = new Player(false);

TheFinalBattle game = new TheFinalBattle(player1, player2);
game.Run();