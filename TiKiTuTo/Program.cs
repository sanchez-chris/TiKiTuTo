using Model;
using System.Media;
using View;

namespace Controller
{

    public class Program
    {
        public static void Main()
        {

            IView view = new ConsoleView();
            InputHandler inputHandler = new InputHandler(view);
            Controller controller = new Controller(view, inputHandler);


            while (true)
            {
                view.ShowMenu();
                int UserInput = inputHandler.GetValidMenuInput();
                
                switch (UserInput)
                {
                    case 1:
                        Tournament tournament = controller.InitTournament();
                        break;
                    case 2:
                        // TODO: Implement functionality for showing old results
                        Team team1 = new Team("Team1");
                        Team team2 = new Team("Team2");
                        Match match = new Match(team1, team2);
                        GameLogicMatch.RunMatch(match, inputHandler);
                        Console.WriteLine("Option 2 selected. Testing timer...");
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
