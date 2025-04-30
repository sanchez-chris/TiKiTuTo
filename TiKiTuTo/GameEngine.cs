using System;
using System.Collections;
using System.Threading;
namespace GameEngine
{
    public class Game
    {

        public void NewGame()
        {
            Console.Clear();
            Console.WriteLine("New game started!");
            Thread.Sleep(5000);
        }
    }
}
