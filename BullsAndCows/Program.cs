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
            while (true)
            {
                Console.WriteLine("1. Hangman\n2. Cows and bulls");
                var key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.D1:
                        IGameIO hangmanIO = new FileBasedGameIO("hangman.txt");
                        new ConsoleGame(new HangmanGame(hangmanIO)).Start();
                        break;
                    case ConsoleKey.D2:
                        IGameIO io = new FileBasedGameIO("bullscows.txt");
                        new ConsoleGame(new BullsAndCows(io)).Start();
                        break;
                    default:
                        break;
                }
                Console.Clear();
            }

        }
    }


}