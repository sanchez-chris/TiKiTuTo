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

        /// <summary>
        /// Saves the current tournament to a JSON file with a timestamped filename.
        /// </summary>
        /// <remarks>
        /// The tournament is serialized and saved in the specified folder using the current date and time 
        /// as part of the filename. After saving, an animation and confirmation are displayed to the user.
        /// </remarks>
        public void SaveTournament()
        {
            string fileName = $"{DateTime.Now:yyyy-MM-dd HH-mm-ss} {_tournamentModel.Tournament.TournamentName}.json";
            SaveToFile(saveFolder, fileName, _tournamentModel.Tournament);
            _view.AnimateAndConfirmSave(Path.Combine(saveFolder, fileName));
        }

        /// <summary>
        /// Saves the finished tournament to a JSON file with a timestamped filename.
        /// </summary>
        /// <remarks>
        /// The finished tournament is serialized and saved in the specified folder designated for completed tournaments.
        /// The filename includes the current date and time for uniqueness. After saving, an animation and confirmation 
        /// are displayed to the user.
        /// </remarks>
        public void SaveFinishedTournament()
        {
            string fileName = $"{DateTime.Now:yyyy-MM-dd HH-mm-ss} {_tournamentModel.Tournament.TournamentName}.json";
            SaveToFile(finishedTournamentFolder, fileName, _tournamentModel.Tournament);
            _view.AnimateAndConfirmSave(Path.Combine(finishedTournamentFolder, fileName));
        }

        /// <summary>
        /// Saves the tournament settings to a JSON file with a timestamped filename.
        /// </summary>
        /// <param name="tournamentSettings">The tournament settings object to be serialized and saved.</param>
        /// <remarks>
        /// The tournament settings are serialized and saved in the designated settings folder.
        /// The filename includes the current date and time along with the settings name for uniqueness.
        /// </remarks>
        public void SaveTournamentSettings(TournamentSettings tournamentSettings)
        {
            var settingsName = tournamentSettings.SettingsName;
            string fileName = $"{DateTime.Now:yyyy-MM-dd HH-mm-ss} {settingsName}.json";
            SaveToFile(settingsFolder, fileName, tournamentSettings);
        }

        /// <summary>
        /// Saves an object to a specified folder and file in JSON format.
        /// </summary>
        /// <param name="folderPath">The path to the folder where the file will be saved.</param>
        /// <param name="fileName">The name of the file to save the object in.</param>
        /// <param name="TikitutoObject">The object to be serialized and saved as JSON.</param>
        /// <remarks>
        /// The method ensures the target folder exists, serializes the object to JSON with indentation, 
        /// and writes it to the specified file. Handles serialization errors, I/O errors, and other unexpected exceptions.
        /// </remarks>
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


        /// <summary>
        /// Loads tournament settings from a specified JSON file.
        /// </summary>
        /// <param name="chosenFile">The path to the JSON file containing the tournament settings.</param>
        /// <returns>
        /// A <see cref="TournamentSettings"/> object if the file is successfully read and deserialized;
        /// otherwise, returns <c>null</c> if an error occurs during the process.
        /// </returns>
        /// <remarks>
        /// The method handles JSON deserialization errors, file not found exceptions, and other unexpected exceptions,
        /// displaying appropriate messages for each scenario.
        /// </remarks>
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

        /// <summary>
        /// Loads tournament settings from a specified JSON file.
        /// </summary>
        /// <param name="chosenFile">The path to the JSON file containing the tournament settings.</param>
        /// <returns>
        /// A <see cref="TournamentSettings"/> object if the file is successfully read and deserialized;
        /// otherwise, returns <c>null</c> if an error occurs during the process.
        /// </returns>
        /// <remarks>
        /// The method deserializes the JSON file into a <see cref="TournamentSettings"/> object. 
        /// If successful, it displays a loading animation with the settings name. 
        /// Handles JSON deserialization errors, file not found exceptions, and other unexpected exceptions, 
        /// displaying appropriate messages for each scenario.
        /// </remarks>
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
        /// Retrieves file paths of unfinished tournament save games.
        /// </summary>
        /// <returns>An array of file paths for unfinished tournaments.</returns>
        public string[] GetUnfinishedTournamentFiles()
        {
            //Array because of return tye of Directory.GetFiles()
            string[] currentSaveGames = Directory.GetFiles(saveFolder);

            return currentSaveGames;
        }

        /// <summary>
        /// Retrieves file paths of finished tournament files.
        /// </summary>
        /// <returns>An array of file paths for finished tournaments.</returns>
        public string[] GetFinishedTournamentFiles()
        {
            string[] finishedTournamentFiles = Directory.GetFiles(finishedTournamentFolder);

            return finishedTournamentFiles;
        }

        /// <summary>
        /// Retrieves file paths of tournament settings files.
        /// </summary>
        /// <returns>An array of file paths for tournament settings.</returns>
        public string[] GetTournamentSettingsFiles()
        {
            //Array because of return tye of Directory.GetFiles()
            string[] tournamentSettings = Directory.GetFiles(settingsFolder);

            return tournamentSettings;
        }

        /// <summary>
        /// Creates a folder at the specified path if it does not already exist.
        /// </summary>
        /// <param name="folderPath">The path where the folder should be created.</param>
        public void InitialCreationOfFolder(string folderPath)
        {
            Directory.CreateDirectory(folderPath);
        }

    }
}
