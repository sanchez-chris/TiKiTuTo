using System.ComponentModel.Design;
using TiKiTuTo.View;
using TiKiTuTo.Model;

namespace TiKiTuTo.Controller
{
    static class InputValidator
    {

        //static bool ValidateTournamentSettings(TournamentSettings)
        //{
        //    bool result;

        //    return result;
        //}



        public static bool IsValidGoalInput(int Goals)
        {
            return (Goals >= 0 & Goals <= 10);
        }



        public static bool IsValidNumberOfTotalTeams(int NumberTeamsTotal)
        {
            bool result = NumberTeamsTotal >= 4;

            return result;

        }

        public static bool IsValidNumberOfTeamsInKORound(int NumberInKO, int NumberTeamsTotal)
        {
            bool result = false;

            return result;

        }

        public static bool IsValidNumberOfPreliminaryGamesPerTeam(int GamesPerTeam, int NumberTeamsTotal)
        {
            bool result = false;

            return result;

        }

        public static bool IsValidMenuInput(int UserInput)
        {

            return UserInput > 0 & UserInput < 7;
        }
    }
}
