using System;
using System.IO;
using System.Collections.Generic;
using GameEngine;

namespace BullsAndCows
{
    class MainClass
    {

        public static void Main(string[] args)
        {
            IGameIO io = new FileBasedGameIO("result.txt");
            IUI ui = new ConsoleUI();
            var controller = new GameController(new BullsAndCowsGame(io), ui);
            controller.Run();
        }
    }


}