using TiKiTuTo.Controller;
using TiKiTuTo.View;
using TiKiTuTo.Model;
using TiKiTuTo.Model.DataObjects;
using TiKiTuTo.Model.BusinessLogic.GameLogic;

namespace TiKiTuTo.Controller
{

    public class Program
    {
        public static void Main()
        {
            TournamentModel model = new();
            IView view = new ConsoleView();
            InputValidator inputValidator = new InputValidator();
            InputHandler inputHandler = new InputHandler(view, inputValidator);
            JSONService json = new JSONService(model, view);
            GameLogicMatch gameLogicMatch = new(inputHandler, json);
            GameLogicRound gameLogicRound = new(inputHandler, json, inputValidator, model);
            GameLogicTournamentSettings gameLogicTournamentSettings = new(model, inputHandler);
            GameLogicTournament gameLogicTournament = new(gameLogicRound, gameLogicTournamentSettings, inputHandler, model, json);
            StateMachine stateMachine = new StateMachine(view, gameLogicTournament, gameLogicRound, model, json, gameLogicTournamentSettings);
            Controller controller = new Controller(view, model, stateMachine, inputHandler, gameLogicMatch, gameLogicRound, gameLogicTournament, gameLogicTournamentSettings);
            
            while (true)
            {
                controller.Run();
            }

        }

    }

}
