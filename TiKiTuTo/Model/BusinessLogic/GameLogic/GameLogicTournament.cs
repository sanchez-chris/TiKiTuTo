using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using TiKiTuTo.View;
using TiKiTuTo.Controller;
using TiKiTuTo.Model.DataObjects;
using TiKiTuTo.Model;

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
        public TournamentModel TournamentModel { get; set; }

        /// <summary>
        /// Creates Tournament based on user input. First creates a TournamentSettings object, then initializes a Tournament based on these settings.
        /// </summary>
        /// <returns>returns a newly initialized Tournament instance.</returns>
        public GameLogicTournament(GameLogicRound glRound, GameLogicTournamentSettings glTournamentSettings, InputHandler inputHandler, TournamentModel model) 
        {
            GameLogicRound = glRound;
            GameLogicTournamentSettings = glTournamentSettings;
            InputHandler = inputHandler;
            TournamentModel = model;
        }


        public void InitTournament()
        {
            SetupTournament();
            GameLogicRound.InitPreliminaryRound();
            
            //JSONService.SaveGame(tournament);
        }

        public void SetupTournament()
        {
            TournamentSettings tournamentSettings = GameLogicTournamentSettings.CreateTournamentSettings();
            CreateTournament(tournamentSettings);

        }

        public void CreateTournament(TournamentSettings tournamentSettings)
        {
            bool emptyNameAllowed = false;
            string name = InputHandler.GetTournamentName("Please enter the name of this tournament!", emptyNameAllowed);
            TournamentModel.Tournament = new Tournament(name, tournamentSettings);

        }


        public void StartTournament()
        {
            GameLogicRound.RunPreliminaryRound();
            GameLogicRound.InitKoRound();
            GameLogicRound.RunKoRound();
        }


    }
}