using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TiKiTuTo.Model;
using TiKiTuTo.Model.DataObjects;
using TiKiTuTo.View;

namespace TiKiTuTo.Controller
{
    public class JSONService
    {
        private string saveFolder = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "TikiTuto", "SaveGames");

        public TournamentModel _tournamentModel;
        private IView _view;
        private InputHandler _inputHandler;


        public JSONService(TournamentModel tournamentModel, IView view, InputHandler inputHandler)
        {
            _tournamentModel = tournamentModel;
            _view = view;
            _inputHandler = inputHandler;

            CreateSaveFolder();
        }

        public void SaveTournament()
        {
            var tournament = _tournamentModel.Tournament;

            try
            {
                if (!Directory.Exists(saveFolder))
                {
                    Directory.CreateDirectory(saveFolder);
                }

                DateTime now = DateTime.Now;

                string filePath = $"{saveFolder}\\{tournament.TournamentName}_{now.ToString("yyyy-MM-dd_HH-mm-ss")}.json";

                JsonSerializerOptions options = new JsonSerializerOptions();
                options.WriteIndented = true;

                string jsonString = JsonSerializer.Serialize(tournament, options);
                File.WriteAllText(filePath, jsonString);

                _view.SavingTournamentAnimation(filePath);
            }
            catch (JsonException ex)
            {
                _view.ShowMessage($"Error during serialization: {ex.Message}");
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
            string[] finishedTournamentFiles = Directory.GetFiles(saveFolder);

            return finishedTournamentFiles;
        }

        public void InitialCreationOfSaveFolder()
        {
            Directory.CreateDirectory(saveFolder);
        }

    }
}
