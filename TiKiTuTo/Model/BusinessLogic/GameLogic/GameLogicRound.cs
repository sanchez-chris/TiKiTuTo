using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Schema;
using TiKiTuTo.Controller;
using TiKiTuTo.Model.DataObjects;
using TiKiTuTo.View;
using Match = TiKiTuTo.Model.DataObjects.Match;


namespace TiKiTuTo.Model.BusinessLogic.GameLogic
{
    /// <summary>
    /// Handles business logic regarding Rounds objects. 
    /// </summary>
    public class GameLogicRound
    {
        Team finalist = new Team("finalist");
        Team thirdPosition = new Team("3.");
        List<Team> semifinalists = new List<Team>();
        bool isSemifinalPlayed = false;


        // Declare properties
        private InputValidator _inputValidator;

        InputHandler InputHandler { get; set; }
        JSONService JSONService { get; set; }
        TournamentModel TournamentModel { get; set; }

        // Declare GameLogicMatch without initializing it here
        GameLogicMatch GameLogicMatch;

        // Constructor
        public GameLogicRound(InputHandler inputHandler, JSONService json, InputValidator inputValidator, TournamentModel tournamentModel)
        {
            // Set properties
            _inputValidator = inputValidator;
            InputHandler = inputHandler;
            JSONService = json;
            TournamentModel = tournamentModel;

            // Initialize GameLogicMatch after properties are set
            GameLogicMatch = new GameLogicMatch(InputHandler, JSONService);
        }   

        private Random random = new Random();

