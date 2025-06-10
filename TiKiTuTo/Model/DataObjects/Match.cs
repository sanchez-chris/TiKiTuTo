using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiKiTuTo.Model.DataObjects

{
    public class Match
    {
        public Team teamA { get; set; }
        public Team teamB { get; set; }
        public int goalsTeamA { get; set; } = 0;
        public int goalsTeamB { get; set; } = 0;

        public bool isFinished { get; set; } = false;

        public Match(Team teamA, Team teamB)
        {
            this.teamA = teamA;
            this.teamB = teamB;
        }

        public Match() { }

    }
}