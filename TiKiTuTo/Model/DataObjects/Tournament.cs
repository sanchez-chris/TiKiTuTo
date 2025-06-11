

namespace TiKiTuTo.Model.DataObjects
{
    public class Tournament
    {
        public string TournamentName { get; set; }
        public TournamentSettings TournamentSettings { get; set; }
        public List<Team> PreliminaryStandings { get; set; } = new();
        public List<Team> KoStandings { get; set; } = new();
        public List<Match> GamePlanPreliminaryRound { get; set; } = new();
        public List<List<Match>> GamePlanKoRound { get; set; } = new();
        public Team Winner { get; set; }
        public Team Finalist { get; set; }
        public List<Team> Semifinalists { get; set; } = new();
        public Team ThirdPosition { get; set; }
        public bool IsSemifinalPlayed { get; set; }
        public int CurrentKoRound { get; set; }
        public bool IsFinished { get; set; }
        

        public Tournament(string name, TournamentSettings settings)
        {
            TournamentName = name;
            TournamentSettings = settings;
        }

        public Tournament() { }
    }
}
