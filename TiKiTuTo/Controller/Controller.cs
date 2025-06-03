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


        public Controller(IView view, StateMachine stateMachine, InputHandler inputHandler)
        {
            _view = view;
            _stateMachine = stateMachine;
            InputHandler = inputHandler;
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
                case AppState.Exit:
                    _view.ShowExitMessage();
                    break;
            }
        }

        //public Tournament InitTournament(InputHandler inputHandler)
        //{
        //    Tournament tournament = GameLogicTournament.SetupTournament(inputHandler);
        //    GameLogicRound.InitPreliminaryRound(tournament);
        //    //JSONService.SaveGame(tournament);
        //    return tournament;
        //}

        //public Tournament StartTournament()
        //{

        //}
    }
}
