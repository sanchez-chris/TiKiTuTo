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
            GameLogicMatch = new GameLogicMatch(InputHandler, JSONService, tournamentModel);
        }

        public void InitPreliminaryRound()
        {
            var tournament = TournamentModel.Tournament;

            CreateGamePlanPreRound(tournament);
            InputHandler.View.WaitForAnyKeyToProceed();
            InputHandler.View.ShowMessage("Good luck to all teams!");
        }
        
        
        public void RunPreliminaryRound()
        {
            var tournament = TournamentModel.Tournament;
            InputHandler.View.ShowMessage($"Preliminary round initialized with {tournament.GamePlanPreliminaryRound.Count} matches.");
            InputHandler.View.ShowMessage("\nGameplan preliminary round:\n");

            tournament.GamePlanPreliminaryRound.ForEach(match => InputHandler.View.ShowMessage($"{match.teamA.TeamName} vs {match.teamB.TeamName}"));

            int matchIndex = 1;

            // take a list of matches tournament.GamePlanPreliminaryRound and execute it, asking the goals scored, updating the teams attributes accordingly (teamA.goalsScored, etc)
            foreach (var match in tournament.GamePlanPreliminaryRound)
            {
                if (match.isFinished)
                {
                    InputHandler.View.ShowMessage($"{match.teamA.TeamName} vs {match.teamB.TeamName} is finished");
                }
                else
                {
                    GameLogicMatch.RunMatch(match);
/*                   if (matchIndex % (tournament.TournamentSettings.NumberOfTeamsTotal / 2) == 0)
                    {
                        InputHandler.View.ShowStandings(tournament);
                    }
*/                }
                matchIndex++;
            }
            tournament.PreliminaryStandings = GenerateRanking(tournament);
            
            //InputHandler.View.ShowStandings(tournament);

        }

        public void InitKoRound()
        {
            var tournament = TournamentModel.Tournament;
            CreateGamePlanKoRound(tournament);
        }

        public void RunKoRound()
        {
            var tournament = TournamentModel.Tournament;

            if (tournament.IsFinished) return;

            // Calculate how many rounds there are in the tournament
            int totalRounds = (int)Math.Ceiling(Math.Log2(tournament.TournamentSettings.NumberOfTeamsInKoRound));

            while (tournament.CurrentKoRound < totalRounds)
            {
                ShowKoGamePlanKoRound(tournament, tournament.CurrentKoRound);


                foreach (var match in tournament.GamePlanKoRound[tournament.CurrentKoRound])
                {
                    UpdateStandings(match);
                }

                tournament.CurrentKoRound++;
                if (tournament.CurrentKoRound < totalRounds)
                {
                    organizeMatchesForNextRound(tournament, tournament.CurrentKoRound);
                }
                if (totalRounds >= 2 && !tournament.IsSemifinalPlayed && tournament.KoStandings.Count == 2) // there is a semifinal
                {
                    tournament.IsSemifinalPlayed = true;
                    InputHandler.View.ShowMessage("\nLets decide the 3. Position!");
                    Match semifinal = new Match(tournament.Semifinalists[0], tournament.Semifinalists[1]);

                    GameLogicMatch.RunMatch(semifinal);
                    if (tournament.Semifinalists[0].NumberGoals > tournament.Semifinalists[1].NumberGoals)
                    {
                        tournament.ThirdPosition = tournament.Semifinalists[0];
                    }
                    else
                    {
                        tournament.ThirdPosition = tournament.Semifinalists[1];

                    }
                    InputHandler.View.ShowMessage($"\n\nThe 3. Position of the KO round is {tournament.ThirdPosition.TeamName}.");
                }
            }



            if (tournament.KoStandings.Count == 1)
            {
                tournament.Winner = tournament.KoStandings[0];
                InputHandler.View.ShowMessage($"\n\nThe winner of the KO round is {tournament.Winner.TeamName}!");
                InputHandler.View.ShowMessage($"\n\n1. {tournament.Winner.TeamName}");
                InputHandler.View.ShowMessage($"\n\n2. {tournament.Finalist.TeamName}");
                if (tournament.ThirdPosition != null)
                {
                    InputHandler.View.ShowMessage($"\n\n3. {tournament.ThirdPosition.TeamName}");
                }
                tournament.IsFinished = true;
                JSONService.SaveFinishedTournament();
                JSONService.SaveTournament();
            }
        }

        public void CreateGamePlanPreRound(Tournament tournament)
        {
            var Settings = tournament.TournamentSettings;
            int gamesPerTeam = Settings.NumberOfPreliminaryGamesPerTeam;
            List<Team> teams = Settings.TeamsInTournament;

            // Create a random object for shuffling
            Random random = new Random();

            // 1. Verify if generating the required matches is possible
            int totalGamesNeeded = teams.Count * gamesPerTeam / 2;
            int totalPossibleMatches = teams.Count * (teams.Count - 1) / 2;
            if (totalGamesNeeded > totalPossibleMatches)
            {
                throw new InvalidOperationException("Not enough teams to generate the required number of matches.");
            }

            // 2. Shuffle teams randomly - this is where all randomness is introduced
            List<Team> shuffledTeams = teams.OrderBy(t => random.Next()).ToList();

            // 3. Initialize tracking variable
            var teamMatchCount = new Dictionary<Team, int>();

            foreach (var team in shuffledTeams)
            {
                teamMatchCount[team] = 0;
            }

            // 4. Apply modified round-robin algorithm
            // For odd number of teams, add a dummy team
            List<Team> schedulingTeams = new List<Team>(shuffledTeams);
            if (schedulingTeams.Count % 2 != 0)
            {
                schedulingTeams.Add(null); // Add dummy team for scheduling
            }

            int numberOfTeamsWDummy = schedulingTeams.Count;
            int maxRounds = (int)Math.Ceiling((double)gamesPerTeam * numberOfTeamsWDummy / (numberOfTeamsWDummy - 1));

            // Create matches using round-robin scheduling
            for (int round = 0; round < maxRounds && teamMatchCount.Values.Any(count => count < gamesPerTeam); round++)
            {
                // In each round, create numberOfTeamsWDummy/2 matches
                for (int i = 0; i < numberOfTeamsWDummy / 2; i++)
                {
                    // Match schedulingTeams[i] with team at mirrored position (first w/ last, second w/ second-to-last...)
                    Team teamA = schedulingTeams[i];
                    Team teamB = schedulingTeams[numberOfTeamsWDummy - 1 - i];

                    // Skip if either team is the dummy
                    if (teamA == null || teamB == null)
                        continue;

                    //Skip if a team has already played often enough
                    if (teamMatchCount[teamA] >= gamesPerTeam || teamMatchCount[teamB] >= gamesPerTeam)
                        continue;

                    // Create the match
                    Match match = new Match(teamA, teamB);
                    tournament.GamePlanPreliminaryRound.Add(match);

                    // Update tracking variables
                    teamMatchCount[teamA]++;
                    teamMatchCount[teamB]++;
                }

                // Rotate teams for the next round (keeping first team fixed)
                // This is a standard rotation technique for round-robin scheduling
                Team temp = schedulingTeams[1];
                for (int i = 1; i < numberOfTeamsWDummy - 1; i++)
                {
                    schedulingTeams[i] = schedulingTeams[i + 1];
                }
                schedulingTeams[numberOfTeamsWDummy - 1] = temp;
            }
        }

        public void CreateGamePlanKoRound(Tournament tournament)
        {
            int currentRound = tournament.CurrentKoRound;


            if (!_inputValidator.HasValidTournamentSettings(tournament))
            {
                throw new ArgumentException("Tournament settings or teams are not properly configured.");
            }

            //tournament.GamePlanKoRound.Clear(); // Why do we need to clear GamePlanKORound here?
            tournament.KoStandings = tournament.PreliminaryStandings.Take(tournament.TournamentSettings.NumberOfTeamsInKoRound).ToList();

            // Select teams for KO round
            InputHandler.View.ShowMessage("\n\nThe contestants for the KO round are:\n");
            foreach (var team in tournament.KoStandings)
            {
                InputHandler.View.ShowMessage($"{team.TeamName}");
            }

            // Randomly reorder the list KoStandings
            Random random = new Random();
            tournament.KoStandings = tournament.KoStandings.OrderBy(x => random.Next()).ToList();

            // Initialize the GamePlanKoRound with empty rounds
            int totalRounds = (int)Math.Ceiling(Math.Log2(tournament.TournamentSettings.NumberOfTeamsInKoRound));
            for (int i = 0; i < totalRounds; i++)
            {
                tournament.GamePlanKoRound.Add(new List<Match>());
            }
            organizeMatchesForNextRound(tournament, currentRound);
        }

        public void ShowKoGamePlanKoRound(Tournament tournament, int currentRound)
        {
            // Calculate how many rounds there are in the tournament
            int totalRounds = (int)Math.Ceiling(Math.Log2(tournament.TournamentSettings.NumberOfTeamsInKoRound)) - 1;

            // Validate that the current round is valid
            if (currentRound < 0 || currentRound > totalRounds)
            {
                InputHandler.View.ShowMessage($"Invalid round number: {currentRound}. There are {totalRounds + 1} rounds in the tournament.");
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
            else if (currentRound == totalRounds - 1 && totalRounds > 1)
            {
                InputHandler.View.ShowMessage("Semifinal:");
                InputHandler.View.ShowMessage("Matches:");
            }
            else
            {
                InputHandler.View.ShowMessage($"Gameplan {currentRound + 1}. KO round:");
            }

            InputHandler.View.ShowMessage(new string('-', 20));

            int spacing = (int)Math.Pow(2, totalRounds - currentRound - 1) * 2; // Dynamic space for the current round

            foreach (var match in tournament.GamePlanKoRound[currentRound])
            {
                InputHandler.View.ShowMessage($"{match.teamA.TeamName.PadRight(spacing)} vs {match.teamB.TeamName.PadRight(spacing)}");
            }

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

        public List<Team> GenerateRanking(Tournament tournament)
        {
            List<Team> Teams = tournament.TournamentSettings.TeamsInTournament;

            return Teams
                    .OrderByDescending(t => t.NumberGamesWon)
                    .ThenByDescending(t => t.Goaldifference)
                    .ThenByDescending(t => t.NumberGoals)
                    .ToList();
        }
        public void UpdateStandings(Match match)
        {
            var tournament = TournamentModel.Tournament;

            while (!tournament.IsFinished)
            {
                if(!match.isFinished)
                {
                    GameLogicMatch.RunMatch(match);
                    if (match.goalsTeamA > match.goalsTeamB)
                    {

                        if (tournament.KoStandings.Count == 4 || tournament.KoStandings.Count == 3) // semifinal 
                        {
                            tournament.Semifinalists.Add(match.teamB);
                        }
                        if (tournament.KoStandings.Count == 2) // final
                        {
                            tournament.Finalist = match.teamB;
                        }

                        tournament.KoStandings.Remove(match.teamB);
                    }
                    else
                    {
                        if (tournament.KoStandings.Count == 4 || tournament.KoStandings.Count == 3) // semifinal
                        {
                            tournament.Semifinalists.Add(match.teamA);
                        }
                        if (tournament.KoStandings.Count == 2) // final
                        {
                            tournament.Finalist = match.teamA;
                        }
                        tournament.KoStandings.Remove(match.teamA);
                    }
                }
                else
                {
                    InputHandler.View.ShowMessage($"\n\n{match.teamA.TeamName} vs {match.teamB.TeamName} is finished.");
                    break;
                }
            }
        }
    }
}