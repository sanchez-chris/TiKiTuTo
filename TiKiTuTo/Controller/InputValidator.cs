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
            return numberInKO >= 2 && IsPowerOfTwo(numberInKO) && numberInKO <= numberTeamsTotal;
        }


        public bool IsPowerOfTwo(int x)
        {
            return x > 0 && (x & x - 1) == 0;
        }


        public bool IsValidNumberOfPreliminaryGamesPerTeam(int gamesPerTeam, int numberTeamsTotal)
        {
            return gamesPerTeam < numberTeamsTotal && numberTeamsTotal * gamesPerTeam / 2.0 % 1 == 0;
        }


        public bool HasValidTournamentSettings(Tournament tournament)
        {
            TournamentSettings settings = tournament.TournamentSettings;
            if (settings == null ||
                !IsValidNumberOfTotalTeams(settings.NumberOfTeamsTotal) ||
                !IsValidNumberOfTeamsInKORound(settings.NumberOfTeamsInKoRound, settings.NumberOfTeamsTotal) ||
                !IsValidNumberOfPreliminaryGamesPerTeam(settings.NumberOfPreliminaryGamesPerTeam, settings.NumberOfTeamsTotal))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
