using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiKiTuTo.View
{
    internal interface IView
    {
        public void ShowMenu();

        public void ShowGamePlan();

        public void ShowStandings();

        public void ShowNextMatches(List<Match> matches);
        
    }
}
