using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiKiTuTo.Controller;
using TiKiTuTo.View;
using TiKiTuTo.Model.BusinessLogic;
using TiKiTuTo.Model.DataObjects;
using TiKiTuTo.Model;
namespace TiKiTuTo.Model.BusinessLogic.GameLogic
{
    /// <summary>
    /// Handles business logic regarding TournamentSettings objects. 
    /// </summary>
    

    public class GameLogicTournamentSettings
    {
        InputHandler InputHandler;
        public TournamentModel TournamentModel { get; set; }

        public GameLogicTournamentSettings(TournamentModel model, InputHandler inputHandler) 
        {
            InputHandler = inputHandler;
            TournamentModel = model;
        }


        /// <summary>
        /// Creates TournamentSettings based on user input.
        /// Validates input so that a functional Tournament can be initialized based on these settings.
        /// </summary>
        /// <returns>TournamentSettings instance holding all relevant, validated parameters to initialize a new Tournament.</returns>
        public TournamentSettings CreateTournamentSettings()
        {

            int NumberOfTeamsTotal;                 //how many teams attend the tournament?
            int NumberOfPreliminaryGamesPerTeam;    //how many games will be played per team in the preliminaries?
            int NumberOfTeamsInKORound;             //how many teams will progress into knockout rounds?
            List<Team> Teams;                       //the actual teams

            //ask for the necessary inputs
            NumberOfTeamsTotal = InputHandler.GetValidNumberOfTotalTeams();
            NumberOfPreliminaryGamesPerTeam = InputHandler.GetValidNumberOfPreliminaryGames(NumberOfTeamsTotal);
            NumberOfTeamsInKORound = InputHandler.GetValidNumberOfTeamsInKORound(NumberOfTeamsTotal);
            Teams = CreateListOfTeams(NumberOfTeamsTotal);
            string TournamentSettingsName = InputHandler.GetMandatoryName("Name the Settings.");

            TournamentSettings tournamentSettings = new TournamentSettings(NumberOfTeamsTotal, NumberOfPreliminaryGamesPerTeam, NumberOfTeamsInKORound, Teams, TournamentSettingsName);
            return tournamentSettings;
        }

        public List<Team> CreateListOfTeams(int NumberOfTeamsTotal)
        {
            List<Team> teams = new List<Team>();
            int maxTeamMembers = InputHandler.GetNumber("How many Teammembers would you like to have?");
            for (int i = 1; i <= NumberOfTeamsTotal; i++)
            {
                teams.Add(CreateTeam(i, maxTeamMembers));
            }
            
            InputHandler.View.ShowMessage($"You have created {teams.Count} teams.\n\nPreliminary round contestant:");
            InputHandler.View.ShowTeamsAndPlayer(teams);
            return teams;
        }


        public Team CreateTeam(int i, int maxTeamMembers)
        {
            bool emptyNameAllowed = true;
            string? teamName = InputHandler.GetTeamName($"Please enter the name of the team. Default name when empty: Team {i}.", i);

            if (string.IsNullOrEmpty(teamName))
            {
                teamName = $"Team {i}";
            }

            List<Player> playerList = new List<Player>();
            Team team = new Team(teamName, playerList);
//           maxTeamMembers = InputHandler.GetNumber("How many Teammembers would you like to have?");
            for (int p = 1; p <= maxTeamMembers; p++)
            {
                string? playerName = InputHandler.GetPlayerName($"Please enter the name of the next team member. Default name when empty: Player {p}.", p);

                if (string.IsNullOrEmpty(playerName))
                {
                    playerName = $"Player {p}";
                }
                Player player = new Player(playerName);
                AddPlayer(player, team);

            }
            return team;
        }

        /// <summary>
        /// Used to add a Player to a Team, needs a Player Object
        /// </summary>
        /// <param name="player"></param>
        public void AddPlayer(Player player, Team team)
        {
            if (!team.PlayerInTeam.Contains(player))
            {
                team.PlayerInTeam.Add(player);
            }
        }


    }
}