using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Tournament
    {
        public string TournamentName { get; set; }
        public TournamentSettings Settings { get; set; }
        public List<Round> Rounds { get; set; } = new List<Round>();
        public List<Team> PreliminaryStandings { get; set; } = new List<Team>();



        public Tournament(string name, TournamentSettings settings)
        {
            TournamentName = name;
            Settings = settings;
        }
    }
}
