using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiKiTuTo
{
    internal class teamBuilding
    {

        private List<string> teamList = new List<string>() { "Nikita", "Chris", "Mahdi", "Robin" };

        public void SayHello()
        {
            foreach (var teamMember in teamList) 
            {
                Console.WriteLine($"Hello {teamMember}");
            }
        }

    }
}
