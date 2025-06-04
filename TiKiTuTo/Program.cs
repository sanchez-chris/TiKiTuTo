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
            JSONService json = new JSONService(model);
            InputHandler inputHandler = new InputHandler(view, inputValidator);
            GameLogicMatch gameLogicMatch = new(inputHandler, json);
            GameLogicRound gameLogicRound = new(inputHandler, json, inputValidator, model);
            GameLogicTournamentSettings gameLogicTournamentSettings = new(inputHandler);
            GameLogicTournament gameLogicTournament = new(gameLogicRound, gameLogicTournamentSettings, inputHandler, model);
            StateMachine stateMachine = new StateMachine(view, gameLogicTournament, model, json);
            Controller controller = new Controller(view, model, stateMachine, inputHandler, gameLogicMatch, gameLogicRound, gameLogicTournament, gameLogicTournamentSettings);
            
            while (true)
            {
                controller.Run();
            }

        }

    }

}
