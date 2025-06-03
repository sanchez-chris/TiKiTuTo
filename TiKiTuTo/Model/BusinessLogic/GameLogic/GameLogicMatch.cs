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
    public class GameLogicMatch
    {
        GameLogicRound GameLogicRound { get; set; }
        InputHandler InputHandler { get; set; }
        JSONService JSONService { get; set; }

        public GameLogicMatch(InputHandler inputHandler, JSONService json)
        {
            GameLogicRound = new GameLogicRound(inputHandler, json);
            InputHandler = inputHandler;
            JSONService = json;
        }

        public void RunMatch(Match match, InputHandler inputHandler)
        {
            GameLogicRound.StartMatchTimer();
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
            teamA.Goaldifference = goalsA - goalsB;
            teamA.NumberGoals += goalsB;
            teamB.Goaldifference = goalsB - goalsA;
            teamB.NumberGoals += goalsB;
        }

        public void FinishMatch(Match match, Tournament tournament)
        {
            match.finished = true;
            UpdateTeamScores(match.teamA, match.goalsTeamA, match.teamB, match.goalsTeamB);
            JSONService.SaveGame(tournament);
        }
    }
}
