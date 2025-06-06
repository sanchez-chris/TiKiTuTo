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
        public TournamentSettings Settings { get; set; }
        public List<Team> PreliminaryStandings { get; set; } = new List<Team>();
        public List<Team> KoStandings { get; set; } = new List<Team>();
        public List<Match> GamePlanPreliminaryRound { get; set; } = new List<Match>();
        public List<List<Match>> GamePlanKoRound { get; set; } = new List<List<Match>>();
        public Team winner = new Team();

        public Team finalist = new Team();
        public List<Team> semifinalists = new List<Team>();
        public Team thirdPosition = new Team("3.");
        public bool isSemifinalPlayed = false;
        public int currentRound = 0;





        public Tournament(string name, TournamentSettings settings)
        {
            TournamentName = name;
            Settings = settings;
        }

        public Tournament() { }
    }
}
