using TiKiTuTo.Controller;
using TiKiTuTo.Model.DataObjects;
namespace TiKiTuTo.Model.BusinessLogic.GameLogic
{
    /// <summary>
    /// Handles business logic regarding TournamentSettings objects. 
    /// </summary>
    

    public class GameLogicTournamentSettings
    {
        private readonly InputHandler _inputHandler;

        public GameLogicTournamentSettings(InputHandler inputHandler) 
        {
            _inputHandler = inputHandler;
        }


        /// <summary>
        /// Creates TournamentSettings based on user input.
        /// Validates input so that a functional Tournament can be initialized based on these settings.
        /// </summary>
        /// <returns>TournamentSettings instance holding all relevant, validated parameters to initialize a new Tournament.</returns>
        public TournamentSettings CreateTournamentSettings()
        {

            int numberOfTeamsTotal;                 //how many teams attend the tournament?
            int numberOfPreliminaryGamesPerTeam;    //how many games will be played per team in the preliminaries?
            int numberOfTeamsInKORound;             //how many teams will progress into knockout rounds?
            bool useTimer;                          //should a timer be used?
            int matchDuration = 0;                  //how long should one match take?
            List<Team> teams;                       //the actual teams

            //ask for the necessary inputs
            numberOfTeamsTotal = _inputHandler.GetValidNumberOfTotalTeams();
            numberOfPreliminaryGamesPerTeam = _inputHandler.GetValidNumberOfPreliminaryGames(numberOfTeamsTotal);
            numberOfTeamsInKORound = _inputHandler.GetValidNumberOfTeamsInKORound(numberOfTeamsTotal);
            useTimer = _inputHandler.GetApproval("Do you want to set a timer for the matches? [[[bold green]Y[/]/[bold red]N[/]]]");
            
            if (useTimer)
            {
                matchDuration = _inputHandler.GetValidMatchDuration();
            }
            teams = CreateListOfTeams(numberOfTeamsTotal);
            
            string tournamentSettingsName = _inputHandler.GetMandatoryName("Name the Settings.");

            TournamentSettings tournamentSettings = new TournamentSettings
                (
                numberOfTeamsTotal, 
                numberOfPreliminaryGamesPerTeam, 
                numberOfTeamsInKORound, 
                teams, 
                useTimer, 
                matchDuration, 
                tournamentSettingsName
                );

            return tournamentSettings;
        }

        public List<Team> CreateListOfTeams(int numberOfTeamsTotal)
        {
            List<Team> teams = new List<Team>();
            int maxTeamMembers = _inputHandler.GetNumber("How many Teammembers would you like to have?");
            for (int i = 1; i <= numberOfTeamsTotal; i++)
            {
                teams.Add(CreateTeam(i, maxTeamMembers));
            }
            
            _inputHandler.View.ShowMessage($"You have created {teams.Count} teams.\n\nPreliminary round contestant:");
            _inputHandler.View.ShowTeamsAndPlayer(teams);
            return teams;
        }


        public Team CreateTeam(int i, int maxTeamMembers)
        {
            string teamName = _inputHandler.GetTeamName($"Please enter the name of the team. Default name when empty: Team {i}.", i);

            Team team = new Team(teamName);
//           maxTeamMembers = InputHandler.GetNumber("How many Teammembers would you like to have?");
            for (int p = 1; p <= maxTeamMembers; p++)
            {
                string playerName = _inputHandler.GetPlayerName($"Please enter the name of the next team member. Default name when empty: Player {p}.", p);

                Player player = new Player(playerName);
                AddPlayer(player, team);

            }
            return team;
        }

        /// <summary>
        /// Used to add a Player to a Team, needs a Player object and a Team object
        /// </summary>
        /// <param name="player">The player added to team</param>
        /// <param name="team">The team to add the player to</param>
        public void AddPlayer(Player player, Team team)
        {
            if (!team.PlayerInTeam.Contains(player))
            {
                team.PlayerInTeam.Add(player);
            }
        }


    }
}