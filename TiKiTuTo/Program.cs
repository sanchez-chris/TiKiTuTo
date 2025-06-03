using TiKiTuTo.Controller;
using TiKiTuTo.View;
using TiKiTuTo.Model.DataObjects;

namespace TiKiTuTo.Controller
{

    public class Program
    {
        public static void Main()
        {

            IView view = new ConsoleView();
            InputHandler inputHandler = new InputHandler(view);
            StateMachine stateMachine = new StateMachine(view);
            Controller controller = new Controller(view, stateMachine, inputHandler);


            while (true)
            {

                controller.Run();
                //view.ShowMainMenu();

                //int UserInput = inputHandler.GetValidMenuInput();
                
                //switch (UserInput)
                //{
                //    case 1:
                //        Tournament tournament = controller.InitTournament(inputHandler);
                //        break;
                //    case 2:
                //        // TODO: Implement functionality for showing old results
                //        Console.WriteLine("Option 2 selected. Functionality not implemented yet.");
                //        break;
                //    case 3:
                //        // TODO: Implement functionality for starting a saved game
                //        Console.WriteLine("Option 3 selected. Functionality not implemented yet.");
                //        break;
                //    case 4:
                //        // TODO: Implement functionality for continuing the game
                //        Console.WriteLine("Option 4 selected. Functionality not implemented yet.");
                //        break;
                //    case 5:
                //        Console.WriteLine("Exiting the program...");
                //        Environment.Exit(0);
                //        break;
                //    default:
                //        Console.WriteLine("Invalid option. Please try again.");
                //        break;
                //}
            }

        }

    }

}
