using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullsAndCows
{
    internal class BullsAndCows : GameBase, IGame
    {
        public string Name => "Bulls and cows";
        public int Turns => GuessesMade;
        private string Answer { get; set; }
        private string Guess { get; set; }
        private int GuessesMade { get; set; } = 0;

        public BullsAndCows(IGameIO gameIO) : base(gameIO)
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
            var stringBuilder = new StringBuilder();
            Random rand = new();
            while (stringBuilder.Length < 4)
            {
                stringBuilder.Append(rand.Next(10));
            }
            Answer = stringBuilder.ToString();
        }



        public string CheckAnswer()
        {
            if (!GameFinished)
            {
                string bulls = CheckBulls();
                string cows = CheckCows();
                return $"{bulls},{cows}";

            }
            return "BBBB";

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

            foreach (var digit in Guess)
            {
                var currentIndex = Guess.IndexOf(digit);
                var currentGuess = Guess[currentIndex];
                var currentAnswer = Answer[currentIndex];
                if (checkedCharacters.Contains(digit))
                    continue;

                checkedCharacters.Add(digit);
                int cows = Answer.Count(ch => ch == digit);
                cows -= currentGuess == currentAnswer ? 1 : 0;
                for (int i = 0; i < cows; i++)
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
            //$"{player.Name} {player.GamesPlayed} {player.Guesses} {player.AverageGuesses()}

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
