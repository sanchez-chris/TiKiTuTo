using System.ComponentModel.Design;
using View;
using Model;

namespace Controller
{
    static class InputValidator
    {

        public static bool IsValidGoalInput(int goals)
        {
            return (goals >= 0 && goals <= 10);
        }


        public static bool IsValidNumberOfTotalTeams(int numberTeamsTotal)
        {
           return numberTeamsTotal >= 4 && numberTeamsTotal <= 256;
        }


        public static bool IsValidNumberOfTeamsInKORound(int numberInKO, int numberTeamsTotal)
        {
            return IsPowerOfTwo(numberInKO) && numberInKO <= numberTeamsTotal;
        }


        public static bool IsPowerOfTwo(int x)
        {
            return (x > 0) && ((x & (x - 1)) == 0);
        }


        public static bool IsValidNumberOfPreliminaryGamesPerTeam(int gamesPerTeam, int numberTeamsTotal)
        {
            return gamesPerTeam < numberTeamsTotal && numberTeamsTotal * gamesPerTeam / 2.0 % 1 == 0;
        }


        public static bool IsValidMenuInput(int userInput, int maxMenuOption)
        {
            return userInput > 0 && userInput <= maxMenuOption;
        }
    }
}
