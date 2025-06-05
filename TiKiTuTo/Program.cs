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
            JSONService jsonService = new JSONService(model, view, inputHandler);
            GameLogicMatch gameLogicMatch = new(inputHandler, jsonService);
            GameLogicRound gameLogicRound = new(inputHandler, jsonService, inputValidator, model);
            GameLogicTournamentSettings gameLogicTournamentSettings = new(inputHandler);
            GameLogicTournament gameLogicTournament = new(gameLogicRound, gameLogicTournamentSettings, inputHandler, model, jsonService);
            StateMachine stateMachine = new StateMachine(view, gameLogicTournament,model, jsonService);
            Controller controller = new Controller(view,model, stateMachine, inputHandler, gameLogicMatch, gameLogicRound, gameLogicTournament, gameLogicTournamentSettings);
            
            while (true)
            {
                controller.Run();
            }

        }

    }

}
