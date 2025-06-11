using TiKiTuTo.Model.DataObjects;

namespace TiKiTuTo.Controller
{
    /// <summary>
    /// Validates input parameters for tournament configuration and gameplay.
    /// Provides validation methods for goals, team counts, knockout rounds, and overall tournament settings.
    /// </summary>
    public class InputValidator
    {

        /// <summary>
        /// Validates if the number of goals is within the acceptable range.
        /// </summary>
        /// <param name="goals">The number of goals to validate.</param>
        /// <returns>True if the goals value is between 0 and 10 (inclusive), otherwise false.</returns>
        public bool IsValidGoalInput(int goals)
        {
            return goals >= 0 && goals <= 10;
        }


        /// <summary>
        /// Validates if the total number of teams is within the acceptable range for a tournament.
        /// </summary>
        /// <param name="numberTeamsTotal">The total number of teams to validate.</param>
        /// <returns>True if the number of teams is between 4 and 256 (inclusive), otherwise false.</returns>
        public bool IsValidNumberOfTotalTeams(int numberTeamsTotal)
        {
            return numberTeamsTotal >= 4 && numberTeamsTotal <= 256;
        }


        /// <summary>
        /// Validates if the number of teams in the knockout round is valid.
        /// </summary>
        /// <param name="numberInKO">The number of teams in the knockout round.</param>
        /// <param name="numberTeamsTotal">The total number of teams in the tournament.</param>
        /// <returns>True if the number is a power of two, at least 2, and not greater than the total number of teams, otherwise false.</returns>
        public bool IsValidNumberOfTeamsInKORound(int numberInKO, int numberTeamsTotal)
        {
            return numberInKO >= 2 && IsPowerOfTwo(numberInKO) && numberInKO <= numberTeamsTotal;
        }


        /// <summary>
        /// Checks if a number is a power of two.
        /// </summary>
        /// <param name="x">The number to check.</param>
        /// <returns>True if the number is a positive power of two, otherwise false.</returns>
        public bool IsPowerOfTwo(int x)
        {
            return x > 0 && (x & x - 1) == 0;
        }


        /// <summary>
        /// Validates if the number of preliminary games per team is valid.
        /// </summary>
        /// <param name="gamesPerTeam">The number of games each team plays in the preliminary round.</param>
        /// <param name="numberTeamsTotal">The total number of teams in the tournament.</param>
        /// <returns>True if the number of games is less than the total teams and results in an integer number of total games, otherwise false.</returns>
        public bool IsValidNumberOfPreliminaryGamesPerTeam(int gamesPerTeam, int numberTeamsTotal)
        {
            return gamesPerTeam < numberTeamsTotal && numberTeamsTotal * gamesPerTeam / 2.0 % 1 == 0;
        }


        /// <summary>
        /// Validates if a tournament has valid settings configuration.
        /// </summary>
        /// <param name="tournament">The tournament to validate.</param>
        /// <returns>True if all tournament settings are valid, otherwise false.</returns>
        public bool HasValidTournamentSettings(Tournament tournament)
        {
            TournamentSettings settings = tournament.TournamentSettings;
            if (!IsValidNumberOfTotalTeams(settings.NumberOfTeamsTotal) ||
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
