using System.ComponentModel.Design;
using TiKiTuTo.View;
using TiKiTuTo.Model;

namespace TiKiTuTo.Controller
{
    public class Controller
    {


        public IView _View { get; }
        public InputHandler _InputHandler { get; }


        public Controller(IView view, InputHandler inputHandler)
        {
            _View = view;
            _InputHandler = inputHandler;
        }



        public void StartTournament()
        {
            Tournament tournament = GameLogicTournament.CreateTournament(_InputHandler);
        }
    }
}
