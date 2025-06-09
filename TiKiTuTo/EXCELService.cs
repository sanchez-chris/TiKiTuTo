using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiKiTuTo.Model.DataObjects;
using ClosedXML.Excel;
using TiKiTuTo.Model;

namespace TiKiTuTo
{
    public class EXCELService
    {
        private readonly string ProjectDirectory; 
        private readonly string ExcelImportFolder;

        TournamentModel TournamentModel { get; set; }


        public EXCELService(TournamentModel tournamentModel)
        {
            TournamentModel = tournamentModel;
            ProjectDirectory = GetProjectDirectoryPath();
            ExcelImportFolder = GetExcelImportFolderPath();

            InitialCreationOfFolder();
            CopyTemplateIfNotExists();
        }


        public TournamentSettings ImportExcelFile()
        {
            var tournamentSettings = TournamentModel.Tournament.TournamentSettings;
            tournamentSettings.TeamsInTournament = new List<Team>();

            using (var workbook = new XLWorkbook($"{ExcelImportFolder}\\Import_Tournament_Settings.xlsx"))
            {
                var tournamentSheet = workbook.Worksheet(1);

                tournamentSettings.NumberOfTeamsTotal = tournamentSheet.Cell("B1").GetValue<int>();
                tournamentSettings.NumberOfTeamsInKoRound = tournamentSheet.Cell("B2").GetValue<int>();
                tournamentSettings.NumberOfPreliminaryGamesPerTeam = tournamentSheet.Cell("B3").GetValue<int>();
                tournamentSettings.MatchDuration = tournamentSheet.Cell("B4").GetValue<int>();
                tournamentSettings.SettingsName = tournamentSheet.Cell("B5").GetValue<string>();


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


        private void CopyTemplateIfNotExists()
        {
            var destinationFile = Path.Combine(ExcelImportFolder, "Import_Tournament_Settings.xlsx");
            string templatePath = Path.Combine(ProjectDirectory, "Templates", "Import_Tournament_Settings.xlsx");

            if (!File.Exists(destinationFile))
            {
                File.Copy(templatePath, destinationFile);
            }
        }
        public void InitialCreationOfFolder()
        {
            Directory.CreateDirectory(ExcelImportFolder);
        }


        public string GetProjectDirectoryPath()
        {
            return Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;
        }

        public string GetExcelImportFolderPath()
        {
            return Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TikiTuto\\1.Import_Folder");
        }


    }
}
