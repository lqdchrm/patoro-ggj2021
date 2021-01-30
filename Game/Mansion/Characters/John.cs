using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Game.Mansion.Characters
{
    class John : BaseCharacter
    {
        public override string StartLocation => "Labor";
        public override string[] Prolog => new[] {
@"Es ist dunkel. Du hetzt durch die kalte Nacht.",
@"Su weißt nicht warum du rennst, aber irgendwas treibt dicht an.",
@"Der Wind heult durch die baumwipfel, die Zweige schlagen dir ins Gesicht.",
@"Der Regen Prasselt lautstartk nieder, jedes Gereuch verdekend. Nur ab und zu übertönt von einem Lauten Donner.",
@"Der dichte Wald macht es kaum möglich etwas zu erkennen. Nur ab und an, wenn sich der Vollmond kurz gegen die dichte Wolkendecke durchzusetzen kann, oder Ein Blitz die Szenerie in Schwarz und weiß in Szene setzt, kannst du dich Orientieren.",
@"Aber deine Gedanken sind vernebelt.",
@"Du kannst nur immer weiter Rennen.",
@"Während hypnotisch der Regen auf deinen Körper daniederschlägt und sich der Geschmack von Eisen in deinem Mund ausbreitet.",
@"Dann spaltet sich mit einem markerschütterndem Knall die Erde und 5 risige verdorrte Arme greifen nach dir und halten dich fest.",
@"Du musst weiter rennen, doch du kannst nicht. Die Hände, jede so groß wie dein Brustkorb fixieren deine Glieder und ziehen dich in die Feurige Tiefe.",
@"...",
@"...",
@"...",
@"Der Kalte Boden hilft dir einen Klaren gedanken zu fassen.",
@"Langsam öffnest du die Augen. Ein Dunkler Raum. Es dauert einen Moment bis deine Gedanken klar werden. Ein Flackerndes Licht erhelt den Raum und hilft dir dich zi Orientieren. Und ein stechender Schmerz durchfährt deinen rechten Arm als du dich bewegst.",

        };
    }
}
