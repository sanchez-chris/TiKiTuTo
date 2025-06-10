using TiKiTuTo.View;
using TiKiTuTo.Model;
using TiKiTuTo.Model.BusinessLogic.GameLogic;
using TiKiTuTo.Model.DataObjects;
using System.Diagnostics;


namespace TiKiTuTo.Controller
{
    /// <summary>
    /// Manages the application state and transitions between different screens/states in TikiTuto.
    /// Controls the flow of the application based on user input and coordinates between view and model components.
    /// </summary>
    public class StateMachine
    {
        public AppState CurrentState { get; private set; }
        private readonly IView _view;
        private GameLogicTournament _gameLogicTournament;
        private GameLogicRound _gameLogicRound;
        private TournamentModel _tournamentModel;
        private JSONService _jsonService;
        private EXCELService _excelService;
        private InputHandler _inputHandler;
        private GameLogicTournamentSettings _gameLogicTournamentSettings;



        /// <summary>
        /// StateMachine constructor. Initializes a new instance of the state machine that manages application flow.
        /// Starts from the main menu by default.
        /// </summary>
        /// <param name="view">Used to display information to the user and collect user input.</param>
        /// <param name="gameLogicTournament">Used to handle tournament-level game logic.</param>
        /// <param name="gameLogicTournamentSettings">Used to create and manage tournament settings.</param>
        /// <param name="gameLogicRound">Used to handle round-level game logic.</param>
        /// <param name="model">Used to store and access tournament data.</param>
        /// <param name="jsonService">Used to save and load tournaments and settings from JSON files.</param>
        /// <param name="excel">Used to import tournament settings from Excel files.</param>
        /// <param name="inputHandler">Used to retrieve and validate user input.</param>
        public StateMachine
            (
            IView view, 
            GameLogicTournament gameLogicTournament,
            GameLogicTournamentSettings gameLogicTournamentSettings, 
            GameLogicRound gameLogicRound, 
            TournamentModel model, 
            JSONService jsonService, 
            EXCELService excel, 
            InputHandler inputHandler)
        {
            CurrentState = AppState.MainMenu;
            _view = view;
            _gameLogicTournament = gameLogicTournament;
            _gameLogicRound = gameLogicRound;
            _tournamentModel = model;
            _jsonService = jsonService;
            _excelService = excel;
            _inputHandler = inputHandler;
            _gameLogicTournamentSettings = gameLogicTournamentSettings;
        }



        /// <summary>
        /// Transitions the application to a new state.
        /// </summary>
        /// <param name="newState">The new application state to transition to.</param>
        public void TransitionTo(AppState newState)
        {
            CurrentState = newState;
            _view.ShowFooter();
        }

        /// <summary>
        /// Executes the logic for the current application state and returns the user's selection.
        /// Handles UI rendering and interaction based on the current state.
        /// </summary>
        /// <returns>An integer representing the user's menu selection.</returns>
        public int ExecuteCurrentState()
        {
            _view.ShowTikiTutoHeader();
            switch (CurrentState)
            {
                case AppState.MainMenu:
                    return _view.MainMenuSelection();
                case AppState.StartTournamentMenu:
                    return _view.StartTournamentMenuSelection();
                case AppState.ShowSavedTournaments:
                    string[] avilableUnfinishedFiles = _jsonService.GetUnfinishedTournamentFiles();
                    return _view.AvailableTournamentSelection(avilableUnfinishedFiles, SelectLoadingType.UnfinishedTournament);
                case AppState.ShowFinishedTournaments:
                    string[] availableFinishedFiles = _jsonService.GetFinishedTournamentFiles();
                    return _view.AvailableTournamentSelection(availableFinishedFiles, SelectLoadingType.FinishedTournament);
                case AppState.TournamentSettingsCreationDialogue:
                    TournamentSettings tournamentSettings = _gameLogicTournamentSettings.CreateTournamentSettings();
                    _jsonService.SaveTournamentSettings(tournamentSettings); 
                    return _view.SettingsCreatedSelection();
                case AppState.ImportTournamentSettingsFromExcelFile:
                    return _view.AvailableExcelFiles();
                case AppState.InitTournament:
                    _gameLogicTournament.InitTournament();
                    return _view.TournamentStartSelection();
                case AppState.ShowLoadableTournamentSettings:
                    string[] availableSettingsFiles = _jsonService.GetTournamentSettingsFiles();
                    return _view.AvailableTournamentSelection(availableSettingsFiles, SelectLoadingType.TournamentSettings);
                case AppState.RunTournament:
                    _gameLogicTournament.RunTournament();
                    _view.ShowStandings(_tournamentModel.Tournament);
                    return _view.DuringTournamentMenuSelection();
                case AppState.ExitOptions:
                    return _view.ExitOptionsSelection();
                case AppState.Exit:
                    _view.ShowExitMessage();
                    _view.ShowFooter();
                    Environment.Exit(0);
                    break;
            }
            _view.ShowFooter(); // Display footer as a fallback
            return 0;
        }





