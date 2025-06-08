using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiKiTuTo.Model.DataObjects;
using ClosedXML.Excel;

namespace TiKiTuTo
{
    public class EXCELService
    {
        public TournamentSettings ImportExcelFile()
        {
            string filePath = "Import_Tournament_Settings\\Teams.xlsx";

            var tournamentSettings = new TournamentSettings();

            tournamentSettings.TeamsInTournament = new List<Team>();

            using (var workbook = new XLWorkbook(filePath))
            {
                var tournamentSheet = workbook.Worksheet(1);

                tournamentSettings.NumberOfTeamsTotal = tournamentSheet.Cell("B1").GetValue<int>();
                tournamentSettings.NumberOfTeamsInKoRound = tournamentSheet.Cell("B2").GetValue<int>();
                tournamentSettings.NumberOfPreliminaryGamesPerTeam = tournamentSheet.Cell("B3").GetValue<int>();
                tournamentSettings.MatchDuration = tournamentSheet.Cell("B4").GetValue<int>();

                var teamsSheet = workbook.Worksheet(2);
                var lastRow = teamsSheet.LastRowUsed().RowNumber();

                for (int row = 2; row <= lastRow; row++)
                {
                    var teamName = teamsSheet.Cell(row, 1).GetValue<string>();

                    var team = new Team { TeamName = teamName };

                    team.PlayerInTeam = new List<Player>();

                    for (int col = 2; col <= teamsSheet.LastColumnUsed().ColumnNumber(); col++)
                    {
                        var playerName = teamsSheet.Cell(row, col).GetValue<string>();
                        if (!string.IsNullOrEmpty(playerName))
                        {
                            team.PlayerInTeam.Add(new Player { Name = playerName });
                        }
                    }
                    tournamentSettings.TeamsInTournament.Add(team);
                }
            }
            return tournamentSettings;
        }
    }
}
