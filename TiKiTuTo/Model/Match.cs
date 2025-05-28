using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model

{
    public class Match
    {
        public Team TeamA { get; set; }
        public Team TeamB { get; set; }
        public int GoalsTeamA { get; set; } = 0;
        public int GoalsTeamB { get; set; } = 0;
        public bool Finished { get; private set; }

        public Match(Team teamA, Team teamB)
        {
            TeamA = teamA;
            TeamB = teamB;
            Finished = false;
        }

    }
}