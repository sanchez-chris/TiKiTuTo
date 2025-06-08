using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;
using TiKiTuTo.Controller;
using TiKiTuTo.Model.DataObjects;

namespace TiKiTuTo.View
{
    public class ConsoleView : IView
    {


        public ConsoleView() 
        {
            Console.CursorVisible = false;
            AnsiConsole.Cursor.Hide();
        }

        public int MainMenuSelection()
        {
            
            List<string> headerLines = new()
            {
                " -----------------------",
                " |     TiKiTuTo        |",
                " -----------------------",
                " -------Main Menu-------",
                " -----------------------",
                
            };

            List<string> options = new()
            {
            "Start New Tournament",
            "Resume Earlier Tournament",
            "Show Results Of Earlier Tournament",
            "Create Tournament Configurations",
            "Exit Application"
            };

            int userChoice = PromptSelectionMultiLine(headerLines, options);

            return userChoice;
        }

        public int StartTournamentMenuSelection()
        {
            List<string> headerLines = new()
            {
                " -----------------------",
                " |     TiKiTuTo        |",
                " -----------------------",
                " ---Start Tournament----",
                " -----------------------"
            };

            List<string> options = new()
            {
            "Start tournament from scratch",
            "Start tournament based on existing tournament settings",
            "Back to Main Menu",
            };

            int userChoice = PromptSelectionMultiLine(headerLines, options);

            return userChoice;

        }

        public int TournamentStartSelection()
        {
            {
                List<string> headerLines = new()
            {
                " -----------------------",
                " |      TiKiTuTo       |",
                " -----------------------",
                " --Start now or later?--",
                " -----------------------"
            };

                List<string> options = new()
            {
            "Now",
            "Later (Return to Menu)",
            };

                int userChoice = PromptSelectionMultiLine(headerLines, options);

                return userChoice;

            }
        }



        public int SavedTournamentsSelection(IEnumerable<string> loadableFiles)
        {
            List<string> headerLines = new()
    {
        "-----------------------",
        "|     TiKiTuTo        |",
        "-----------------------",
        "---Saved Tournaments---",
        "-----------------------"
    };

            string[] formattedFileNames = new string[loadableFiles.Length];
            for (int i = 0; i < loadableFiles.Length; i++)
            {
                string fileName = Path.GetFileName(loadableFiles[i]);
                formattedFileNames[i] = fileName;
            }

            var sortedFileNames = formattedFileNames
                .Where(fileName =>
                {
                    string[] parts = fileName.Split('_');
                    return parts.Length > 1 && parts[^1].EndsWith(".json");
                })
                .OrderBy(fileName =>
                {
                    string[] parts = fileName.Split('_');
                    return parts[^1]; 
                })
                .ToList();

            sortedFileNames.Add("Back to Main Menu");

            int userChoice = PromptSelectionMultiLine(headerLines, options);

            return userChoice;
        }

        public int FinishedTournamentsSelection(IEnumerable<string> availableFiles)
        {
            List<string> headerLines = new()
            {
                "-----------------------",
                "|     TiKiTuTo        |",
                "-----------------------",
                "--Finished Tournaments-",
                "-----------------------"
            };

            List<string> options = availableFiles.ToList();
            options.Add("Back to Main Menu");


            int userChoice = PromptSelectionMultiLine(headerLines, options);

            return userChoice;

        }

        public int SettingsCreatedSelection()
        {
            {
                List<string> headerLines = new()
            {
                " -----------------------",
                " |      TiKiTuTo       |",
                " -----------------------",
                "----Settings created----",
                "----Create another------",
                "----or return to Menu?--",
                " -----------------------"
            };

                List<string> options = new()
            {
            "Create another setting",
            "Return to main menu",
            };

                int userChoice = PromptSelectionMultiLine(headerLines, options);

                return userChoice;

            }
        }

        public int LoadableTournamentSettingsSelection(IEnumerable<string> availableFiles)
        {
            List<string> headerLines = new()
            {
                "-----------------------",
                "|     TiKiTuTo        |",
                "-----------------------",
                "--Tournament Settings--",
                "-----------------------"
            };

            List<string> options = availableFiles.ToList();
            options.Add("Back to Main Menu");


            int userChoice = PromptSelectionMultiLine(headerLines, options);

            return userChoice;

        }

