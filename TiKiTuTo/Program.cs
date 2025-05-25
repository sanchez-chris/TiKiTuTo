using TiKiTuTo.View;

namespace TiKiTuTo.Controller
{

    public class Program
    {
        public static void Main()
        {

            IView view = new ConsoleView();
            Controller controller = new Controller(view);


            while (true)
            {
                view.ShowMenu();

                int UserInput = InputHandler.GetValidMenuInput(view);
                
                switch (UserInput)
                {
                    case 1:
                        controller.StartTournament();
                        break;
                    case 2:
                        // TODO: Implement functionality for showing old results
                        Console.WriteLine("Option 2 selected. Functionality not implemented yet.");
                        break;
                    case 3:
                        // TODO: Implement functionality for starting a saved game
                        Console.WriteLine("Option 3 selected. Functionality not implemented yet.");
                        break;
                    case 4:
                        // TODO: Implement functionality for continuing the game
                        Console.WriteLine("Option 4 selected. Functionality not implemented yet.");
                        break;
                    case 5:
                        Console.WriteLine("Exiting the program...");
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }

        }

    }

}