        /// <summary>
        /// A wrapper to handle user input for different internal states.
        /// </summary>
        /// <param name="choice">User input</param>
        public void HandleUserChoice(int choice)
        {
            switch (CurrentState)
            {
                case AppState.MainMenu:
                    HandleMainMenuChoice(choice);
                    break;
                case AppState.StartTournamentMenu:
                    HandleStartTournamentMenuChoice(choice);
                    break;
                case AppState.ShowSavedTournaments:
                    HandleShowSavedTournamentsChoice(choice);
                    break;
                case AppState.ShowFinishedTournaments:
                    HandleShowFinishedTournamentsChoice(choice);
                    break;
                case AppState.TournamentSettingsCreationDialogue:
                    HandleSettingsCreationChoice(choice);
                    break;
                case AppState.ShowLoadableTournamentSettings:
                    HandleShowLoadableTournamentSettingsChoice(choice);
                    break;
                case AppState.ImportTournamentSettingsFromExcelFile:
                    HandleImportTournamentSettingsFromExcel();
                    break;
                case AppState.InitTournament:
                    HandleTournamentStartChoice(choice);
                    break;
                case AppState.RunTournament:
                    HandleRunTournamentChoice(choice);
                    break;
                case AppState.ExitOptions:
                    HandleExitChoice(choice);
                    break;
                case AppState.Exit:
                    HandleExitChoice(choice);
                    break;
            }
        }


        /// <summary>
        /// Handles transitions from the main menu based on user input.
        /// </summary>
        /// <param name="choice">User input</param>
        private void HandleMainMenuChoice(int choice)
        {
            switch (choice)
            {
                case 1:
                    TransitionTo(AppState.StartTournamentMenu);
                    break;
                case 2:
                    TransitionTo(AppState.ShowSavedTournaments);
                    break;
                case 3:
                    TransitionTo(AppState.ShowFinishedTournaments);
                    break;
                case 4:
                    TransitionTo(AppState.TournamentSettingsCreationDialogue);
                    break;
                case 5:
                    TransitionTo(AppState.ExitOptions);
                    break;
            }
        }

        /// <summary>
        /// Handles transitions from the StartTournament menu based on user input.
        /// </summary>
        /// <param name="choice">user input</param>
        private void HandleStartTournamentMenuChoice(int choice)
        {
            switch (choice)
            {
                case 1:
                    TransitionTo(AppState.InitTournament);
                    break;
                case 2:
                    TransitionTo(AppState.ShowLoadableTournamentSettings);
                    break;
                case 3:
                    TransitionTo(AppState.ImportTournamentSettingsFromExcelFile);
                    break;
                case 4:
                    TransitionTo(AppState.MainMenu);
                    break;

            }
        }


        /// <summary>
        /// Handles transitions from the ShowSavedTournaments menu based on user input.
        /// The number of valid options depends on the number of unfinished tournaments found in the saved games folder.
        /// </summary>
        /// <param name="choice">user input</param>
        private void HandleShowSavedTournamentsChoice(int choice)
        {
            string[] unfinishedTournamentFiles = _jsonService.GetUnfinishedTournamentFiles();

            Array.Reverse(unfinishedTournamentFiles);

            if (choice == unfinishedTournamentFiles.Length + 1) 
            {
                TransitionTo(AppState.MainMenu);
            }
            else if (choice >= 1 && choice <= unfinishedTournamentFiles.Length) //a valid tournament file
            {
                string chosenTournament = unfinishedTournamentFiles[choice-1];
                _view.ShowMessage($"Opening {Path.GetFileName(chosenTournament)}");
                _jsonService.LoadTournament(chosenTournament); 
                TransitionTo(AppState.RunTournament);
            }
            else //choosing last option
            {
                TransitionTo(AppState.MainMenu);
            }
        }


