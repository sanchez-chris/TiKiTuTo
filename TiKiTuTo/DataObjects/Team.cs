using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using TiKiTuTo.Controller;

namespace TiKiTuTo.Model
{
    public class Team
    {
        public string TeamName { get; set; }
        public List<Player> PlayerInTeam { get; set; }
        public int NumberGamesWon { get; set; } = 0;
        public int NumberGoals { get; set; } = 0;
        public int Goaldifference { get; set; } = 0;

        /// <summary>
        /// Constructor for the Team class. Requires a team name and an optional list of initial players.
        /// If no players are provided, default players will be added.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="initialPlayers"></param>
        public Team(string name, List<Player>? initialPlayers = null)
        {
            TeamName = name;
            PlayerInTeam = new List<Player>();
        }
    }
}
