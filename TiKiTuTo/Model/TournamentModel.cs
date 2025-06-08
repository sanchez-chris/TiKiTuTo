using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiKiTuTo.Model.DataObjects;

namespace TiKiTuTo.Model
{
    public class TournamentModel
    {
        public Tournament Tournament { get; set; }
       
        public TournamentModel()
        {
            // Initialisiere Tournament und dessen Settings
            Tournament = new Tournament
            {
                TournamentSettings = new TournamentSettings()
            };
        }
    }

}