        /// <summary>
        /// Handles transitions from the ShowFinishedTournaments menu based on user input.
        /// The number of valid options depends on the number of finished tournaments found in the finished games folder.
        /// </summary>
        /// <param name="choice">user input</param>
        private void HandleShowFinishedTournamentsChoice(int choice)
        {
            string[] finishedTournamentFiles = _jsonService.GetFinishedTournamentFiles();

            Array.Reverse(finishedTournamentFiles);

            if (choice == finishedTournamentFiles.Length + 1) 
            {
                TransitionTo(AppState.MainMenu);
            }
            else if (choice >= 1 && choice <= finishedTournamentFiles.Length) //a valid tournament file
            {
                string chosenTournament = finishedTournamentFiles[choice - 1];
                _view.ShowMessage($"Opening {Path.GetFileName(chosenTournament)}");
                _jsonService.LoadTournament(chosenTournament);
                TransitionTo(AppState.RunTournament);
            }
            else //choosing last option
            {
                TransitionTo(AppState.MainMenu);
            }
        }

        /// <summary>
        /// Handles transitions after tournament settings creation based on user input.
        /// </summary>
        /// <param name="choice">User input determining whether to create another settings file or return to main menu.</param>
        private void HandleSettingsCreationChoice(int choice)
        {
            switch (choice)
            {
                case 1:
                    TransitionTo(AppState.TournamentSettingsCreationDialogue);
                    break;
                case 2:
                    TransitionTo(AppState.MainMenu);
                    break;
            }
        }

        /// <summary>
        /// Handles transitions from the ShowLoadableTournamentSettings menu based on user input.
        /// The number of valid options depends on the number of available tournament settings files found in the settings folder.
        /// </summary>
        /// <param name="choice">user input</param>
        private void HandleShowLoadableTournamentSettingsChoice(int choice)
        {
            string[] tournamentSettingsFiles = _jsonService.GetTournamentSettingsFiles();

            Array.Reverse(tournamentSettingsFiles);
            
            if (choice == tournamentSettingsFiles.Length + 1)
            {
                TransitionTo(AppState.MainMenu);
            }
            else if (choice >= 1 && choice <= tournamentSettingsFiles.Length) //a valid tournament file
            {
                string chosenSetting = tournamentSettingsFiles[choice - 1];
                _view.ShowMessage($"Opening {Path.GetFileName(chosenSetting)}");
                var tournamentSettings = _jsonService.LoadTournamentSettings(chosenSetting); 
                _gameLogicTournament.CreateTournament(tournamentSettings);
                _gameLogicRound.InitPreliminaryRound();
                TransitionTo(AppState.RunTournament);
            }
            else
            {
                TransitionTo(AppState.MainMenu);
            }
        } 
        
        /// <summary>
        /// Handles the Excel import process for tournament settings.
        /// Opens the import folder in Explorer and processes the selected Excel file.
        /// </summary>
        private void HandleImportTournamentSettingsFromExcel()
        {
            bool answer = _inputHandler.GetApproval("Do you want to continue? [[[bold green]Y[/]/[bold red]N[/]]]");
            if (answer)
            {
                Process.Start("explorer.exe", _excelService.ExcelImportFolder);
                TournamentSettings importedTournamentSettings = _excelService.ImportExcelFile();
                _view.ConfirmingImportAction();
                _gameLogicTournament.CreateTournament(importedTournamentSettings);
                _gameLogicRound.InitPreliminaryRound();
                TransitionTo(AppState.RunTournament);
            }
            else 
            {
               TransitionTo(AppState.StartTournamentMenu);
            }
        }


        /// <summary>
        /// Handles transitions from the tournament running screen based on user input.
        /// </summary>
        /// <param name="choice">User input determining whether to continue the tournament, return to main menu, or exit.</param>
        private void HandleRunTournamentChoice(int choice)
        {
            switch (choice)
            {
                case 1:
                    TransitionTo(AppState.RunTournament);
                    break;
                case 2:
                    TransitionTo(AppState.MainMenu);
                    break;
                case 3:
                    TransitionTo(AppState.Exit);
                    break;
            }
        }


        /// <summary>
        /// Handles transitions from the exit confirmation screen based on user input.
        /// </summary>
        /// <param name="choice">User input determining whether to exit the application or return to main menu.</param>
        private void HandleExitChoice(int choice)
        {
            switch (choice)
            {
                case 1:
                    TransitionTo(AppState.Exit);
                    break;
                case 2:
                    TransitionTo(AppState.MainMenu);
                    break;
            }
        }

        /// <summary>
        /// Handles transitions after tournament initialization based on user input.
        /// </summary>
        /// <param name="choice">User input determining whether to run the tournament or return to main menu.</param>
        private void HandleTournamentStartChoice(int choice)
        {
            switch (choice)
            {
                case 1:
                    TransitionTo(AppState.RunTournament);
                    break;
                case 2:
                    TransitionTo(AppState.MainMenu);
                    break;
            }
        }

    }
}
