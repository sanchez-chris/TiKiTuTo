using System;
using System.Collections.Generic;
using System.Linq;
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
        }

        /// <summary>
        /// Shows Main Menu using standard console
        /// </summary>
        public void ShowMainMenu() { }
        //{
        //    Console.Clear();
        //    Console.WriteLine(" -----------------------");
        //    Console.WriteLine(" |     TiKiTuTo        | ");
        //    Console.WriteLine(" -----------------------");
        //    Console.WriteLine(" -------Main Menu-------");
        //    Console.WriteLine(" -----------------------");
        //    Console.WriteLine("1. Start New Tournament");
        //    Console.WriteLine("2. Resume Earlier Tournament");
        //    Console.WriteLine("3. Show Results Of Earlier Tournament");
        //    Console.WriteLine("4. Manage Tournament Configurations");
        //    Console.WriteLine("5. Exit Application");

        //    WriteEmptyLine();
        //}

        public int MainMenuSelection()
        {
            
            List<string> headerLines = new()
            {
                " -----------------------",
                " |     TiKiTuTo        |",
                " -----------------------",
                " -------Main Menu-------",
                " -----------------------"
            }; 

            List<string> options = new()
            {
            "Start New Tournament",
            "Resume Earlier Tournament",
            "Show Results Of Earlier Tournament",
            "Manage Tournament Configurations",
            "Exit Application"
            };

            int userChoice = PromptSelectionMulti(headerLines, options);

            return userChoice;
        }


        public void ShowStartTournamentMenu() { }
        //{
        //    Console.Clear();
        //    Console.WriteLine(" ----------------------");
        //    Console.WriteLine(" |     TiKiTuTo        | ");
        //    Console.WriteLine(" ----------------------");
        //    Console.WriteLine(" ---Start Tournament---");
        //    Console.WriteLine(" ----------------------");
        //    Console.WriteLine("1. Start tournament from scratch");
        //    Console.WriteLine("2. Start tournament based on existing tournament settings");
        //    Console.WriteLine("3. Back to Main Menu");
        //    WriteEmptyLine();
        //}


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

            int userChoice = PromptSelectionMulti(headerLines, options);

            return userChoice;

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

            List<string> options = loadableFiles.ToList();
            options.Add("Back to Main Menu");


            int userChoice = PromptSelectionMulti(headerLines, options);

            return userChoice;

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

        public void ShowStandings() 
        {
            // TODO: Implement functionality for showing the current standings
        }
        public void ShowNextMatches(List<Match> matches)
        {
            // TODO: Implement functionality for showing the next match
        }


        public void ShowMessage(string message)
        {
            Console.WriteLine(message);
            /*Alternatives
             * AnsiConsole.WriteLine(message) 
             * AnsiConsole.Markup($"[bold]{message}[/]");*/
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


        /*reusable promptSelection function recieving an IEnumerable<string> (so it doesnt matter if the 
         * argument is type list<string>, string[] ...)
         * Please note, that PageSize only determines how many options are visible on the screen at one time.
         * Additional options may be available through scrolling.*/
        public string PromptSelection(string headline, IEnumerable<string> options)
        {
            var userChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title($"[yellow]{headline}[/]")
                    .PageSize(5)
                    .AddChoices(options));

            return userChoice;
        }




        public int PromptSelectionMulti(IEnumerable<string> headerLines, IEnumerable<string> options)
        {
            AnsiConsole.Clear();
            
            foreach (string line in headerLines)
            {
                AnsiConsole.MarkupLine($"[yellow]{line}[/]");
            }
            var userChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Please select an option:[/]")
                    .PageSize(5)
                    .AddChoices(options));
            
            return options.ToList().IndexOf(userChoice) + 1;
        }



        //hardcoded ShowMenu functionality in case we do not use PromptSelection
        public string ShowSpectreMenu()
        {
            var userChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Bitte wähle eine Option aus:[/]")
                    .PageSize(4)
                    .AddChoices(
                        "   1: New Tournament",
                        "   2: Show old results",
                        "   3: Load Settings",
                        "   4: Continue game",
                        "   5: Exit"));
            return userChoice;
        }


    }
}
