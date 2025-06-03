

using TiKiTuTo.Model.BusinessLogic.GameLogic;
using TiKiTuTo.View;

namespace TiKiTuTo.Controller
{
    public class StateMachine
    {
        public AppState CurrentState { get; private set; }
        private readonly IView _view;
        private JSONService _jsonService;
        private Model.Model _model;


        /// <summary>
        /// StateMachine constructor. Starts from the main menu by default.
        /// </summary>
        /// <param name="view">A view used to render for different states.</param>
        public StateMachine(IView view, JSONService jsonService, Model.Model model)
        {
            CurrentState = AppState.MainMenu;
            _view = view;
            _jsonService = jsonService;
            _model = model;

        }



        public void TransitionTo(AppState newState)
        {
            CurrentState = newState;

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
                    HandleTournamentSettingsCreationDialogueChoice(choice);
                    break;
                case AppState.ShowLoadableTournamentSettings:
                    HandleShowLoadableTournamentSettingsChoice(choice);
                    break;
                case AppState.ShowLoadableTournamentSettings:
                    HandleShowLoadableTournamentSettingsChoice(choice);
                    break;
                case AppState.RunTournament:
                    HandleRunTournamentChoice(choice);
                    break;
                case AppState.ShowEditableTournamentSettings:
                    HandleShowEditableTournamentSettingsChoice(choice);
                    break;
                case AppState.TournamentSettingsEditingDialogue:
                    HandleTournamentSettingsEditingDialogueChoice(choice);
                    break;
                case AppState.Exit:
                    HandleExitChoice(choice);
                    break;
            }
        }



        //MainMenu,
        //StartTournamentMenu,
        //ShowSavedTournaments,
        //ShowFinishedTournaments,
        //ManageSettingsMenu,
        TournamentSettingsCreationDialogue,
        ShowLoadableTournamentSettings,
        RunTournament,
        ShowEditableTournamentSettings,
        TournamentSettingsEditingDialogue,
        Exit





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
 //                  
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
                    TransitionTo(AppState.Exit);
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
            List<string> unfinishedTournamentFiles = JSONService.GetUnfinishedTournamentFiles();
            if (choice <= unfinishedTournamentFiles.Count) //a valid tournament file
            {
                string chosenTournament = unfinishedTournamentFiles[choice];
                _view.ShowMessage($"Opening {chosenTournament}");
                _model.Tournament = _jsonService.LoadGame(chosenTournament);
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
            List<string> unfinishedTournamentFiles = JSONService.GetFinishedTournamentFiles();
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

        }


        //TODO: HandleShowAvailableTournamentSettingsChoice (variable number of valid options...)

    }
    }
