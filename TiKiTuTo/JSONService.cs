using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Classes;
using GameEngine;

namespace TiKiTuTo
{
    internal class JSONService
    {
        private static string saveFolder = "SaveGame";

        public static void SaveGame(Tournament tournament)
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
            File.WriteAllText(filePath, jsonString );
            Console.WriteLine($"Tournament has been saved: {filePath}");
        }


        public static void LoadGame()
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
