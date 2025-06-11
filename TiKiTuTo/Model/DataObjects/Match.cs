
namespace TiKiTuTo.Model.DataObjects

{
    public class Match
    {
        public Team TeamA { get; set; }
        public Team TeamB { get; set; }
        public int GoalsTeamA { get; set; } = 0;
        public int GoalsTeamB { get; set; } = 0;

        public bool IsFinished { get; set; } = false;

        public Match(Team teamA, Team teamB)
        {
            this.TeamA = teamA;
            this.TeamB = teamB;
        }

        public Match() { }

    }
}