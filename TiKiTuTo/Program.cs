using TiKiTuTo.Controller;
using TiKiTuTo.View;
using TiKiTuTo.Model.DataObjects;
using TiKiTuTo.Model.BusinessLogic.GameLogic;

namespace TiKiTuTo.Controller
{

    public class Program
    {
        public static void Main()
        {

            IView view = new ConsoleView();
            InputValidator inputValidator = new InputValidator();
            InputHandler inputHandler = new InputHandler(view, inputValidator);
            Model.TournamentModel model = new();
            JSONService json = new JSONService(model);
            GameLogicMatch gameLogicMatch = new(inputHandler, json);
            GameLogicRound gameLogicRound = new(inputHandler, json);
            GameLogicTournamentSettings gameLogicTournamentSettings = new(inputHandler);
            GameLogicTournament gameLogicTournament = new(gameLogicRound, gameLogicTournamentSettings, inputHandler, model);
            StateMachine stateMachine = new StateMachine(view, gameLogicTournament,model);
            Controller controller = new Controller(view,model, stateMachine, inputHandler, gameLogicMatch, gameLogicRound, gameLogicTournament, gameLogicTournamentSettings);
            
            while (true)
            {
                controller.Run();
            }

        }

    }

}
