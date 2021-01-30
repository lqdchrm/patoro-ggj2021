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
    public class LokisWorld : CommonRoom
    {

        //private bool isLight;
        private bool isChained = true;
        private bool isSchleimed;
        private bool isHandWounded;

        public override string Name => "Lokis World";


        protected override bool IsCommandVisible(string cmd)
        {
            if (cmd == "MOVE" && isChained)
                return false;

            return base.IsCommandVisible(cmd);
        }

        [Command("MOVE", "Bewege dich in die Freiheit")]
        public async Task Move(PlayerCommand cmd)
        {
            if (cmd.Player is not Player player)
                return;


            await player.SendGameEventAsync("Du öffnest die Tür und trittst in die Freiheit.");
            await player.SendGameEventAsync("Dies ist das Ende der Shareware.");
            await player.SendGameEventAsync("Wenn dir das Spiel gefällt Schicke 0€ in einem Umschlag.");

        }

        [Command("USE", "Benutze [X] mit [Y]. [Schreibe USE X Y]")]
        public async Task Use(PlayerCommand cmd)
        {
            if (cmd.Player is not Player player)
                return;

            if (cmd.Args.Any(x => (x.Equals("schleim", StringComparison.OrdinalIgnoreCase) || x.Equals("schleimig", StringComparison.OrdinalIgnoreCase))
                && x.Equals("hand", StringComparison.OrdinalIgnoreCase)))
            {
                if (isChained)
                {
                    if (isSchleimed)
                    {
                        await player.SendGameEventAsync("Du trägst noch etwas mehr schleim auf.");
                        await player.SendGameEventAsync("Das schadet nicht...");
                        await player.SendGameEventAsync("...hoffentlich");
                    }
                    else
                    {
                        await player.SendGameEventAsync("Vorsichtig Fährst du mit deinem Fuß in den Schleim.");
                        await player.SendGameEventAsync("Aber du musst nicht sonderlich aufpassen als du das Bein zurückziehst um ihn dan mit der Hand zu erreichen.");
                        await player.SendGameEventAsync("So einfach wird er nicht mehr abgehen.");
                        await player.SendGameEventAsync("Du schmierst den Schleim auf die Schelle.");
                        await player.SendGameEventAsync("Es sollte jetzt sicherer sein die Hand herauszuziehen.");
                    }
                    isSchleimed = true;
                }
                else if (isHandWounded)
                {
                    await player.SendGameEventAsync("Deine Wunde Brennt als der Schleim mit Ihr in berührung kommg.");

                }
                else
                {
                    await player.SendGameEventAsync("Deine Hand ist voll schleim.");

                }

            }
            else if (cmd.Args.Count >= 2)
            {
                await player.SendGameEventAsync("Du denkst nach, aber bist dir nicht sicher wie das funkltionieren soll.");

            }
        }


        [Command("PULL", "Ziehe an X [PULL X]")]
        public async Task Pull(PlayerCommand cmd)
        {
            if (cmd.Player is not Player player)
                return;

            if (cmd.Args.Any(x => x.Equals("boden", StringComparison.OrdinalIgnoreCase)))
            {
                if (isChained)
                    await player.SendGameEventAsync("Mit den Zehen tastest du an den Fugen der Steine entlang.");
                else
                    await player.SendGameEventAsync("Du versuchst mit deinen Fingern zwischen die Steine zu kommen.");
                await player.SendGameEventAsync("Aber du findest keinen Anpack.");

            }
            else if (cmd.Args.Any(x => x.Equals("schleimiges", StringComparison.OrdinalIgnoreCase) || x.Equals("schleim", StringComparison.OrdinalIgnoreCase)))
            {
                await player.SendGameEventAsync("Du ziehst das ES über den boden.");
                await player.SendGameEventAsync("Es macht schmatzende geräuche.");
            }
            else if (cmd.Args.Any(x => x.Equals("hand", StringComparison.OrdinalIgnoreCase)))
            {
                if (isChained)
                {

                    if (isSchleimed)
                    {
                        await player.SendGameEventAsync("Vorsichtig gleitet deine Hand durch die schelle.");
                    }
                    else
                    {
                        await player.SendGameEventAsync("Du Ziehst deine Hand langsam über die Scharfen Kanten.");
                        await player.SendGameEventAsync("Je weiter du Ziehst desto höher must du den Druck erhöhen.");
                        await player.SendGameEventAsync("Ein beißender Schmerz fährt durch deine Hand als du die Letzten Zentimerter mit einem Ruck überwunden hast.");
                        isHandWounded = true;
                    }
                    isChained = false;
                    await player.SendGameEventAsync("Du bist Frei.");
                    await player.SendGameEventAsync("Neuer Befehl [MOVE].");
                }
            }
            else if (cmd.Args.Any())
            {
                await player.SendGameEventAsync($"Du suchst nach {string.Join(" ", cmd.Args)}");
                await player.SendGameEventAsync($"Kannst aber nichts finden");
            }
            else
            {
                await player.SendGameEventAsync("Was willst du Ziehen");
            }
        }


        [Command("LOOK", "Schau dich um. [Optional schaue etwas an]")]
        public async Task Look(PlayerCommand cmd)
        {
            if (cmd.Player is not Player player)
                return;


            {
                if (cmd.Args.Any(x => x.Equals("boden", StringComparison.OrdinalIgnoreCase)))
                {
                    await player.SendGameEventAsync("Du tastet mit den füßen auf dem kalten steien. Als du gegen etwas [schleimiges] stößt.");
                    await player.SendGameEventAsync("Du bist dir nicht sicher was es ist, eventuell eine übergroße Made.");
                    await player.SendGameEventAsync("Aber du willst es liber garnicht wissen.");

                }
                else if (cmd.Args.Any(x => x.Equals("schleimiges", StringComparison.OrdinalIgnoreCase) || x.Equals("schleim", StringComparison.OrdinalIgnoreCase)))
                {
                    await player.SendGameEventAsync("Du übst etwas druck mit deinen Zehen aus.");
                    await player.SendGameEventAsync("Plötzlich durschtößt du die Schleimige Haut.");
                    await player.SendGameEventAsync("...");
                    await Task.Delay(TimeSpan.FromSeconds(2));
                    await player.SendGameEventAsync("Das innere ist angenehm war.");
                }
                else if (cmd.Args.Any(x => x.Equals("hand", StringComparison.OrdinalIgnoreCase)))
                {
                    await player.SendGameEventAsync("Um dein Handgelenk schneidet sich eine Metallene schelle in dein Fleisch. Sie scheint mit einer Kette von der Decke zu hängen.");
                    await player.SendGameEventAsync("Sie ist etwas zu groß. Allerdings auch scharfkantig.");
                }
                else if (cmd.Args.Any())
                {
                    await player.SendGameEventAsync($"Du suchst nach {string.Join(" ", cmd.Args)}");
                    await player.SendGameEventAsync($"Kannst aber nichts finden");
                }
                else
                {
                    await player.SendGameEventAsync("Du stehst in einem Dunklen Raum ohne etwas sehen zu können. Du fühlst den kalten steinernen [Boden] unter deinen Füßen.");
                    await player.SendGameEventAsync("Deine Rechte [Hand] hängt in irgendwas fest");
                    await player.SendGameEventAsync("Ein schmaler Lischtstrahl schimmert auf der Anderen seite des Raumes. Dort scheint eine tür zu sein.");
                }

            }
        }

    }
}
