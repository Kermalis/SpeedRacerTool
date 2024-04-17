![Wireframe](https://github.com/Kermalis/SpeedRacerTool/assets/29823718/82762fde-3214-4080-b049-4cf3047588d7)

![PhysicsLol](https://github.com/Kermalis/SpeedRacerTool/assets/29823718/57b204b8-2240-4eed-a27c-0e598b463c74)

![FloorPhysics](https://github.com/Kermalis/SpeedRacerTool/assets/29823718/8723b883-a062-4c5e-8bc1-ba71843a572f)

![PropsWow](https://github.com/Kermalis/SpeedRacerTool/assets/29823718/e4ff604c-f1e9-42e4-b5f3-b34c1e737528)

![FooJee](https://github.com/Kermalis/SpeedRacerTool/assets/29823718/90d0c9ee-80b2-4cfb-98f0-6a87bfbfed7e)

# What is this
This is a tool I wrote to read the data from Speed Racer: The Videogame (PS2 & Wii).
Basically I was just curious to figure out what game engine was used for this game, so I checked my PS2 version and it led to an entire rabbithole once I found ".nif" and ".xds" inside.
Well I wanted to see how to dump the tracks obviously, once I found out the .nif files contain them.

### PS2 extraction
Get your PS2 .iso.
Extract the "PS2.ZIP" file out of it and then extract the "ps2_ps2" folder out of that, then you have the game contents.

### Wii extraction
The Wii version will need to be converted to .iso I think, since I had that already done.
Extract the "wii_wiics" folder out of that, then you have the game contents.

# Game data
The game contents is just 7 or 8 folders:
* game
* homebutton
* replays
* resourcesets
* save (PS2 only)
* scripts
* tracks
* ui

The interesting stuff is in "game" and "tracks".
"game" has general game data:
* Vehicle models and stats
* Game settings such as AI difficulty, maximum boosts (more than 4 is possible), boosts at the startline, etc.
* General graphics

The vehicle stats and AI difficulty are easy to change since they're .xml files.
It's how I made my ["Speed Racer Always Cheats (Meme)"](https://www.youtube.com/watch?v=lVawtFND6y8) video.
You just need to find a way to place them back in the .iso, which this tool can do, but I'll write about that below...

"tracks" has the track data like you thought it would:
* Models
* Props
* Collisions

Interestingly, there are remnants of a track editor.
I assume this is how they made the tracks originally, and it seems they made the tracks on the actual hardware rather than in a PC, since there are controller bindings.
It could be possible that they wanted to make a track editor for the public like in Trackmania, but I seriously doubt that since there would be a severe lack of scenery for user tracks.
Not to mention the insane complexity that went into making the tracks in this game.
There are several joints, track variations (short/medium/long), obstacles/speedups, and each track piece itself is made with splines unlike in Trackmania and other Hotwheels games.
That's how this game gets its unique feel and gameplay; from the extremely custom bends and shapes of the tracks, and how they bend to the scenery and props of each stadium.

### What are ".nif" and ".xds" then?
.nif is a format for the "Gamebryo" game engine.
This game specifically is using version `20.3.0.9`, which is important when trying to dump data for it.
The only reason I even got this tool working was because of the work done by the [NifSkope](https://github.com/niftools/nifskope) tool.
Specifically, I used [this fork](https://github.com/fo76utils/nifskope) as reference since it's way more up-to-date and had less crashes for me.
That tool can read and extract data from dozens of games, but not from Speed Racer unfortunately.
It's not because of the game engine version, which is supported, but because of the geometry data that's in Speed Racer.
Also, that tool cannot extract textures from the PS2 version of this game because the textures are also in a unique format.

So this tool is adapted to C# first of all, but also supports both versions of Speed Racer.
If it weren't for NifSkope's structure documentation, it would've taken way longer, or I would've been discouraged and not made this tool and stuck to the basic editing of the AI.

.xds is an unknown format.
The files themselves don't even mention where they're from, and have no real hints.
Each file has a "MabStream" portion to it, but I have searched and found nothing related to that, so I had to reverse-engineer this file format myself.
Luckily it was straightforward, though there's no rigid structure to each file's contents.
It appears that each file has a "file type" ID number, and the game reads the file based on that.
So each file type itself must be reverse-engineered independently...
However it's not too difficult to do luckily.

# Okay so what do it do
All of the main commands come from `Program.cs`.

### Patching the PS2 .iso
I realized that the "PS2.ZIP" file is inserted raw (0 compression) into the .iso.
So my method of patching the .iso is to copy the entire `original.iso` to `new.iso`, and then replace "PS2.ZIP" inside the `new.iso`.
This avoids rebuilding the entire file system and needing weird .iso burning tools.
This method restricts the new "PS2.ZIP" to be <= the original's size, but that's easy to handle since the .xml files all have lots of whitespace you can remove.

To summarize the process:
* Copy `original.iso` to `new.iso`
* Extract "PS2.ZIP" from `original.iso`
* Extract files from "PS2.ZIP"
* Edit whichever files you like
* Place the edited files into "PS2.ZIP" and overwrite the originals
* Use this tool (or a hex editor) to place "PS2.ZIP" into `new.iso` at the correct file offset

The "correct offset" is going to differ by region and game revision, so this tool has offsets for my specific dump.

### Test NIF files
This functionality lets you read .nif files and see if they were read correctly.
Once you determine if they are read correctly, you can log info about them or extract certain .nif geometry to .obj files, which is how I got the screenshots above.
The limitation of that is the rotation isn't automated or simple to do, so I ignored rotation in those examples (and you probably didn't notice since rotation is used sparsely).

### Test XDS files
Similar to the above, it lets you read .xds files and log them etc.
Currently I have a test function to dump "heightfield" collision data for tracks, again to .obj and without rotation.

# How do I use it?
There is no GUI because I have 0 interest in making one, even though it'd be extremely helpful.
So you need to edit the stuff in `Program.cs`.
Honestly it should be straightforward to anyone who's a programmer or even a novice.
Just replace my hardcoded filepaths with your own, and uncomment the test functions (like .obj dumping) to run them.

If you thought this was some massive modding framework for some niche game, then yeah I wish it was too BUDDY!
Let's keep dreaming.

Yes I could make it way more user-friendly with cmd-line arguments and a download, but man that is just so much work.
I'm doing this for free and in my own time.
Been there done that, I hate it, just edit the file lol it's really not that hard

----
# SpeedRacerTool uses:
* [EndianBinaryIO](https://github.com/Kermalis/EndianBinaryIO)

----
# TO DO
* Finish ".glb" export so we can have rotation, pivot points, textures, node hierarchy, etc.
* Fix Wii ".nif" files (they broke somehow when I wasn't looking)
* Wii geometry ".nif" files support
* Wii track ".xds" files support (they're different from PS2 but still similar enough)
* Non-heightfield collision dumping
* Maybe make it easier to use or talk to me