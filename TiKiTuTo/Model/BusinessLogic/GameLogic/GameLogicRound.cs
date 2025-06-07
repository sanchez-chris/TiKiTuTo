using System;
using System.Collections.Generic;
using System.Linq;
using TiKiTuTo.Controller;
using TiKiTuTo.Model.DataObjects;
using TiKiTuTo.View;

namespace TiKiTuTo.Model.BusinessLogic.GameLogic
{
    /// <summary>
    /// Handles business logic related to rounds in a tournament.
    /// This includes initializing, running, and managing preliminary and KO rounds.
    /// </summary>
    public class GameLogicRound
    {
        // Dependencies injected via the constructor
        private readonly InputValidator _inputValidator;
        private readonly InputHandler _inputHandler;
        private readonly JSONService _jsonService;
        private readonly TournamentModel _tournamentModel;

        // GameLogicMatch instance for handling individual matches
        private readonly GameLogicMatch _gameLogicMatch;

        // Random instance for generating random values
        private readonly Random _random = new Random();

        /// <summary>
        /// Constructor for GameLogicRound.
        /// Initializes dependencies and sets up the GameLogicMatch instance.
        /// </summary>
        /// 
        public GameLogicRound(InputHandler inputHandler, JSONService jsonService, InputValidator inputValidator, TournamentModel tournamentModel)
        {
            _inputValidator = inputValidator;
            _inputHandler = inputHandler;
            _jsonService = jsonService;
            _tournamentModel = tournamentModel;

            // Initialize GameLogicMatch after dependencies are set
            _gameLogicMatch = new GameLogicMatch(inputHandler, jsonService);
        }

        /// <summary>
        /// Initializes the preliminary round by generating matches based on tournament settings.
        /// </summary>
        /// 
        public void InitPreliminaryRound()
        {
            ValidateTournamentSettings();

            var tournament = _tournamentModel.Tournament;
            var teams = tournament.TournamentSettings.TeamsInTournament;
            var gamePlan = tournament.GamePlanPreliminaryRound;

            var teamMatchCount = InitializeMatchCount(teams);

            GenerateRandomMatches(teams, gamePlan, teamMatchCount);

            _inputHandler.View.ShowMessage("Good luck to all teams!");
            WaitForUserToStart();
        }

        private Dictionary<Team, int> InitializeMatchCount(List<Team> teams)
        {
            var teamMatchCount = new Dictionary<Team, int>();
            foreach (var team in teams)
            {
                teamMatchCount[team] = 0;
            }
            return teamMatchCount;
        }

        private void GenerateRandomMatches(List<Team> teams, List<Match> gamePlan, Dictionary<Team, int> teamMatchCount)
        {
            int gamesPerTeam = _tournamentModel.Tournament.TournamentSettings.NumberOfPreliminaryGamesPerTeam;
            var matchesCreated = new HashSet<(Team, Team)>();

            while (teamMatchCount.Values.Any(count => count < gamesPerTeam))
            {
                var availableTeams = teams.Where(t => teamMatchCount[t] < gamesPerTeam).ToList();

                Team teamA = availableTeams[_random.Next(availableTeams.Count)];
                Team teamB = availableTeams[_random.Next(availableTeams.Count)];

                if (teamA != teamB && !matchesCreated.Contains((teamA, teamB)) && !matchesCreated.Contains((teamB, teamA)))
                {
                    var match = new Match(teamA, teamB);
                    gamePlan.Add(match);

                    teamMatchCount[teamA]++;
                    teamMatchCount[teamB]++;
                    matchesCreated.Add((teamA, teamB));
                }
            }
        }


        /// <summary>
        /// Runs the preliminary round by executing each match and updating team standings.
        /// </summary>
        public void RunPreliminaryRound()
        {
            var tournament = _tournamentModel.Tournament;

            _inputHandler.View.ShowMessage($"Preliminary round initialized with {tournament.GamePlanPreliminaryRound.Count} matches.");
            _inputHandler.View.ShowMessage("\nGameplan preliminary round:\n");

            // Display all matches in the preliminary round
            tournament.GamePlanPreliminaryRound.ForEach(match =>
                _inputHandler.View.ShowMessage($"{match.teamA.TeamName} vs {match.teamB.TeamName}"));

            // Execute each match and update team attributes
            foreach (var match in tournament.GamePlanPreliminaryRound)
            {
                _gameLogicMatch.RunMatch(match);
            }

            // Generate rankings based on match results
            tournament.PreliminaryStandings = GenerateRanking(tournament);

            // Display the rankings
            _inputHandler.View.ShowMessage("\nRankings: ");
            foreach (var team in tournament.PreliminaryStandings)
            {
                _inputHandler.View.ShowMessage($"{team.TeamName} - Games won: {team.NumberGamesWon} - Goals difference: {team.Goaldifference} - Goals scored: {team.NumberGoals} - Goals received: {team.NumberGoals - team.Goaldifference}");
            }
        }

