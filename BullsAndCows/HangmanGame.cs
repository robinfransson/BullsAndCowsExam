using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BullsAndCows
{
    public class HangmanGame : GameBase, IGame
    {
        private string[] words = { "prins", "slott", "barmhärtighetsinrättningarnas" };

        public string Name => "Hangman";

        public int Turns => Turns;

        public bool GameFinished => !CurrentProgress.Any(ch => ch == '_');

        private string HiddenWord { get; set; }
        private string CurrentProgress { get; set; }
        private int Guesses { get; set; }
        private string Guess { get; set; }
        private List<char> alreadyGuessed = new();



        public HangmanGame(IGameIO gameIO) : base(gameIO)
        {
        }

        public string CheckAnswer()
        {
            foreach (var character in Guess)
            {
                Guesses++;
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
            CurrentPlayer.Update(Turns);
            base.SaveHiscores();
            return GetTopList();
        }

        public void ResetGame()
        {
            SetHiddenWord();
            GenerateProgress();
            Guesses = 0;
            Guess = "";
            alreadyGuessed = new();
        }

        public void SetupGame(string playerName)
        {
            base.Setup(playerName);
            SetHiddenWord();
            GenerateProgress();
        }

        private void GenerateProgress()
        {
            StringBuilder sb = new();
            while(sb.Length != HiddenWord.Length)
            {
                sb.Append('_');
            }
            CurrentProgress = sb.ToString();
        }

        private void SetHiddenWord()
        {
            Random rand = new();
            HiddenWord = words[rand.Next(words.Length)];
        }

        public bool ValidateInput(string input)
        {
            if (input.Any(ch => char.IsDigit(ch)) || string.IsNullOrWhiteSpace(input))
                return false;

            Guess = input;
            return true;
        }
    }
}