        public int DuringTournamentMenuSelection()
        {
            {
                List<string> headerLines = new()
            {
                " -----------------------",
                " |     TiKiTuTo        |",
                " -----------------------",
                " ------Mini Menu--------",
                " -----------------------"
            };

                List<string> options = new()
            {
            "Continue Tournament",
            "Back to Main Menu",
            "Exit",
            };

                int userChoice = PromptSelectionMultiLine(headerLines, options);

                return userChoice;

            }
        }

        public int ExitOptionsSelection()
        {
            {
                List<string> headerLines = new()
            {
                " -----------------------",
                " |      TiKiTuTo       |",
                " -----------------------",
                " ----Exit to desktop?---",
                " -----------------------"
            };

                List<string> options = new()
            {
            "Yes",
            "No",
            };

                int userChoice = PromptSelectionMultiLine(headerLines, options);

                return userChoice;

            }
        }



        public void ShowInvalidInputMessage()
        {
            Console.WriteLine("Invalid input. Please enter a valid number.");
        }

        public void ShowExitMessage()
        {
            Console.WriteLine("Exiting application...");
        }


        public void ShowTeamsAndPlayer(List<Team> teams)
        {
            foreach (Team team in teams)
            {
                ShowMessage(team.TeamName);
                foreach (Player player in team.PlayerInTeam)
                {
                    ShowMessage(player.Name);
                }
                WriteEmptyLine();
            }
        }


        public void ShowGamePlan()
        {
            // TODO: Implement functionality for showing a game plan
        }

        public void ShowStandings(Tournament tournament) 
        {
            List<Team> Teams = tournament.TournamentSettings.TeamsInTournament;

            List<Team> sortedTeams = Teams

                     .OrderByDescending(t => t.NumberGamesWon)

                     .ThenByDescending(t => t.Goaldifference)

                     .ThenByDescending(t => t.NumberGoals)

                     .ToList();

            foreach (var team in sortedTeams)

            {

                ShowMessage($"{team.TeamName} - Games won: {team.NumberGamesWon} - Goals difference: {team.Goaldifference} - Goals scored: {team.NumberGoals} - Goals received: {team.NumberGoals - team.Goaldifference}");

            }

            Console.ReadKey();
        }



        public void ShowNextMatches(List<Match> matches)
        {
            // TODO: Implement functionality for showing the next match
        }


        public void ShowMessage(string message)
        {
            //AnsiConsole.Markup($"[bold]{message}[/]");
            AnsiConsole.WriteLine(message);
        }

        public void WriteEmptyLine()
        {
            Console.WriteLine("");
        }

        public string ReadInput()
        {
            return Console.ReadLine();
        }

        /// <summary>
        /// This method deletes the last line written. Can be used to have a regularly updated display
        /// without cluttering the Console (e.g. for running timer)
        /// </summary>
        public void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, currentLineCursor - 1);
            Console.WriteLine(new string(' ', Console.BufferWidth));
            Console.SetCursorPosition(0, currentLineCursor - 1);
        }


        public void DisplayFiles(string[] currentFiles)
        {
            for (int i = 0; i < currentFiles.Length; i++)
            {
                string fileName = Path.GetFileName(currentFiles[i]);
                ShowMessage($"{i}: {fileName}");
            }
        }


        public int PromptSelectionMultiLine(IEnumerable<string> headerLines, IEnumerable<string> options)
        {
            AnsiConsole.Clear();
            
            foreach (string line in headerLines)
            {
                AnsiConsole.MarkupLine($"[yellow]{line}[/]");
            }

            string title = $"[yellow]Please select an option:[/]";

            if (!(options.Count() > 1))
            {
                title = $"[red]No options available[/]";   
            }

                var userChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title(title)
                    .PageSize(5)
                    .AddChoices(options));

            return options.ToList().IndexOf(userChoice) + 1;
        }


        public void AnimateAndConfirmSave(string filePath)
        {
            AnsiConsole.Progress()
            .Start(ctx =>
            {
                var task = ctx.AddTask("[green]Saving Tournament...[/]");

                while (!task.IsFinished)
                {
                    task.Increment(10); // Increment progress by 10%
                    Task.Delay(80).Wait(); // Wait for 80ms
                }
            });

            ShowMessage($"Tournament has been saved: {filePath}");
            WriteEmptyLine();
        }

        


    }
}
