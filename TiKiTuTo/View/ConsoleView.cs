using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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


        public void ShowMainMenu()
        {
            Console.Clear();
            Console.WriteLine(" -----------------------");
            Console.WriteLine(" |     TiKiTuTo        | ");
            Console.WriteLine(" -----------------------");
            Console.WriteLine(" -------Main Menu-------");
            Console.WriteLine(" -----------------------");
            Console.WriteLine("1. Start New Tournament");
            Console.WriteLine("2. Resume Earlier Tournament");
            Console.WriteLine("3. Show Results Of Earlier Tournament");
            Console.WriteLine("4. Manage Tournament Configurations");
            Console.WriteLine("5. Exit Application");

            WriteEmptyLine();
        }

        public void ShowStartTournamentMenu()
        {
            Console.Clear();
            Console.WriteLine(" ----------------------");
            Console.WriteLine(" |     TiKiTuTo        | ");
            Console.WriteLine(" ----------------------");
            Console.WriteLine(" ---Start Tournament---");
            Console.WriteLine(" ----------------------");
            Console.WriteLine("1. Start tournament from scratch");
            Console.WriteLine("2. Start tournament based on existing tournament settings");
            Console.WriteLine("3. Back to Main Menu");
            WriteEmptyLine();
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
    }
}
