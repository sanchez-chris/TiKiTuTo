using TiKiTuTo.View;
using TiKiTuTo.Model;
using TiKiTuTo.Model.BusinessLogic.GameLogic;
using TiKiTuTo.Model.DataObjects;


namespace TiKiTuTo.Controller
{
    public class StateMachine
    {
        public AppState CurrentState { get; private set; }
        private readonly IView _view;
        private GameLogicTournament _gameLogicTournament;
        private GameLogicRound _gameLogicRound;
        private TournamentModel _tournamentModel;
        private JSONService _jsonService;
        private GameLogicTournamentSettings _gameLogicTournamentSettings;



        /// <summary>
        /// StateMachine constructor. Starts from the main menu by default.
        /// </summary>
        /// <param name="view">used to show output to user</param>
        /// <param name="gameLogicTournament">used to handle tournament logic</param>
        /// <param name="model">used to store the tournament</param>
        /// <param name="jsonService">used to save and load tournaments</param>
        public StateMachine(IView view, GameLogicTournament gameLogicTournament, GameLogicRound gameLogicRound, TournamentModel model, JSONService jsonService, GameLogicTournamentSettings gameLogicTournamentSettings)
        {
            CurrentState = AppState.MainMenu;
            _view = view;
            _gameLogicTournament = gameLogicTournament;
            _gameLogicRound = gameLogicRound;
            _tournamentModel = model;
            _jsonService = jsonService;
            _gameLogicTournamentSettings = gameLogicTournamentSettings;
        }



        public void TransitionTo(AppState newState)
        {
            CurrentState = newState;

        }



        /////// TODO FOR ExecuteCurrentState
        //MainMenu DONE
        //StartTournamentMenu DONE
        //ShowSavedTournaments WORK IN PROGRESS
        //ShowFinishedTournaments  WORK IN PROGRESS
        //ManageSettingsMenu DONE
        //TournamentSettingsCreationDialogue DONE
        //ShowLoadableTournamentSettings TODO
        //RunTournament TODO
        //ShowEditableTournamentSettings TODO
        //TournamentSettingsEditingDialogue TODO
        //InGameMenu  DONE
        //Exit  DONE

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
            //base case for states where the user decides to stay in the current context (no state transition)
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


        /////// TODO FOR HandleChoiceMethods
        //MainMenu DONE
        //StartTournamentMenu DONE
        //ShowSavedTournaments WORK IN PROGRESS
        //ShowFinishedTournaments  WORK IN PROGRESS
        //ManageSettingsMenu DONE
        //TournamentSettingsCreationDialogue DONE
        //ShowLoadableTournamentSettings TODO
        //RunTournament TODO
        //ShowEditableTournamentSettings TODO
        //TournamentSettingsEditingDialogue TODO
        //InGameMenu  DONE
        //Exit  DONE





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
                _view.ShowMessage($"Opening {chosenTournament}");
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
                _view.ShowMessage($"Opening {chosenTournament}");
                _jsonService.LoadTournament(chosenTournament);
                TransitionTo(AppState.RunTournament);
            }
            else //choosing last option
            {
                TransitionTo(AppState.MainMenu);
            }
        }
                
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
        /// The number of valid options depends on the number of available tournament settings files found in the finished games folder.
        /// </summary>
        /// <param name="choice">user input</param>
        private void HandleShowLoadableTournamentSettingsChoice(int choice)
        {
            string[] tournamentSettingsFiles = _jsonService.GetTournamentSettingsFiles();
            if (tournamentSettingsFiles.Length > 0 && choice - 1 < tournamentSettingsFiles.Length) //a valid tournament file
            {
                string chosenSetting = tournamentSettingsFiles[choice - 1];
                _view.ShowMessage($"Opening {chosenSetting}");
                var tournamentSettings = _jsonService.LoadTournamentSettings(chosenSetting); 
                _gameLogicTournament.CreateTournament(tournamentSettings);
                _gameLogicRound.InitPreliminaryRound();
                TransitionTo(AppState.RunTournament);
            }
            else //choosing last option
            {
                TransitionTo(AppState.MainMenu);
            }
        }        


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
