using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiKiTuTo.Model
{
    public class TournamentSettings
    {
        public int NumberOfPreliminaryGamesPerTeam { get; set; }
        public int NumberOfTeamsInKoRound { get; set; }
        public int NumberOfTeamsTotal { get; set; }
        public List<Team> TeamsInTournament { get; set; }
        public int MatchDuration { get; set; }

        public TournamentSettings(int totalteams, int preliminaryTeamNumber, int koteams,List<Team> teams)
        {
            NumberOfTeamsTotal = totalteams;
            NumberOfPreliminaryGamesPerTeam = preliminaryTeamNumber;
            NumberOfTeamsInKoRound = koteams;
            TeamsInTournament = teams;
            MatchDuration = 10;
        }

    }
}
