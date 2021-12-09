using BullsAndCows;
using BullsCowsTest.Mocks;
using GameEngine;
using NUnit.Framework;

namespace BullsCowsTest
{
    public class GeneralGameTests
    {
        MockUI ui = null;
        MockIO GameIO = null;
        IGame game = null;

        [SetUp]
        public void Setup()
        {
            GameIO = new MockIO();
            game = new WordGuessGame(GameIO);
            ui = new MockUI();
            game.SetupGame();
            game.SetPlayerName("Robin");
        }


        [Test]
        public void Game_Is_Completed_When_Answer_Is_Correct()
        {
            var answer = game.GetAnswer();
            game.MakeGuess(answer);


            Assert.That(game.GameFinished, Is.True);
        }

        [Test]
        public void Game_Is_Resetable()
        {
            var answerBeforeReset = game.GetAnswer();

            game.MakeGuess(answerBeforeReset);
            string progressBeforeReset = game.GetProgress();
            bool gameFinished = game.GameFinished;

            game.Reset();
            game.MakeGuess("1");
            string answer = game.GetAnswer();
            string progress = game.GetProgress();


            Assert.Multiple(() =>
            {
                Assert.That(answerBeforeReset, Is.Not.EqualTo(answer));
                Assert.That(progressBeforeReset, Is.Not.EqualTo(progress));
                Assert.That(game.GameFinished, Is.False);
            });
            
        }



    }
}