using View;
using Model;

namespace Controller
{
    /// <summary>
    /// This class implements all methods which retrieve user input, for integers as well as strings.
    /// </summary>
    public class InputHandler
    {

        IView _View { get; set; }

        public InputHandler(IView view) 
        {
            _View = view;
        }

        /// <summary>
        /// Asks user for a number to perform a Menu option.
        /// </summary>
        /// <returns>A number guaranteed to trigger a valid option in the main menu.</returns>
        public int GetValidMenuInput()
        {
            int choice = GetNumber($"Please choose what to do (pick a number).");

            while (!InputValidator.IsValidMenuInput(choice))
            { 
                choice = GetNumber($"This was not a valid number!");
            }

            return choice;
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
                _View.ShowMessage("Try again (info what is wrong and how to do it right)");
                NumberOfTeams = GetNumber("Please give me the number now!!!");
            }

            return NumberOfTeams;
        }

        //TODO
        public int GetValidPreliminaryGames(int NumberOfTeamsTotal)
        {
            int NumberOfPreliminaryGames = 0;
            return NumberOfPreliminaryGames;
        }

        //TODO
        public int GetValidNumberOfTeamsInKORound(int NumberOfTeamsTotal)
        {
            int NumberOfTeams = 0;
            return NumberOfTeams;
        }




        
        /// <summary>
        /// Asks the user to enter a number, using the prompt argument. Repeats until a valid number is added.
        /// </summary>
        /// <param name="prompt">Message shown to the user to prompt input.</param>
        /// <returns>an integer entered by the user.</returns>
        public int GetNumber(string prompt)
        {
            int result;
            string? userInput = _View.ReadInput();

            _View.ShowMessage(prompt);

            while (!int.TryParse(userInput, out result))
            {
                _View.ShowMessage($"\"{userInput}\" is not a valid input. Try again!");
                userInput = _View.ReadInput();
            }
            return result;
        }

        /// <summary>
        /// Asks the user to enter any string, using the prompt argument. Repeats until valid string is entered.
        /// </summary>
        /// <param name="prompt">Message shown to the user to prompt input.</param>
        /// <param name="emptyAllowed">boolean defining whether empty result is OK.</param>
        /// <param name="_View">The specific _View implementation to interact with.</param>
        /// <returns>a string entered by the user.</returns>
        public string? GetPlayerName(string prompt, bool emptyAllowed)
        {
            _View.ShowMessage(prompt); 
            
            string? userInput = _View.ReadInput();

            
            while (userInput == null & !emptyAllowed)
            {
                _View.ShowMessage("Your input can not be empty.");
                userInput = _View.ReadInput();
            }
            _View.ShowMessage($"Welcome {userInput}");
            return userInput;
        }

        public string? GetTeamName(string prompt, bool emptyAllowed)
        {
            _View.ShowMessage(prompt);

            string? userInput = _View.ReadInput();


            while (userInput == null & !emptyAllowed)
            {
                _View.ShowMessage("Your input can not be empty.");
                userInput = _View.ReadInput();
            }
            _View.ShowMessage($"Creating Team {userInput}");
            return userInput;
        }

        public string GetTournamentName(string prompt, bool emptyAllowed)
        {
            _View.ShowMessage(prompt);

            string userInput = _View.ReadInput();


            while (userInput == null & !emptyAllowed)
            {
                _View.ShowMessage("Your input can not be empty.");
                userInput = _View.ReadInput();
            }
            _View.ShowMessage($"Tournament {userInput} created.");
            return userInput;
        }
    }
}