using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using TiKiTuTo.Model;
using TiKiTuTo.Controller;
using TiKiTuTo.View;

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
        public static Tournament CreateTournament(IView View)
        {
            TournamentSettings TournamentSettings = GameLogicTournamentSettings.CreateTournamentSettings(View);

            Tournament tournament = CreateTournament(TournamentSettings, View);

            return tournament;
        }




        /// <summary>
        /// Initializes Tournament based on a TournamentSettings object.
        /// </summary>
        /// <param name="tournamentSettings">a TournamentSettings instance holding all necessary parameters.</param>
        /// <returns>returns a newly initialized Tournament instance.</returns>
        public static Tournament CreateTournament(TournamentSettings tournamentSettings, IView View)
        {
            bool emptyNameAllowed = false;
            string name = InputHandler.GetName("Please enter the name of this tournament!", emptyNameAllowed, View);

            Tournament tournament = new Tournament(name, tournamentSettings);
            return tournament;
        }

    }
}