# About the game
**🐶Find Losty** is an old-school-like text adventure for multiple players, which can be played via Discord.

## 📖Story
Your dog **[🐶Losty]** ran away and somehow entered the scary abandoned **[🏡mansion]** down the road. You gather a few friends and start a recue mission.

The game starts directly in front of the **[🏡mansion]**.

## 🎮Controls
The game is controlled by a virtual GameMaster aka **[🤖Bot]** (this is some kind of AI if you will). This **[🤖Bot]** gives you a textual representation of the things happening in-game.
Each location in the game happens inside a specific Discord-*VoiceChannel*, which you join. This way you can only talk to and hear the persons staying with you at the same location.

To take action, each player gets a dedicated Discord-*TextChannel*. You can act in the game by typing in commands like eg. `open door`. For each command you get textual feedback,
which can contain **[🧻new things to interact with]**, **[🏠new rooms to explore]** and **[❓challenging riddles to solve]**. Some riddles require cooperation with other players.

For a start try to type `help` inside your personal *Text-Channel*, which lists all available commands, or type `look` to get information about your surroundings. You can change rooms either by changing you Discord-*VoiceChannel* or by typing the `goto`-command eg. `goto kitchen`.

## 🚦How to start
In order to use the Game with Discord, you have to invite the **[🤖Bot]** to your Server (see [🔧Preparation](#🔧Preparation)). You start the executable and a new section **`FindLosty`** should appear on the left side in Discord.

Inside this section there are the various *VoiceChannels*, one per room you discovered. `FrontYard` is the game's starting location. Players can join this *Voice-Channel* to join the game.
After joining, each player gets a personal *TextChannel* where the adventure takes place.


# 👩‍💻For developers
## 🔧Preparation

### ✨Getting a token for the bot
The game aka **[🤖Bot]** needs to identify to Discord via a **[✨ token]** at startup. Therefore you need to get a **[✨token}** for your **[🤖Bot]** from Discord.

The process of obtaining a **[✨token}** works roughly as follows:
* create a new app at the [Discord developer page](https://discord.com/developers/applications), this gives you a *client-id*
* create a new **[🤖Bot]** inside this app, which gives you the **[✨ token]**

Then you need to create a new file called `.env` or you could rename the existing file `.env.sample` to `.env`.

Inside this file you enter your **[🤖Bot]**'s **[✨ token]**:

> DISCORD_TOKEN=XXXXXxxxxxXXXXXXXXXXXXXxxxxxxxxxxxxxxxxxxx

This `.env` file must be put inside the same folder as your executable, or - if you build from source - at the root of the project.

### 🤖Inviting the bot to your Discord-Server
To invite the **[🤖Bot]**, you need to authorize it at your server via a **[🧝‍♂️link]**.
When you start the executable the **[🧝‍♂️link]** for authorization will be shown in the **[💻terminal]**, eg.

~~~ bash
[ENGINE] Creating Discord client ...
[ENGINE] ... Discord client created
[ENGINE] Adding Handlers ...
[ENGINE] ... Handlers added 
[ENGINE] Connecting ...
[2021-02-07 21:50:57 +01:00] [101 /Startup     ] [Info ] DSharpPlus, version 4.0.0-rc1
[ENGINE] ... Connected
Use following URL to invite the bot to your Server:
https://discord.com/api/oauth2/authorize?client_id=$CLIENTID&permissions=297802768&scope=bot
[ENGINE] ... Guild added $YOUR_DISCORD_SERVER
~~~

Just open this **[🧝‍♂️link]** and Discord will ask you which server you want the **[🤖Bot]** been invited to.

## Building from source
**🐶Find Losty** is programmed in `C#` with [DotNet SDK](https://dotnet.microsoft.com/download/dotnet/5.0).

The usual commands `dotnet restore`, `dotnet build`, `dotnet run` and `dotnet publish` apply.

For debugging purpose the Game-Engine contains a local mode, which can be started by:

> dotnet run -- interactive

Warning: You need to run this in a unicode/utf-8 enabled **[💻terminal]**.

Now you can go crazy, eg. by piping **[📜script files]** with commands to the locally running process. See `./test.sh` for an example.

# License
This game is published under the [CC BY 4.0 License](https://creativecommons.org/licenses/by/4.0/), which basically means:

## You are free to:
* Share — copy and redistribute the material in any medium or format
* Adapt — remix, transform, and build upon the material for any purpose, even commercially.
* This license is acceptable for Free Cultural Works.
* The licensor cannot revoke these freedoms as long as you follow the license terms.

## Under the following terms:
* Attribution — You must give appropriate credit, provide a link to the license, and indicate if changes were made. You may do so in any reasonable manner, but not in any way that suggests the licensor endorses you or your use.

* No additional restrictions — You may not apply legal terms or technological measures that legally restrict others from doing anything the license permits.

