using System.ComponentModel.Design;
using TiKiTuTo.View;
using TiKiTuTo.Model;
using TiKiTuTo.Model.DataObjects;

namespace TiKiTuTo.Controller
{
     public class InputValidator
    {

        public bool IsValidGoalInput(int goals)
        {
            return goals >= 0 && goals <= 10;
        }


        public bool IsValidNumberOfTotalTeams(int numberTeamsTotal)
        {
            return numberTeamsTotal >= 4 && numberTeamsTotal <= 256;
        }


        public bool IsValidNumberOfTeamsInKORound(int numberInKO, int numberTeamsTotal)
        {
            return IsPowerOfTwo(numberInKO) && numberInKO <= numberTeamsTotal;
        }


        public bool IsPowerOfTwo(int x)
        {
            return x > 0 && (x & x - 1) == 0;
        }


        public bool IsValidNumberOfPreliminaryGamesPerTeam(int gamesPerTeam, int numberTeamsTotal)
        {
            return gamesPerTeam < numberTeamsTotal && numberTeamsTotal * gamesPerTeam / 2.0 % 1 == 0;
        }


        public bool IsValidMenuInput(int userInput, int maxMenuOption)
        {
            return userInput > 0 && userInput <= maxMenuOption;
        }

        public bool HasValidTournamentSettings(Tournament tournament)
        {

            if (tournament.TournamentSettings == null || tournament.TournamentSettings.TeamsInTournament == null || tournament.TournamentSettings.TeamsInTournament.Count < 2)
            {
                return false;
            }
            return true;
        }
    }
}
