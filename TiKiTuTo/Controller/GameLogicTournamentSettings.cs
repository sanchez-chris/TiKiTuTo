using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiKiTuTo.Model;
using TiKiTuTo.View;
namespace TiKiTuTo.Controller
{
    /// <summary>
    /// Handles business logic regarding TournamentSettings objects. 
    /// </summary>
    public class GameLogicTournamentSettings
    {
        /// <summary>
        /// Creates TournamentSettings based on user input.
        /// Validates input so that a functional Tournament can be initialized based on these settings.
        /// </summary>
        /// <returns>TournamentSettings instance holding all relevant, validated parameters to initialize a new Tournament.</returns>
        public static TournamentSettings CreateTournamentSettings(IView View)
        {

            int NumberOfTeamsTotal;                 //how many teams attend the tournament?
            int NumberOfPreliminaryGamesPerTeam;    //how many games will be played per team in the preliminaries?
            int NumberOfTeamsInKORound;             //how many teams will progress into knockout rounds?
            List<Team> Teams;                       //the actual teams

            //ask for the necessary inputs
            NumberOfTeamsTotal = InputHandler.GetValidNumberOfTotalTeams(View);
            NumberOfPreliminaryGamesPerTeam = InputHandler.GetValidPreliminaryGames(NumberOfTeamsTotal, View);
            NumberOfTeamsInKORound = InputHandler.GetValidNumberOfTeamsInKORound(NumberOfTeamsTotal, View);
            Teams = BasicFunctions.CreateNTeams(NumberOfTeamsTotal, View);

            TournamentSettings settings = new TournamentSettings(NumberOfTeamsTotal, NumberOfPreliminaryGamesPerTeam, NumberOfTeamsInKORound, Teams);

            return settings;
        }

    }
}