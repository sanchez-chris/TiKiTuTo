using TiKiTuTo.Controller;
using TiKiTuTo.View;
using TiKiTuTo.Model;
using TiKiTuTo.Model.DataObjects;
using TiKiTuTo.Model.BusinessLogic.GameLogic;
using System.Text;

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
            EXCELService excel = new EXCELService(model);
            GameLogicMatch gameLogicMatch = new(inputHandler, json, model);
            GameLogicRound gameLogicRound = new(inputHandler, json, inputValidator, model);
            GameLogicTournamentSettings gameLogicTournamentSettings = new(model, inputHandler);
            GameLogicTournament gameLogicTournament = new(gameLogicRound, gameLogicTournamentSettings, inputHandler, model, json);
            StateMachine stateMachine = new StateMachine(view, gameLogicTournament, gameLogicTournamentSettings, gameLogicRound, model, json, excel, inputHandler);
            Controller controller = new Controller(view, model, stateMachine, inputHandler, gameLogicMatch, gameLogicRound, gameLogicTournament, gameLogicTournamentSettings);
            Console.Title = "TikiTuto";

            while (true)
            {
                controller.Run();
            }

        }

    }

}
