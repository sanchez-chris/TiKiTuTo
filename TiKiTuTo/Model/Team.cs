using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Team
    {
        public string TeamName { get; set; }
        public List<Player> PlayerInTeam { get; set; }
        public int NumberGamesWon { get; set; } = 0;
        /// <summary>
        /// Konstruktor der Klasse Team, benötigt den Namen des Teams und eine Liste von Teammitgliedern, falls keine angegeben werden werden diese mit default...
        /// </summary>
        /// <param name="name"></param>
        /// <param name="InitialPlayers"></param>
        public Team(string name, List<Player> ?InitialPlayers = null)
        {
            TeamName = name;
            PlayerInTeam = new List<Player>();

/*            if (initialPlayers != null && initialPlayers.Count > 0)
            {
                foreach (Player player in initialPlayers)
                {
                    AddPlayer(player);
                }
            }
            else
            {
                InitializeDefaultPlayers(); // Initialize default players if no initial players are provided
            }*/
        }
    }
}
