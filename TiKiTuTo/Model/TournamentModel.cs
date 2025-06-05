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
        public TournamentSettings TournamentSettings { get; set; }

        public TournamentModel() { }
    }

}
