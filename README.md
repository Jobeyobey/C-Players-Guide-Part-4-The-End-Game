# The End Game

The aim of this project is to create a command-line based game.
The game is the final challenge of the [C# Player's Guide by RB Whitaker](https://csharpplayersguide.com/).
The intention of the challenge is to make use of the OOP principles and techniques I have practiced throughout all other challenges in the book.

### Aim of the game

In the game, there are two parties, the 'Heroes' and the 'Monsters'. The heroes win multiple rounds against the monsters, until there are none left. The heroes and monsters can both be controlled by human players, or the computer.

### Review

#### General

I'm happy with how the game has come out. I did have to stop myself from continuing on it, as I knew there would always be something to add, or something to improve.
I definitely found as I expanded aspects of the game, I could have improved how I planned the initial code.

I believe the strongest parts of my design is the ease with which you can add new hero/monster types. The weakest parts of my code are in the menus.

#### Organisation

My file organisation started well, I separated the main game and characters, actions, player objects etc. However it quickly fell apart as I
expanded the game. For example, 'Actions.cs' contains actions, attacks and items. In future, I would definitely spread these across their own separate files.

I'm very happy with the 'Settings.cs' file I created, which allowed me to quickly balance the game at the end of the challenge.
I could easily adjust most of the values within this single file.

My commenting could do with some work. I tend to comment *what* a function is doing, rather than *why*. Although I believe both can be helpful
depending on the complexity of the function, most programmers should be able to see what a function is doing. But the *why* is not always apparent.


#### Expandability

Some aspects of the game are designed well to be able to easily implement new attacks, gear and characters. The main challenge is adding items
that have special abilities, for example the Cannon of Consolas and the Tome. I believe I managed to come up with a system for this, where gear has
a bool that identifies whether it has a special effect, in which case this is called instead of the standard attack. The special effect is able to 
call on the standard attack if it is also required.

However, the part I struggled most with was making it easy to add a new item or piece of gear that the computer knew how to use efficiently. Adding a new item
would result in having to write a large amount of new logic for the computer to be able to decide whether it *should* use that new item in a particular scenario.
I do imagine this is normal, that specific cases need to be written for the computer player when a new item is added to make sure it's used effectively.

The most difficult part about expanding the game is as was mentioned before, where my organisation fell apart. Sometimes adding a new feature would mean
jumping across multiple files, for example adding attacks/actions/items to the switches in 'TheFinalBattleGame.cs'. More time spent planning before
implementing would help prevent this in future.