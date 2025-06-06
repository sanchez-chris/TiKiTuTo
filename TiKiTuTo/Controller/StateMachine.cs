using TiKiTuTo.View;
using TiKiTuTo.Model;
using TiKiTuTo.Model.BusinessLogic.GameLogic;


namespace TiKiTuTo.Controller
{
    public class StateMachine
    {
        public AppState CurrentState { get; private set; }
        private readonly IView _view;
        private GameLogicTournament _gameLogicTournament;
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
        public StateMachine(IView view, GameLogicTournament gameLogicTournament, TournamentModel model, JSONService jsonService, GameLogicTournamentSettings gameLogicTournamentSettings)
        {
            CurrentState = AppState.MainMenu;
            _view = view;
            _gameLogicTournament = gameLogicTournament;
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
            switch (CurrentState)
            {
                case AppState.MainMenu:
                    return _view.MainMenuSelection();
                case AppState.StartTournamentMenu:
                    return _view.StartTournamentMenuSelection();
                case AppState.ShowSavedTournaments:
                    string[] loadableFiles = _jsonService.GetUnfinishedTournamentFiles();
                    return _view.SavedTournamentsSelection(loadableFiles);
                case AppState.ShowFinishedTournaments:
                    _view.ShowMessage("Finished tournaments (not implemented yet).");
                    break;
                case AppState.TournamentSettingsCreationDialogue:
                    _gameLogicTournamentSettings.CreateTournamentSettings();
//                    _jsonService.SaveTournamentSettings(); //TODO implement this
                    return _view.SettingsCreatedSelection();
                case AppState.InitTournament:
                    _gameLogicTournament.InitTournament();
                    return _view.TournamentStartSelection();
                case AppState.ShowLoadableTournamentSettings:
                    return _view.LoadableTournamentSettingsSelection();
                case AppState.RunTournament:
                    _gameLogicTournament.RunTournament();
                    _view.ShowStandings();
                    return _view.DuringTournamentMenuSelection();
                case AppState.ExitOptions:
                    return _view.ExitOptionsSelection();
                case AppState.Exit:
                    _view.ShowExitMessage();
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
                    //HandleShowLoadableTournamentSettingsChoice(choice);
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
        /// The number of valid options depends on the number of unfinished tournaments found in the savegame folder.
        /// </summary>
        /// <param name="choice">user input</param>
        private void HandleShowSavedTournamentsChoice(int choice)
        {
            string[] unfinishedTournamentFiles = _jsonService.GetUnfinishedTournamentFiles();
            if (choice <= unfinishedTournamentFiles.Length) //a valid tournament file
            {
                string chosenTournament = unfinishedTournamentFiles[choice-1];
                _view.ShowMessage($"Opening {chosenTournament}");
                _jsonService.LoadTournament(chosenTournament); //jsonService.LoadGame should take a filename or path, no?
                TransitionTo(AppState.RunTournament);
            }
            else if (choice-1 == unfinishedTournamentFiles.Length) //back to main menu
            {
                TransitionTo(AppState.MainMenu);
            }
            else //not a valid input
            {
                _view.ShowInvalidInputMessage();
            }
        }


        /// <summary>
        /// Handles transitions from the ShowFinishedTournaments menu based on user input.
        /// The number of valid options depends on the number of unfinished tournaments found in the savegame folder.
        /// </summary>
        /// <param name="choice">user input</param>
        private void HandleShowFinishedTournamentsChoice(int choice)
        {
            string[] unfinishedTournamentFiles = _jsonService.GetFinishedTournamentFiles();
            if (choice-1 <= unfinishedTournamentFiles.Length) //a valid tournament file
            {
                string chosenTournament = unfinishedTournamentFiles[choice-1];
                _view.ShowMessage($"Opening {chosenTournament}");
                TransitionTo(AppState.RunTournament);
            }
            else if (choice-1 == unfinishedTournamentFiles.Length) //back to main menu
            {
                TransitionTo(AppState.MainMenu);
            }
            else //not a valid input
            {
                _view.ShowInvalidInputMessage();
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
