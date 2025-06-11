using TiKiTuTo.Model.DataObjects;
using ClosedXML.Excel;
using TiKiTuTo.Model;

namespace TiKiTuTo
{
    public class ExcelService
    {
        private readonly string _projectDirectory; 
        public readonly string ExcelImportFolder;
        TournamentModel TournamentModel { get; set; }


        public ExcelService(TournamentModel tournamentModel)
        {
            TournamentModel = tournamentModel;
            _projectDirectory = GetProjectDirectoryPath();
            ExcelImportFolder = GetExcelImportFolderPath();

            InitialCreationOfFolder();
            CopyTemplateIfNotExists();
        }

        /// <summary>
        /// Imports tournament settings and team data from an Excel file.
        /// </summary>
        /// <returns>
        /// A <see cref="TournamentSettings"/> object populated with data from the Excel file.
        /// </returns>
        /// <remarks>
        /// Reads tournament settings from the first worksheet and team/player data from the second worksheet.
        /// Handles file not found, I/O, and data format errors during the import process.
        /// </remarks>
        public TournamentSettings ImportExcelFile()
        {
            TournamentSettings tournamentSettings = TournamentModel.Tournament.TournamentSettings;
            tournamentSettings.TeamsInTournament = new List<Team>();

            try
            {


                using (XLWorkbook workbook = new XLWorkbook($"{ExcelImportFolder}\\Import_Tournament_Settings.xlsx"))
                {
                    IXLWorksheet? tournamentSheet = workbook.Worksheet(1);

                    try
                    {
                        tournamentSettings.SettingsName = tournamentSheet.Cell("B2").GetValue<string>();
                        tournamentSettings.NumberOfTeamsTotal = tournamentSheet.Cell("B3").GetValue<int>();
                        tournamentSettings.NumberOfPreliminaryGamesPerTeam = tournamentSheet.Cell("B4").GetValue<int>();
                        tournamentSettings.NumberOfTeamsInKoRound = tournamentSheet.Cell("B5").GetValue<int>();
                        string useTimerAnswer = tournamentSheet.Cell("B6").GetValue<string>();
                        tournamentSettings.MatchDuration = tournamentSheet.Cell("B7").GetValue<int>();
                        if (useTimerAnswer == "YES")
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

                    IXLWorksheet? teamsSheet = workbook.Worksheet(2);
                    int lastRow = teamsSheet.LastRowUsed().RowNumber();

                    if (lastRow < 2)
                    {
                        throw new Exception("The Teams worksheet is empty or improperly formatted.");
                    }

                    for (int row = 2; row <= lastRow; row++)
                    {

                        string? teamName = teamsSheet.Cell(row, 1).GetValue<string>();

                        if (string.IsNullOrEmpty(teamName))
                        {
                            throw new Exception($"Team name is missing in row {row}.");
                        }

                        Team team = new Team { TeamName = teamName };

                        team.PlayerInTeam = new List<Player>();

                        for (int col = 2; col <= teamsSheet.LastColumnUsed().ColumnNumber(); col++)
                        {
                            string? playerName = teamsSheet.Cell(row, col).GetValue<string>();
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


        /// <summary>
        /// Copies the tournament settings template to the import folder if it does not already exist.
        /// </summary>
        /// <remarks>
        /// Checks if the template file exists in the destination; if not, copies it from the project directory.
        /// </remarks>
        private void CopyTemplateIfNotExists()
        {
            var destinationFile = Path.Combine(ExcelImportFolder, "Settings.xlsx");
            string templatePath = Path.Combine(_projectDirectory, "Templates", "Settings.xlsx");

            if (!File.Exists(destinationFile))
            {
                File.Copy(templatePath, destinationFile);
            }
        }


        /// <summary>
        /// Ensures the Excel import folder exists by creating it if necessary.
        /// </summary>
        /// <remarks>
        /// Checks if the folder exists and creates it if it does not.
        /// </remarks>
        public void InitialCreationOfFolder()
        {
            if (!Directory.Exists(ExcelImportFolder))
            {
                Directory.CreateDirectory(ExcelImportFolder);
            }
        }

        /// <summary>
        /// Retrieves the full path to the project directory.
        /// </summary>
        /// <returns>The full path of the project directory.</returns>
        /// <remarks>
        /// Navigates up the directory hierarchy from the application's base directory to locate the project root.
        /// </remarks>
        public string GetProjectDirectoryPath()
        {
            return Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;
        }


        /// <summary>
        /// Retrieves the full path to the Excel import folder within the local application data directory.
        /// </summary>
        /// <returns>The full path of the Excel import folder.</returns>
        /// <remarks>
        /// Combines the local application data path with the specific folder structure for the import folder.
        /// </remarks>
        public string GetExcelImportFolderPath()
        {
            return Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TikiTuto\\Import_Folder");
        }


    }
}
