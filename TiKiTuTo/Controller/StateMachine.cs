

using TiKiTuTo.View;

namespace TiKiTuTo.Controller
{
    public class StateMachine
    {
        public AppState CurrentState { get; private set; }
        private readonly IView _view;


        /// <summary>
        /// StateMachine constructor. Starts from the main menu by default
        /// </summary>
        public StateMachine(IView view)
        {
            CurrentState = AppState.MainMenu;
            _view = view;
        }



        public void TransitionTo(AppState newState)
        {
            CurrentState = newState;
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
                    // Add handling for other states...
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
                    break;
            }
        }



    }
}
