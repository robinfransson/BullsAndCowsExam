using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BullsAndCows
{
    public class BullsAndCowsGame : GameBase, IGame
    {
        public string Name => "Bulls and cows";
        public int Turns => GuessesMade;
        private string Answer { get; set; }
        private string Guess { get; set; }
        private int GuessesMade { get; set; } = 0;

        public BullsAndCowsGame(IGameIO gameIO) : base(gameIO)
        {

        }


        public string GetAnswer() => Answer;

        public bool GameFinished => Answer == Guess;


        public bool ValidateInput(string input)
        {
            bool validInput = input.All(ch => char.IsDigit(ch)) && input.Length == 4;

            if (!validInput)
                return false;


            if (!GameFinished)
            {
                GuessesMade++;
                Guess = input;
            }
            return true;
        }

        public void SetupGame(string playerName)
        {
            base.Setup(playerName);
            GenerateAnswer();
        }

        private void GenerateAnswer()
        {
            List<int> digits = new();
            Random rand = new();
            while (digits.Count < 4)
            {
                int digit = rand.Next(10);
                if (digits.Contains(digit))
                    continue;

                digits.Add(digit);

            }
            Answer = string.Join("", digits);
        }



        public string CheckAnswer()
        {
            if (!GameFinished)
            {
                string bulls = CheckBulls();
                string cows = CheckCows();
                return $"{bulls},{cows}";

            }
            return "BBBB,";

        }

        private string CheckBulls()
        {
            var stringBuilder = new StringBuilder();
            for (int i = 0; i < Guess.Length; i++)
            {
                if (Answer[i] == Guess[i])
                    stringBuilder.Append('B');
            }

            return stringBuilder.ToString();
        }
        private string CheckCows()
        {

            StringBuilder stringBuilder = new();

            List<char> checkedCharacters = new();

            for(int i = 0; i < Guess.Length; i++)
            {
                var currentGuess = Guess[i];
                var currentAnswer = Answer[i];

                if (checkedCharacters.Contains(currentGuess))
                    continue;

                checkedCharacters.Add(currentGuess);

                int cows = Answer.Count(ch => ch == currentGuess);
                cows -= currentGuess == currentAnswer ? 1 : 0;
                for (int j = 0; j < cows; j++)
                {
                    stringBuilder.Append('C');
                }


            }

            return stringBuilder.ToString();
        }

        public string GetPlayerName()
        {
            return CurrentPlayer.Name;
        }

        public string OnFinish()
        {
            CurrentPlayer.Update(GuessesMade);
            SaveHiscores();
            return GetTopList();
        }

        public void ResetGame()
        {
            GuessesMade = 0;
            Guess = null;
            GenerateAnswer();
        }
    }
}
