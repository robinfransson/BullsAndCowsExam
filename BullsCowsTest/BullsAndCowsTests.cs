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
        MockUI ui = null;
        MockIO GameIO = null;
        IGame game = null;

        [SetUp]
        public void Setup()
        {
            GameIO = new MockIO();
            game = new BullsAndCowsGame(GameIO);
            ui = new MockUI();
            game.SetupGame("Robin");
        }

        [Test] 
        public void Partial_Correct_Order_Returns_Bulls_And_Cows()
        {
            string answer = game.GetAnswer();
            char last = answer[3];
            char first = answer[0];

            StringBuilder sb = new(answer);
            sb[3] = first;
            sb[0] = last;

            string guess = sb.ToString();
            game.ValidateInput(guess);
            Assert.That(game.CheckAnswer(), Is.EqualTo("BB,CC"));
        }

        public void Correct_Answer_Returns_Bulls()
        {
            string answer = game.GetAnswer();
            game.ValidateInput(answer);
            Assert.That(game.CheckAnswer(), Is.EqualTo("BBBB,"));
            Assert.That(game.GameFinished, Is.True);
        }

        [Test]
        public void Correct_Digits_Wrong_Order_Returns_Cows()
        {
            var reversed = game.GetAnswer().Reverse();

            string guess = string.Join("", reversed);

            game.ValidateInput(guess);
            Assert.That(game.CheckAnswer(), Is.EqualTo(",CCCC"));
        }
    }
}
