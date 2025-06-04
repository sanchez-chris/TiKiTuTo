using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using TiKiTuTo.Controller;
using TiKiTuTo.Model.DataObjects;
using TiKiTuTo.View;
using TiKiTuTo.Model;


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
            var GamePlanPreliminaryRound = TournamentModel.Tournament.GamePlanPreliminaryRound;


            // fill the list of matches tournament.GamePlanPreliminaryRound
            if (!_inputValidator.HasValidTournamentSettings(TournamentModel.Tournament))
            {
                throw new ArgumentException("Tournament settings or teams are not properly configured.");
            }
            
            int gamesPerTeam = Settings.NumberOfPreliminaryGamesPerTeam;
            List<Team> teams = Settings.TeamsInTournament;

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

                    GamePlanPreliminaryRound.Add(match);

                    // Update counts
                    teamMatchCount[teamA]++;
                    teamMatchCount[teamB]++;
                    matchesCreated.Add((teamA, teamB));
                }
            }
            InputHandler.View.ShowMessage($"Preliminary round initialized with {GamePlanPreliminaryRound.Count} matches.");
            GamePlanPreliminaryRound.ForEach(match => InputHandler.View.ShowMessage($"{match.teamA.TeamName} vs {match.teamB.TeamName}"));
            InputHandler.View.ShowMessage("Good luck to all teams!");
        }

        public void RunPreliminaryRound()
        {
            var Tournament = TournamentModel.Tournament;
            

            // take a list of matches tournament.GamePlanPreliminaryRound and execute it, asking the goals scored, updating the teams attributes accordingly (teamA.goalsScored, etc)
            foreach (var match in Tournament.GamePlanPreliminaryRound)
            {
                GameLogicMatch.RunMatch(match);
            }
            
            Tournament.PreliminaryStandings = GenerateRanking();
            InputHandler.View.ShowMessage("Rankings:");
            foreach (var team in Tournament.PreliminaryStandings)
            {
                InputHandler.View.ShowMessage(team.TeamName);
            }
        }

        public void InitKoRound()
        {
            var Tournament = TournamentModel.Tournament;
            var GamePlanKoRound = TournamentModel.Tournament.GamePlanKoRound;
            var KoStandings = TournamentModel.Tournament.KoStandings;
            var PreliminaryStandings = TournamentModel.Tournament.PreliminaryStandings;
            var Settings = TournamentModel.Tournament.Settings;


            GamePlanKoRound.Clear(); // Clear previous matches if any
            InputHandler.View.ShowMessage("KO Round contestants:");
            // teams for ko round are selected (how many teams, in Tournament.TournamentSettings.NumberOfTeamsInKoRound) -> fill Tournament.TeamsInKoRound
            KoStandings = PreliminaryStandings.Take(Settings.NumberOfTeamsInKoRound).ToList();
            foreach (var team in KoStandings)
            {
                InputHandler.View.ShowMessage(team.TeamName);
            }


            if (!_inputValidator.HasValidTournamentSettings(Tournament))
            {
                throw new ArgumentException("Tournament settings or teams are not properly configured.");
            }
            
            
            int numberOfTeamsInKoRound = Settings.NumberOfTeamsInKoRound;
            KoStandings = PreliminaryStandings.OrderByDescending(t => t.NumberGoals).Take(numberOfTeamsInKoRound).ToList();
            // and organice them for the knockout round -> fill list of matches for KO round "tournament.GamePlanKoRound"
            // shuffle the list
            var shuffledTeams = KoStandings.OrderBy(x => random.Next()).ToList();
            // create matches in pairs
            for (int i = 0; i < shuffledTeams.Count; i += 2)
            {
                if (i + 1 < shuffledTeams.Count) // Ensure there is a pair
                {
                    var match = new Match(shuffledTeams[i], shuffledTeams[i + 1]);
                    GamePlanKoRound.Add(match);
                }
            }
        }

        public void RunKoRound()
        {
            var GamePlanKoRound = TournamentModel.Tournament.GamePlanKoRound;


            // take a list of matches tournament.GamePlanKoRound and execute it, asking the goals scored, updating the teams accordingly
            foreach (var match in GamePlanKoRound)
            {
                GameLogicMatch.RunMatch(match);
            }
            // at the end there is a winner
            if (GamePlanKoRound.Count > 0)
            {
                var winner = GamePlanKoRound[0].teamA; // Assuming the first match's teamA is the winner
                InputHandler.View.ShowMessage($"The winner of the knockout round is {winner.TeamName}!");
            }
            else
            {
                InputHandler.View.ShowMessage("No matches were played in the knockout round.");
            }
        }

        public List<Team> GenerateRanking()
        {
            var Settings = TournamentModel.Tournament.Settings;
            List<Team> Teams = Settings.TeamsInTournament;


            return Teams
                    .OrderByDescending(t => t.NumberGamesWon)
                    .ThenByDescending(t => t.Goaldifference)
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