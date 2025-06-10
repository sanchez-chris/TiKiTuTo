using ClosedXML.Excel;
using TiKiTuTo.Model;
using TiKiTuTo.Model.DataObjects;

namespace TiKiTuTo
{
    public class EXCELService
    {
        private readonly string ProjectDirectory;
        public readonly string ExcelImportFolder;

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

            try
            {


                using (var workbook = new XLWorkbook($"{ExcelImportFolder}\\Import_Tournament_Settings.xlsx"))
                {
                    var tournamentSheet = workbook.Worksheet(1);

                    try
                    {
                        tournamentSettings.SettingsName = tournamentSheet.Cell("B2").GetValue<string>();
                        tournamentSettings.NumberOfTeamsTotal = tournamentSheet.Cell("B3").GetValue<int>();
                        tournamentSettings.NumberOfPreliminaryGamesPerTeam = tournamentSheet.Cell("B4").GetValue<int>();
                        tournamentSettings.NumberOfTeamsInKoRound = tournamentSheet.Cell("B5").GetValue<int>();
                        string answer = tournamentSheet.Cell("B6").GetValue<string>();
                        tournamentSettings.MatchDuration = tournamentSheet.Cell("B7").GetValue<int>();
                        if (answer == "Yes")
                        {
                            tournamentSettings.UseTimer = true;
                        }
                        else
                        {
                            tournamentSettings.UseTimer = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error reading tournament settings from the Excel file. Please ensure the format is correct.", ex);
                    }

                    var teamsSheet = workbook.Worksheet(2);
                    var lastRow = teamsSheet.LastRowUsed().RowNumber();

                    if (lastRow < 2)
                    {
                        throw new Exception("The Teams worksheet is empty or improperly formatted.");
                    }

                    for (int row = 2; row <= lastRow; row++)
                    {

                        var teamName = teamsSheet.Cell(row, 1).GetValue<string>();

                        if (string.IsNullOrEmpty(teamName))
                        {
                            throw new Exception($"Team name is missing in row {row}.");
                        }

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
            }
            catch (FileNotFoundException)
            {
                throw new Exception($"The file '{ExcelImportFolder}\\Import_Tournament_Settings.xlsx' was not found. Please ensure the file exists and the path is correct.");
            }
            catch (IOException ex)
            {
                throw new Exception("An error occurred while accessing the Excel file. Please check file permissions and ensure the file is not open in another program.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred during the Excel import process.", ex);
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
            if (!Directory.Exists(ExcelImportFolder))
            {
                Directory.CreateDirectory(ExcelImportFolder);
            }
        }


        public string GetProjectDirectoryPath()
        {
            return Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;
        }

        public string GetExcelImportFolderPath()
        {
            return Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TikiTuto\\Import_Folder");
        }


    }
}
