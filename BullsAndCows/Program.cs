using System;
using System.IO;
using System.Collections.Generic;
using GameEngine;
using BullsAndCows.IO;
using System.Linq;

namespace BullsAndCows
{
    class Program
    {

        public static void Main(string[] args)
        {
            IFileIOWrapper wrapper = new FileIOWrapper();
            IGameIO io = new FileBasedGameIO("result.txt", wrapper);
            IGameUI ui = new ConsoleUI();
            IGame game = new BullsAndCowsGame(io);

            var controller = new GameController(game, ui);
            controller.Start();
        }
    }


}