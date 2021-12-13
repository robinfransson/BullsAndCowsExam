using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BullsAndCows
{
    public class BullsAndCowsGame : IGame
    {
        public string GameName => "Cows and bulls";
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

        public BullsAndCowsGame(IGameIO gameIO, string answer)
        {
            IO = gameIO;
            Answer = answer;
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


       
        public List<Player> GetPlayers()
        {

            return IO.LoadPlayerData();
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
            List<int> selectedNumbers = new();
            Random rand = new();


            while (selectedNumbers.Count < 4)
            {
                int digit = rand.Next(10);

                if (selectedNumbers.Contains(digit))
                    continue;

                
                selectedNumbers.Add(digit);
            }

            Answer = string.Join("", selectedNumbers);
        }



        private string CheckBullsCows()
        {
            var bulls = new StringBuilder();
            var cows = new StringBuilder();



            for (int i = 0; i < Answer.Length; i++)
            {
                for (int j = 0; j < Guess?.Length; j++)
                {
                    bool samePositions = i == j;
                    bool sameDigits = Answer[i] == Guess[j];

                    if (samePositions && sameDigits)
                    {
                        bulls.Append('B');
                    }
                    else if (sameDigits)
                    {
                        cows.Append('C');
                    }
                }
            }
            return string.Format("{0},{1}", bulls.ToString(), cows.ToString());
        }

    }
}
