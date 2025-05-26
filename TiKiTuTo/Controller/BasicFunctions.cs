using System.ComponentModel.Design;
using View;
using Model;

namespace Controller
{
    public class BasicFunctions
    {
        /// <summary>
        /// Used to add a Player to a Team, needs a Player Object
        /// </summary>
        /// <param name="player"></param>
        public static void AddPlayer(Player player, Team team)
        {
            if (!team.PlayerInTeam.Contains(player))
            {
                team.PlayerInTeam.Add(player);
            }
        }

        public static Team CreateTeam(int i, InputHandler inputHandler)
        {
            bool emptyNameAllowed = true;
            string? name = inputHandler.GetTeamName($"Please enter the name of the team. Default name when empty: Team {i}.", emptyNameAllowed);

            if (string.IsNullOrEmpty(name))
            {
                name = $"Team {i}";
            }
            //How do we add the players?
            //define number of players before?
            //do all teams need the same number of players?
            //Stop once the user gives a certain input?

            List<Player> playerList = new List<Player>();

            Team team = new Team(name, playerList);

            return team;
        }

        public static List<Team> CreateListOfTeams(int NumberOfTeamsTotal, InputHandler inputHandler)
        {
            List<Team> teams = new List<Team>();

            for (int i = 1; i <= NumberOfTeamsTotal; i++)
            {
                teams.Add(CreateTeam(i, inputHandler));
            }
            return teams;
        }

        /// <summary>
        /// Enter goals made and goals received to update the TeamScore of the winner.
        /// </summary>
        /// <param name="goalsMade"></param>
        /// <param name="goalsReceived"></param>
        public static void AddOneWinToTeam(int goalsMade, int goalsReceived, Team team)
        {
            if (goalsMade > goalsReceived)
            {
                team.NumberGamesWon++;
            }
        }

    }
}