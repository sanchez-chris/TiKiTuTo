using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TiKiTuTo.Classes
{
    public class Round
    {
        public enum RoundType
        {
            League,
            KO,
        }

        public RoundType TypeOf { get; set; }
        public List<Match> Matches { get; set; } = new List<Match>();
        public int ActiveRoundIndex { get; set; }


        public Round(RoundType typeOf, List<Match> matches)
        {
            TypeOf = typeOf;
            Matches = matches;
            ActiveRoundIndex = 0;
        }
        Tournament Tournament = new Tournament("Blub");
        // List<Team> Teams = Tournament.AddTeam();

        public void StartRound(RoundType typeOf, int koContestants, int gamesPerTeam)
        {
            if (this.TypeOf == RoundType.League)
            {
                List<Team> finalisten = PreliminaryRound(Tournament.Teams, koContestants, gamesPerTeam);
            }
            else if (this.TypeOf == RoundType.KO)
            {
                //KORound();
            }

        }


        public List<Team> PreliminaryRound(List<Team> teams, int KOContestants, int gamesPerTeam)
        {
            Console.Clear();
            Console.WriteLine("=== Preliminary Round ===");

            List<Match> matchesCreated = new List<Match>();
            var random = new Random();

            while (true)
            {
                // Prüfe, ob alle Teams genug Spiele haben
                if (teams.All(t => t.GamesPlayed >= gamesPerTeam))
                    break;

                // Finde zwei Teams, die noch nicht genug Spiele haben
                var availableTeams = teams.Where(t => t.GamesPlayed < gamesPerTeam).OrderBy(_ => random.Next()).ToList();

                Team team1 = null;
                Team team2 = null;
                bool foundPair = false;

                for (int i = 0; i < availableTeams.Count; i++)
                {
                    for (int j = i + 1; j < availableTeams.Count; j++)
                    {
                        Team t1 = availableTeams[i];
                        Team t2 = availableTeams[j];

                        // Prüfe, ob diese Paarung schon gespielt wurde
                        bool alreadyPlayed = Matches.Any(m =>
                            (m.Team1 == t1 && m.Team2 == t2) ||
                            (m.Team1 == t2 && m.Team2 == t1));

                        if (!alreadyPlayed)
                        {
                            team1 = t1;
                            team2 = t2;
                            foundPair = true;
                            break;
                        }
                    }
                    if (foundPair) break;
                }

                if (!foundPair)
                {
                    Console.WriteLine("No more valid matches can be generated.");
                    break;
                }

                // Match-Eingabe
                Console.WriteLine($"\nMatch: {team1.TeamName} vs {team2.TeamName}");
                int goalsTeam1 = GetValidGoalInput(team1.TeamName);
                int goalsTeam2 = GetValidGoalInput(team2.TeamName);

                Match match = new Match(team1, team2);
                match.UpdateMatchResults(goalsTeam1, goalsTeam2);
                Matches.Add(match);

                // Ausgabe
                if (goalsTeam1 > goalsTeam2)
                    Console.WriteLine($"{team1.TeamName} wins!");
                else if (goalsTeam2 > goalsTeam1)
                    Console.WriteLine($"{team2.TeamName} wins!");
                else
                    Console.WriteLine("It's a draw. No team wins!");
            }

            // Tabelle ausgeben
            Console.WriteLine("\n=== Standings after the first matches ===");
            PrintStandings(teams);

            // Top-Teams auswählen für KO-Runde
            List<Team> sortedTeams = new List<Team>(teams);
            sortedTeams.Sort((a, b) =>
            {
                int winCompare = b.GamesWon.CompareTo(a.GamesWon);
                if (winCompare != 0) return winCompare;
                return b.GoalDifference.CompareTo(a.GoalDifference);
            });

            List<Team> koTeams = sortedTeams.GetRange(0, KOContestants);

            Console.WriteLine($"\n=== Teams advancing to KO Round ({KOContestants}) ===");
            foreach (var team in koTeams)
            {
                Console.WriteLine($"{team.TeamName} | Wins: {team.GamesWon}, Goal Diff: {team.GoalDifference}");
            }

            // Countdown
            for (int i = 5; i > 0; i--)
            {
                Console.Write($"\rStarting KO Round in {i} seconds... ");
                Thread.Sleep(1000);
            }

            Console.Clear();
            return koTeams;
        }



        private bool AllTeamsPlayedSameNumberOfGames(List<Team> teams)
        {
            int games = teams[0].GamesPlayed;
            foreach (var team in teams)
            {
                if (team.GamesPlayed != games)
                    return false;
            }
            return true;
        }

        private void PrintStandings(List<Team> teams)
        {
            // Create a sorted copy of the list
            List<Team> sortedTeams = new List<Team>(teams);
            sortedTeams.Sort((a, b) =>
            {
                int winCompare = b.GamesWon.CompareTo(a.GamesWon);
                if (winCompare != 0) return winCompare;
                return b.GoalDifference.CompareTo(a.GoalDifference);
            });

            Console.WriteLine();
            Console.WriteLine($"{"Rank",4} {"Team",-20} {"Played",7} {"Wins",5} {"Goals",5} {"Goal Diff",10}");
            Console.WriteLine("---------------------------------------------------------------------------------");

            int rank = 1;
            foreach (var team in sortedTeams)
            {
                Console.WriteLine($"{rank,4} {team.TeamName,-20} {team.GamesPlayed,7} {team.GamesWon,5} {team.GoalsTotal,5} {team.GoalDifference,10}");
                rank++;
            }

}

        // Private helper for goal input (inside Round)
        private int GetValidGoalInput(string teamName)
            {
                string input;
                do
                {
                    Console.Write($"Enter goals for {teamName}: ");
                    input = Console.ReadLine();

                    if (!Regex.IsMatch(input, @"^-?\d+$"))
                    {
                        Console.WriteLine("Invalid input. Please enter a positive whole number.");
                    }

                } while (!Regex.IsMatch(input, @"^-?\d+$"));

                return int.Parse(input);
            }




    public List<Team> FinishRound()
        {
            List<Team> winners = new List<Team>();

            foreach (var match in Matches)
            {
                if (match.Team1Score > match.Team2Score)
                    winners.Add(match.Team1);
                else if (match.Team2Score > match.Team1Score)
                    winners.Add(match.Team2);
            }

            ActiveRoundIndex++;
            return winners;
        }

        public void SetupKORound(List<Team> teamsInRound)
        {
            Matches.Clear();  // Wichtiger Schritt: alte Matches löschen
            var random = new Random();

            // Shuffle Teams
            var shuffledTeams = teamsInRound.OrderBy(t => random.Next()).ToList();

            for (int i = 0; i < shuffledTeams.Count; i += 2)
            {
                Team team1 = shuffledTeams[i];
                Team team2 = shuffledTeams[i + 1];
                Matches.Add(new Match(team1, team2));
            }
        }
    }

}
