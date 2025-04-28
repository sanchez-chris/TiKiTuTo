class Program
{
    static void Main()
    {
        // var TikiTuTo = new GameEngine.Game();
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
            Console.WriteLine("1. Neues Turnier");
            Console.SetCursorPosition(10, 10);
            Console.WriteLine("2. Alte ergebnisse anzeigen");
            Console.SetCursorPosition(10, 11);
            Console.WriteLine("3. Einstellungen laden");
            Console.SetCursorPosition(10, 12);
            Console.WriteLine("4. Spiel fortsetzen");
            Console.SetCursorPosition(10, 13);
            Console.Write("5. Exit");


            char op = Console.ReadKey().KeyChar;

            if (op == '1')
            {
                 TikiTuTo.StartGame();
            }
            else if (op == '2')
            {
            }
            if (op == '3')
            {
                // TikiTuTo.StartGame();
            }
            else if (op == '4')
            {
            }
            else if (op == '5')
            {
                Environment.Exit(0);
            }
        }
    }
}
