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
using Spectre.Console;

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
        JSONService JSONService { get; set; }
        public TournamentModel TournamentModel { get; set; }

        /// <summary>
        /// Creates Tournament based on user input. First creates a TournamentSettings object, then initializes a Tournament based on these settings.
        /// </summary>
        /// <returns>returns a newly initialized Tournament instance.</returns>
        public GameLogicTournament(GameLogicRound glRound, GameLogicTournamentSettings glTournamentSettings, InputHandler inputHandler, TournamentModel model, JSONService jsonService) 
        {
            GameLogicRound = glRound;
            GameLogicTournamentSettings = glTournamentSettings;
            InputHandler = inputHandler;
            TournamentModel = model;
            JSONService = jsonService;
        }


        public void InitTournament()
        {
            SetupTournament();
            GameLogicRound.InitPreliminaryRound();
            JSONService.SaveTournament();
        }

        public void SetupTournament()
        {
            TournamentSettings tournamentSettings = GameLogicTournamentSettings.CreateTournamentSettings();
//            TournamentModel.Tournament.TournamentSettings = GameLogicTournamentSettings.CreateTournamentSettings();
//            CreateTournament(TournamentModel.Tournament.TournamentSettings);
            CreateTournament(tournamentSettings);
            JSONService.SaveTournamentSettings();

        }

        public void CreateTournament(TournamentSettings tournamentSettings)
        {
            bool emptyNameAllowed = false;
            string name = InputHandler.GetMandatoryName("Please enter the name of this tournament!");
            TournamentModel.Tournament = new Tournament(name, tournamentSettings);

        }


        public void RunTournament()
        {
            //if(!TournamentModel.Tournament.GamePlanKoRound.Any())
            //{
                GameLogicRound.RunPreliminaryRound();
                GameLogicRound.InitKoRound();
                GameLogicRound.RunKoRound();
            //}
            //else
            //{
            //    GameLogicRound.RunKoRound();
            //}


        }


    }
}