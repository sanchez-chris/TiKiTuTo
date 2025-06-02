using System.ComponentModel.Design;
using View;
using TiKiTuTo.Model.BusinessLogic.GameLogic;
using TiKiTuTo.Model.DataObjects;

namespace TiKiTuTo.Controller
{
    public class Controller
    {
        public IView View { get; }
        public InputHandler InputHandler { get; }


        public Controller(IView view, InputHandler inputHandler)
        {
            View = view;
            InputHandler = inputHandler;
        }



        public Tournament InitTournament(InputHandler inputHandler)
        {
            Tournament tournament = GameLogicTournament.SetupTournament(inputHandler);
            GameLogicRound.InitPreliminaryRound(tournament);
            //JSONService.SaveGame(tournament);
            return tournament;
        }

        //public Tournament StartTournament()
        //{

        //}
    }
}
