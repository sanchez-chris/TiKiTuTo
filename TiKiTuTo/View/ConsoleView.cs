using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.Intrinsics.X86;
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

        public void ShowTikiTutoHeader()
        {
            AnsiConsole.Clear();


            AnsiConsole.Write(
                new Panel(
                    Align.Center(
                        new FigletText("TikiTuto")
                            .Color(new Color(201, 245, 5))))
                .Border(BoxBorder.Double)
                .BorderColor(new Color(0, 150, 199))
                .Padding(1, 1)
                .Header("[rgb(0,150,199)]Tournament Manager[/]")
                .HeaderAlignment(Justify.Center));

            AnsiConsole.WriteLine();
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
            "Import tournament settings from excel import file",
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



        public int AvailableTournamentSelection(string[] availableFiles, Enum SelectLoadingType)
        {
            List<string> headerLines = new()
            {
                "-----------------------",
                "|     TiKiTuTo        |",
                "-----------------------",
                "---Saved Tournaments---",
                "-----------------------"
            };

            Array.Reverse(availableFiles);

            List<string> options = new List<string>();

            for (int i = 0; i < availableFiles.Length; i++)
            {
                string fileName = Path.GetFileName(availableFiles[i]);
                options.Add(fileName);
            }

            options.Add("Back to Main Menu");

            int userChoice = PromptSelectionMultiLine(headerLines, options, SelectLoadingType);

            return userChoice;
        }

        public int AvailableExcelFiles()
        {
            AnsiConsole.MarkupLine("[bold blue]Excel File Import Instructions[/]");
            AnsiConsole.MarkupLine("[blue]=====================================[/]");

            // Display the introduction
            AnsiConsole.MarkupLine("[bold green]Next Step:[/]");
            AnsiConsole.WriteLine("You will be taken to the Excel Import folder of the program.");
            AnsiConsole.WriteLine("In this folder, you will find an Excel file that provides the required format for entering your data.\n");

            // Display the steps
            AnsiConsole.MarkupLine("[bold yellow]Steps to Import:[/]");
            AnsiConsole.MarkupLine("[bold]1.[/] [green]Enter Your Data:[/] Open the Excel file and input your data according to the format provided.");
            AnsiConsole.MarkupLine("[bold]2.[/] [green]Save the File:[/] Make sure to save the file after entering your data.");
            AnsiConsole.MarkupLine("[bold]3.[/] [green]Alternative Option:[/] You can use your own Excel file, but it must be named 'Settings.xlsx'.");
            AnsiConsole.MarkupLine("[bold]4.[/] [green]Confirm Import:[/] After saving the Excel file, return to the console and confirm that you have completed the process. Once confirmed, the Excel file will be imported automatically.\n");

            // Display the important notes
            AnsiConsole.MarkupLine("[bold red]Important:[/]");
            AnsiConsole.WriteLine("- Follow the instructions in the file carefully.");
            AnsiConsole.WriteLine("- Ensure that you adhere to the specified formatting.\n");
            return 1;
            }

        public void ConfirmingImportAction()
        {
            AnsiConsole.WriteLine("When you have ensured the file is saved and all instructions have been followed, press random key to continue");
            Console.ReadLine();
            Console.Clear();
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

        //currently unused
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
            WriteEmptyLine();
            WaitForAnyKeyToProceed();
        }


        //currently unused
        public void ShowNextMatches(List<Match> matches)
        {
            // TODO: Implement functionality for showing the next match
        }


        public void ShowMessage(string message)
        {
            AnsiConsole.WriteLine(message);
        }

        public void WriteEmptyLine()
        {
            AnsiConsole.WriteLine("");
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


        //currently unused
        public void DisplayFiles(string[] currentFiles)
        {
            for (int i = 0; i < currentFiles.Length; i++)
            {
                string fileName = Path.GetFileName(currentFiles[i]);
                ShowMessage($"{i}: {fileName}");
            }
        }

        /// <summary>
        /// This wrapper for AnsiConsole.Prompt presents a multiline header followed by a SelectionPrompt. 
        /// Choosing one of the selectable options returns the natural index [1-based] of the chosen option.
        /// </summary>
        /// <param name="headerLines"> The lines making up the header</param>
        /// <param name="options"> selectable options</param>
        /// <returns>The index of the chosen option.</returns>
        public int PromptSelectionMultiLine(IEnumerable<string> headerLines, IEnumerable<string> options, Enum? SelectedLoadingType = null)
        {
            AnsiConsole.Clear();

            foreach (string line in headerLines)
            {
                AnsiConsole.MarkupLine($"[yellow]{line}[/]");
            }

            string title;
            switch (SelectedLoadingType)
            {
                case SelectLoadingType.UnfinishedTournament:
                    title = "[yellow]Saved Tournaments[/]";
                    break;
                case SelectLoadingType.FinishedTournament:
                    title = "[yellow]Finished Tournaments[/]";
                    break;
                case SelectLoadingType.TournamentSettings:
                    title = "[yellow]Tournament Settings[/]";
                    break;
                default:
                    title = $"[yellow]Please select an option[/]";
                    break;
            }

            if (!(options.Count() > 1))
            {
                title += $": [red]No options available[/]";
            }

            var userChoice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title(title)
            .PageSize(10)
            .MoreChoicesText($"[grey](Use arrow keys to navigate and press Enter to select)[/]")
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

            ShowMessage($"Tournament has been saved: {Path.GetFileName(filePath)}");
            WriteEmptyLine();
        }

        public void WaitForAnyKeyToProceed()
        {
            ShowMessage("Press any key to continue.");
            Console.ReadKey();
        }




    }
}
