using GameEngine;
using System;

namespace BullsAndCows
{
    public class ConsoleUI : IUI
    {
        public string GetString()
        {
            return Console.ReadLine();
        }

        public bool Continue()
        {
            Console.WriteLine("Press Y to play again or any other key to return to the menu");

            var key = Console.ReadKey(true).Key;
            return key == ConsoleKey.Y;

        }

        public void PutString(string s)
        {
            Console.WriteLine(s);
        }
    }
}
