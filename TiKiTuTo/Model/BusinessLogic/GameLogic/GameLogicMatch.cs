using System.Media;
using TiKiTuTo.Controller;
using TiKiTuTo.Model.DataObjects;
using Timer = System.Timers.Timer;

namespace TiKiTuTo.Model.BusinessLogic.GameLogic
{
    public class GameLogicMatch
    {
        //GameLogicRound GameLogicRound { get; set; }
        InputHandler InputHandler { get; set; }
        JSONService JSONService { get; set; }
        bool isTimerFinished = false;
        public GameLogicMatch(InputHandler inputHandler, JSONService json)
        {
            //GameLogicRound = new GameLogicRound(inputHandler, json);
            InputHandler = inputHandler;
            JSONService = json;
        }

    /*    public void RunMatch(Match match)
        {
            int goalsA = 0;
            int goalsB = 0;

            StartMatchTimer(match);
            bool flag = true;
            while (flag)
            {
                goalsA = InputHandler.GetValidGoalInput(match.teamA.TeamName);
                match.goalsTeamA = goalsA;

                goalsB = InputHandler.GetValidGoalInput(match.teamB.TeamName);
                match.goalsTeamB = goalsB;
                if (goalsA==goalsB)
                {
                    InputHandler.View.ShowMessage("You can't have a draw.");
                    continue;
                }
                InputHandler.View.ShowMessage($"Match finished! {match.teamA.TeamName} {goalsA} - {goalsB} {match.teamB.TeamName}");
                FinishMatch(match);
                flag = false;
            }
        }*/

        public void RunMatch(Match match)
        {
            
            int goalsA = 0;
            int goalsB = 0;
            InputHandler.View.ShowMessage($"\n\nMatch: {match.teamA.TeamName} vs {match.teamB.TeamName}:");
            StartMatchTimer(match);
            bool goalsAsked = false;
            isTimerFinished = false;

            while (!goalsAsked)
            {
                if (isTimerFinished)
                {
                    goalsA = InputHandler.GetNumber($"\nHow many goals has {match.teamA.TeamName}?");
                    match.goalsTeamA = goalsA;

                    goalsB = InputHandler.GetNumber($"How many goals has {match.teamB.TeamName}?");
                    match.goalsTeamB = goalsB;
                    if (goalsA == goalsB)
                    {
                        InputHandler.View.ShowMessage("You can't have a draw.");
                        continue;
                    }
                    goalsAsked = true;
                }
            }
            InputHandler.View.ShowMessage($"Match finished! {match.teamA.TeamName} {goalsA} - {goalsB} {match.teamB.TeamName}");
            FinishMatch(match);
        }

        private DateTime _endTime;
        public void StartMatchTimer(Match match, double? duration = 0.05) //duration has to be 10 for production
        {
            // Set the end time for the specified length in minutes
            while (duration == 0)
            {
                duration = InputHandler.GetNumber("Please enter the match duration in full minutes");
            }
            _endTime = DateTime.Now.AddMinutes((double)duration);

            // Create a timer with a 1 second interval
            Timer timer = new Timer(1000);

            //lambda method to allow for usage of inputHandler.View.
            timer.Elapsed += (sender, e) =>
            {
                TimeSpan timeRemaining = _endTime - DateTime.Now;

                if (timeRemaining.TotalSeconds <= 0)
                {
                    InputHandler.View.ShowMessage("Time's up!");
                    SystemSounds.Asterisk.Play();
                    isTimerFinished = true;
                    //FinishMatch(match);
                    timer.Stop();
                }
                else
                {
                    InputHandler.View.ClearCurrentConsoleLine();
                    InputHandler.View.ShowMessage($"Time remaining: {timeRemaining:mm\\:ss}");
                }
            };

            // Start the timer
            InputHandler.View.ShowMessage($"Timer started for {duration} minutes.");
            InputHandler.View.WriteEmptyLine();
            timer.Start();
        }

        public void UpdateTeamScores(Team teamA, int goalsA, Team teamB, int goalsB)
        {
            if (goalsA > goalsB)
            {
                teamA.NumberGamesWon++;
                InputHandler.View.ShowMessage($"{teamA.TeamName} wins");

            }
            if (goalsB > goalsA)
            {
                teamB.NumberGamesWon++;
                InputHandler.View.ShowMessage($"{teamB.TeamName} wins");

            }
            teamA.Goaldifference += goalsA - goalsB;
            teamA.NumberGoals += goalsA;
            teamB.Goaldifference += goalsB - goalsA;
            teamB.NumberGoals += goalsB;
        }



        public void FinishMatch(Match match)
        {
            match.isFinished = true;
            UpdateTeamScores(match.teamA, match.goalsTeamA, match.teamB, match.goalsTeamB);
            JSONService.SaveTournament();
        }
    }
}
