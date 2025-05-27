using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Controller
{
    public static class GameLogicMatch
    {
        public static Match CreateMatch(Team team1, Team team2, int timer)
        {
            return new Match(team1, team2);
        }
                private static DateTime _endTime;
        public static void StartMatchTimer(InputHandler inputHandler, int length = 10)
        {
            // Set the end time for the specified length in minutes
            length = inputHandler.GetNumber("How long should a match be? (In minutes)");
            _endTime = DateTime.Now.AddMinutes(length);

            // Create a timer with a 1 second interval
            Timer timer = new Timer(1000);
            timer.Elapsed += OnTimedEvent;

            // Start the timer
            timer.Start();
            inputHandler.View.ShowMessage($"Timer started for {length} minutes.");
        }

        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            TimeSpan timeRemaining = _endTime - DateTime.Now;

            if (timeRemaining.TotalSeconds <= 0)
            {
                Console.WriteLine("Time's up!");
                Timer timer = (Timer)source;
                timer.Stop();
            }
            else
            {
                Console.WriteLine($"Time remaining: {timeRemaining:mm\\:ss}");
            }
    }
}
