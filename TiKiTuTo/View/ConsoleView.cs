using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace View 
{
    public class ConsoleView : IView
    {




        public ConsoleView() 
        {
            Console.CursorVisible = false;
        }


        public void ShowMenu()
        {
                //Console.Clear();
                Console.WriteLine(" -----------------------");
                Console.WriteLine(" |     TiKiTuTo        | ");
                Console.WriteLine(" -----------------------");
                Console.WriteLine("1. New Tournament");
                Console.WriteLine("2. Show old results");
                Console.WriteLine("3. Load settings");
                Console.WriteLine("4. Continue game");
                Console.WriteLine("5. Exit");
                
                WriteEmptyLine();
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
