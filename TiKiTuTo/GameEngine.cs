using System;
using System.Collections;
using System.Threading;
using Rounds; 
namespace GameEngine
{
    public class Game
    {
        public void NewGame()
        {
            var Rounds = new Rounds.Rounds();

            Rounds.PreliminaryRound();
            Rounds.KORound();
        }
    }
}
