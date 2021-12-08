using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BullsAndCows
{
    public class BullsAndCowsGame : GameBase, IGame
    {
        public string GameName => "Bulls and cows";
        public int Turns => GuessesMade;
        public string GetAnswer() => Answer;
        public bool GameFinished => Answer == Guess;

        public BullsAndCowsGame(IGameIO gameIO) : base(gameIO)
        {

        }

        private string Answer { get; set; }
        private string Guess { get; set; }
        private int GuessesMade { get; set; } = 0;



        public bool ValidateInput(string input)
        {
            bool validInput = input.All(ch => char.IsDigit(ch)) && !string.IsNullOrWhiteSpace(input);

            if (!validInput)
                return false;

            GuessesMade++;
            Guess = input;
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
                return CheckBullsCows();
                

            }
            return "BBBB,";

        }

        private string CheckBullsCows()
        {
            string bulls = "";
            string cows = "";
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < Guess.Length; j++)
                {
                    bool samePositions = i == j;
                    bool sameDigits = Answer[i] == Guess[j];



                    if (samePositions && sameDigits)
                            bulls += "B";

                    else if(sameDigits)
                        cows += "C";
                    
                }
            }
            return string.Format("{0},{1}", bulls, cows);
        }

        public string GetPlayerName()
        {
            return CurrentPlayer.Name;
        }

        public string OnFinish()
        {
            SaveHiscores(GuessesMade);
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
