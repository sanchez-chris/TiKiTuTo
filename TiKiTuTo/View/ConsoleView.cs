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

        public void CreateFrame()
        {
            ShowTikiTutoHeader();
            ShowFooter();
        }

        public void ShowTikiTutoHeader()
        {
            AnsiConsole.Clear();

            AnsiConsole.Write(new Rule("[yellow]Welcome to TikiTuto[/]").RuleStyle("cyan").Centered());

            AnsiConsole.Write(
                new Panel(
                    Align.Center(
                        new FigletText("TikiTuto")
                            .Color(Color.LightSkyBlue1)))
                .Border(BoxBorder.Rounded)
                .BorderColor(Color.NavajoWhite1)
                .Padding(2, 2)
                .Header("[bold blue]DAS TischkickerTurniertool[/]")
                .HeaderAlignment(Justify.Center));

            AnsiConsole.WriteLine();
        }


        public int MainMenuSelection()
        {

            List<string> headerLines = new()
            {
                $"       [underline]Main Menu[/]",
                ""
            };

            List<string> options = new()
            {
            "Start New Tournament",
            "Resume Earlier Tournament",
            "Show Results Of Finished Tournaments",
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
               $"       [underline]Start Tournament[/]"
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
                " Do you wish to start the tournament [underline]now[/] or [underline]later?[/]",
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
                //$"   Saved Tournaments",
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
        }


        public int SettingsCreatedSelection()
        {
            {
                List<string> headerLines = new()

            {
                "Your settings have been saved.",
                "Create another configuration",
                "or return to the main menu?"
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
                $"           Mini Menu",
                $"  ___________________________" ,
                $" |             |             |",
                $" |___          |          ___|",
                $" |_  |         |         |  _|",
                $",| | |,       ,|,       ,| | |,",
                $"|| | | )     ( | )     ( | | ||",
                $"'|_| |'       '|'       '| |_|'",
                $" |___|         |         |___|",
                $" |             |             |",
                $" |_____________|_____________|",

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
                $"[bold red] Exit[/] to desktop?",
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
            AnsiConsole.MarkupLine("[maroon]Invalid input.[/] [italics]Please enter a valid number.[/]");
        }

        public void ShowExitMessage()
        {
            Console.WriteLine("Exiting application...");
            string[] ball = {
            "  .OOOO.  ",
            " .OOOOOO. ",
            ".OOOOOOOO ",
            " .OOOOOO. ",
            "  .OOOO.  "
        };

            int screenWidth = Console.WindowWidth;
            int ballWidth = ball[0].Length;

            // Loop to simulate ball rolling across the screen
            for (int i = 0; i < screenWidth - ballWidth; i++)
            {
                // Clear the console
                Console.Clear();

                // Print spaces to position the ball
                Console.WriteLine(new string(' ', i) + ball[0]);
                Console.WriteLine(new string(' ', i) + ball[1]);
                Console.WriteLine(new string(' ', i) + ball[2]);
                Console.WriteLine(new string(' ', i) + ball[3]);
                Console.WriteLine(new string(' ', i) + ball[4]);

                // Pause for animation effect
                Thread.Sleep(10);

            }
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

        public void ShowStandings(Tournament tournament)
        {
            var table = new Table();
            table.AddColumn(new TableColumn("[bold yellow4]Position[/]").Padding(1, 1).Alignment(Justify.Center));
            table.AddColumn(new TableColumn("[bold blue]Team Name[/]").Padding(2, 2).Alignment(Justify.Center));
            table.AddColumn(new TableColumn("[bold blue]Games Won[/]").Padding(1, 1).Alignment(Justify.Center));
            table.AddColumn(new TableColumn("[bold blue]Goal Difference[/]").Padding(1, 1).Alignment(Justify.Center));
            table.AddColumn(new TableColumn("[bold green]Goals Scored[/]").Padding(1, 1).Alignment(Justify.Center));
            table.AddColumn(new TableColumn("[bold red]Goals Received[/]").Padding(1, 1).Alignment(Justify.Center));
            table.Border(TableBorder.Rounded);
            table.BorderColor(Color.Wheat4);

            var sortedTeams = tournament.TournamentSettings.TeamsInTournament
                .OrderByDescending(t => t.NumberGamesWon)
                .ThenByDescending(t => t.Goaldifference)
                .ThenByDescending(t => t.NumberGoals)
                .ToList();
            int i = 1;
            foreach (var team in sortedTeams)
            {
                table.AddRow(
                    Convert.ToString(i),
                    team.TeamName,
                    team.NumberGamesWon.ToString(),
                    team.Goaldifference.ToString(),
                    team.NumberGoals.ToString(),
                    (team.NumberGoals - team.Goaldifference).ToString());
                i++;
            }
            AnsiConsole.Write(
                        new Panel("[bold yellow]Tournament Standings[/]")
                            .Border(BoxBorder.Rounded)
                            .BorderColor(Color.Blue)
                            );


            AnsiConsole.Write(table);
            WriteEmptyLine();
            WaitForAnyKeyToProceed();
        }

        public void ShowLoadingAnimation(string message)
        {
            AnsiConsole.Status()
                .Start(message, ctx =>
                {
                    Task.Delay(2000).Wait(); // Simulate work
                });
        }

        public void ShowMessage(string message)
        {
            if (message.Contains("Match"))
                AnsiConsole.MarkupLine($"[darkred on darkseagreen]{message}[/]");
            else if (message.Contains("Final"))
                AnsiConsole.MarkupLine($"[darkred on darkseagreen]{message}[/]");
            else AnsiConsole.MarkupLine(message);
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
            ShowTikiTutoHeader();
            WriteEmptyLine();
            foreach (string line in headerLines)
            {
                AnsiConsole.MarkupLine($"[bold]{line}[/]");
            }

            string title;
            switch (SelectedLoadingType)
            {
                case SelectLoadingType.UnfinishedTournament:
                    title = $"       [underline]Saved Tournaments[/]";
                    break;
                case SelectLoadingType.FinishedTournament:
                    title = $"       [underline]Finished Tournaments[/]";
                    break;
                case SelectLoadingType.TournamentSettings:
                    title = $"       [underline]Tournament Settings[/]";
                    break;
                default:
                    title = $"[italic lightskyblue3_1]Please select an option to continue.[/]";
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
        .MoreChoicesText("[italic grey](Use arrow keys to navigate and press Enter to select)[/]")
        .AddChoices(options)
        .UseConverter(option => option)
);

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
                    task.Increment(5); // Increment progress by 5%
                    Task.Delay(25).Wait(); // Wait for 25ms
                }
            });

            AnsiConsole.MarkupLine($"[bold green]Tournament has been saved:[/] [yellow]{Path.GetFileName(filePath)}[/]");
            WriteEmptyLine();
        }

        public void WaitForAnyKeyToProceed()
        {
            AnsiConsole.MarkupLine("Press [underline]any[/] key to continue.");
            Console.ReadKey();
        }

        public void ShowFooter()
        {
            // Calculate the position for the footer
            int footerPosition = Console.WindowHeight - 2;
            Console.SetCursorPosition(0, footerPosition);

            AnsiConsole.Write(new Rule("[italic grey]PlayTeach Solutions© 2025 TikiTuto[/]").Centered());
        }

        public void ShowKoTree(Tournament tournament)
        {
            var tree = new Tree("[bold yellow]Knockout Tournament[/]");

            // Loop through each round
            for (int round = 0; round < tournament.GamePlanKoRound.Count; round++)
            {
                string roundName = round == tournament.GamePlanKoRound.Count - 1
                    ? "Final"
                    : round == tournament.GamePlanKoRound.Count - 2
                        ? "Semifinals"
                        : $"Round {round + 1}";

                var roundNode = tree.AddNode($"[bold red]{roundName}[/]");

                // Add matches for the current round
                foreach (var match in tournament.GamePlanKoRound[round])
                {
                    roundNode.AddNode($"[green]{match.teamA.TeamName}[/] vs [green]{match.teamB.TeamName}[/]");
                }
            }

            // Display the tree
            AnsiConsole.Write(tree);
        }
    }
}
