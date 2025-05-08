using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiKiTuTo.Classes
{

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
}
