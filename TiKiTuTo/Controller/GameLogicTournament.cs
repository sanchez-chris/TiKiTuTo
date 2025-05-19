using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using TiKiTuTo.Model;

namespace TiKiTuTo.Controller
{
    /// <summary>
    /// Handles business logic regarding Tournament objects. 
    /// </summary>
    public class GameLogicTournament
    {
        /// <summary>
        /// Creates Tournament based on user input. First creates a TournamentSettings object, then initializes a Tournament based on these settings.
        /// </summary>
        /// <returns>returns a newly initialized Tournament instance.</returns>
        public static Tournament CreateTournament()
        {


            
            TournamentSettings TournamentSettings = GameLogicTournamentSettings.CreateTournamentSettings();


            Tournament tournament = CreateTournament(TournamentSettings);

            return tournament;
        }




        /// <summary>
        /// Initializes Tournament based on a TournamentSettings object.
        /// </summary>
        /// <param name="tournamentSettings">a TournamentSettings instance holding all necessary parameters.</param>
        /// <returns>returns a newly initialized Tournament instance.</returns>
        public static Tournament CreateTournament(TournamentSettings tournamentSettings)
        {
            bool emptyNameAllowed = false;
            string name = BasicFunctions.GetName("Please enter the name of this tournament!", emptyNameAllowed);

            Tournament tournament = new Tournament(name, tournamentSettings);
            return tournament;
        }

    }
}