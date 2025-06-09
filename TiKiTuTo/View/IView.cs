using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;
using TiKiTuTo.Model.DataObjects;

namespace TiKiTuTo.View
{
    public interface IView
    {
        //Methods which directly return user input 

        public void CreateFrame();

        public void ShowTikiTutoHeader();

        public int MainMenuSelection();

        public int StartTournamentMenuSelection();

        public int TournamentStartSelection();

        public int AvailableTournamentSelection(string[] loadableFiles, Enum SelectLoadingType);

        public int SettingsCreatedSelection();

        public int DuringTournamentMenuSelection();

        public int ExitOptionsSelection();


        //Methods which simply show things

        public void ShowInvalidInputMessage();

        public void ShowTeamsAndPlayer(List<Team> teams);

        public void ShowStandings(Tournament tournament);
        
        public void ShowExitMessage();

        public void ShowLoadingAnimation(string message);

        public void ShowMessage(string message);

        public void WriteEmptyLine();

        public string ReadInput();

        public void ClearCurrentConsoleLine();

        public void DisplayFiles(string[] currentFiles);

        public void AnimateAndConfirmSave(string filePath);

        public void WaitForAnyKeyToProceed();

        public void ShowFooter();

    }
}
