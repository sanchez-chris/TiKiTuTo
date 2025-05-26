using System.ComponentModel.Design;
using View;
using Model;

namespace Controller
{
    static class InputValidator
    {

        //static bool ValidateTournamentSettings(TournamentSettings)
        //{
        //    bool result;

        //    return result;
        //}



        //TODO
        public static bool IsValidGoalInput(int Goals)
        {
            return (Goals >= 0 & Goals <= 10);
        }

        public static bool IsPowerOfTwo(int x)
        {
            return (x > 0) && ((x & (x - 1)) == 0);
        }

        //TODO
        public static bool IsValidNumberOfTotalTeams(int NumberTeamsTotal)
        {
            bool result = NumberTeamsTotal >= 4;

            return result;

        }

        //TODO
        public static bool IsValidNumberOfTeamsInKORound(int NumberInKO, int NumberTeamsTotal)
        {
            bool result = false;

            return result;

        }

        //TODO
        public static bool IsValidNumberOfPreliminaryGamesPerTeam(int GamesPerTeam, int NumberTeamsTotal)
        {
            bool result = false;

            return result;

        }


        //TODO
        public static bool IsValidMenuInput(int UserInput)
        {

            return UserInput > 0 & UserInput < 7;
        }
    }
}
