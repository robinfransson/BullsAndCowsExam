using GameEngine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace BullsAndCows
{
    public class ConsoleUI : IUI
    {

        public string GetInput()
        {
            return Console.ReadLine().Trim();
        }

        public bool Continue()
        {
            Console.WriteLine("Press Y to play again or any other key to return to exit.");

            var key = Console.ReadKey(true).Key;
            return key == ConsoleKey.Y;

        }

        public void Output(string s)
        {
            Console.WriteLine(s);
        }

        public void ShowHiscores(IEnumerable<PlayerData> hiscores)
        {
            var scoreBuilder = new StringBuilder();
            int longestName = hiscores.Max(player => player.Name.Length);

            string format = "|{0,-" + longestName + "}|{1,5}|{2,7}|{3,7}|";

            scoreBuilder.AppendLine("Current leaderboard:");
            scoreBuilder.AppendLine(string.Format(format, "Name", "Games", "Guesses", "Average"));

            foreach (var player in hiscores)
            {
                scoreBuilder.AppendLine(string.Format(format,
                                        player.Name, player.GamesPlayed, player.TotalGuesses, player.AverageGuesses));
            }

            string scoreboard = scoreBuilder.ToString();
            Console.WriteLine(scoreboard);
        }

        public void Clear()
        {
            Console.Clear();
        }
    }
}
