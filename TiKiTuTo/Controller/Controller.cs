using System.ComponentModel.Design;
using View;
using Model;

namespace Controller
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



        public void StartTournament()
        {
            Tournament tournament = GameLogicTournament.CreateTournament(InputHandler);
        }
    }
}
