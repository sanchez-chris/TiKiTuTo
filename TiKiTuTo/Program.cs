
namespace TiKiTuTo
{

    public class Program
    {
        public static void Main()
        {
            var TikiTuTo = new GameEngine.Game();
            Console.CursorVisible = false;


            while (true)
            {
                Console.Clear();
                Console.SetCursorPosition(10, 5);
                Console.WriteLine(" -----------------------");
                Console.SetCursorPosition(10, 6);
                Console.WriteLine(" |     TiKiTuTo        | ");
                Console.SetCursorPosition(10, 7);
                Console.WriteLine(" -----------------------");
                Console.SetCursorPosition(10, 9);
                Console.WriteLine("1. New Tournament");
                Console.SetCursorPosition(10, 10);
                Console.WriteLine("2. Show old results");
                Console.SetCursorPosition(10, 11);
                Console.WriteLine("3. Load settings");
                Console.SetCursorPosition(10, 12);
                Console.WriteLine("4. Continue game");
                Console.SetCursorPosition(10, 13);
                Console.Write("5. Exit");


                char op = Console.ReadKey().KeyChar;

                switch (op)
                {
                    case '1':
                        TikiTuTo.NewGame();
                        Console.WriteLine("New game started!");
                        break;
                    case '2':
                        // TODO: Implement functionality for showing old results
                        Console.WriteLine("Option 2 selected. Functionality not implemented yet.");
                        break;
                    case '3':
                        // TODO: Implement functionality for starting a saved game
                        Console.WriteLine("Option 3 selected. Functionality not implemented yet.");
                        break;
                    case '4':
                        // TODO: Implement functionality for continuing the game
                        Console.WriteLine("Option 4 selected. Functionality not implemented yet.");
                        break;
                    case '5':
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