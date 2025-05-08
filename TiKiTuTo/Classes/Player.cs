using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TiKiTuTo.Classes;

/*
Player player = new Player("Tim");
Player player2 = new Player("Bob");
Console.WriteLine(player2.PlayerID);
*/
namespace TiKiTuTo.Classes
{
    public class Player
    {
        private static int NextID = 1;
        public string PlayerName { get; set; }
        public int PlayerID { get; private set; }


        public Player(string name)
        {
            PlayerName = name;
            PlayerID = NextID++; //nach unique ID methode suchen
        }
    }
}


