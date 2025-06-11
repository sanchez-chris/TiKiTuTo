
namespace TiKiTuTo.Model.DataObjects
{
    public class Team
    {
        public string TeamName { get; set; }
        public List<Player> PlayerInTeam { get; set; }
        public int NumberGamesWon { get; set; } = 0;
        public int NumberGoals { get; set; } = 0;
        public int GoalDifference { get; set; } = 0;

        /// <summary>
        /// Constructor for the Team class. Requires a team name.
        /// </summary>
        /// <param name="name"></param>
        public Team(string name)
        {
            TeamName = name;
            PlayerInTeam = [];
        }
        public Team() { }
    }
}
