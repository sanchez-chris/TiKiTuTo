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
            
            List<Team> rankings = GenerateRanking(tournament.GamePlanPremilimaryRound);
            InputHandler.View.ShowMessage("Rankings:");
            foreach (var team in rankings)
            {
                InputHandler.View.ShowMessage(team.TeamName);
            }
        }

        public void InitKoRound(Tournament tournament)
        {
            tournament.GamePlanKoRound.Clear(); // Clear previous matches if any

            // teams for ko round are selected (how many teams, in Tournament.TournamentSettings.NumberOfTeamsInKoRound) -> fill Tournament.TeamsInKoRound
            if (tournament.Settings == null || tournament.Settings.TeamsInTournament == null || tournament.Settings.TeamsInTournament.Count < 2)
            {
                throw new ArgumentException("Tournament settings or teams are not properly configured.");
            }
            
            
            int numberOfTeamsInKoRound = tournament.Settings.NumberOfTeamsInKoRound;
            tournament.TeamsInKoRound = tournament.PreliminaryStandings.OrderByDescending(t => t.NumberGoals).Take(numberOfTeamsInKoRound).ToList();
            // and organice them for the knockout round -> fill list of matches for KO round "tournament.GamePlanKoRound"
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

        public List<Team> GenerateRanking(List<Match> GamePlanPreliminaryRound)
        {
                List<Team> Teams = new List<Team>();

                foreach (var match in GamePlanPreliminaryRound)
                {
                    if (!Teams.Contains(match.teamA))
                    {
                        Teams.Add(match.teamA);
                    }

                    if (!Teams.Contains(match.teamB))
                    {
                        Teams.Add(match.teamB);
                    }

                    if (match.goalsTeamA > match.goalsTeamB)
                    {
                        match.teamA.NumberGamesWon++;
                    }
                    else if (match.goalsTeamA < match.goalsTeamB)
                    {
                        match.teamB.NumberGamesWon++;
                    }

                match.teamA.Goaldifference += (match.goalsTeamA - match.goalsTeamB);
                match.teamB.Goaldifference += (match.goalsTeamB - match.goalsTeamA);
                }

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