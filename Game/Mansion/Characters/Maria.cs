namespace LostAndFound.Game.Mansion.Characters
{
    class Maria : BaseCharacter
    {
        public override string StartLocation => "Wohnzimmer";

        public override string[] Prolog => new[] {
@"Du liegst auf einem Tepisch. An der Decke Tanzen die Schatten angetrieben durch das Feuer das Draußen vor dem Fenster lodert.",
@"Ein Blitz ist ein geschlagen und scheint einen Baum entzündet zu haben.",
@"Langsam läßt deine Paralyse nach und du Kannst dich wieder Bewegen.",
@"Du weißt noch das du auf dem Weg zur Arbeit warst. Aber das Ist das letzte an das du dich errinnerst.",
@"Nicht wie du in diese Haus gelangt bis, und erst recht nicht was das für ein Haus ist.",
@"Die Nacht ist bereits herein gebrochen. Wieviel Zeit wohl vergangen ist?",

        };
    }
}
