namespace TiKiTuTo.Model.DataObjects
{
    public class TournamentSettings
    {
        public int NumberOfPreliminaryGamesPerTeam { get; set; }
        public int NumberOfTeamsInKoRound { get; set; }
        public int NumberOfTeamsTotal { get; set; }
        public List<Team> TeamsInTournament { get; set; }
        public bool UseTimer { get; set; }
        public int MatchDuration { get; set; }
        public string SettingsName { get; set; }

        public TournamentSettings(int totalteams, int preliminaryTeamNumber, int koteams, List<Team> teams, bool useTimer, int matchDuration, string settingsName)
        {
            NumberOfTeamsTotal = totalteams;
            NumberOfPreliminaryGamesPerTeam = preliminaryTeamNumber;
            NumberOfTeamsInKoRound = koteams;
            TeamsInTournament = teams;
            UseTimer = useTimer;
            MatchDuration = matchDuration;
            SettingsName = settingsName;
        }

        public TournamentSettings() { }

    }
}
