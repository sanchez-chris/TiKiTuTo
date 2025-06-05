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
        private InputValidator _inputValidator;

        InputHandler InputHandler { get; set; }
        JSONService JSONService { get; set; }

        // Declare GameLogicMatch without initializing it here
        GameLogicMatch GameLogicMatch;

        // Constructor
        public GameLogicRound(InputHandler inputHandler, JSONService json, InputValidator inputValidator)
        {
            // Set properties
            _inputValidator = inputValidator;
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

            InputHandler.View.ShowMessage("Teams in Tournament:");

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
            InputHandler.View.ShowMessage($"Preliminary round initialized with {tournament.GamePlanPremilimaryRound.Count} matches.");
            tournament.GamePlanPremilimaryRound.ForEach(match => InputHandler.View.ShowMessage($"{match.teamA.TeamName} vs {match.teamB.TeamName}"));
            InputHandler.View.ShowMessage("Good luck to all teams!");
        }

        public void RunPreliminaryRound(Tournament tournament)
        {
            // take a list of matches tournament.GamePlanPreliminaryRound and execute it, asking the goals scored, updating the teams attributes accordingly (teamA.goalsScored, etc)
            foreach (var match in tournament.GamePlanPremilimaryRound)
            {
                GameLogicMatch.RunMatch(match);
            }
            
            tournament.PreliminaryStandings = GenerateRanking(tournament);
            InputHandler.View.ShowMessage("Rankings:");
            foreach (var team in tournament.PreliminaryStandings)
            {
                InputHandler.View.ShowMessage($"{team.TeamName} - Games won: {team.NumberGamesWon} - Goals scored: {team.NumberGoals}");
            }
        }

        public void InitKoRound(Tournament tournament)
        {
            if (!_inputValidator.IsValidTournamentSettings(tournament))
            {
                throw new ArgumentException("Tournament settings or teams are not properly configured.");
            }

            tournament.GamePlanKoRound.Clear(); // Clear previous matches if any
            InputHandler.View.ShowMessage("KO Round contestants:");

            // Select teams for KO round
            tournament.KoStandings = tournament.PreliminaryStandings.Take(tournament.Settings.NumberOfTeamsInKoRound).ToList();

            // Randomly reorder the list KoStandings
            Random random = new Random();
            tournament.KoStandings = tournament.KoStandings.OrderBy(x => random.Next()).ToList();

            // Initialize the GamePlanKoRound with empty rounds
            int totalRounds = (int)Math.Ceiling(Math.Log2(tournament.KoStandings.Count));
            for (int i = 0; i < totalRounds; i++)
            {
                tournament.GamePlanKoRound.Add(new List<Match>());
            }

            // Organize them for the knockout round
            for (int i = 0; i < tournament.KoStandings.Count; i += 2)
            {
                if (i + 1 < tournament.KoStandings.Count) // Ensure there is a pair
                {
                    var match = new Match(tournament.KoStandings[i], tournament.KoStandings[i + 1]);
                    tournament.GamePlanKoRound[0].Add(match); // Add to the first round
                }
            }
        }

        public void ShowKGamePlanKoRound(Tournament tournament, int currentRound)
        {
            // Calculate how many rounds there are in the tournament
            int totalRounds = (int)Math.Ceiling(Math.Log2(tournament.KoStandings.Count));

            InputHandler.View.ShowMessage("\n KO Round\n");

            // Validate that the current round is valid
            if (currentRound < 0 || currentRound > totalRounds)
            {
                InputHandler.View.ShowMessage($"Invalid round number: {currentRound}. There are {totalRounds} rounds in the tournament.");
                return;
            }

            // Show the current round
            InputHandler.View.ShowMessage($"Current Round: {currentRound + 1}");
            InputHandler.View.ShowMessage(new string('-', 20));

            int spacing = (int)Math.Pow(2, totalRounds - currentRound - 1) * 2; // Dynamic space for the current round

            foreach (var match in tournament.GamePlanKoRound[currentRound])
            {
                InputHandler.View.ShowMessage($"{match.teamA.TeamName.PadRight(spacing)} vs {match.teamB.TeamName.PadRight(spacing)}");
            }

            InputHandler.View.ShowMessage("\n");
        }

        public void RunKoRound(Tournament tournament)
        {
            // Calculate how many rounds there are in the tournament
            int totalRounds = (int)Math.Ceiling(Math.Log2(tournament.KoStandings.Count));
            int currentRound = 0;

            while (currentRound < totalRounds)
            {
                ShowKGamePlanKoRound(tournament, currentRound);

                foreach (var match in tournament.GamePlanKoRound[currentRound])
                {
                    bool hasWinner = false;

                    while (!hasWinner)
                    {
                        GameLogicMatch.RunMatch(match);

                        if (match.goalsTeamA > match.goalsTeamB)
                        {
                            tournament.KoStandings.Remove(match.teamB);
                            hasWinner = true;
                        }
                        else if (match.goalsTeamA < match.goalsTeamB)
                        {
                            tournament.KoStandings.Remove(match.teamA);
                            hasWinner = true;
                        }
                        else
                        {
                            InputHandler.View.ShowMessage($"The match between {match.teamA.TeamName} and {match.teamB.TeamName} ended in a draw. Repeating the match...");
                        }
                    }
                }

                currentRound++;
                // Prepare matches for the next round
                if (currentRound < totalRounds)
                {
                    for (int i = 0; i < tournament.KoStandings.Count; i += 2)
                    {
                        if (i + 1 < tournament.KoStandings.Count) // Ensure there is a pair
                        {
                            var match = new Match(tournament.KoStandings[i], tournament.KoStandings[i + 1]);
                            tournament.GamePlanKoRound[currentRound].Add(match);
                        }
                    }
                }
            }

            if (tournament.KoStandings.Count == 1)
            {
                Team winner = tournament.KoStandings.First();
                InputHandler.View.ShowMessage($"The winner of the knockout round is {winner.TeamName}!");
            }
            else
            {
                InputHandler.View.ShowMessage("No matches were played in the knockout round.");
            }
        }

        public List<Team> GenerateRanking(Tournament tournament)
        {
            List<Team> Teams = tournament.Settings.TeamsInTournament;

            return Teams
                    .OrderByDescending(t => t.NumberGamesWon)
                    .ThenByDescending(t => t.NumberGoals)
                    .ToList();
         }

            public void UpdateTeamScores(Team teamA, int goalsA, Team teamB, int goalsB)
            {
            if (goalsA > goalsB)
            {
                teamA.NumberGamesWon++;
            }
            if (goalsB > goalsA)
            {
                teamB.NumberGamesWon++;
            }
            teamA.Goaldifference += goalsA - goalsB;
            teamA.NumberGoals += goalsA;
            teamB.Goaldifference += goalsB - goalsA;
            teamB.NumberGoals += goalsB;
            }


    }
    }