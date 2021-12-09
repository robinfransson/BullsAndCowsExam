using BullsAndCows;
using BullsCowsTest.Mocks;
using GameEngine;
using NUnit.Framework;
using System;
using System.Linq;
using System.Text;

namespace BullsCowsTest
{
    internal class BullsAndCowsTests
    {
        MockIO GameIO = null;
        IGame game = null;

        [SetUp]
        public void Setup()
        {
            GameIO = new MockIO();
            game = new BullsAndCowsGame(GameIO);
            game.SetPlayerName("Robin");
            game.SetupGame();
        }

        [Test] 
        public void Partial_Correct_Order_Returns_Bulls_And_Cows()
        {
            string answer = game.GetAnswer();
            char first = answer.First();
            char last = answer.Last();

            StringBuilder sb = new(answer);
            sb[answer.Length - 1] = first;
            sb[0] = last;

            string guess = sb.ToString();
            game.MakeGuess(guess);
            Assert.That(game.GetProgress(), Is.EqualTo("BB,CC"));
        }

        public void Correct_Answer_Returns_Bulls()
        {
            string answer = game.GetAnswer();
            game.MakeGuess(answer);
            Assert.That(game.GetProgress(), Is.EqualTo("BBBB,"));
            Assert.That(game.GameFinished, Is.True);
        }

        [Test]
        public void Correct_Digits_But_Wrong_Order_Returns_Cows()
        {
            var reversed = game.GetAnswer().Reverse();

            string guess = string.Join("", reversed);

            game.MakeGuess(guess);

            string progress = game.GetProgress();

            Assert.That(progress, Is.EqualTo(",CCCC"));
        }
    }
}
