using TiKiTuTo.View;
using TiKiTuTo.Model.BusinessLogic;

namespace TiKiTuTo.Controller
{
    /// This class implements all methods which retrieve user input, for integers as well as strings.
    /// Handles validation and re-prompting of user input for tournament configuration and gameplay.
    public class InputHandler
    {

        public IView View { get; set; }
        private readonly InputValidator _inputValidator;

        public InputHandler(IView view, InputValidator inputValidator)
        {
            View = view;
            _inputValidator = inputValidator;
        }

        /// <summary>
        /// Prompts the user for a valid goal count for a specific team and validates the input.
        /// </summary>
        /// <param name="teamName">The name of the team scoring the goals.</param>
        /// <returns>A valid number of goals (between 0 and 10).</returns>
        public int GetValidGoalInput(string teamName)
        {
            int goalInput = GetNumber($"\nHow many goals has {teamName} made?");

            while (!_inputValidator.IsValidGoalInput(goalInput))
            {
                goalInput = GetNumber("This was not a valid goal Input [italic grey]Max value is 10[/]");
            }

            return goalInput;
        }

        /// <summary>
        /// Asks user for the number of total teams in a tournament until a valid value is entered.
        /// </summary>
        /// <returns>The number of teams playing in a tournament, guaranteed to be a valid value.</returns>
        public int GetValidNumberOfTotalTeams()
        {
            int numberOfTeams = GetNumber("How many teams are going to join this tournament? [italic grey]minimum 4, maximum 256[/]");

            while (!_inputValidator.IsValidNumberOfTotalTeams(numberOfTeams))
            {
                numberOfTeams = GetNumber("This is [bold red]not[/] a valid number of teams for a tournament. [italic grey]minimum 4, maximum 256[/]");
            }

            return numberOfTeams;
        }

        /// <summary>
        /// Asks user for the number of games each team should play in the preliminaries until a valid value is entered.
        /// </summary>
        /// <param name="numberOfTeamsTotal"> The total number of teams in the tournament defines which values are valid</param>
        /// <returns>The number of games each team has to play in the preliminaries, guaranteed to be a valid value.</returns>
        public int GetValidNumberOfPreliminaryGames(int numberOfTeamsTotal)
        {
            int numberOfPreliminaryGames = GetNumber($"How many games should each team play in the preliminaries? [italic grey]minimum 1, maximum {numberOfTeamsTotal - 1}[/]");

            while (!_inputValidator.IsValidNumberOfPreliminaryGamesPerTeam(numberOfPreliminaryGames, numberOfTeamsTotal))
            {
                numberOfPreliminaryGames = GetNumber("This is [bold red]not[/] a valid number of games per team.");
            }
            return numberOfPreliminaryGames;
        }

        /// <summary>
        /// Asks user for the number of teams progressing into the KO round. Has to be a power of 2 and not larger than the total number of teams.
        /// </summary>
        /// <param name="numberOfTeamsTotal"> The total number of teams in the tournament defines an upper bound to the number of teams progressing.</param>
        /// <returns>The number of teams progressing into KO, guaranteed to be a valid value.</returns>
        public int GetValidNumberOfTeamsInKORound(int numberOfTeamsTotal)
        {
            int maxAllowed = BasicFunctions.HighestPowerOf2(numberOfTeamsTotal);
            int numberOfTeamsInKo = GetNumber($"How many teams should continue into the KO phase? [italic grey]minimum 2, maximum {maxAllowed}[/]");
            while (!_inputValidator.IsValidNumberOfTeamsInKORound(numberOfTeamsInKo, numberOfTeamsTotal))
            {
                numberOfTeamsInKo = GetNumber($"This is [bold red]not[/] a valid number of teams for the KO round. [italic grey]minimum 2, maximum {maxAllowed}[/].");
            }
            return numberOfTeamsInKo;
        }


