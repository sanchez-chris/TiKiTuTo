using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using View;
using TiKiTuTo.Controller;
using TiKiTuTo.Model.DataObjects;

namespace TiKiTuTo.Model.BusinessLogic.GameLogic
{
    /// <summary>
    /// Handles business logic regarding Tournament objects. 
    /// </summary>
    public static class GameLogicTournament
    {
        /// <summary>
        /// Creates Tournament based on user input. First creates a TournamentSettings object, then initializes a Tournament based on these settings.
        /// </summary>
        /// <returns>returns a newly initialized Tournament instance.</returns>
        public static Tournament SetupTournament(InputHandler inputHandler)
        {
            TournamentSettings tournamentSettings = GameLogicTournamentSettings.CreateTournamentSettings(inputHandler);
            Tournament tournament = CreateTournament(inputHandler, tournamentSettings);

            return tournament;
        }

        public static Tournament CreateTournament(InputHandler inputHandler, TournamentSettings tournamentSettings)
        {
             bool emptyNameAllowed = false;
            string name = inputHandler.GetTournamentName("Please enter the name of this tournament!", emptyNameAllowed);
            Tournament tournament = new Tournament(name, tournamentSettings);

            return tournament;
        }
       

    }
}