using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BullsAndCows
{
    public class BullsAndCowsGame : IGame
    {
        public string GameName => "Bulls and cows";
        public int Turns => GuessesMade;
        public string GetAnswer() => Answer;
        public bool GameFinished => Answer == Guess;


        private readonly IGameIO IO;
        private string PlayerName { get; set; }
        private string Answer { get; set; }
        private string Guess { get; set; }
        private int GuessesMade { get; set; }




        public BullsAndCowsGame(IGameIO gameIO)
        {
            IO = gameIO;
        }

        public void SetPlayerName(string name)
        {
            PlayerName = name;
        }

        public void MakeGuess(string input)
        {
            GuessesMade++;
            Guess = input;
        }

        public void SetupGame()
        {
            SetAnswer();
        }


        public string GetProgress()
        {
            if (!GameFinished)
            {
                return CheckBullsCows();
            }

            return "BBBB,";

        }

       
        public List<PlayerData> GetPlayers()
        {

            return IO.GetPlayerData();
        }



        public void Reset()
        {
            GuessesMade = 0;
            Guess = null;
            SetAnswer();
        }



        public void SaveScore()
        {
            IO.SavePlayerData(PlayerName, GuessesMade);
        }



        private void SetAnswer()
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



        private string CheckBullsCows()
        {
            string bulls = "";
            string cows = "";
            for (int i = 0; i < Answer.Length; i++)
            {
                for (int j = 0; j < Guess?.Length; j++)
                {
                    bool samePositions = i == j;
                    bool sameDigits = Answer[i] == Guess[j];

                    if (samePositions && sameDigits)
                    {
                        bulls += "B";
                    }
                    else if (sameDigits)
                    {
                        cows += "C";
                    }
                }
            }
            return string.Format("{0},{1}", bulls, cows);
        }

    }
}
