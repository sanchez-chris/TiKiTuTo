using TiKiTuTo.Controller;
using TiKiTuTo.Model.DataObjects;
using Match = TiKiTuTo.Model.DataObjects.Match;


namespace TiKiTuTo.Model.BusinessLogic.GameLogic
{
    /// <summary>
    /// Handles business logic regarding Rounds objects. 
    /// </summary>

    public class GameLogicRound
    {
        // Declare properties
        private readonly InputValidator _inputValidator;

        private InputHandler InputHandler { get; }
        private JSONService JSONService { get; }
        private TournamentModel TournamentModel { get; }

        // Declare GameLogicMatch without initializing it here
        private readonly GameLogicMatch _gameLogicMatch;

        // Constructor
        public GameLogicRound(InputHandler inputHandler, JSONService json, InputValidator inputValidator, TournamentModel tournamentModel)
        {
            // Set properties
            _inputValidator = inputValidator;
            InputHandler = inputHandler;
            JSONService = json;
            TournamentModel = tournamentModel;
            // Initialize GameLogicMatch after properties are set
            _gameLogicMatch = new GameLogicMatch(InputHandler, JSONService, tournamentModel);
        }

        public void InitPreliminaryRound()
        {
            CreateGamePlanPreRound();
            InputHandler.View.WaitForAnyKeyToProceed();
            InputHandler.View.ShowMessage("Good luck to all teams!");
        }
        
        
        public void RunPreliminaryRound()
        {
            var tournament = TournamentModel.Tournament;
            InputHandler.View.ShowMessage($"Preliminary round initialized with {tournament.GamePlanPreliminaryRound.Count} matches.");
            InputHandler.View.ShowMessage("\nGameplan preliminary round:\n");

            tournament.GamePlanPreliminaryRound.ForEach(match =>
                InputHandler.View.ShowMessage($"{match.TeamA.TeamName} vs {match.TeamB.TeamName}"));

            //To print an empty line between the last played game and the first unplayed match
            if (tournament.GamePlanPreliminaryRound.Any(match => match.IsFinished))
            {
                InputHandler.View.WriteEmptyLine();
            }

            // take a list of matches tournament.GamePlanPreliminaryRound and execute it, asking the goals scored, updating the teams attributes accordingly (teamA.goalsScored, etc)
            foreach (var match in tournament.GamePlanPreliminaryRound)
            {
                if (match.IsFinished)
                {
                    InputHandler.View.ShowMessage($"{match.TeamA.TeamName} vs {match.TeamB.TeamName} is finished");
                }
                else
                {
                    _gameLogicMatch.RunMatch(match);
/*                   if (matchIndex % (tournament.TournamentSettings.NumberOfTeamsTotal / 2) == 0)
                    {
                        InputHandler.View.ShowStandings(tournament);
                    }
*/                }
            }
            tournament.PreliminaryStandings = GenerateRanking();

            //InputHandler.View.ShowStandings(tournament);

        }

        public void InitKoRound()
        {
            CreateGamePlanKoRound();
        }

