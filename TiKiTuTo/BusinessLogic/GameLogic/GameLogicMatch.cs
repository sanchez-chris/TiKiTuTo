using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Controller;
using Model;

namespace TiKiTuTo.BusinessLogic.GameLogic
{
    public static class GameLogicMatch
    {
        public static void RunMatch(Match match, InputHandler inputHandler)
        {
            ModelApi.StartMatchTimer(inputHandler);
        }
        public static void UpdateTeamScores(Team team1, int goals1, Team team2, int goals2)
        {
            if (goals1 > goals2)
            {
                team1.NumberGamesWon++;
            }
            if (goals2 > goals1)
            {
                team2.NumberGamesWon++;
            }
            team1.Goaldifference = goals1 - goals2;
            team1.NumberGoals += goals1;
            team2.Goaldifference = goals2 - goals1;
            team2.NumberGoals += goals2;
        }
        public static void FinishMatch(Match match)
        {
            match.Finished = true;
        }
    }
}
