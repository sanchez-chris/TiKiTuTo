using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiKiTuTo.Model.DataObjects
{
    public class Round
    {
        public List<Match> Matches { get; set; } = new List<Match>();
        public bool IsFinished { get; set; } = false;


        public Round(List<Match> matches)
        {
            Matches = matches;
        }

        public Round() { }
    }
}
