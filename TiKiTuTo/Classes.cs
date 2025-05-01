using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    public class Player
    {
        public string PlayerName { get; set; }
        public int GoalsScored { get; private set; }
        public int GamesWon { get; private set; }
        public int GamesLost { get; private set; }
        public Team Team { get; set; }

        public Player(string name)
        {
            PlayerName = name;
        }

        public void ScoreGoal()
        {
            GoalsScored++;
        }

        public void WinGame()
        {
            GamesWon++;
        }

        public void LoseGame()
        {
            GamesLost++;
        }
    }

    public class Team
    {
        public string TeamName { get; set; }
        public List<Player> Players { get; set; } = new List<Player>();
        public int GamesPlayed { get; private set; }
        public int GamesWon { get; private set; }
        public int GoalsTotal { get; private set; }
        public int GoalDifference { get; private set; }

        public Team(string name)
        {
            TeamName = name;
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
    }

    public class Match
    {
        public Team Team1 { get; set; }                                 // Erstes Team
        public Team Team2 { get; set; }                                 // Zweites Team
        public int Team1Score { get; set; }                             // Tore von Team 1
        public int Team2Score { get; set; }                             // Tore von Team 2
        public bool Finished { get; private set; }                      // Status des Spiels (abgeschlossen oder nicht)

        public Match(Team team1, Team team2)
        {
            Team1 = team1;
            Team2 = team2;
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
        public RoundType TypeOf {  get; set; }
        public List<Match> Matchs { get; set; } = new List<Match>();
        public int ActiveRoundIndex { get; set; }

        public Round(RoundType typeOf)
        {
            TypeOf = typeOf;
        }

        public List<Team> GetWinners()
        {
            return new List<Team>();                                //Placeholder
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

    }
}