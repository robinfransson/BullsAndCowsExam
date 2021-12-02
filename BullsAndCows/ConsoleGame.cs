using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullsAndCows
{
    public class ConsoleGame
    {
        private IGame _game;

        public ConsoleGame(IGame game)
        {
            _game = game;
        }
        public void Start()
        {

            Console.Clear();
            bool playing = true;
            Console.WriteLine($"Welcome to {_game.Name}");
            Console.WriteLine("Enter your name:");
            string name = Console.ReadLine();
            _game.SetupGame(name);
            while (playing)
            {

                Console.WriteLine("Welcome " + _game.GetPlayerName());
                Console.WriteLine("For testing: answer is " + _game.GetAnswer());
                while (!_game.GameFinished)
                {
                    Console.WriteLine("Enter a guess: ");
                    string guess = Console.ReadLine();
                    bool validInout = _game.ValidateInput(guess);

                    if (!validInout)
                        continue;

                    string result = _game.CheckAnswer();
                    Console.WriteLine(result);
                }
                Console.WriteLine($"Completed the game in {_game.Turns} turns! Well done.");
                Console.WriteLine(_game.OnFinish());
                Console.WriteLine("Press Y to play again or any other key to return to the menu");
                var key = Console.ReadKey(false).Key;
                playing = CheckInput(key);
            }
        }

        private bool CheckInput(ConsoleKey key)
        {
            if(key == ConsoleKey.Y)
            {
                _game.ResetGame();
                return true;
            }
            return false;

        }
    }
}
