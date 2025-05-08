using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiKiTuTo.Classes
{
    public class Tournament
    {
        public int TournamentID { get; set; }
        public string TournamentName { get; set; }
        public int TeamCount { get; set; }
        public int Prelimininaries { get; set; }
        public List<Team> Teams { get; set; }
        public List<Round> Rounds { get; set; }
        public int ActiveRoundIndex { get; set; }

        public Tournament(string name)
        {
            TournamentName = name;
        }

        public void AddTeam(Team team)
        {
            if (!Teams.Contains(team))
            {
                Teams.Add(team);
            }
        }



    }
}
