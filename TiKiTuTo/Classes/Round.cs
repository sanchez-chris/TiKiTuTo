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

        public void StartRound(RoundType typeOf)
        {
            if (this.TypeOf == RoundType.League)
            {
                //preliminaryRound();
            }
            else if (this.TypeOf == RoundType.KO)
            {
                //KORound();
            }

        }
        
        public List<Team> PreliminaryRound(List<Team> teams, int KOContestants)
            {
                Console.Clear();
                Console.WriteLine("=== Preliminary Round ===");

                for (int i = 0; i < teams.Count; i++)
                {
                    for (int j = i + 1; j < teams.Count; j++) // only once per pair
                    {
                        Team team1 = teams[i];
                        Team team2 = teams[j];

                        Console.WriteLine($"\\nMatch: {team1.TeamName} vs {team2.TeamName}");

                        int goalsTeam1 = GetValidGoalInput(team1.TeamName);
                        int goalsTeam2 = GetValidGoalInput(team2.TeamName);

                        // Create and store the match
                        Match match = new Match(team1, team2);
                        match.UpdateMatchResults(goalsTeam1, goalsTeam2);
                        Matches.Add(match);

                        // Print result
                        if (goalsTeam1 > goalsTeam2)
                        {
                            Console.WriteLine($"{team1.TeamName} wins!");
                        }
                        else if (goalsTeam2 > goalsTeam1)
                        {
                            Console.WriteLine($"{team2.TeamName} wins!");
                        }
                        else
                        {
                            Console.WriteLine("It's a draw. No team wins!");
                        }
                    }
                // After every "match day" (all teams have at least 1 game)
                if (AllTeamsPlayedSameNumberOfGames(teams))
                {
                    Console.WriteLine("\\n=== Standings after the first matches ===");
                    PrintStandings(teams);
                }
            }

            // Final standings
            Console.WriteLine("\\n=== Final Standings ===");
            PrintStandings(teams);

            // Sort teams: first by wins, then by goal difference
            List<Team> sortedTeams = new List<Team>(teams);
            sortedTeams.Sort((a, b) =>
            {
                int winCompare = b.GamesWon.CompareTo(a.GamesWon);
                if (winCompare != 0) return winCompare;
                return b.GoalDifference.CompareTo(a.GoalDifference);
            });

            // Select the top X teams for KO round
            List<Team> koTeams = sortedTeams.GetRange(0, KOContestants);

            Console.WriteLine($"\\n=== Teams advancing to KO Round ({KOContestants}) ===");
            foreach (var team in koTeams)
            {
                Console.WriteLine($"{team.TeamName} | Wins: {team.GamesWon}, Goal Difference: {team.GoalDifference}");
            }

            // Countdown before KO round
            for (int i = 5; i > 0; i--)
            {
                Console.Write($"\\rStarting KO Round in {i} seconds... ");
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
    }

}
