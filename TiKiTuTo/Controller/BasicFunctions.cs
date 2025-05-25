using System.ComponentModel.Design;
using TiKiTuTo.View;
using TiKiTuTo.Model;

namespace TiKiTuTo.Controller
{
    public class BasicFunctions
    {

        public static Team CreateTeam(int i, IView View)
        {
            bool emptyNameAllowed = true;
            string? name = InputHandler.GetName($"Please enter the name of the team. Default name when empty: Team {i}.", emptyNameAllowed, View);

            if (string.IsNullOrEmpty(name))
            {
                name = $"Team {i}";
            }

            View.ShowMessage($"Welcome {name}");

            //How do we add the players?
            //define number of players before?
            //do all teams need the same number of players?
            //Stop once the user gives a certain input?

            List<Player> playerList = new List<Player>();

            Team team = new Team(name, playerList);

            return team;
        }




        public static List<Team> CreateNTeams(int NumberOfTeamsTotal, IView View)
        {
            List<Team> teams = new List<Team>();

            for (int i = 1; i <= NumberOfTeamsTotal; i++)
            {
                View.ShowMessage($"Creating Team {i}");
                teams.Add(CreateTeam(i, View));
            }
            return teams;
        }

    }
}