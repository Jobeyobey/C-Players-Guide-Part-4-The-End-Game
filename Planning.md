# Planning

## Initial Notes

- Two collections of characters (two parties). (Player Object?)
	1. Heroes/Monsters
	2. Items
	3. Human/Computer

- Multiple types of characters. (Character Objects)
	1. Name/Type
	2. Hit Points
	3. Weapons
	
- Battles (Game Object)
	1. Battle consists of multiple rounds until one party dies

- Actions (Action interface and objects?)
	1. Characters can take multiple types of actions.
	2. Punch/Do Nothing/Weapon/Item/Equip
	3. Attacks need datatypes (damage, type, target)

## Objects

- Game Object
	1. Contains both player objects
	2. Runs game loop (Track round, repeat rounds until round is over. Detect in/loss)
- Player Object
	1. Human/Computer
	2. Party status (Who is in the party?)
	3. Items
- Character Object
	1. Name (This could be a string of the object type)
	2. Max HP
	3. Current HP
	4. Weapon
- Action Interface
    1. Used for to extend different actions a player can take.
- Item Object
- Weapon Object
- Attack Object

#### Game Object
This will keep track of and run the game. It should know the players in the game, how many rounds there are,
how many rounds are left, whe nthe game ends etc.

It must have a 'Run' method to run the game loop.

#### Player Object
The player object must know whether it is Computer or Human, so that when taking actions we know whether to wait for
user input. It must also know who is in the party. I imagine this would be tracked with a character list.

The item list can be tracked on the player object, seeing as this would effectively be the party's anyway.

#### Character Object
Character objects should know their name. This could be done with ToString(). It could be overloaded to
create a custom name using the name of the object, as well as any items they carry.

Max HP will need to be tracked, so characters can not be overhealed in future.

Characters will track their own weapons, which will also affect the choice of actions a character can take.

#### Interfaces
In the Fountain of Objects game, we used Interfaces for Commands and Sensing.
Commands had a single 'Execute' function that would take the gameobject as an argument.
Commands would then extend the ICommand, adding in their own logic and executing it by
overwriting the Execute command of the Interface. The same was done for senses, however
instead of an' Execute' command, it was 'CanSense' and 'DisplaySense', which were both
overwritten. This can be used to allow easy expansion of the game in future, when adding new actions.

#### Item
The item object will likely be for one-time consumables.

#### Weapons
I will need a weapon object that can have types of weapons derived from it. Damage, Damage Type, Hit Chance etc.

#### Attack Object
I'm not sure if this would necesarily be an object or just an action. It would need to know the above about the weapons,
as well as targets and possibly sources of attack. This could possibly just be part of the Attack Action interface.
