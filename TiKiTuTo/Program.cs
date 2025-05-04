using Classes;

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


        public static void TournamentSaveTest()
        {
            // Create a sample tournament to test the save functionality.
            Tournament tournament = CreateSampleTournament();

            // Save the tournament to a JSON file.
            JSONService.SaveGame(tournament);
        }

        public static void TournamentLoadTest()
        {
            // Create a sample tournament to test the load functionality.
            Tournament tournament = CreateSampleTournament();

            // Load a tournament from a JSON file.
            JSONService.LoadGame();
        }
        
        public static Tournament CreateSampleTournament()
        {
            // Create Teams
            Team team1 = new Team("Team Alpha");
            team1.Players.Add(new Player("Alice"));
            team1.Players.Add(new Player("Bob"));

            Team team2 = new Team("Team Beta");
            team2.Players.Add(new Player("Charlie"));
            team2.Players.Add(new Player("Diana"));

            Team team3 = new Team("Team Gamma");
            team3.Players.Add(new Player("Eve"));
            team3.Players.Add(new Player("Frank"));

            // Create Matches
            Match match1 = new Match(team1, team2);
            match1.UpdateMatchResults(2, 1); // Team Alpha gewinnt gegen Team Beta

            Match match2 = new Match(team2, team3);
            match2.UpdateMatchResults(3, 0); // Team Beta gewinnt gegen Team Gamma

            Match match3 = new Match(team1, team3);
            match3.UpdateMatchResults(1, 1); // Team Alpha und Team Gamma spielen unentschieden

            // Create Rounds
            Round round1 = new Round(Round.RoundType.League);
            round1.Matchs.Add(match1);
            round1.Matchs.Add(match2);

            Round round2 = new Round(Round.RoundType.KO);
            round2.Matchs.Add(match3);

            // Create Tournament
            Tournament tournament = new Tournament
            {
                TournamentID = 1,
                TournamentName = "BallerLeague",
                TeamCount = 3,
                Prelimininaries = 1,
                Teams = new List<Team> { team1, team2, team3 },
                Rounds = new List<Round> { round1, round2 },
                ActiveRoundIndex = 0
            };

            return tournament;
        }

    }

}
