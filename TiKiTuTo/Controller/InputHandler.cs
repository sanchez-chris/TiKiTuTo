using System.ComponentModel.Design;
using TiKiTuTo.View;
using TiKiTuTo.Model;

namespace TiKiTuTo.Controller
{
    static class InputHandler
    {


        //public IView View { get; }
        
        //public InputHandler(IView view) 
        //{ 
        //    View = view; 
        //}




        /// <summary>
        /// Asks user for the number of total teams until a valid value is entered
        /// </summary>
        /// <returns></returns>
        static int GetValidNumberOfTotalTeams(IView View)
        {
            int NumberOfTeams = GetNumber($"How many teams are going to join this tournament?", View);

            while (!InputValidator.IsValidNumberOfTotalTeams(NumberOfTeams))
            {
                View.ShowMessage("Try again (info what is wrong and how to do it right)");
                NumberOfTeams = GetNumber("Please give me the number now!!!", View);
            }

            return NumberOfTeams;
        }

        //TODO
        public static int GetValidPreliminaryGames(int NumberOfTeamsTotal, IView View)
        {
            int NumberOfPreliminaryGames = 0;
            return NumberOfPreliminaryGames;
        }

        //TODO
        public static int GetValidNumberOfTeamsInKORound(int NumberOfTeamsTotal, IView View)
        {
            int NumberOfTeams = 0;
            return NumberOfTeams;
        }



        public static Team CreateTeam(int i, IView View)
        {
            bool emptyNameAllowed = true;
            string? name = GetName($"Please enter the name of the team. Default name when empty: Team {i}.", emptyNameAllowed, View);

            if (name == null) 
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

        /// <summary>
        /// Asks the user to enter a number, using the prompt argument. Repeats until a valid number is added.
        /// </summary>
        /// <param name="prompt">Message shown to the user to prompt input.</param>
        /// <returns>an integer entered by the user.</returns>
        public static int GetNumber(string prompt, IView View)
        {
            int result = 0;

            View.ShowMessage(prompt);

            
            while (result == 0) 
            {
                View.ShowMessage("Please enter a natural number.");
                string? userInput = View.ReadInput();
                bool _ = int.TryParse(userInput, out result);
            }
            return result;
        }

        /// <summary>
        /// Asks the user to enter any string, using the prompt argument. Repeats until valid string is entered.
        /// </summary>
        /// <param name="prompt">Message shown to the user to prompt input.</param>
        /// <param name="emptyAllowed">boolean defining whether empty result is OK.</param>
        /// <param name="View">The specific View implementation to interact with.</param>
        /// <returns>a string entered by the user.</returns>
        public static string? GetName(string prompt, bool emptyAllowed, IView View)
        {
            View.ShowMessage(prompt); 
            
            string? userInput = View.ReadInput();

            
            while (userInput == null & !emptyAllowed)
            {
                View.ShowMessage("Your input can not be empty.");
                userInput = View.ReadInput();
            }
            return userInput;
        }
    }
}