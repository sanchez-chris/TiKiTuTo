using System;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;
// using Teams;

namespace Rounds
{
    public class Rounds
    {

        public void PreliminaryRound()
        {
            // team are generated: 
            // var Teams = new Teams.Teams();

            // Teams.generate();
            var team1 = new
            {
                TeamName = "The Champions",
                CountGamesWon = 0,
                CountGoalsScored = 0,
                ID = 1
            };

            var team2 = new
            {
                TeamName = "The Warriors",
                CountGamesWon = 0,
                CountGoalsScored = 0,
                ID = 2
            };

            var team3 = new
            {
                TeamName = "The Legends",
                CountGamesWon = 0,
                CountGoalsScored = 0,
                ID = 3
            };

            var team4 = new
            {
                TeamName = "The Titans",
                CountGamesWon = 0,
                CountGoalsScored = 0,
                ID = 4
            };

            var team5 = new
            {
                TeamName = "The Invincibles",
                CountGamesWon = 0,
                CountGoalsScored = 0,
                ID = 5
            };

            var team6 = new
            {
                TeamName = "The Gladiators",
                CountGamesWon = 0,
                CountGoalsScored = 0,
                ID = 6
            };

            var teams = new List<dynamic>
            {
                team1,
                team2,
                team3,
                team4,
            };

            Console.Clear();
            // Match each team against every other team once
            for (int i = 0; i < teams.Count; i++)
            {

                for (int j = 0; j < teams.Count; j++)
                {
                    if(i == j)
                    {
                        continue; // Skip if the same team
                    }
                    Console.WriteLine($"Match: {teams[i].TeamName} vs {teams[j].TeamName}");

                    string inputGoalsScored1;
                    do
                    {
                        // Get goals scored by each team
                        Console.Write($"Enter goals scored by {teams[i].TeamName}: ");
                        inputGoalsScored1 = Console.ReadLine();

                        if (!Regex.IsMatch(inputGoalsScored1, @"^-?\d+$")) // Check if the input is NOT a valid integer
                        {
                            Console.WriteLine("Invalid input. Please enter a valid integer.");
                        }

                    } while (!Regex.IsMatch(inputGoalsScored1, @"^-?\d+$"));
                    int goalsTeam1 = int.Parse(inputGoalsScored1);


                    string inputGoalsScored2;
                    do
                    {
                        // Get goals scored by each team
                        Console.Write($"Enter goals scored by {teams[j].TeamName}: ");
                        inputGoalsScored2 = Console.ReadLine();

                        if (!Regex.IsMatch(inputGoalsScored2, @"^-?\d+$")) // Check if the input is NOT a valid integer
                        {
                            Console.WriteLine("Invalid input. Please enter a valid integer.");
                        }

                    } while (!Regex.IsMatch(inputGoalsScored2, @"^-?\d+$"));
                    int goalsTeam2 = int.Parse(inputGoalsScored2);

                    // Update goals scored
                    teams[i] = new
                    {
                        TeamName = teams[i].TeamName,
                        CountGamesWon = teams[i].CountGamesWon,
                        CountGoalsScored = teams[i].CountGoalsScored + goalsTeam1,
                        ID = teams[i].ID
                    };
                    teams[i] = new
                    {
                        TeamName = teams[i].TeamName,
                        CountGamesWon = teams[i].CountGamesWon,
                        CountGoalsScored = teams[i].CountGoalsScored + goalsTeam2,
                        ID = teams[i].ID
                    };
                    // Determine winner and update games won
                    if (goalsTeam1 > goalsTeam2)
                    {
                        teams[i] = new
                        {
                            TeamName = teams[i].TeamName,
                            CountGamesWon = teams[i].CountGamesWon + 1,
                            CountGoalsScored = teams[i].CountGoalsScored,
                            ID = teams[i].ID
                        };
                        Console.WriteLine($"{teams[i].TeamName} wins!\n");
                    }
                    else if (goalsTeam2 > goalsTeam1)
                    {
                        teams[i] = new
                        {
                            TeamName = teams[i].TeamName,
                            CountGamesWon = teams[i].CountGamesWon + 1,
                            CountGoalsScored = teams[i].CountGoalsScored,
                            ID = teams[i].ID
                        }; Console.WriteLine($"{teams[j].TeamName} wins!\n");
                    }
                    else
                    {
                        Console.WriteLine("It's a draw, 0 points for both teams!\n");
                    }
                }
            }

            // Display final results
            Console.WriteLine("Final Results:");
            foreach (var team in teams)
            {
                Console.WriteLine($"{team.TeamName}: Games Won = {team.CountGamesWon}, Goals Scored = {team.CountGoalsScored}");
            }

            for (int i = 10; i > 0; i--)
            {
                Console.WriteLine($"Starting the KO Round in {i} seconds...");
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                Thread.Sleep(1000000);

            }
            

        }

        public void KORound()
        {
            Console.Clear();
            Console.WriteLine("start the KO Round");
            Thread.Sleep(50000);


        }
    }

}
