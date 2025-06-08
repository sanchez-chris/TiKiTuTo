using System.Text.Json;
using System.Text.Json.Serialization;
using TiKiTuTo.Model;
using TiKiTuTo.Model.BusinessLogic.GameLogic;
using TiKiTuTo.Model.DataObjects;
using TiKiTuTo.View;

namespace TiKiTuTo.Controller
{
    public class JSONService
    {
        private readonly string baseFolder = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "TikiTuto");

        private string saveFolder => Path.Combine(baseFolder, "Saved_Tournaments");
        private string settingsFolder => Path.Combine(baseFolder, "Saved_Tournament_Settings");
        private string finishedTournamentFolder => Path.Combine(baseFolder, "Finished_Tournaments");
        
        private TournamentModel _tournamentModel;
        private IView _view;


        public JSONService(TournamentModel tournamentModel, IView view)
        {
            _tournamentModel = tournamentModel;
            _view = view;

            InitialCreationOfFolder(settingsFolder);
            InitialCreationOfFolder(saveFolder);
            InitialCreationOfFolder(finishedTournamentFolder);
        }


        public void SaveTournament()
        {
            string fileName = $"{DateTime.Now:yyyy-MM-dd_HH-mm-ss}_{_tournamentModel.Tournament.TournamentName}.json";
            SaveToFile(saveFolder, fileName, _tournamentModel.Tournament);
            _view.AnimateAndConfirmSave(Path.Combine(saveFolder, fileName));
        }

        public void SaveFinishedTournament()
        {
            string fileName = $"{DateTime.Now:yyyy-MM-dd_HH-mm-ss}_{_tournamentModel.Tournament.TournamentName}.json";
            SaveToFile(finishedTournamentFolder, fileName, _tournamentModel.Tournament);
            _view.AnimateAndConfirmSave(Path.Combine(finishedTournamentFolder, fileName));
        }

        public void SaveTournamentSettings(TournamentSettings tournamentSettings)
        {
            var settingsName = tournamentSettings.SettingsName;
            string fileName = $"{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.json";
            SaveToFile(settingsFolder, fileName, tournamentSettings);
        }


        private void SaveToFile(string folderPath, string fileName, object TikitutoObject)
        {
            try
            {
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string filePath = Path.Combine(folderPath, fileName);

                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    WriteIndented = true
                };

                string jsonString = JsonSerializer.Serialize(TikitutoObject, options);

                File.WriteAllText(filePath, jsonString);

            }
            catch (JsonException ex)
            {
                _view.ShowMessage($"Error during serialization: {ex.Message}");
            }
            catch (IOException ex)
            {
                _view.ShowMessage($"I/O error while writing the file: {ex.Message}");
            }
            catch (Exception ex)
            {
                _view.ShowMessage($"An unexpected error occurred: {ex.Message}");
            }
        }


        public void LoadTournament(string chosenFile)
        {
            try
            {
                string tournamentJSON = File.ReadAllText(chosenFile);

                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    PropertyNameCaseInsensitive = true 
                };
                Tournament loadedTournament = JsonSerializer.Deserialize<Tournament>(tournamentJSON, options);
             

                if (loadedTournament != null)
                {
                    _view.ShowMessage($"Turnier {loadedTournament.TournamentName} geladen");
                    _tournamentModel.Tournament = loadedTournament;
                }

            }
            catch (JsonException ex)
            {
                _view.ShowMessage($"Error during serialization: {ex.Message}");
            }
            catch (FileNotFoundException ex)
            {
                _view.ShowMessage($"File not Found");
            }
            catch (Exception ex)
            {
                _view.ShowMessage($"An unexpected error occured {ex.Message}");
            }
        }

        public TournamentSettings LoadTournamentSettings(string chosenFile)
        {
            try
            {

                string tournamentJSON = File.ReadAllText(chosenFile);

                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    PropertyNameCaseInsensitive = true
                };
                TournamentSettings loadedTournamentSettings = JsonSerializer.Deserialize<TournamentSettings>(tournamentJSON, options);


                if (loadedTournamentSettings != null)
                {
                    _view.ShowMessage($"TournamentSettings geladen");
                    return loadedTournamentSettings;
                }


            }
            catch (JsonException ex)
            {
                _view.ShowMessage($"Error during serialization: {ex.Message}");
            }
            catch (FileNotFoundException ex)
            {
                _view.ShowMessage($"File not Found");
            }
            catch (Exception ex)
            {
                _view.ShowMessage($"An unexpected error occured {ex.Message}");
            }

            return null;

        }


        /// <summary>
        /// Returns all files containing unfinished tournaments (ready to be continued)
        /// </summary>
        /// <returns> List<string> of file names for the tournament JSON files.</returns>
        public string[] GetUnfinishedTournamentFiles()
        {
            //Array because of return tye of Directory.GetFiles()
            string[] currentSaveGames = Directory.GetFiles(saveFolder);

            return currentSaveGames;
        }

        /// <summary>
        /// Returns all files containing finished tournaments (ready to show results)
        /// </summary>
        /// <returns> List<string> of file names for the tournament JSON files.</returns>
        public string[] GetFinishedTournamentFiles()
        {
            string[] finishedTournamentFiles = Directory.GetFiles(finishedTournamentFolder);

            return finishedTournamentFiles;
        }

        /// <summary>
        /// Returns all files containing earlier saved tournament settings
        /// </summary>
        /// <returns> List<string> of file names for the tournament settings JSON files.</returns>
        public string[] GetTournamentSettingsFiles()
        {
            //Array because of return tye of Directory.GetFiles()
            string[] tournamentSettings = Directory.GetFiles(settingsFolder);

            return tournamentSettings;
        }

        public void InitialCreationOfFolder(string folderPath)
        {
            Directory.CreateDirectory(folderPath);
        }

    }
}
