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

        public int TournamentStartSelection();

        public int SettingsCreatedSelection();

        public int SavedTournamentsSelection(IEnumerable<string> loadableFiles);

        public int FinishedTournamentsSelection(IEnumerable<string> availableFiles);

        public int LoadableTournamentSettingsSelection(IEnumerable<string> availableFiles);

        public int DuringTournamentMenuSelection();

        public int ExitOptionsSelection();


        //Methods which simply show things

        public void ShowInvalidInputMessage();

        public void ShowExitMessage();

        public void ShowGamePlan();

        public void ShowStandings(Tournament tournament);

        public void ShowNextMatches(List<Match> matches);

        public void ShowMessage(string message);

        public string ReadInput();

        public void WriteEmptyLine();

        public void ClearCurrentConsoleLine();

        public void ShowTeamsAndPlayer(List<Team> teams);

        public void AnimateAndConfirmSave(string filePath);

        public void DisplayFiles(string[] currentFiles);

        public void WaitForAnyKeyToProceed();

    }
}
