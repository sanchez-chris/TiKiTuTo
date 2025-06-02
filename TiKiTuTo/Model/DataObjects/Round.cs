using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiKiTuTo.Model.DataObjects
{
    public class Round
    {
        public enum RoundType
        {
            League,
            KO,
        }

        public RoundType TypeOf { get; set; }
        public List<Match> Matches { get; set; } = new List<Match>();
        public bool IsFinished { get; set; } = false;


        public Round(RoundType typeOf, List<Match> matches)
        {
            TypeOf = typeOf;
            Matches = matches;
        }
    }
}
