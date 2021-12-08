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
            game = new BullsAndCowsGame(GameIO);
            ui = new MockUI();
        }

        [Test]
        public void Setup_Creates_Player_When_List_Is_Empty()
        {
            GameIO.Data = new();
            game.SetupGame("Robin");
            Assert.That(game.GetPlayerName(), Is.EqualTo("Robin"));
        }

        [Test]
        public void Game_Is_Completed_When_Answer_Is_Correct()
        {
            game.SetupGame("Robin");
            var answer = game.GetAnswer();
            TestContext.WriteLine("the answer is " + answer);
            game.ValidateInput(answer);
            string progress = game.CheckAnswer();
            TestContext.WriteLine("the progress is " + progress);
            Assert.That(game.GameFinished, Is.True);
        }

        [Test]
        public void Game_Is_Resetable()
        {
            game.SetupGame("Robin");
            var answerBeforeReset = game.GetAnswer();

            game.ValidateInput(answerBeforeReset);
            string progressBeforeReset = game.CheckAnswer();
            bool gameFinished = game.GameFinished;
            game.ResetGame();

            Assert.That(gameFinished, Is.True);

            game.SetupGame("Robin");
            game.ValidateInput(game.GetAnswer().Substring(0,2));
            Assert.That(answerBeforeReset, Is.Not.EqualTo(game.GetAnswer()));
            Assert.That(progressBeforeReset, Is.Not.EqualTo(game.CheckAnswer()));
            Assert.That(game.GameFinished, Is.False);

        }

        public void IO_Can_Load_From_File()
        {
        }


    }
}