        /// <summary>
        /// Initializes the knockout (KO) round by selecting teams and organizing matches.
        /// </summary>
        public void InitKoRound()
        {
            var tournament = _tournamentModel.Tournament;

            ValidateTournamentSettings();

            tournament.GamePlanKoRound.Clear(); // Clear previous KO round matches
            tournament.KoStandings = tournament.PreliminaryStandings
                .Take(tournament.TournamentSettings.NumberOfTeamsInKoRound)
                .ToList();

            // Display KO round contestants
            _inputHandler.View.ShowMessage("\nKO Round contestants:\n");
            foreach (var team in tournament.KoStandings)
            {
                _inputHandler.View.ShowMessage($"{team.TeamName}");
            }

            // Randomly shuffle teams for the KO round
            tournament.KoStandings = tournament.KoStandings.OrderBy(x => _random.Next()).ToList();

            // Initialize KO round matches
            int totalRounds = (int)Math.Ceiling(Math.Log2(tournament.KoStandings.Count)) - 1;
            for (int i = 0; i < totalRounds; i++)
            {
                tournament.GamePlanKoRound.Add(new List<Match>());
            }

            OrganizeMatchesForNextRound(tournament, 0);
        }

        /// <summary>
        /// Displays the game plan for the current KO round.
        /// </summary>
        public void ShowKoGamePlanKoRound(Tournament tournament, int currentRound)
        {
            int totalRounds = (int)Math.Ceiling(Math.Log2(tournament.KoStandings.Count));

            // Validate round number
            if (currentRound < 0 || currentRound > totalRounds)
            {
                _inputHandler.View.ShowMessage($"Invalid round number: {currentRound}. There are {totalRounds} rounds in the tournament.");
                return;
            }

            // Display the current round
            _inputHandler.View.ShowMessage("\n");

            if (currentRound == totalRounds)
            {
                _inputHandler.View.ShowMessage("Final:");
            }
            else if (currentRound == totalRounds - 1 && totalRounds > 2)
            {
                _inputHandler.View.ShowMessage("Semifinal:");
            }
            else
            {
                _inputHandler.View.ShowMessage("Gameplan KO round:");
            }

            _inputHandler.View.ShowMessage(new string('-', 20));

            // Display matches with dynamic spacing
            int spacing = (int)Math.Pow(2, totalRounds - currentRound - 1) * 2;
            foreach (var match in tournament.GamePlanKoRound[currentRound])
            {
                _inputHandler.View.ShowMessage($"{match.teamA.TeamName.PadRight(spacing)} vs {match.teamB.TeamName.PadRight(spacing)}");
            }
        }

        /// <summary>
        /// Runs the knockout (KO) round by executing matches and updating standings.
        /// </summary>
        public void RunKoRound()
        {
            var tournament = _tournamentModel.Tournament;
            int totalRounds = (int)Math.Ceiling(Math.Log2(tournament.KoStandings.Count));

            while (tournament.CurrentRound < totalRounds)
            {
                ShowKoGamePlanKoRound(tournament, tournament.CurrentRound);

                foreach (var match in tournament.GamePlanKoRound[tournament.CurrentRound])
                {
                    UpdateStandings(tournament, match);
                }

                tournament.CurrentRound++;
                if (tournament.CurrentRound < totalRounds)
                {
                    OrganizeMatchesForNextRound(tournament, tournament.CurrentRound);
                }

                HandleSemifinal(tournament);
            }

            DeclareWinner(tournament);
        }

