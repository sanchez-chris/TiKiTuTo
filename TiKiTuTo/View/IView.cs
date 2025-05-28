using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace View
{
    public interface IView
    {
        public void ShowMenu();

        public void ShowGamePlan();

        public void ShowStandings();

        public void ShowNextMatches(List<Match> matches);

        public void ShowMessage(string message);

        public string ReadInput();

        public void WriteEmptyLine();

        public void ClearCurrentConsoleLine();
        

    }
}
