using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using TiKiTuTo.Controller;
using TiKiTuTo.Model.DataObjects;
using TiKiTuTo.View;


namespace TiKiTuTo.Model.BusinessLogic.GameLogic
{
    /// <summary>
    /// Handles business logic regarding Rounds objects. 
    /// </summary>
    public class GameLogicRound
    {

        // Declare properties
        InputHandler InputHandler { get; set; }
        JSONService JSONService { get; set; }

        // Declare GameLogicMatch without initializing it here
        GameLogicMatch GameLogicMatch;

        // Constructor
        public GameLogicRound(InputHandler inputHandler, JSONService json)
        {
            // Set properties
            InputHandler = inputHandler;
            JSONService = json;

            // Initialize GameLogicMatch after properties are set
            GameLogicMatch = new GameLogicMatch(InputHandler, JSONService);
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

            Console.WriteLine("Teams in Tournament:");
            foreach (Team team in teams)
            {
                Console.WriteLine(team);
            }

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
                    Match match = new Match(teamA, teamB);

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
            Thread.Sleep(3000); // Simulate some delay for better readability in console output - TEMP
        }

        public void RunPreliminaryRound(Tournament tournament)
        {
            // take a list of matches tournament.GamePlanPreliminaryRound and execute it, asking the goals scored, updating the teams attributes accordingly (teamA.goalsScored, etc)
            foreach (var match in tournament.GamePlanPremilimaryRound)
            {
                GameLogicMatch.RunMatch(match);
            }
        }

        public void InitKoRound(Tournament tournament)
        {
            // teams for ko round are selected (how many teams, in Tournament.TournamentSettings.NumberOfTeamsInKoRound) -> fill Tournament.TeamsInKoRound
            if (tournament.Settings == null || tournament.Settings.TeamsInTournament == null || tournament.Settings.TeamsInTournament.Count < 2)
            {
                throw new ArgumentException("Tournament settings or teams are not properly configured.");
            }
            int numberOfTeamsInKoRound = tournament.Settings.NumberOfTeamsInKoRound;
            tournament.TeamsInKoRound = tournament.PreliminaryStandings.OrderByDescending(t => t.NumberGoals).Take(numberOfTeamsInKoRound).ToList();
            // and organice them for the knockout round -> fill list of matches for KO round "tournament.GamePlanKoRound"
            tournament.GamePlanKoRound.Clear(); // Clear previous matches if any
            // shuffle the list
            var shuffledTeams = tournament.TeamsInKoRound.OrderBy(x => random.Next()).ToList();
            // create matches in pairs
            for (int i = 0; i < shuffledTeams.Count; i += 2)
            {
                if (i + 1 < shuffledTeams.Count) // Ensure there is a pair
                {
                    var match = new Match(shuffledTeams[i], shuffledTeams[i + 1]);
                    tournament.GamePlanKoRound.Add(match);
                }
            }
        }

        public void RunKoRound(Tournament tournament)
        {
            // take a list of matches tournament.GamePlanKoRound and execute it, asking the goals scored, updating the teams accordingly
            foreach (var match in tournament.GamePlanKoRound)
            {
                GameLogicMatch.RunMatch(match);
            }
            // at the end there is a winner
            if (tournament.GamePlanKoRound.Count > 0)
            {
                var winner = tournament.GamePlanKoRound[0].teamA; // Assuming the first match's teamA is the winner
                InputHandler.View.ShowMessage($"The winner of the knockout round is {winner.TeamName}!");
            }
            else
            {
                InputHandler.View.ShowMessage("No matches were played in the knockout round.");
            }
        }


    }
    }