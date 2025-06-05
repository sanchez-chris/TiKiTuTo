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



        /// <summary>
        /// StateMachine constructor. Starts from the main menu by default.
        /// </summary>
        /// <param name="view">used to show output to user</param>
        /// <param name="gameLogicTournament">used to handle tournament logic</param>
        /// <param name="model">used to store the tournament</param>
        /// <param name="jsonService">used to save and load tournaments</param>
        public StateMachine(IView view, GameLogicTournament gameLogicTournament, TournamentModel model, JSONService jsonService)
        {
            CurrentState = AppState.MainMenu;
            _view = view;
            _gameLogicTournament = gameLogicTournament;
            _tournamentModel = model;
            _jsonService = jsonService;
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
        //TournamentSettingsCreationDialogue TODO
        //ShowLoadableTournamentSettings TODO
        //RunTournament TODO
        //ShowEditableTournamentSettings TODO
        //TournamentSettingsEditingDialogue TODO
        //InGameMenu  TODO
        //Exit  TODO

        public int ExecuteCurrentState()
        {
            switch (CurrentState)
            {
                case AppState.MainMenu:
                    //_view.ShowMainMenu();
                    return _view.MainMenuSelection();
                    break;
                case AppState.StartTournamentMenu:
                    //_view.ShowStartTournamentMenu();
                    return _view.StartTournamentMenuSelection();
                    break;
                case AppState.ShowSavedTournaments:
                    //_view.ShowMessage("Saved tournaments (not implemented yet).");
                    List<string> loadableFiles = _jsonService.GetUnfinishedTournamentFiles();
                    return _view.SavedTournamentsSelection(loadableFiles);
                    break;
                case AppState.ShowFinishedTournaments:
                    _view.ShowMessage("Finished tournaments (not implemented yet).");
                    break;
                case AppState.ManageSettingsMenu:
                    _view.ShowMessage("Settings menu (not implemented yet).");
                    break;
                case AppState.TournamentSettingsCreationDialogue:
                    _gameLogicTournament.InitTournament();
                    _gameLogicTournament.StartTournament();
//Here we actually need to differentiate between initiation and starting. Between the two, the user should be able to decide whether to start immediately or whether to go back and start later.
                    break;
                case AppState.ShowLoadableTournamentSettings:
                    _view.ShowMessage("ShowLoadableTournamentSettings (not implemented yet).");
                    break;
                case AppState.RunTournament:
                    _gameLogicTournament.StartTournament();
                    break;
                case AppState.ShowEditableTournamentSettings:
                    _view.ShowMessage("ShowEditableTournamentSettings (not implemented yet).");
                    break; 
                case AppState.TournamentSettingsEditingDialogue:
                    _view.ShowMessage("TournamentSettingsEditingDialogue (not implemented yet).");
                    break;
                case AppState.InGameMenu:
                    _view.ShowMessage("InGameMenu (not implemented yet).");
                    break;
                case AppState.ExitOptions:
                    _view.ShowExitOptions();
                    break;
                case AppState.Exit:
                    _view.ShowExitMessage();
                    Environment.Exit(0);
                    break;
                default:
                    _view.ShowMessage("Not a valid state.");
                    break;
            }
            //base case for states where the user decides to stay in the current context (no state transition)
            return 0;
        }





        /// <summary>
        /// A wrapper to handle user input for different internal states.
        /// </summary>
        /// <param name="choice">User input</param>
        public void HandleInput(int choice)
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
                case AppState.ManageSettingsMenu:
                    HandleManageSettingsMenuChoice(choice);
                    break;
                case AppState.TournamentSettingsCreationDialogue:
                    //HandleTournamentSettingsCreationDialogueChoice(choice);
                    break;
                case AppState.ShowLoadableTournamentSettings:
                    //HandleShowLoadableTournamentSettingsChoice(choice);
                    break;                
                case AppState.RunTournament:
                    HandleRunTournamentChoice(choice);
                    break;
                case AppState.ShowEditableTournamentSettings:
                    //HandleShowEditableTournamentSettingsChoice(choice);
                    break;
                case AppState.TournamentSettingsEditingDialogue:
                    //HandleTournamentSettingsEditingDialogueChoice(choice);
                    break;
                case AppState.InGameMenu:
                    HandleInGameMenuChoice(choice);
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
        //TournamentSettingsCreationDialogue TODO
        //ShowLoadableTournamentSettings TODO
        //RunTournament TODO
        //ShowEditableTournamentSettings TODO
        //TournamentSettingsEditingDialogue TODO
        //InGameMenu  DONE
        //Exit  TODO





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
                    TransitionTo(AppState.ManageSettingsMenu);
                    break;
                case 5:
                    TransitionTo(AppState.ExitOptions);
                    break;
                default:
                    _view.ShowInvalidInputMessage();
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
                    TransitionTo(AppState.TournamentSettingsCreationDialogue);
                    //TransitionTo(AppState.RunTournament);
                    break;
                case 2:
                    TransitionTo(AppState.ShowLoadableTournamentSettings);
                    break;
                case 3:
                    TransitionTo(AppState.MainMenu);
                    break;
                default:
                    _view.ShowInvalidInputMessage();
                    break;
            }
        }

        /// <summary>
        /// Handles transitions from the ManageSettings menu based on user input.
        /// </summary>
        /// <param name="choice">user input</param>
        private void HandleManageSettingsMenuChoice(int choice)
        {
            switch (choice)
            {
                case 1:
                    TransitionTo(AppState.TournamentSettingsCreationDialogue);
                    break;
                case 2:
                    TransitionTo(AppState.ShowEditableTournamentSettings);
                    break;
                case 3:
                    TransitionTo(AppState.MainMenu);
                    break;
                default:
                    _view.ShowInvalidInputMessage();
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
            List<string> unfinishedTournamentFiles = _jsonService.GetUnfinishedTournamentFiles();
            if (choice <= unfinishedTournamentFiles.Count) //a valid tournament file
            {
                string chosenTournament = unfinishedTournamentFiles[choice];
                _view.ShowMessage($"Opening {chosenTournament}");
                //_tournamentModel.Tournament = _jsonService.LoadGame(chosenTournament); jsonService.LoadGame should take a filename or path, no?
                TransitionTo(AppState.RunTournament);
            }
            else if (choice == unfinishedTournamentFiles.Count + 1) //back to main menu
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
            List<string> unfinishedTournamentFiles = _jsonService.GetFinishedTournamentFiles();
            if (choice <= unfinishedTournamentFiles.Count) //a valid tournament file
            {
                string chosenTournament = unfinishedTournamentFiles[choice];
                _view.ShowMessage($"Opening {chosenTournament}");
                TransitionTo(AppState.RunTournament);
            }
            else if (choice == unfinishedTournamentFiles.Count + 1) //back to main menu
            {
                TransitionTo(AppState.MainMenu);
            }
            else //not a valid input
            {
                _view.ShowInvalidInputMessage();
            }

        }

        private void HandleRunTournamentChoice(int choice)
        {
            switch (choice)
            {
                case 1:
                    TransitionTo(AppState.TournamentSettingsCreationDialogue);
                    break;
                case 2:
                    TransitionTo(AppState.ShowEditableTournamentSettings);
                    break;
                case 3:
                    TransitionTo(AppState.MainMenu);
                    break;
                default:
                    _view.ShowInvalidInputMessage();
                    break;
            }
        }

        private void HandleInGameMenuChoice(int choice)
        {
            switch (choice)
            {
                case 1:
                    _jsonService.SaveGame();
                    break;
                case 2:
                    TransitionTo(AppState.MainMenu);
                    break;
                case 3:
                    _jsonService.SaveGame();
                    TransitionTo(AppState.Exit);
                    break;
                default:
                    _view.ShowInvalidInputMessage();
                    break;
            }
        }

        //TODO: HandleShowAvailableTournamentSettingsChoice (variable number of valid options...)
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
                default:
                    _view.ShowInvalidInputMessage();
                    break;
            }
        }

    }
}
