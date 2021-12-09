using GameEngine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace BullsAndCows
{
    public class ConsoleUI : IGameUI
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
            
            string scoreboard = GenerateScoreboard(hiscores);
            Output(scoreboard);
        }



        private string GenerateScoreboard(IEnumerable<PlayerData> hiscores)
        {
            int longestNameLength = hiscores.Max(player => player.Name.Length);

            string format = "{0,-" + longestNameLength + "} {1,5} {2,7}";


            var scoreBuilder = new StringBuilder();

            scoreBuilder.AppendLine("Current leaderboard:");
            scoreBuilder.AppendLine(string.Format(format, "Name", "Games", "Average"));



            foreach (var player in hiscores.OrderBy(player => player.AverageGuesses))
            {
                scoreBuilder.AppendLine(string.Format(format, player.Name, player.GamesPlayed, player.AverageGuesses));
            }


            return scoreBuilder.ToString();
        }





        public void Clear()
        {
            Console.Clear();
        }
    }
}
