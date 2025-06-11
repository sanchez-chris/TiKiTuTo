using TiKiTuTo.View;
using TiKiTuTo.Model;
using TiKiTuTo.Controller;
using TiKiTuTo.Model.BusinessLogic.GameLogic;

namespace TiKiTuTo
{
    internal static class Program
    {
        public static void Main()
        {
            TournamentModel model = new();
            IView view = new ConsoleView();
            InputValidator inputValidator = new InputValidator();
            InputHandler inputHandler = new InputHandler(view, inputValidator);
            JSONService json = new JSONService(model, view);
            ExcelService excel = new ExcelService(model);
            GameLogicRound gameLogicRound = new(inputHandler, json, inputValidator, model);
            GameLogicTournamentSettings gameLogicTournamentSettings = new(inputHandler);
            GameLogicTournament gameLogicTournament =
                new(gameLogicRound, gameLogicTournamentSettings, inputHandler, model, json);
            StateMachine stateMachine = new StateMachine(view, gameLogicTournament, gameLogicTournamentSettings,
                gameLogicRound, model, json, excel, inputHandler);
            Controller.Controller controller = new Controller.Controller(stateMachine);

            while (true)
            {
                controller.Run();
            }

        }
    }
}
