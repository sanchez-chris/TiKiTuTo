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
        public TournamentModel TournamentModel { get; set; }
        IView View { get; set; }
        InputHandler InputHandler { get; set; }

        public JSONService(TournamentModel model, IView view, InputHandler inputHandler)
        {
            TournamentModel = model;
            View = view;
            InputHandler = inputHandler;
        }

        public void SaveGame()
        {
            var Tournament = TournamentModel.Tournament;

            try
            {
                if (!Directory.Exists(saveFolder))
                {
                    Directory.CreateDirectory(saveFolder);
                }

                DateTime now = DateTime.Now;

                string filePath = $"{saveFolder}\\{Tournament.TournamentName}_{now.ToString("yyyy-MM-dd_HH-mm-ss")}.json";

                JsonSerializerOptions options = new JsonSerializerOptions();
                options.WriteIndented = true;

                string jsonString = JsonSerializer.Serialize(Tournament, options);
                File.WriteAllText(filePath, jsonString);

                View.SavingTournamentAnimation(filePath);
            }
            catch (JsonException ex)
            {
                View.ShowMessage($"Error during serialization: {ex.Message}");
            }
            catch (Exception ex)
            {
                View.ShowMessage($"An unexpected error occurred: {ex.Message}");
            }

        }
        public void LoadGame()
        {

            string[] currentSaveGames = Directory.GetFiles(saveFolder);

            View.DisplayFiles(currentSaveGames);
            int chosenFileIndex = InputHandler.GetValidFileSelection();
            string chosenFile = currentSaveGames[chosenFileIndex];


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
                    View.ShowMessage($"Turnier {loadedTournament.TournamentName} geladen");
                }
                
            }
            catch (JsonException ex)
            {
                View.ShowMessage($"Error during serialization: {ex.Message}");
            }
            catch (FileNotFoundException ex)
            {
                View.ShowMessage($"File not Found");
            }
            catch (Exception ex)
            {
                View.ShowMessage($"An unexpected error occured {ex.Message}");
            }

        }

    }
}
