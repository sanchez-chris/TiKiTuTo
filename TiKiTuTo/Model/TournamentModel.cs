using TiKiTuTo.Model.DataObjects;

namespace TiKiTuTo.Model
{
    public class TournamentModel
    {
        public Tournament Tournament { get; set; }
        
        //  Prevention of NullReferenceExceptions
        public TournamentModel()
        {
            Tournament = new Tournament
            {
                TournamentSettings = new TournamentSettings()
            };
        }
    }

}
