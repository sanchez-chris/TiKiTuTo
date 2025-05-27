using View;
using Model;

namespace Controller
{
    /// <summary>
    /// This class implements all methods which retrieve user input, for integers as well as strings.
    /// </summary>
    public class InputHandler
    {

        IView View { get; set; }
        readonly int maxMenuOption = 5;

        public InputHandler(IView view) 
        {
            View = view;
        }

        /// <summary>
        /// Asks user for a number to perform a Menu option.
        /// </summary>
        /// <returns>A number guaranteed to trigger a valid option in the main menu.</returns>
        public int GetValidMenuInput()
        {
            int MenuInput = GetNumber($"Please choose what to do (pick a number between 1 and {maxMenuOption}).");

            while (!InputValidator.IsValidMenuInput(MenuInput, maxMenuOption))
            {
                MenuInput = GetNumber($"This was not a valid number!");
            }

            return MenuInput;
        }

        /// <summary>
        /// Asks user for the number of total teams in a tournament until a valid value is entered.
        /// </summary>
        /// <returns>The number of teams playing in a tournament, guaranteed to be a valid value.</returns>
        public int GetValidNumberOfTotalTeams()
        {
            int NumberOfTeams = GetNumber($"How many teams are going to join this tournament?");

            while (!InputValidator.IsValidNumberOfTotalTeams(NumberOfTeams))
            {
                NumberOfTeams = GetNumber("This is not a valid number of teams (minimum 4, maximum 256).");
            }

            return NumberOfTeams;
        }

        /// <summary>
        /// Asks user for the number of games each team should play in the preliminaries until a valid value is entered.
        /// </summary>
        /// <param name="NumberOfTeamsTotal"> The total number of teams in the tournament defines which values are valid</param>
        /// <returns>The number of games each team has to play in the preliminaries, guaranteed to be a valid value.</returns>
        public int GetValidPreliminaryGames(int NumberOfTeamsTotal)
        {
            int NumberOfPreliminaryGames = GetNumber("How many games should each team play in the preliminaries?");

            while (!InputValidator.IsValidNumberOfPreliminaryGamesPerTeam(NumberOfPreliminaryGames, NumberOfTeamsTotal))
            {
                NumberOfPreliminaryGames = GetNumber($"This is not a valid number of games per team (minimum 1, maximum {NumberOfTeamsTotal-1}).");
            }
            return NumberOfPreliminaryGames;
        }

        /// <summary>
        /// Asks user for the number of teams progressing into the KO round. Has to be a power of 2 and not larger than the total number of teams.
        /// </summary>
        /// <param name="NumberOfTeamsTotal"> The total number of teams in the tournament defines an upper bound to the number of teams progressing.</param>
        /// <returns>The number of teams progressing into KO, guaranteed to be a valid value.</returns>
        public int GetValidNumberOfTeamsInKORound(int NumberOfTeamsTotal)
        {
            int maxAllowed = BasicFunctions.HighestPowerOf2(NumberOfTeamsTotal);
            int NumberOfTeamsInKO = GetNumber($"How many teams should continue into the KO phase (minimum 2, maximum {maxAllowed})?");
            while (!InputValidator.IsValidNumberOfTeamsInKORound(NumberOfTeamsInKO, NumberOfTeamsTotal))
            {
                NumberOfTeamsInKO = GetNumber($"This is not a valid number of teams for KO round (has to be 2^n, minimum 2, maximum {maxAllowed}).");
            }
            return NumberOfTeamsInKO;
        }




        
        /// <summary>
        /// Asks the user to enter a number, using the prompt argument. Repeats until a valid number is added.
        /// </summary>
        /// <param name="prompt">Message shown to the user to prompt input.</param>
        /// <returns>an integer entered by the user.</returns>
        public int GetNumber(string prompt)
        {
            int result;

            View.ShowMessage(prompt);
            string? userInput = View.ReadInput();

            while (!int.TryParse(userInput, out result))
            {
                View.ShowMessage($"\"{userInput}\" is not a valid number input, please try again!");
                userInput = View.ReadInput();
            }
            return result;
        }

        
        
        //TODO: ADJUST XML COMMENT FOR GetXYName methods
        
        /// <summary>
        /// Asks the user to enter any string, using the prompt argument. Repeats until valid string is entered.
        /// </summary>
        /// <param name="prompt">Message shown to the user to prompt input.</param>
        /// <param name="emptyAllowed">boolean defining whether empty result is OK.</param>
        /// <returns>a string entered by the user.</returns>
        public string? GetPlayerName(string prompt, bool emptyAllowed)
        {
            View.ShowMessage(prompt); 
            string? userInput = View.ReadInput();


            while (string.IsNullOrEmpty(userInput) && !emptyAllowed)
            {
                View.ShowMessage("Your input can not be empty.");
                userInput = View.ReadInput();
            }
            View.ShowMessage($"Welcome {userInput}");
            return userInput;
        }

        public string? GetTeamName(string prompt, bool emptyAllowed)
        {
            View.ShowMessage(prompt);
            string? userInput = View.ReadInput();


            while (string.IsNullOrEmpty(userInput) && !emptyAllowed)
            {
                View.ShowMessage("Your input can not be empty.");
                userInput = View.ReadInput();
            }
            View.ShowMessage($"Creating Team {userInput}");
            return userInput;
        }

        public string GetTournamentName(string prompt, bool emptyAllowed)
        {
            View.ShowMessage(prompt);

            string userInput = View.ReadInput();


            while (string.IsNullOrEmpty(userInput) && !emptyAllowed)
            {
                View.ShowMessage("Your input can not be empty.");
                userInput = View.ReadInput();
            }
            View.ShowMessage($"Tournament {userInput} created.");
            return userInput;
        }
    }
}