        /// <summary>
        /// Prompts the user for a valid match duration in minutes.
        /// </summary>
        /// <returns>A valid match duration in minutes (between 1 and 30).</returns>
        public int GetValidMatchDuration()
        {
            int minAllowed = 1;
            int maxAllowed = 30;
            string prompt = $"How many minutes should each match run for? [italic gray]minimum: {minAllowed}, maximum: {maxAllowed}[/]";
            int matchDuration = GetNumber(prompt);
            while (!(matchDuration >= minAllowed && matchDuration <= maxAllowed))
            {
                matchDuration = GetNumber($"This is [bold red]not[/] a valid duration for the timer. [italic gray]minimum: {minAllowed}, maximum: {maxAllowed}[/]");
            }
            return matchDuration;
        }

        /// <summary>
        /// Prompts the user for a yes/no response and returns the corresponding boolean value.
        /// </summary>
        /// <param name="prompt">The message to display when asking for approval.</param>
        /// <returns>True if the user approves (y/Y/yes/YES), false if the user disapproves (n/N/no/NO).</returns>
        public bool GetApproval(string prompt)
        {
            List<string> validInputsYes = ["y", "Y", "YES", "yes"];
            List<string> validInputsNo = ["n", "N", "NO", "no"];

            bool result = false;
            bool answerGiven = false;
            View.ShowMessage(prompt);
            
            while(!answerGiven)
            {
                string userInput = View.ReadInput();

                if (validInputsYes.Contains(userInput))
                {
                    result = true;
                    answerGiven = true;
                }
                else if (validInputsNo.Contains(userInput))
                {
                    result = false;
                    answerGiven = true;
                }
                else View.ShowMessage($"\"{userInput}\" is [bold red]not[/] a valid input, try again! [y/n]");
            }
            return result;
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
            string userInput = View.ReadInput();

            while (!int.TryParse(userInput, out result))
            {
                View.ShowMessage($"\"{userInput}\" is [bold red]not[/] a valid number input, please try again!");
                userInput = View.ReadInput();
            }
            return result;
        }

        /// <summary>
        /// Prompts the user to enter a player name. If no name is entered, generates a default name.
        /// </summary>
        /// <param name="prompt">Message shown to the user to prompt input.</param>
        /// <param name="playerNumber">Index of the player being created. Used for generating default names.</param>
        /// <returns>The player name entered by the user or a default name if none was provided.</returns>
        public string GetPlayerName(string prompt, int playerNumber)
        {
            View.ShowMessage(prompt);
            string playerName = View.ReadInput();

            if (string.IsNullOrEmpty(playerName))
            {
                playerName = $"Player {playerNumber}";
            }
            View.ShowMessage($"Welcome {playerName}");
          return playerName;
        }

        /// <summary>
        /// Asks the user to enter a team name string, using the prompt argument. Defaults to a team name
        /// using teamNumber.
        /// </summary>
        /// <param name="prompt">Message shown to the user to prompt input.</param>
        /// <param name="teamNumber">Index of the player created. Used for default names.</param>
        /// <returns>A player name.</returns>
        public string GetTeamName(string prompt, int teamNumber)
        {
            View.ShowMessage(prompt);
            string teamName = View.ReadInput();

            if (string.IsNullOrEmpty(teamName))
            {
                teamName = $"Team {teamNumber}";
            }

            View.ShowMessage($"Creating Team {teamName}");
            return teamName;
        }

        /// <summary>
    /// Prompts the user for a name that cannot be empty. Continues to prompt until a non-empty name is provided.
    /// </summary>
    /// <param name="prompt">Message shown to the user to prompt input.</param>
    /// <returns>A non-empty string containing the name entered by the user.</returns>
        public string GetMandatoryName(string prompt)
        {
            View.ShowMessage(prompt);

            string name = View.ReadInput();


            while (string.IsNullOrEmpty(name))
            {
                View.ShowMessage("Your input can [bold red]not[/] be empty.");
                name = View.ReadInput();
            }
            View.ShowMessage($"{name} has been created.");
                    return name;
        }

    }
}
