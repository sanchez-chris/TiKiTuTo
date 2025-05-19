using System.ComponentModel.Design;
using View = TiKiTuTo.View;
using TiKiTuTo.Model;

namespace TiKiTuTo.Controller
{
    public class BasicFunctions
    {







        //TODO
        public static int GetValidNumberOfTotalTeams()
        {
            int NumberOfTeams = 0;
            return NumberOfTeams;
        }

        //TODO
        public static int GetValidPreliminaryGames(int NumberOfTeamsTotal)
        {
            int NumberOfPreliminaryGames = 0;
            return NumberOfPreliminaryGames;
        }

        //TODO
        public static int GetValidNumberOfTeamsInKORound(int NumberOfTeamsTotal)
        {
            int NumberOfTeams = 0;
            return NumberOfTeams;
        }



        public static Team CreateTeam(int i)
        {
            bool emptyNameAllowed = true;
            string? name = GetName($"Please enter the name of the team. Default name when empty: Team {i}.", emptyNameAllowed);

            if (name == null) 
            {
                name = $"Team {i}";
            }
            
            View.ShowMessage($"Welcome {name}");

            //How do we add the players?
            //define number of players before?
            //do all teams need the same number of players?
            //Stop once the user gives a certain input?

            Team team = new Team(name, players);

            return team;
        }




        public static List<Team> CreateNTeams(int NumberOfTeamsTotal)
        {
            List<Team> teams = new List<Team>();

            for (int i = 1; i <= NumberOfTeamsTotal; i++)
            {
                View.ShowMessage($"Creating Team {i}");
                teams.Add(CreateTeam(i));
            }
            return teams;
        }

        /// <summary>
        /// Asks the user to enter a number, using the prompt argument. Repeats until a valid number is added.
        /// </summary>
        /// <param name="prompt">Message shown to the user to prompt input.</param>
        /// <returns>an integer entered by the user.</returns>
        public static int GetNumber(string prompt)
        {
            int result = 0;

            View.ShowMessage(prompt);

            
            while (result == 0) 
            {
                View.ShowMessage("Please enter a natural number.");
                string? userInput = ReadInput();
                bool _ = int.TryParse(userInput, out result);
            }
            return result;
        }

        /// <summary>
        /// Asks the user to enter any string, using the prompt argument. Repeats until valid string is entered.
        /// </summary>
        /// <param name="prompt">Message shown to the user to prompt input.</param>
        /// /// <param name="emptyAllowed">boolean defining whether empty result is OK.</param>
        /// <returns>a string entered by the user.</returns>
        public static string? GetName(string prompt, bool emptyAllowed)
        {
            View.ShowMessage(prompt); 
            
            string? userInput = ReadInput();

            
            while (userInput == null & !emptyAllowed)
            {
                View.ShowMessage("Your input can not be empty.");
                userInput = ReadInput();
            }
            return userInput;
        }


        /// <summary>
        /// For lack of an abstraction of the user input surface, this simply calls Console.Readline (and violates MVC separation :'( )
        /// </summary>
        /// <returns>a string entered by the user</returns>
        public static string? ReadInput()
        {
            return Console.ReadLine();
        }
    }
}