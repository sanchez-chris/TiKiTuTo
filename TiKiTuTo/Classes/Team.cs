using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiKiTuTo.Classes
{

    public class Team
    {
        public string TeamName { get; set; }
        public List<Player> TeamMembers { get; set; }
        public int GamesPlayed { get; private set; }
        public int GamesWon { get; private set; }
        public int GoalsTotal { get; private set; }
        public int GoalDifference { get; private set; }


        public Team(string name, List<Player> initialPlayers = null)
        {
            TeamName = name;
            TeamMembers = new List<Player>();
            GamesPlayed = 0;
            GamesWon = 0;
            GoalsTotal = 0;
            GoalDifference = 0;

            if (initialPlayers != null && initialPlayers.Count > 0)
            {
                foreach (Player player in initialPlayers)
                {
                    AddPlayer(player);
                }
            }
            else
            {
                InitializeDefaultPlayers(); // Initialize default players if no initial players are provided
            }
        }

        private void InitializeDefaultPlayers()
        {
            // Add default players to the team
            for (int i = 1; i <= 2; i++) // Example: 2 default players per team
            {
                Player defaultPlayer = new Player($"DefaultPlayer{i} ({TeamName})");
                AddPlayer(defaultPlayer);
            }
        }


        public void UpdateTeamScore(int goalsMade, int goalsReceived)
        {
            GamesPlayed++;
            GoalsTotal += goalsMade;
            GoalDifference += goalsMade - goalsReceived;
            if (goalsMade > goalsReceived)
            {
                GamesWon++;
            }
        }
        public void AddPlayer(Player player)
        {
            if (!TeamMembers.Contains(player))
            {
                TeamMembers.Add(player);
            }
        }
    }

}
