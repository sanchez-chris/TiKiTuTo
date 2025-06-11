using System.Text.Json;
using System.Text.Json.Serialization;
using TiKiTuTo.Model;
using TiKiTuTo.Model.DataObjects;
using TiKiTuTo.View;

namespace TiKiTuTo
{
    public class JSONService
    {
        private readonly string _baseFolder = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "TikiTuto");

        private string SaveFolder => Path.Combine(_baseFolder, "Saved_Tournaments");
        private string SettingsFolder => Path.Combine(_baseFolder, "Saved_Tournament_Settings");
        private string FinishedTournamentFolder => Path.Combine(_baseFolder, "Finished_Tournaments");
        
        private readonly TournamentModel _tournamentModel;
        private readonly IView _view;


        public JSONService(TournamentModel tournamentModel, IView view)
        {
            _tournamentModel = tournamentModel;
            _view = view;

            InitialCreationOfFolder(SettingsFolder);
            InitialCreationOfFolder(SaveFolder);
            InitialCreationOfFolder(FinishedTournamentFolder);
        }


        public void SaveTournament()
        {
            string fileName = $"{DateTime.Now:yyyy-MM-dd HH-mm-ss} {_tournamentModel.Tournament.TournamentName}.json";
            SaveToFile(SaveFolder, fileName, _tournamentModel.Tournament);
            _view.AnimateAndConfirmSave(Path.Combine(SaveFolder, fileName));
        }

        public void SaveFinishedTournament()
        {
            string fileName = $"{DateTime.Now:yyyy-MM-dd HH-mm-ss} {_tournamentModel.Tournament.TournamentName}.json";
            SaveToFile(FinishedTournamentFolder, fileName, _tournamentModel.Tournament);
            _view.AnimateAndConfirmSave(Path.Combine(FinishedTournamentFolder, fileName));
        }

        public void SaveTournamentSettings(TournamentSettings tournamentSettings)
        {
            var settingsName = tournamentSettings.SettingsName;
            string fileName = $"{DateTime.Now:yyyy-MM-dd HH-mm-ss} {settingsName}.json";
            SaveToFile(SettingsFolder, fileName, tournamentSettings);
        }


        private void SaveToFile(string folderPath, string fileName, object tikitutoObject)
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

                string jsonString = JsonSerializer.Serialize(tikitutoObject, options);

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
                    _view.ShowLoadingAnimation($"Tournament {loadedTournament.TournamentName} loaded");
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
                    _view.ShowLoadingAnimation($"TournamentSettings {loadedTournamentSettings.SettingsName} loaded");
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
            string[] currentSaveGames = Directory.GetFiles(SaveFolder);

            return currentSaveGames;
        }

        /// <summary>
        /// Returns all files containing finished tournaments (ready to show results)
        /// </summary>
        /// <returns> List<string> of file names for the tournament JSON files.</returns>
        public string[] GetFinishedTournamentFiles()
        {
            string[] finishedTournamentFiles = Directory.GetFiles(FinishedTournamentFolder);

            return finishedTournamentFiles;
        }

        /// <summary>
        /// Returns all files containing earlier saved tournament settings
        /// </summary>
        /// <returns> List<string> of file names for the tournament settings JSON files.</returns>
        public string[] GetTournamentSettingsFiles()
        {
            //Array because of return tye of Directory.GetFiles()
            string[] tournamentSettings = Directory.GetFiles(SettingsFolder);

            return tournamentSettings;
        }

        public void InitialCreationOfFolder(string folderPath)
        {
            Directory.CreateDirectory(folderPath);
        }

    }
}
