using System.ComponentModel.Design;
using TiKiTuTo.View;
using TiKiTuTo.Model.BusinessLogic.GameLogic;
using TiKiTuTo.Model.DataObjects;

namespace TiKiTuTo.Controller
{
    public class Controller
    {
        private IView _view { get; }
        private StateMachine _stateMachine { get; }
        public InputHandler InputHandler { get; }
        private GameLogicMatch GameLogicMatch { get; }
        private GameLogicRound GameLogicRound { get; }
        private GameLogicTournament GameLogicTournament { get; }
        private GameLogicTournamentSettings GameLogicTournamentSettings { get; }


        public Controller(
            IView view, 
            StateMachine stateMachine, 
            InputHandler inputHandler, 
            GameLogicMatch gameLogicMatch, 
            GameLogicRound gameLogicRound, 
            GameLogicTournament gameLogicTournament, 
            GameLogicTournamentSettings gameLogicTournamentSettings)
        {
            _view = view;
            _stateMachine = stateMachine;
            InputHandler = inputHandler;
            GameLogicMatch = gameLogicMatch;
            GameLogicRound = gameLogicRound;
            GameLogicTournament = gameLogicTournament;
            GameLogicTournamentSettings = gameLogicTournamentSettings;
        }



        public void Run()
        {
            while (_stateMachine.CurrentState != AppState.Exit)
            {
                RenderCurrentState();
                int userChoice = InputHandler.GetValidMenuInput();
                _stateMachine.HandleInput(userChoice);
                
            }
        }

        private void RenderCurrentState()
        {
            switch (_stateMachine.CurrentState)
            {
                case AppState.MainMenu:
                    _view.ShowMainMenu();
                    break;
                case AppState.StartTournamentMenu:
                    _view.ShowStartTournamentMenu();
                    break;
                case AppState.RunTournament:
                    GameLogicTournament.InitTournament();
                    //CreateTournament(GameLogicTournamentSettings.CreateTournamentSettings());
                    break;
                case AppState.Exit:
                    _view.ShowExitMessage();
                    break;
            }
        }


    }
}
