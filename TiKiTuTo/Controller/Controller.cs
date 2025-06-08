using System.ComponentModel.Design;
using TiKiTuTo.View;
using TiKiTuTo.Model.BusinessLogic.GameLogic;
using TiKiTuTo.Model.DataObjects;
using TiKiTuTo.Model;

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
        private Model.TournamentModel _TournamentModel { get; }


        public Controller
            (
            IView view, 
            Model.TournamentModel tournamentModel,
            StateMachine stateMachine, 
            InputHandler inputHandler, 
            GameLogicMatch gameLogicMatch, 
            GameLogicRound gameLogicRound, 
            GameLogicTournament gameLogicTournament, 
            GameLogicTournamentSettings gameLogicTournamentSettings
            )
        {
            _view = view;
            _TournamentModel = tournamentModel;
            _stateMachine = stateMachine;
            InputHandler = inputHandler;
            GameLogicMatch = gameLogicMatch;
            GameLogicRound = gameLogicRound;
            GameLogicTournament = gameLogicTournament;
            GameLogicTournamentSettings = gameLogicTournamentSettings;
        }



        public void Run()
        {
            while (true)
            {
                int userChoice = _stateMachine.ExecuteCurrentState();
                _stateMachine.HandleUserChoice(userChoice);
            }
        }




    }
}
