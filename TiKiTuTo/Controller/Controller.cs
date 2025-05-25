using System.ComponentModel.Design;
using TiKiTuTo.View;
using TiKiTuTo.Model;

namespace TiKiTuTo.Controller
{
    public class Controller
    {


        public IView View { get; }


        public Controller(IView view)
        {
            View = view;
        }



        public void StartTournament()
        {
            Tournament tournament = GameLogicTournament.CreateTournament(View);
        }
    }
}
