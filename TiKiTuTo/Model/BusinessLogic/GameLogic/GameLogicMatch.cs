using TiKiTuTo.Controller;
using TiKiTuTo.Model.DataObjects;
using Timer = System.Timers.Timer;

namespace TiKiTuTo.Model.BusinessLogic.GameLogic
{
    public class GameLogicMatch
    {
        InputHandler InputHandler { get; }
        private JSONService JSONService { get; }

        TournamentModel TournamentModel { get; }

        private bool _isTimerFinished;
        public GameLogicMatch(InputHandler inputHandler, JSONService json, TournamentModel model)
        {
            InputHandler = inputHandler;
            JSONService = json;
            TournamentModel = model;
        }


        public void RunMatch(Match match)
        {
            
            int goalsA = 0;
            int goalsB = 0;
            bool isThereTimer = false;
            InputHandler.View.ShowMessage($"\n\nMatch: {match.TeamA.TeamName} vs {match.TeamB.TeamName}");

            if (TournamentModel.Tournament.TournamentSettings.UseTimer)
            {
                StartMatchTimer((double)TournamentModel.Tournament.TournamentSettings.MatchDuration);
                isThereTimer = true;
            }    
            
            bool goalsAsked = false;
            _isTimerFinished = false;

            while (!goalsAsked)
            {
                if (_isTimerFinished || !isThereTimer)
                {
                    goalsA = InputHandler.GetValidGoalInput(match.TeamA.TeamName);
                    match.GoalsTeamA = goalsA;
                    goalsB = InputHandler.GetValidGoalInput(match.TeamB.TeamName);
                    match.GoalsTeamB = goalsB; 

                    if (goalsA == goalsB)
                    {
                        InputHandler.View.ShowMessage("You can't have a draw.");
                        continue;
                    }
                    goalsAsked = true;
                }
            }
            InputHandler.View.ShowMessage($"Match finished! \nResult: {match.TeamA.TeamName}   {goalsA}:{goalsB}   {match.TeamB.TeamName}");
            FinishMatch(match);
        }

        private DateTime _endTime;

        private void StartMatchTimer(double? duration = 0.1) //duration has to be 10 for production
        {
            InputHandler.View.WaitForAnyKeyToProceed();

            // Set the end time for the specified length in minutes
            while (duration == 0)
            {
                duration = InputHandler.GetNumber("Please enter the match duration in full minutes");
            }
            _endTime = DateTime.Now.AddMinutes((double)duration);

            // Create a timer with a 1 second interval
            Timer timer = new Timer(1000);

            //lambda method to allow for usage of inputHandler.View.
            timer.Elapsed += (_, _) =>
            {
                TimeSpan timeRemaining = _endTime - DateTime.Now;

                if (timeRemaining.TotalSeconds <= 0)
                {
                    InputHandler.View.ShowMessage("Time's up!");
                    //SystemSounds.Exclamation.Play();

                    _isTimerFinished = true;
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
            if (duration == 1)
            {
                InputHandler.View.ShowMessage($"Timer started for {duration} minute.");

            }
            else
            {
                InputHandler.View.ShowMessage($"Timer started for {duration} minutes.");
            }
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
            teamA.GoalDifference += goalsA - goalsB;
            teamA.NumberGoals += goalsA;
            teamB.GoalDifference += goalsB - goalsA;
            teamB.NumberGoals += goalsB;
        }



        public void FinishMatch(Match match)
        {
            match.IsFinished = true;
            UpdateTeamScores(match.TeamA, match.GoalsTeamA, match.TeamB, match.GoalsTeamB);
            JSONService.SaveTournament();
        }
    }
}
