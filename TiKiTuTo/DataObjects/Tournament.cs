using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiKiTuTo.Model
{
    public class Tournament
    {
        public string TournamentName { get; set; }
        public TournamentSettings Settings { get; set; }
        public List<Team> PreliminaryStandings { get; set; } = new List<Team>();
        public List<Match> GamePlanPremilimaryRound { get; set; } = new List<Match>();



        public Tournament(string name, TournamentSettings settings)
        {
            TournamentName = name;
            Settings = settings;
        }
    }
}
