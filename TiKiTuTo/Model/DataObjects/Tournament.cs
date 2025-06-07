using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiKiTuTo.Model.DataObjects
{
    public class Tournament
    {
        public string TournamentName { get; set; }
        public TournamentSettings TournamentSettings { get; set; }
        public List<Team> PreliminaryStandings { get; set; } = new List<Team>();
        public List<Team> KoStandings { get; set; } = new List<Team>();
        public List<Match> GamePlanPreliminaryRound { get; set; } = new List<Match>();
        public List<List<Match>> GamePlanKoRound { get; set; } = new List<List<Match>>();
        public Team Winner { get; set; }
        public Team Finalist { get; set; }
        public List<Team> Semifinalists { get; set; } = new List<Team>();
        public Team ThirdPosition { get; set; }
        public bool IsSemifinalPlayed { get; set; } = false;
        public int CurrentRound { get; set; } = 0;

        public Tournament(string name, TournamentSettings settings)
        {
            TournamentName = name;
            TournamentSettings = settings;
        }

        public Tournament() { }
    }
}
