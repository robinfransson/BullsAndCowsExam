using System;
using GameEngine;
using BullsAndCows.IO;

namespace BullsAndCows
{
    class Program
    {
        

        public static void Main(string[] args)
        {
            IGame game;

            IGameUI ui = new ConsoleUI();
            while (true)
            {
                Console.WriteLine("Enter a game name to start it");
                game = SelectGame();

                if (game == null)
                    continue;

                var controller = new GameController(game, ui);
                controller.Start();

            }


        }


        private static IGame SelectGame()
        {

            IFileIOWrapper wrapper = new FileIOWrapper();
            IGameIO io;
            Console.WriteLine("1. Bulls and cows");
            Console.WriteLine("2. Word guess");
            Console.WriteLine("q: Quit");

            var selected = Console.ReadLine().ToLower();

            switch(selected)
            {

                 case "1":
                    io = new FileBasedGameIO("bullsandcows.txt", wrapper);
                    Console.Clear();
                    return new BullsAndCowsGame(io);
                case "2":
                    io = new FileBasedGameIO("wordguess.txt", wrapper);
                    Console.Clear();
                    return new WordGuessingGame(io);
                case "q":
                    Environment.Exit(0);
                    return null;
                default:
                    return null;
            }
        }

    }


}