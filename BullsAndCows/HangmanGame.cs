using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BullsAndCows
{
    internal class HangmanGame : GameBase, IGame
    {
        private string[] words = { "prins", "slott", "barmhärtighetsinrättningarnas" };

        public string Name => "Hangman";

        public int Turns => turns;

        public bool GameFinished => !CurrentProgress.Any(ch => ch == '_');

        private string HiddenWord { get; set; }
        private string CurrentProgress = "";
        private int turns = 0;
        private string guess = "";
        private List<char> alreadyGuessed = new();
        public HangmanGame(IGameIO gameIO) : base(gameIO)
        {
        }

        public string CheckAnswer()
        {
            foreach (var character in guess)
            {
                turns++;
                if (alreadyGuessed.Contains(character))
                    continue;

                if (HiddenWord.Any(ch => ch == character))
                    UpdateProgress(character);

                alreadyGuessed.Add(character);
            }
            return CurrentProgress;
        }

        private void UpdateProgress(char character)
        {
            var sb = new StringBuilder(CurrentProgress);
            for (int i = 0; i < HiddenWord.Length; i++)
            {
                if (HiddenWord[i] == character)
                    sb[i] = character;
            }
            CurrentProgress = sb.ToString();
        }

        public string GetAnswer()
        {
            return HiddenWord;
        }

        public string GetPlayerName()
        {
            return CurrentPlayer.Name;
        }

        public string OnFinish()
        {
            CurrentPlayer.Update(turns);
            base.SaveHiscores();
            return GetTopList();
        }

        public void ResetGame()
        {
            GetHiddenWord();
            GenerateProgress();
            turns = 0;
            guess = "";
            alreadyGuessed = new();
        }

        public void SetupGame(string playerName)
        {
            base.Setup(playerName);
            GetHiddenWord();
            GenerateProgress();
        }

        private void GenerateProgress()
        {
            StringBuilder sb = new();
            foreach (var ch in HiddenWord)
            {
                sb.Append("_");
            }
            CurrentProgress = sb.ToString();
        }

        private void GetHiddenWord()
        {
            Random rand = new();
            HiddenWord = words[rand.Next(words.Length)];
        }

        public bool ValidateInput(string input)
        {
            if (input.Any(ch => char.IsDigit(ch)) || string.IsNullOrWhiteSpace(input))
                return false;

            guess = input;
            return true;
        }
    }
}