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
            IFileIOWrapper wrapper = new FileIOWrapper();
            IGameIO io = new FileBasedGameIO("result.txt", wrapper);
            IGameUI ui = new ConsoleUI();
            IGame game = new BullsAndCowsGame(io);
            var controller = new GameController(game, ui);
            controller.Run();
        }
    }


}