        public void InitPreliminaryRound()
        {
            var Tournament = TournamentModel.Tournament;

            var Settings = TournamentModel.Tournament.Settings;

            List<Match> GamePlanPreliminaryRound = Tournament.GamePlanPreliminaryRound;

            // fill the list of matches tournament.GamePlanPreliminaryRound
            if (Settings == null || Settings.TeamsInTournament == null || Settings.TeamsInTournament.Count < 2)
            {
                throw new ArgumentException("Tournament settings or teams are not properly configured.");
            }
            int gamesPerTeam = Settings.NumberOfPreliminaryGamesPerTeam;
            List<Team> teams = Settings.TeamsInTournament;

            InputHandler.View.ShowMessage("\n\nTeams in Tournament:");

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

                    GamePlanPreliminaryRound.Add(match);

                    // Update counts
                    teamMatchCount[teamA]++;
                    teamMatchCount[teamB]++;
                    matchesCreated.Add((teamA, teamB));
                }
            }
            InputHandler.View.ShowMessage($"Preliminary round initialized with {GamePlanPreliminaryRound.Count} matches.");
            GamePlanPreliminaryRound.ForEach(match => InputHandler.View.ShowMessage($"{match.teamA.TeamName} vs {match.teamB.TeamName}"));
            InputHandler.View.ShowMessage("\n\nGood luck to all teams!");
        }

        public void RunPreliminaryRound()
        {
            var tournament = TournamentModel.Tournament;
            // take a list of matches tournament.GamePlanPreliminaryRound and execute it, asking the goals scored, updating the teams attributes accordingly (teamA.goalsScored, etc)
            foreach (var match in tournament.GamePlanPreliminaryRound)
            {
                GameLogicMatch.RunMatch(match);
            }
            
            tournament.PreliminaryStandings = GenerateRanking(tournament);
            InputHandler.View.ShowMessage("\n\nRankings:");
            foreach (var team in tournament.PreliminaryStandings)
            {
                InputHandler.View.ShowMessage($"{team.TeamName} - Games won: {team.NumberGamesWon} - Goals difference: {team.Goaldifference} - Goals scored: {team.NumberGoals} - Goals received: {team.NumberGoals - team.Goaldifference}");
            }
        }

        public void InitKoRound()
        {
            var tournament = TournamentModel.Tournament;

            int currentRound = 0;
            if (!_inputValidator.HasValidTournamentSettings(tournament))
            {
                throw new ArgumentException("Tournament settings or teams are not properly configured.");
            }

            tournament.GamePlanKoRound.Clear(); // Clear previous matches if any

            // Select teams for KO round
            tournament.KoStandings = tournament.PreliminaryStandings.Take(tournament.Settings.NumberOfTeamsInKoRound).ToList();
            InputHandler.View.ShowMessage("\n\nKO Round contestants:\n");
            foreach (var team in tournament.KoStandings)
            {
                InputHandler.View.ShowMessage($"{team.TeamName}");
            }


            // Randomly reorder the list KoStandings
            Random random = new Random();
            tournament.KoStandings = tournament.KoStandings.OrderBy(x => random.Next()).ToList();

            // Initialize the GamePlanKoRound with empty rounds
            int totalRounds = (int)Math.Ceiling(Math.Log2(tournament.KoStandings.Count)) -1;
            for (int i = 0; i < totalRounds; i++)
            {
                tournament.GamePlanKoRound.Add(new List<Match>());
            }
            organizeMatchesForNextRound(tournament, currentRound);
        }

        public void ShowKGamePlanKoRound(Tournament tournament, int currentRound)
        {
            // Calculate how many rounds there are in the tournament
            int totalRounds = (int)Math.Ceiling(Math.Log2(tournament.KoStandings.Count));

            // Validate that the current round is valid
            if (currentRound < 0 || currentRound > totalRounds)
            {
                InputHandler.View.ShowMessage($"Invalid round number: {currentRound}. There are {totalRounds} rounds in the tournament.");
                return;
            }

            // Show the current round
            //InputHandler.View.ShowMessage($"Current Round: {currentRound + 1}");
            InputHandler.View.ShowMessage("\n");

            if (currentRound == totalRounds)
            {
                InputHandler.View.ShowMessage("Final:");
                InputHandler.View.ShowMessage("Match:");
            }
            else if (currentRound == totalRounds-1 && totalRounds > 2)
            {
                InputHandler.View.ShowMessage("Semifinal:");
                InputHandler.View.ShowMessage("Matches:");
            }
            else
            {
                InputHandler.View.ShowMessage("Matches:");
            }

            InputHandler.View.ShowMessage(new string('-', 20));

            int spacing = (int)Math.Pow(2, totalRounds - currentRound - 1) * 2; // Dynamic space for the current round

            foreach (var match in tournament.GamePlanKoRound[currentRound])
            {
                InputHandler.View.ShowMessage($"{match.teamA.TeamName.PadRight(spacing)} vs {match.teamB.TeamName.PadRight(spacing)}");
            }

        }

        public void RunKoRound()
        {
            var tournament = TournamentModel.Tournament;

            // Calculate how many rounds there are in the tournament
            int totalRounds = (int)Math.Ceiling(Math.Log2(tournament.KoStandings.Count));
            int currentRound = 0;

            while (currentRound < totalRounds)
            {
                ShowKGamePlanKoRound(tournament, currentRound);


                foreach (var match in tournament.GamePlanKoRound[currentRound])
                {
                    updateStandings(tournament, match);
                }

                currentRound++;
                if (currentRound < totalRounds)
                {
                    organizeMatchesForNextRound(tournament, currentRound);
                }
            }

            if (tournament.KoStandings.Count == 1)
            {
                Team winner = tournament.KoStandings[0];
                InputHandler.View.ShowMessage($"\n\nThe winner of the KO round is {winner.TeamName}!");
                InputHandler.View.ShowMessage($"\n\nThe 2. Position of the KO round is {finalist.TeamName}!");

                if (totalRounds >= 2 && !isSemifinalPlayed) // there is a semifinal
                {
                    isSemifinalPlayed = true;
                    InputHandler.View.ShowMessage("\nLets decide the 3. Position!");
                    Match semifinal = new Match(semifinalists[0], semifinalists[1]);
                    GameLogicMatch.RunMatch(semifinal);
                    if (semifinalists[0].NumberGoals > semifinalists[1].NumberGoals)
                    {
                        thirdPosition = semifinalists[0];
                    }
                    else
                    {
                        thirdPosition = semifinalists[1];

                    }
                    InputHandler.View.ShowMessage($"\n\nThe 3. Position of the KO round are {thirdPosition.TeamName}.");
                }

                


            }
        }

        public List<Team> GenerateRanking(Tournament tournament)
        {
            List<Team> Teams = tournament.Settings.TeamsInTournament;

            return Teams
                    .OrderByDescending(t => t.NumberGamesWon)
                    .ThenByDescending(t => t.Goaldifference)
                    .ThenByDescending(t => t.NumberGoals)
                    .ToList();
         }


        public void organizeMatchesForNextRound(Tournament tournament, int currentRound)
        {
            for (int i = 0; i < tournament.KoStandings.Count; i += 2)
            {
                if (i + 1 < tournament.KoStandings.Count) // Ensure there is a pair
                {
                    var match = new Match(tournament.KoStandings[i], tournament.KoStandings[i + 1]);
                    // ensure there is place for a new round
                    while (tournament.GamePlanKoRound.Count <= currentRound)
                    {
                        tournament.GamePlanKoRound.Add(new List<Match>());
                    }
                    tournament.GamePlanKoRound[currentRound].Add(match);
                }
            }
        }

        public void updateStandings(Tournament tournament, Match match)
        {

            bool hasWinner = false;

            while (!hasWinner)
            {
                GameLogicMatch.RunMatch(match);
                if (match.goalsTeamA > match.goalsTeamB)
                {

                    if (tournament.KoStandings.Count == 4 || tournament.KoStandings.Count == 3) // semifinal 
                    {
                        semifinalists.Add(match.teamB);
                    }
                    if (tournament.KoStandings.Count == 2) // final
                    {
                        finalist = match.teamB;
                    }

                    tournament.KoStandings.Remove(match.teamB);
                    hasWinner = true;
                }
                else if (match.goalsTeamA < match.goalsTeamB)
                {
                    if (tournament.KoStandings.Count == 4 || tournament.KoStandings.Count == 3) // semifinal
                    {
                        semifinalists.Add(match.teamA);
                    }
                    if (tournament.KoStandings.Count == 2) // final
                    {
                        finalist = match.teamA;
                    }
                    tournament.KoStandings.Remove(match.teamA);
                    hasWinner = true;
                }
                else
                {
                    InputHandler.View.ShowMessage($"\n\nThe match between {match.teamA.TeamName} and {match.teamB.TeamName} ended in a draw. Repeating the match...");
                }
            }
        }
    }
    }