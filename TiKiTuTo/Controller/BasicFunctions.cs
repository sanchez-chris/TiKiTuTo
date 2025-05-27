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

        /// <summary>
        /// Used to add default players should none be given by the user.
        /// </summary>
        public static void InitializeDefaultPlayers(Team team)
        {
            // Add default players to the team
            for (int i = 1; i <= 2; i++) // Example: 2 default players per team
            {
                Player defaultPlayer = new Player($"DefaultPlayer{i} ({team.TeamName})");
                AddPlayer(defaultPlayer, team);
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

        /// <summary>
        /// For a given integer n, calculates the largest number p where p <= n and p = 2^x, where x is a natural number.
        /// </summary>
        /// <param name="n">The integer in question</param>
        /// <returns>The largest number smaller or equal to n which is a power of 2.</returns>
        public static int HighestPowerOf2(int n)
        {
            int p = (int)(Math.Log(n) /
                           Math.Log(2));
            return (int)Math.Pow(2, p);
        }
        

    }
}