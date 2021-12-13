using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BullsAndCows
{
    public class WordGuessingGame : IGame
    {
        public string GameName => "Word guessing game";

        public int Turns { get; private set; }

        public bool GameFinished => !CurrentProgress.Contains('_');



        private string Guess { get; set; }
        private string Answer { get; set; }
        private string CurrentProgress { get; set; }
        private string PlayerName { get; set; }

        private string[] words = new[] { "banan", "potatis", "anakonda", "hejsan" };


        private readonly IGameIO IO;

        public WordGuessingGame(IGameIO IO)
        {
            this.IO = IO;
        }



        public WordGuessingGame(IGameIO IO, string answer)
        {
            this.IO = IO;
            Answer = answer;
            SetInitialProgress();
        }




        public string GetAnswer()
        {
            return Answer;
        }

        public List<Player> GetPlayers()
        {
            return IO.LoadPlayerData();
        }






        public string GetProgress()
        {
            return CurrentProgress;
        }






        public void MakeGuess(string input)
        {
            var guess = input.ToArray();
            Turns += guess.Length;

            CheckMatches(input);

        }





        private void CheckMatches(string guess)
        {

            var foundMatches = new List<int>();

            foreach (var currentInGuess in guess)
            {
                for(int i = 0; i < Answer.Length; i++)
                {
                    char currentInAnswer = Answer[i];
                    if(currentInAnswer == currentInGuess)
                    {
                        foundMatches.Add(i);
                    }
                }
            }

            if(foundMatches.Any())
            {
                UpdateProgress(foundMatches);
            }


        }

        private void UpdateProgress(IEnumerable<int> matches)
        {
            var builder = new StringBuilder(CurrentProgress);
            foreach (int index in matches)
            {
                bool alreadyGuessed = builder[index] == Answer[index];

                if (alreadyGuessed)
                    continue;

                builder[index] = Answer[index];
            }


            CurrentProgress = builder.ToString();
        }

        public void Reset()
        {
            Turns = 0;
            SetAnswer();
            SetInitialProgress();

        }

        public void SaveScore()
        {
            IO.SavePlayerData(PlayerName, Turns);
        }




        public void SetPlayerName(string name)
        {
            PlayerName = name;
        }




        public void SetupGame()
        {
            SetAnswer();
            SetInitialProgress();
        }




        private void SetInitialProgress()
        {
            var sb = new StringBuilder();
            while (sb.Length < Answer.Length)
            {
                sb.Append('_');
            }

            CurrentProgress = sb.ToString();
        }




        private void SetAnswer()
        {

            Random rand = new();
            int randomIndex = rand.Next(words.Length);
            Answer = words[randomIndex];
        }

    }
}
