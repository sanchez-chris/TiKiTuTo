using Model;
using Timer = System.Timers.Timer;
using System.Timers;


namespace Controller
{
    public class BasicFunctions
    {
        /// <summary>
        /// Used to add a Player to a Team, needs a Player Object
        /// </summary>
        /// <param name="player"></param>
        public static void AddPlayer(Player player, Team team)
        {
            if (!team.PlayerInTeam.Contains(player))
            {
                team.PlayerInTeam.Add(player);
            }
        }


        public static Team CreateTeam(int i, InputHandler inputHandler)
        {
            bool emptyNameAllowed = true;
            string? teamName = inputHandler.GetTeamName($"Please enter the name of the team. Default name when empty: Team {i}.", emptyNameAllowed);

            if (string.IsNullOrEmpty(teamName))
            {
                teamName = $"Team {i}";
            }

            List<Player> playerList = new List<Player>();
            Team team = new Team(teamName, playerList);
            int maxTeamMembers = inputHandler.GetNumber("How many Teammembers would you like to have?");
            for (int p = 1; p <= maxTeamMembers; p++)
            {
                string? playerName = inputHandler.GetPlayerName($"Please enter the name of the next team member. Default name when empty: Player {p}.", emptyNameAllowed);

                if (string.IsNullOrEmpty(playerName))
                {
                    playerName = $"Player {p}";
                }
                Player player = new Player(playerName);
                AddPlayer(player, team);

            }
            return team;
        }


        public static List<Team> CreateListOfTeams(int NumberOfTeamsTotal, InputHandler inputHandler)
        {
            List<Team> teams = new List<Team>();

            for (int i = 1; i <= NumberOfTeamsTotal; i++)
            {
                teams.Add(CreateTeam(i, inputHandler));
            }
            ShowTeamsAndPlayer(teams, inputHandler);
            return teams;
        }

        public static void ShowTeamsAndPlayer(List<Team> teams, InputHandler inputHandler)
        {
            foreach (Team team in teams)
            {
                inputHandler.View.ShowMessage(team.TeamName);
                foreach (Player player in team.PlayerInTeam)
                {
                    inputHandler.View.ShowMessage(player.Name);
                }
                inputHandler.View.WriteEmptyLine();
            }
        }


        /// <summary>
        /// Enter goals made and goals received to update the TeamScore of the winner.
        /// </summary>
        /// <param name="goalsMade"></param>
        /// <param name="goalsReceived"></param>
        public static void AddOneWinToTeam(int goalsMade, int goalsReceived, Team team)
        {
            if (goalsMade > goalsReceived)
            {
                team.NumberGamesWon++;
            }
        }

        /// <summary>
        /// For a given integer n, calculates the largest number p where p <= n and p = 2^x, where x is a natural number.
        /// </summary>
        /// <param name="n">The integer in question</param>
        /// <returns>The largest number smaller or equal to n which is a power of 2.</returns>
        public static int HighestPowerOf2(int n)
        {
            int p = (int)(Math.Log(n) /
                           Math.Log(2));
            return (int)Math.Pow(2, p);
        }


        private static DateTime _endTime;
        public static void StartMatchTimer(InputHandler inputHandler, int length =10)
        {
            // Set the end time for the specified length in minutes
            length = inputHandler.GetNumber("How long should a match be? (In minutes)");
            _endTime = DateTime.Now.AddMinutes(length);

            // Create a timer with a 1 second interval
            Timer timer = new Timer(1000);
            timer.Elapsed += OnTimedEvent;

            // Start the timer
            timer.Start();
            inputHandler.View.ShowMessage($"Timer started for {length} minutes.");
        }

        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            TimeSpan timeRemaining = _endTime - DateTime.Now;

            if (timeRemaining.TotalSeconds <= 0)
            {
                Console.WriteLine("Time's up!");
                Timer timer = (Timer)source;
                timer.Stop();
            }
            else
            {
                Console.WriteLine($"Time remaining: {timeRemaining:mm\\:ss}");
            }
        }
    }
}