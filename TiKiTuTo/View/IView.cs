using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiKiTuTo.Model.DataObjects;

namespace TiKiTuTo.View
{
    public interface IView
    {
        //Methods which directly return user input 
        public int MainMenuSelection();

        public int StartTournamentMenuSelection();

        public int SavedTournamentsSelection(IEnumerable<string> loadableFiles);

        //Methods which simply show things
        public void ShowMainMenu();

        public void ShowStartTournamentMenu();

        public int ShowIngameMenu();

        public void ShowInvalidInputMessage();

        public void ShowExitMessage();

        public int ShowExitOptions();

        public void ShowGamePlan();

        public void ShowStandings();

        public void ShowNextMatches(List<Match> matches);

        public void ShowMessage(string message);

        public string ReadInput();

        public void WriteEmptyLine();

        public void ClearCurrentConsoleLine();

        public void ShowTeamsAndPlayer(List<Team> teams);

        public string PromptSelection(string title, IEnumerable<string> options);

        public string ShowSpectreMenu();

        
    }
}
