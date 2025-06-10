using TiKiTuTo.View;
using TiKiTuTo.Model;
using TiKiTuTo.Model.BusinessLogic;
using Spectre.Console;

namespace TiKiTuTo.Controller
{
    /// <summary>
    /// This class implements all methods which retrieve user input, for integers as well as strings.
    /// </summary>
    public class InputHandler
    {

        public IView View { get; set; }
        readonly int maxMenuOption = 5;
        private InputValidator _inputValidator;

        public InputHandler(IView view, InputValidator inputValidator)
        {
            View = view;
            _inputValidator = inputValidator;
        }

        
        public int GetValidGoalInput(string teamName)
        {
            int GoalInput = GetNumber($"\nHow many goals has {teamName} made?");

            while (!_inputValidator.IsValidGoalInput(GoalInput))
            {
                GoalInput = GetNumber($"This was [bold red]not[/] a valid goal Input [italic grey]Max value is 10[/]");
            }

            return GoalInput;
        }

        /// <summary>
        /// Asks user for the number of total teams in a tournament until a valid value is entered.
        /// </summary>
        /// <returns>The number of teams playing in a tournament, guaranteed to be a valid value.</returns>
        public int GetValidNumberOfTotalTeams()
        {
            int NumberOfTeams = GetNumber($"How many teams are going to join this tournament? [italic grey]minimum 4, maximum 256[/]");

            while (!_inputValidator.IsValidNumberOfTotalTeams(NumberOfTeams))
            {
                NumberOfTeams = GetNumber("This is [bold red]not[/] a valid number of teams for a tournament. [italic grey]minimum 4, maximum 256[/]");
            }

            return NumberOfTeams;
        }

        /// <summary>
        /// Asks user for the number of games each team should play in the preliminaries until a valid value is entered.
        /// </summary>
        /// <param name="NumberOfTeamsTotal"> The total number of teams in the tournament defines which values are valid</param>
        /// <returns>The number of games each team has to play in the preliminaries, guaranteed to be a valid value.</returns>
        public int GetValidNumberOfPreliminaryGames(int NumberOfTeamsTotal)
        {
            int NumberOfPreliminaryGames = GetNumber($"How many games should each team play in the preliminaries? [italic grey]minimum 1, maximum {NumberOfTeamsTotal - 1}[/]");

            while (!_inputValidator.IsValidNumberOfPreliminaryGamesPerTeam(NumberOfPreliminaryGames, NumberOfTeamsTotal))
            {
                NumberOfPreliminaryGames = GetNumber($"This is [bold red]not[/] a valid number of games per team.");
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
            int NumberOfTeamsInKO = GetNumber($"How many teams should continue into the KO phase? [italic grey]minimum 2, maximum {maxAllowed}[/]");
            while (!_inputValidator.IsValidNumberOfTeamsInKORound(NumberOfTeamsInKO, NumberOfTeamsTotal))
            {
                NumberOfTeamsInKO = GetNumber($"This is [bold red]not[/] a valid number of teams for the KO round. [italic grey]minimum 2, maximum {maxAllowed}[/].");
            }
            return NumberOfTeamsInKO;
        }



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
        
        public bool GetApproval(string prompt)
        {
            List<string> validInputsYes = new() { "y", "Y", "YES", "yes" };
            List<string> validInputsNo = new() { "n", "N", "NO", "no" };

            bool result = false;
            bool answerGiven = false;
            View.ShowMessage(prompt);
            
            while(!answerGiven)
            {
                string? userInput = View.ReadInput();

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
            string? userInput = View.ReadInput();

            while (!int.TryParse(userInput, out result))
            {
                View.ShowMessage($"\"{userInput}\" is [bold red]not[/] a valid number input, please try again!");
                userInput = View.ReadInput();
            }
            return result;
        }



        //TODO: ADJUST XML COMMENT FOR GetXYName methods

        /// <summary>
        /// Asks the user to enter any string, using the prompt argument. Repeats until valid string is entered.
        /// </summary>
        /// <param name="prompt">Message shown to the user to prompt input.</param>
        /// <returns>a string entered by the user.</returns>
        public string? GetPlayerName(string prompt, int playerNumber)
        {
            View.ShowMessage(prompt);
            string? playerName = View.ReadInput();

            if (string.IsNullOrEmpty(playerName))
            {
                playerName = $"Player {playerNumber}";
            }
            View.ShowMessage($"Welcome {playerName}");
          return playerName;
        }

        public string? GetTeamName(string prompt, int teamNumber)
        {
            View.ShowMessage(prompt);
            string? teamName = View.ReadInput();

            if (string.IsNullOrEmpty(teamName))
            {
                teamName = $"Team{teamNumber}";
            }

            View.ShowMessage($"Creating Team {teamName}");
            return teamName;
        }

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
