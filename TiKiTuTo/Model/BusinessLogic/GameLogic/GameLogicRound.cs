using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using TiKiTuTo.Controller;
using TiKiTuTo.Model.DataObjects;
using TiKiTuTo.View;
using Timer = System.Timers.Timer;
using System.Media;

namespace TiKiTuTo.Model.BusinessLogic.GameLogic
{
    /// <summary>
    /// Handles business logic regarding Rounds objects. 
    /// </summary>
    public class GameLogicRound
    {
        InputHandler InputHandler { get; set; }
        JSONService JSONService { get; set; }
        public GameLogicRound(InputHandler inputHandler, JSONService json) 
        {
            InputHandler = inputHandler;
            JSONService = json;
        }
        private Random random = new Random();

        public void InitPreliminaryRound(Tournament tournament)
        {
            // fill the list of matches tournament.GamePlanPreliminaryRound
            if (tournament.Settings == null || tournament.Settings.TeamsInTournament == null || tournament.Settings.TeamsInTournament.Count < 2)
            {
                throw new ArgumentException("Tournament settings or teams are not properly configured.");
            }
            int gamesPerTeam = tournament.Settings.NumberOfPreliminaryGamesPerTeam;
            List<Team> teams = tournament.Settings.TeamsInTournament;

            // Verify if it's possible to generate the required number of matches
            int totalGamesNeeded = teams.Count * gamesPerTeam / 2;
            int totalPossibleMatches = teams.Count * (teams.Count - 1) / 2;
            if (totalGamesNeeded > totalPossibleMatches)
            {
                throw new InvalidOperationException("Not enough teams to generate the required number of matches.");
            }

            // Initialize the preliminary round matches
            var teamMatchCount = new Dictionary<Team, int>();
            var matchesCreated = new HashSet<(Team, Team)>();

            // Initialize match count for each team
            foreach (var team in teams)
            {
                teamMatchCount[team] = 0;
            }

            // Generate matches randomly
            while (teamMatchCount.Values.Any(count => count < gamesPerTeam))
            {
                // Select two random teams
                var availableTeams = teams.Where(t => teamMatchCount[t] < gamesPerTeam).ToList();

                Team teamA = availableTeams[random.Next(availableTeams.Count)];
                Team teamB = availableTeams[random.Next(availableTeams.Count)];

                // Ensure the teams are not the same and have not already played against each other
                if (teamA != teamB && !matchesCreated.Contains((teamA, teamB)) && !matchesCreated.Contains((teamB, teamA)))
                {
                    // Create the match
                    var match = new Match(teamA, teamB);
                    tournament.GamePlanPremilimaryRound.Add(match);

                    // Update counts
                    teamMatchCount[teamA]++;
                    teamMatchCount[teamB]++;
                    matchesCreated.Add((teamA, teamB));
                }
            }
            Console.WriteLine($"Preliminary round initialized with {tournament.GamePlanPremilimaryRound.Count} matches.");
            tournament.GamePlanPremilimaryRound.ForEach(match => Console.WriteLine($"{match.teamA.TeamName} vs {match.teamB.TeamName}"));
            Console.WriteLine("Good luck to all teams!");
            Thread.Sleep(10000); // Simulate some delay for better readability in console output - delete it in prod
        }

        public void RunPreliminaryRound(Tournament tournament)
        {
            // take a list of matches tournament.GamePlanPreliminaryRound and execute it, asking the goals scored, updating the teams attributes accordingly (teamA.goalsScored, etc)
        }

        public void InitKoRound(Tournament tournament)
        {
            // teams for ko round are selected -> fill tournament.TeamsInKoRound
            // and organice them for the knockout round -> fill list of matches for KO round "tournament.GamePlanKoRound"
        }

        public void RunKoRound(Tournament tournament)
        {
            // take a list of matches tournament.GamePlanKoRound and execute it, asking the goals scored, updating the teams accordingly
            // at the end there is a winner
        }

        private DateTime _endTime;
        public void StartMatchTimer( int? duration = 10)
        {
            // Set the end time for the specified length in minutes
            while (duration == 0)
            {
                duration = InputHandler.GetNumber("Please enter the match duration in full minutes");
            }
            _endTime = DateTime.Now.AddMinutes((double)duration);

            // Create a timer with a 1 second interval
            Timer timer = new Timer(1000);

            //lambda method to allow for usage of inputHandler.View.
            timer.Elapsed += (sender, e) =>
            {
                TimeSpan timeRemaining = _endTime - DateTime.Now;

                if (timeRemaining.TotalSeconds <= 0)
                {
                    InputHandler.View.ShowMessage("Time's up!");
                    SystemSounds.Asterisk.Play();
                    //                    GameLogicMatch.FinishMatch();
                    timer.Stop();
                }
                else
                {
                    // inputHandler.View.ClearCurrentConsoleLine();
                    InputHandler.View.ShowMessage($"Time remaining: {timeRemaining:mm\\:ss}");
                }
            };

            // Start the timer
            InputHandler.View.ShowMessage($"Timer started for {duration} minutes.");
            InputHandler.View.WriteEmptyLine();
            timer.Start();
        }
    }
    }