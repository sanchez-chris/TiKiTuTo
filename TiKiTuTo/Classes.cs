using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace Classes
{
    public class Player
    {
        public string PlayerName { get; set; }
        public Team Team { get; set; }



        public Player(){}

        public Player(string name, Team team)

        {
            PlayerName = name;
            Team = team;
            Team?.AddPlayer(this);
        }
    }


    public class Team
    {
        public string TeamName { get; set; }
        public List<Player> TeamMembers { get; set; }
        public int GamesPlayed { get; private set; }
        public int GamesWon { get; private set; }
        public int GoalsTotal { get; private set; }
        public int GoalDifference { get; private set; }

   
        public Team() {}

        public Team(string name, List<Player> initialPlayers = null)

        {
            TeamName = name;
            TeamMembers = new List<Player>();
            GamesPlayed = 0;
            GamesWon = 0;
            GoalsTotal = 0;
            GoalDifference = 0;

            if (initialPlayers != null && initialPlayers.Count > 0)
            {
                foreach (var player in initialPlayers)
                {
                    AddPlayer(player);
                }
            }
            else
            {
                InitializeDefaultPlayers(); // Initialize default players if no initial players are provided
            }
        }

        private void InitializeDefaultPlayers()
        {
            // Add default players to the team
            for (int i = 1; i <= 2; i++) // Example: 2 default players per team
            {
                Player defaultPlayer = new Player($"DefaultPlayer{i} ({TeamName})", this);
                AddPlayer(defaultPlayer);
            }
        }


        public void UpdateTeamScore(int goalsMade, int goalsReceived)
        {
            GamesPlayed++;
            GoalsTotal += goalsMade;
            GoalDifference += (goalsMade - goalsReceived);
            if (goalsMade > goalsReceived)
            {
                GamesWon++;
            }
            else if (goalsMade < goalsReceived)
            {
                GamesWon--;
            }
        }
        public void AddPlayer(Player player)
        {
            if (!TeamMembers.Contains(player))
            {
                TeamMembers.Add(player);
            }
        }
    }


    public class Match
    {
        public Team Team1 { get; set; }                                 // Erstes Team
        public Team Team2 { get; set; }                                 // Zweites Team
        public int Team1Score { get; set; }                             // Tore von Team 1
        public int Team2Score { get; set; }                             // Tore von Team 2
        public bool Finished { get; private set; }                      // Status des Spiels (abgeschlossen oder nicht)


        //Parameterless Constructor for JSON Deserialization
        public Match() {}
        public Match(Team team1, Team team2)
        {
            Team1 = team1;
            Team2 = team2;
            Team1Score = 0;
            Team2Score = 0;
            Finished = false;
        }


        public void UpdateMatchResults(int scoreTeam1, int scoreTeam2)
        {
            Team1Score = scoreTeam1;
            Team2Score = scoreTeam2;
            Finished = true;
            Team1.UpdateTeamScore(scoreTeam1, scoreTeam2);
            Team2.UpdateTeamScore(scoreTeam2, scoreTeam1);
        }
    }


    public class Round
    {
        public enum RoundType
        {
            League,
            KO,
            ThirdPlace,
        }

        public RoundType TypeOf { get; set; }
        public List<Match> Matches { get; set; } = new List<Match>();
        public int ActiveRoundIndex { get; set; }

        public Round(){}

        public Round(RoundType typeOf, List<Match> matches)
        {
            TypeOf = typeOf;
            Matches = matches;
            ActiveRoundIndex = 0;
        }


        public List<Team> FinishRound()
        {
            List<Team> winners = new List<Team>();

            foreach (var match in Matches)
            {
                if (match.Team1Score > match.Team2Score)
                    winners.Add(match.Team1);
                else if (match.Team2Score > match.Team1Score)
                    winners.Add(match.Team2);
            }

            ActiveRoundIndex++;
            return winners;
        }
    }


    public class Tournament
    {
        public int TournamentID { get; set; }
        public string TournamentName { get; set; }
        public int TeamCount { get; set; }
        public int Prelimininaries { get; set; }
        public List<Team> Teams { get; set; }
        public List<Round> Rounds { get; set; }
        public int ActiveRoundIndex { get; set; }


        public Tournament(string name)
        {
            TournamentName = name;
        }

        public void AddTeam(Team team)
        {
            if (!Teams.Contains(team))
            {
                Teams.Add(team);
            }
        }

        public void StartRound(Round.RoundType typeOf)
        {
            var matches = GenerateMatches();
            var round = new Round(typeOf, matches);
            Rounds.Add(round);
            ActiveRoundIndex = Rounds.Count - 1;
        }

        private List<Match> GenerateMatches()
        {
            // Logic to generate matches for the round
            return new List<Match>();
        }

    }
}
