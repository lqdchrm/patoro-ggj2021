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
@"Du weißt nicht warum du rennst, aber irgendwas treibt dich an.",
@"Der Wind heult durch die Baumwipfel, die Zweige schlagen dir ins Gesicht.",
@"Der Regen prasselt lautstartk nieder, jedes Geräusch verdeckend, nur ab und zu übertönt von einem lauten Donner.",
@"Der dichte Wald macht es kaum möglich etwas zu erkennen. Nur ab und an, wenn sich der Vollmond kurz gegen die dichte Wolkendecke durchzusetzen vermag, oder ein blitz die Szenerie in schwarz und weiß in Szene setzt, kannst du dich orientieren.",
@"Aber deine Gedanken sind vernebelt.",
@"Du kannst nur immer weiter rennen,",
@"während hypnotisch der Regen auf deinen Körper niederschlägt und sich der Geschmack von Eisen in deinem Mund ausbreitet.",
@"Dann spaltet sich mit einem markerschütternden Knall die Erde und 5 rissige verdorrte Arme greifen nach dir und halten dich fest.",
@"Du musst weiter rennen, doch du kannst nicht. Die Hände, jede so groß wie dein Brustkorb, fixieren deine Glieder und ziehen dich in die feurige Tiefe.",
@"...",
@"...",
@"...",
@"Der kalte Boden hilft dir einen klaren Gedanken zu fassen.",
@"Langsam öffnest du die Augen. Ein dunkler Raum. Es dauert einen Moment bis deine Gedanken klar werden. Ein Flackerndes Licht erhellt den Raum und hilft dir dich zu orientieren. Ein stechender Schmerz durchfährt deinen rechten Arm als du dich bewegst.",

        };
    }
}
