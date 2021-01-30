namespace LostAndFound.Game.Mansion.Characters
{
    class Maria : BaseCharacter
    {
        public override string StartLocation => "Wohnzimmer";

        public override string[] Prolog => new[] {
@"Du liegst auf einem Teppich. An der Decke tanzen die Schatten angetrieben durch das Feuer, das draußen vor dem Fenster lodert.",
@"Ein Blitz ist eingeschlagen und scheint einen Baum entzündet zu haben.",
@"Langsam lässt deine Paralyse nach und du kannst dich wieder Bewegen.",
@"Du weißt noch, dass du auf dem Weg zur Arbeit warst. Aber das ist das Letzte, an das du dich erinnerst.",
@"Nicht, wie du in diese Haus gelangt bist, und erst recht nicht was das für ein Haus ist.",
@"Die Nacht ist bereits herein gebrochen. Wieviel Zeit wohl vergangen ist?",

        };
    }
}
