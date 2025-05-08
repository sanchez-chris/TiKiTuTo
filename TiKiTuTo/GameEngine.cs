using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TiKiTuTo.Classes;
namespace GameEngine
{
    public class Game
    {
        private Tournament tournament;

        public void NewGame()
        {
            Console.Clear();
            Console.WriteLine("Starting a new tournament...");

            // 1️⃣ Number of teams
            int teamCount = GetEvenNumberInput("Enter the number of teams (must be even): ");
            var teams = new List<Team>();

            // 2️⃣ Team setup
            for (int i = 1; i <= teamCount; i++)
            {
                Console.Write($"Enter name for Team {i} (leave empty for default): ");
                string teamName = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(teamName))
                {
                    teamName = $"Team {i}";
                }
                teams.Add(new Team(teamName));
            }

            // 3️⃣ Vorrundenspiele einlesen
            int gamesPerTeam = GetDivisibleInput("Enter number of preliminary games per team (must divide evenly): ", teamCount);

            // 4️⃣ KO-Teams einlesen
            int koTeams = GetPowerOfTwoInput("Enter the number of teams advancing to KO round (must be power of 2): ", teamCount);

            // 5️⃣ Tournament erstellen
            tournament = new Tournament("My Tournament");
            foreach (var team in teams)
            {
                tournament.AddTeam(team);
            }

            // 6️⃣ Vorrunde ausführen
            Round preliminaryRound = new Round(Round.RoundType.League, new List<Match>());
            var koParticipants = preliminaryRound.PreliminaryRound(tournament.Teams, koTeams, gamesPerTeam);

            // 7️⃣ KO-Plan erstellen & anzeigen
            Round koRound = new Round(Round.RoundType.KO, new List<Match>());
            koRound.SetupKORound(koParticipants);

            Console.WriteLine("\nKO Matches:");
            foreach (var match in koRound.Matches)
            {
                Console.WriteLine($"{match.Team1.TeamName} vs {match.Team2.TeamName}");
            }

            Console.WriteLine("\nTournament setup complete. Press any key to return to menu.");
            Console.ReadKey();
        }

        // === Helper methods ===
        private int GetEvenNumberInput(string prompt)
        {
            int value;
            do
            {
                Console.Write(prompt);
            }
            while (!int.TryParse(Console.ReadLine(), out value) || value % 2 != 0 || value < 2);
            return value;
        }

        private int GetDivisibleInput(string prompt, int teamCount)
        {
            int value;
            do
            {
                Console.Write(prompt);
            }
            while (!int.TryParse(Console.ReadLine(), out value) || value <= 0 || (value * teamCount) % 2 != 0);
            return value;
        }

        private int GetPowerOfTwoInput(string prompt, int max)
        {
            int value;
            do
            {
                Console.Write(prompt);
            }
            while (!int.TryParse(Console.ReadLine(), out value) || value < 2 || value > max || !IsPowerOfTwo(value));
            return value;
        }

        private bool IsPowerOfTwo(int x)
        {
            return (x > 0) && ((x & (x - 1)) == 0);
        }
    }
}

