//using TiKiTuTo.Model.Model.Tournament;
using TiKiTuTo.Model.BusinessLogic.GameLogic;
using TiKiTuTo.View;

namespace TiKiTuTo.Controller
{
    public class StateMachine
    {
        public AppState CurrentState { get; private set; }
        private readonly IView _view;
        private GameLogicTournament _gameLogicTournament;
        private Model.TournamentModel _tournamentModel;
        private JSONService _jsonService;


        /// <summary>
        /// StateMachine constructor. Starts from the main menu by default
        /// </summary>
        public StateMachine(IView view, GameLogicTournament gameLogicTournament, Model.TournamentModel model, JSONService jsonService)
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
            RenderCurrentState();

        }

        public void RenderCurrentState()
        {
            switch (CurrentState)
            {
                case AppState.MainMenu:
                    _view.ShowMainMenu();
                    break;
                case AppState.StartTournamentMenu:
                    _view.ShowStartTournamentMenu();
                    break;
                case AppState.RunTournament:
                    _gameLogicTournament.InitTournament();
                    _gameLogicTournament.StartTournament();
                    break;
                case AppState.Exit:
                    _view.ShowExitMessage();
                    break;
                case AppState.ShowSavedTournaments:
                    _jsonService.LoadGame();
                    break;
                case AppState.ShowFinishedTournaments:
                    _view.ShowMessage("Finished tournaments (not implemented yet).");
                    break;
                case AppState.ManageSettingsMenu:
                    _view.ShowMessage("Settings menu (not implemented yet).");
                    break;
                default:
                    _view.ShowMessage("Not a valid state.");
                    break;
            }
        }

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
                case AppState.ShowFinishedTournaments:
                case AppState.ManageSettingsMenu:
                    TransitionTo(AppState.MainMenu); 
                    break;
                default:
                    _view.ShowInvalidInputMessage();
                    RenderCurrentState();
                    break;
            }
        }

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
                    TransitionTo(AppState.Exit);
                    break;
                default:
                    _view.ShowInvalidInputMessage();
                    RenderCurrentState();
                    break;
            }
        }


        private void HandleStartTournamentMenuChoice(int choice)
        {
            switch (choice)
            {
                case 1:
                    TransitionTo(AppState.RunTournament);
                    break;
                case 2:
                    TransitionTo(AppState.ShowAvailableTournamentSettings);
                    break;
                case 3:
                    TransitionTo(AppState.MainMenu);
                    break;
                default:
                    _view.ShowInvalidInputMessage();
                    RenderCurrentState();
                    break;
            }
        }

/*        private void HandelRunTournamentChoice(int choice)
        {

        }
*/


    }
}
