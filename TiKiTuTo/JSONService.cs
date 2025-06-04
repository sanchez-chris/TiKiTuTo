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
        private string saveFolder = "SaveGame";
        public TournamentModel TournamentModel { get; set; }
        IView View { get; set; }

        public JSONService(TournamentModel model, IView view)
        {
            TournamentModel = model;
            View = view;
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
                Console.WriteLine($"Error during serialization: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }

        }
        public void LoadGame()
        {

            // PART I: Auswahl des SaveGames
            string currentDirectory = Directory.GetCurrentDirectory();
            string[] currentSaveGames = Directory.GetFiles($"{currentDirectory}\\{saveFolder}");

            for (int i = 0; i < currentSaveGames.Length; i++)
            {
                string fileName = Path.GetFileName(currentSaveGames[i]);
                Console.WriteLine($"{i}: {fileName}");
            }
            Console.WriteLine("Please enter the number corresponding to the game you wish to load.");
            int number = Convert.ToInt32(Console.ReadLine());

            string filePath = currentSaveGames[number];

            // PART II: JSON-Konvertierung
            try
            {
                string tournamentJSON = File.ReadAllText(filePath);
                Tournament loadedTournament = JsonSerializer.Deserialize<Tournament>(tournamentJSON);

                if (loadedTournament != null)
                {
                    Console.WriteLine($"Turnier {loadedTournament.TournamentName} geladen");
                }
                else
                {
                    Console.WriteLine("Fehler beim Laden des Turniers: Das JSON konnte nicht deserialisiert werden.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Lesen der Datei: {ex.Message}");
            }
        }

    }
}
