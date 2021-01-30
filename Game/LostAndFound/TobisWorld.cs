using DSharpPlus.Entities;
using LostAndFound.Engine;
using LostAndFound.Engine.Attributes;
using LostAndFound.Engine.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Game.LostAndFound

{
    public class TobisWorld : CommonRoom
    {
        public bool LightOn = false;

        public override string Name => "Tobis World";

        [Command("look", "look around")]
        public async Task Look(PlayerCommand cmd)
        {
            if (cmd.Player is Player player)
            {
                if (!LightOn)
                {
                    await player.SendGameEventAsync("You can't see a thing. You move your hand in front of your face and you can barely see your hand moving.");
                }
                else
                {
                    await player.SendGameEventAsync("The room looks like a cellar made mostly out of sandstone." +
                                                    "There is a small barred drain in the middle of the room." +
                                                    DoorDescription());
                }
            }
        }


        private int kick_count = 0;
        private long time_of_last_kick = 0;
             
        [Command("kick", "kick something")]
        public async Task Kick(PlayerCommand cmd)
        {
            string message = "";
            if (cmd.Args.Count >= 2)
            {
                message = "You can only kick one thing at a time.";
            }
            else if (cmd.Args.Count == 0)
            {
                message = "You kick the air in front of you. Nothing happens.";
            }
            else if (cmd.Args.Count == 1)
            {
                if (cmd.Args[0] == "door")
                {
                    if (door_life != 0)
                    {
                        long time_of_kick = DateTime.Now.Ticks;
                        long time_since_last_kick = time_of_last_kick - time_of_last_kick;
                        if (time_since_last_kick < 0)
                        {
                            time_since_last_kick = long.MaxValue;
                        }
                        if (kick_count == 0)
                        {
                            message = "The door shakes and there are some cracking sounds. But it feels like you need more force.";
                            kick_count = 1;
                            time_of_last_kick = time_of_kick;
                        }
                        else if (kick_count == 1 && time_since_last_kick < 2000)
                        {
                            message = "The combined force shake the door and there are cracking sounds. But it feels like you need more force.";
                            kick_count = 2;
                            time_of_last_kick = time_of_kick;
                        }
                        else if (kick_count == 2 && time_since_last_kick < 2000 && door_life > 1)
                        {
                            message = "The combined force shake the door and you can feel it crack. You definitely destroyed it a little.";
                            kick_count = 2;
                            time_of_last_kick = time_of_kick;
                        }
                        else if (kick_count == 2 && time_since_last_kick < 2000 && door_life == 1)
                        {
                            message = "The combined forces shatter the door into splinters.";
                        }
                        else
                        {
                            kick_count = 0;
                        }
                    }
                    else
                    {
                        message = $"You kick the splinters on the floor. Nothing happens.";
                    }
                }
                else
                {
                    message = $"There is no {cmd.Args[0]} to kick.";
                }
            }

            if (cmd.Player is Player player)
            {
                await player.SendGameEventAsync(message);
            }
        }

        private int door_life = 3;

        private string DoorDescription()
        {
            if (door_life == 3)
                return "There is a small wooden 'door'.";
            else if (door_life == 2)
                return "There is a small wooden 'door', it has some dents.";
            else if (door_life == 1)
                return "There is a small wooden 'door', with a large crack in it.";
            else
                return "There a parts of a splintered door on the floor.";
        }
    }
}