        /// <summary>
        /// Generates rankings based on team performance in the preliminary round.
        /// </summary>
        public List<Team> GenerateRanking(Tournament tournament)
        {
            return tournament.TournamentSettings.TeamsInTournament
                .OrderByDescending(t => t.NumberGamesWon)
                .ThenByDescending(t => t.Goaldifference)
                .ThenByDescending(t => t.NumberGoals)
                .ToList();
        }

        /// <summary>
        /// Organizes matches for the next KO round.
        /// </summary>
        public void OrganizeMatchesForNextRound(Tournament tournament, int currentRound)
        {
            for (int i = 0; i < tournament.KoStandings.Count; i += 2)
            {
                if (i + 1 < tournament.KoStandings.Count)
                {
                    var match = new Match(tournament.KoStandings[i], tournament.KoStandings[i + 1]);

                    while (tournament.GamePlanKoRound.Count <= currentRound)
                    {
                        tournament.GamePlanKoRound.Add(new List<Match>());
                    }

                    tournament.GamePlanKoRound[currentRound].Add(match);
                }
            }
        }

        /// <summary>
        /// Waits for user input to proceed.
        /// </summary>
        public void WaitForUserToStart()
        {
            _inputHandler.View.ShowMessage("Press enter to continue.");
            _inputHandler.View.ReadInput();
        }

        /// <summary>
        /// Updates standings after a match has been completed.
        /// </summary>
        public void UpdateStandings(Tournament tournament, Match match)
        {
            bool hasWinner = false;

            while (!hasWinner)
            {
                if (!match.finished)
                {
                    _gameLogicMatch.RunMatch(match);
                }

                if (match.goalsTeamA > match.goalsTeamB)
                {
                    HandleMatchWinner(tournament, match.teamA, match.teamB);
                    hasWinner = true;
                }
                else
                {
                    HandleMatchWinner(tournament, match.teamB, match.teamA);
                    hasWinner = true;
                }
            }
        }

        /// <summary>
        /// Validates tournament settings to ensure proper configuration.
        /// </summary>
        private void ValidateTournamentSettings()
        {
            var settings = _tournamentModel.Tournament.TournamentSettings;
            if (settings == null || settings.TeamsInTournament == null || settings.TeamsInTournament.Count < 2)
            {
                throw new ArgumentException("Tournament settings or teams are not properly configured.");
            }
        }

        /// <summary>
        /// Handles the winner of a match in the KO round.
        /// </summary>
        private void HandleMatchWinner(Tournament tournament, Team winner, Team loser)
        {
            if (tournament.KoStandings.Count <= 4) // Semifinal logic
            {
                tournament.Semifinalists.Add(loser);
            }

            if (tournament.KoStandings.Count == 2) // Final logic
            {
                tournament.Finalist = loser;
            }

            tournament.KoStandings.Remove(loser);
        }

        /// <summary>
        /// Handles logic for the semifinal round.
        /// </summary>
        private void HandleSemifinal(Tournament tournament)
        {
            if (tournament.KoStandings.Count == 2 && !tournament.IsSemifinalPlayed)
            {
                tournament.IsSemifinalPlayed = true;
                _inputHandler.View.ShowMessage("\nLet's decide the 3rd position!");

                var semifinal = new Match(tournament.Semifinalists[0], tournament.Semifinalists[1]);
                _gameLogicMatch.RunMatch(semifinal);

                tournament.ThirdPosition = semifinal.goalsTeamA > semifinal.goalsTeamB
                    ? tournament.Semifinalists[0]
                    : tournament.Semifinalists[1];

                _inputHandler.View.ShowMessage($"\nThe 3rd position goes to { tournament.ThirdPosition.TeamName}.");
            }
        }

        /// <summary>
        /// Declares the winner of the tournament.
        /// </summary>
        private void DeclareWinner(Tournament tournament)
        {
            if (tournament.KoStandings.Count == 1)
            {
                tournament.Winner = tournament.KoStandings[0];
                _inputHandler.View.ShowMessage($"\nThe winner is { tournament.Winner.TeamName }!");

                _inputHandler.View.ShowMessage($"\n1. { tournament.Winner.TeamName}");
                _inputHandler.View.ShowMessage($"\n2. { tournament.Finalist.TeamName}");

                if (tournament.ThirdPosition != null)
                {
                    _inputHandler.View.ShowMessage($"\n3. { tournament.ThirdPosition.TeamName}");
                }
            }
        }
    }
}