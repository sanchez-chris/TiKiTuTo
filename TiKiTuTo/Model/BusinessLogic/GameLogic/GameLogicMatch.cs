using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiKiTuTo.Controller;
using TiKiTuTo.Model.BusinessLogic;
using TiKiTuTo.Model.DataObjects;

namespace TiKiTuTo.Model.BusinessLogic.GameLogic
{
    public static class GameLogicMatch
    {
        public static void RunMatch(Match match, InputHandler inputHandler)
        {
            ModelApi.StartMatchTimer(inputHandler);
        }
        public static void UpdateTeamScores(Team teamA, int goalsA, Team teamB, int goalsB)
        {
            if (goalsA > goalsB)
            {
                teamA.NumberGamesWon++;
            }
            if (goalsB > goalsA)
            {
                teamB.NumberGamesWon++;
            }
            teamA.Goaldifference = goalsA - goalsB;
            teamA.NumberGoals += goalsB;
            teamB.Goaldifference = goalsB - goalsA;
            teamB.NumberGoals += goalsB;
        }
        public static void FinishMatch(Match match)
        {
            match.finished = true;
            UpdateTeamScores(match.teamA, match.goalsTeamA, match.teamB, match.goalsTeamB);
        }
    }
}
