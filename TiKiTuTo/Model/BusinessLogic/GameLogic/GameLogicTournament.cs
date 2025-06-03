using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using TiKiTuTo.View;
using TiKiTuTo.Controller;
using TiKiTuTo.Model.DataObjects;

namespace TiKiTuTo.Model.BusinessLogic.GameLogic
{
    /// <summary>
    /// Handles business logic regarding Tournament objects. 
    /// </summary>
    public class GameLogicTournament
    {
        GameLogicRound GameLogicRound { get; set; }
        GameLogicTournamentSettings GameLogicTournamentSettings { get; set; }
        InputHandler InputHandler { get; set; }
        /// <summary>
        /// Creates Tournament based on user input. First creates a TournamentSettings object, then initializes a Tournament based on these settings.
        /// </summary>
        /// <returns>returns a newly initialized Tournament instance.</returns>
        public GameLogicTournament(GameLogicRound glRound, GameLogicTournamentSettings glTournamentSettings, InputHandler inputHandler) 
        {
            GameLogicRound = glRound;
            GameLogicTournamentSettings = glTournamentSettings;
            InputHandler = inputHandler;

        }


        public Tournament InitTournament()
        {
            Tournament tournament = SetupTournament();
            GameLogicRound.InitPreliminaryRound(tournament);
            //JSONService.SaveGame(tournament);
            return tournament;
        }

        public Tournament SetupTournament()
        {
            TournamentSettings tournamentSettings = GameLogicTournamentSettings.CreateTournamentSettings();
            Tournament tournament = CreateTournament(tournamentSettings);

            return tournament;
        }

        public Tournament CreateTournament(TournamentSettings tournamentSettings)
        {
             bool emptyNameAllowed = false;
            string name = InputHandler.GetTournamentName("Please enter the name of this tournament!", emptyNameAllowed);
            Tournament tournament = new Tournament(name, tournamentSettings);

            return tournament;
        }



        public void StartTournament(Tournament tournament)
        {
            GameLogicRound.RunPreliminaryRound(tournament);

        }


    }
}