        public void RunKoRound()
        {
            var tournament = TournamentModel.Tournament;

            if (tournament.IsFinished) return;

            // Calculate how many rounds there are in the tournament
            int totalRounds = (int)Math.Ceiling(Math.Log2(tournament.TournamentSettings.NumberOfTeamsInKoRound));

            while (tournament.CurrentKoRound < totalRounds)
            {
                //ShowKoGamePlanKoRound(tournament, tournament.CurrentKoRound);
                InputHandler.View.ShowKoTree(tournament);

                //To print an empty line between the last played game and the first unplayed match
                if (tournament.GamePlanKoRound[tournament.CurrentKoRound].Any(match => match.IsFinished))
                {
                    InputHandler.View.WriteEmptyLine();
                }

                foreach (var match in tournament.GamePlanKoRound[tournament.CurrentKoRound])
                {
                    UpdateStandings(match);
                }

                tournament.CurrentKoRound++;
                if (tournament.CurrentKoRound < totalRounds)
                {
                    OrganizeMatchesForNextRound();
                }
                if (totalRounds >= 2 && !tournament.IsSemifinalPlayed && tournament.KoStandings.Count == 2) // there is a semifinal
                {
                    tournament.IsSemifinalPlayed = true;
                    InputHandler.View.ShowMessage("\nLets decide the 3. Position!");
                    Match semifinal = new Match(tournament.Semifinalists[0], tournament.Semifinalists[1]);

                    _gameLogicMatch.RunMatch(semifinal);
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
                InputHandler.View.ShowMessage($"\n\nThe winner of the KO round is [gold3]{tournament.Winner.TeamName}![/]");
 //               InputHandler.View.ShowMessage($"\n\n1. {tournament.Winner.TeamName}");
                InputHandler.View.ShowMessage($"\n\nOur runner up on the second place is [lightskyblue1]{tournament.Finalist.TeamName}[/]");
                if (tournament.ThirdPosition != null)
                {
                    InputHandler.View.ShowMessage($"\n\n3. {tournament.ThirdPosition.TeamName}");
                }
                tournament.IsFinished = true;
                JSONService.SaveFinishedTournament();
                JSONService.SaveTournament();
            }
        }

        private void CreateGamePlanPreRound()
        {
            Tournament tournament = TournamentModel.Tournament;
            var settings = tournament.TournamentSettings;
            int gamesPerTeam = settings.NumberOfPreliminaryGamesPerTeam;
            List<Team> teams = settings.TeamsInTournament;

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
            List<Team> shuffledTeams = teams.OrderBy(_ => random.Next()).ToList();

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

        public void CreateGamePlanKoRound()
        {
            Tournament tournament = TournamentModel.Tournament;


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
            tournament.KoStandings = tournament.KoStandings.OrderBy(_ => random.Next()).ToList();

            // Initialize the GamePlanKoRound with empty rounds
            int totalRounds = (int)Math.Ceiling(Math.Log2(tournament.TournamentSettings.NumberOfTeamsInKoRound));
            for (int i = 0; i < totalRounds; i++)
            {
                tournament.GamePlanKoRound.Add(new List<Match>());
            }
            OrganizeMatchesForNextRound();
        }

        public void ShowKoGamePlanKoRound()
        {
            Tournament tournament = TournamentModel.Tournament;
            int currentRound = tournament.CurrentKoRound;

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
                InputHandler.View.ShowMessage($"{match.TeamA.TeamName.PadRight(spacing)} vs {match.TeamB.TeamName.PadRight(spacing)}");
            }

        }

        public void OrganizeMatchesForNextRound()
        {
            Tournament tournament = TournamentModel.Tournament;
            int currentRound = tournament.CurrentKoRound;

            for (int i = 0; i < tournament.KoStandings.Count; i += 2)
            {
                if (i + 1 < tournament.KoStandings.Count)
                {
                    var match = new Match(tournament.KoStandings[i], tournament.KoStandings[i + 1]);
                    // ensure there is place for a new round
                    while (tournament.GamePlanKoRound.Count <= currentRound)
                    {
                        tournament.GamePlanKoRound.Add(new List<Match>());
                    }

                    tournament.GamePlanKoRound[currentRound].Add(match);
                } // Ensure there is a pair
            }
        }

        public List<Team> GenerateRanking()
        {
            Tournament tournament = TournamentModel.Tournament;
            List<Team> teams = tournament.TournamentSettings.TeamsInTournament;

            return teams
                    .OrderByDescending(t => t.NumberGamesWon)
                    .ThenByDescending(t => t.GoalDifference)
                    .ThenByDescending(t => t.NumberGoals)
                    .ToList();
        }
        public void UpdateStandings(Match match)
        {
            var tournament = TournamentModel.Tournament;

            while (!tournament.IsFinished)
            {
                if(!match.IsFinished)
                {
                    _gameLogicMatch.RunMatch(match);
                    if (match.GoalsTeamA > match.GoalsTeamB)
                    {

                        if (tournament.KoStandings.Count == 4 || tournament.KoStandings.Count == 3) // semifinal 
                        {
                            tournament.Semifinalists.Add(match.TeamB);
                        }
                        if (tournament.KoStandings.Count == 2) // final
                        {
                            tournament.Finalist = match.TeamB;
                        }

                        tournament.KoStandings.Remove(match.TeamB);
                    }
                    else
                    {
                        if (tournament.KoStandings.Count == 4 || tournament.KoStandings.Count == 3) // semifinal
                        {
                            tournament.Semifinalists.Add(match.TeamA);
                        }
                        if (tournament.KoStandings.Count == 2) // final
                        {
                            tournament.Finalist = match.TeamA;
                        }
                        tournament.KoStandings.Remove(match.TeamA);
                    }
                }
                else
                {
                    InputHandler.View.ShowMessage($"\n\n{match.TeamA.TeamName} vs {match.TeamB.TeamName} is finished.");
                    break;
                }
            }
        }
    }
}