using BullsAndCows;
using BullsAndCows.IO;
using GameEngine;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    internal class BullsAndCowsTests
    {
        private static IGame _game;
        private IGameIO _gameIO;
        private Mock<IFileIOWrapper> _fakeIOWrapper;
        private Mock<IGameUI> _fakeUI;
        private Mock<IGame> _fakeGame;










        [SetUp]
        public void Setup()
        {
            _fakeUI = new Mock<IGameUI>();
            _fakeGame = new Mock<IGame>();
            _fakeIOWrapper = new Mock<IFileIOWrapper>();


            _gameIO = new FileBasedGameIO("bullsandcows.txt", _fakeIOWrapper.Object);
            _game = new BullsAndCowsGame(_gameIO);

            _game.SetupGame();

        }





        [Test]
        [TestCase("5647", "BBBB,")]
        [TestCase("5640", "BBB,")]
        [TestCase("1476", ",CCC")]
        [TestCase("4765", ",CCCC")]
        [TestCase("5674", "BB,CC")]
        [TestCase("5764", "B,CCC")]
        [TestCase("5555", "B,CCC")]
        [TestCase("1238", ",")]
        public void Game_Returns_Bulls_And_Cows_Correctly(string guess, string expected)
        {
            _game = new BullsAndCowsGame(_gameIO, answer: "5647");


            _game.MakeGuess(guess);
            string progress = _game.GetProgress();


            Assert.That(progress, Is.EqualTo(expected));
        }






        [Test]
        public void Game_Is_Completed_When_Answer_Is_Correct()
        {
            string answer = _game.GetAnswer();


            _game.MakeGuess(answer);


            Assert.That(_game.GameFinished, Is.True);
        }



        [Test]
        public void Game_Is_Resetable()
        {
            string answerBeforeReset = _game.GetAnswer();
            _game.MakeGuess(answerBeforeReset);
            string progressBeforeReset = _game.GetProgress();
            
            
            
            _game.Reset();
            string answer = _game.GetAnswer();
            string progress = _game.GetProgress();

            
            
            Assert.That(answerBeforeReset, Is.Not.EqualTo(answer));
            Assert.That(progressBeforeReset, Is.Not.EqualTo(progress));
            Assert.That(_game.Turns, Is.Zero);
            Assert.That(_game.GameFinished, Is.False);

        }





        public static IEnumerable<TestCaseData> OrderedByAverageGuessesData
        {
            get
            {
                yield return new TestCaseData(new string[] { "RF#&#1", "RF#&#1", "RF#&#1", "Robin#&#3", "Robin#&#3", "Robin#&#3", "RobinFransson#&#2",
                                                             "RobinFransson#&#15","RobinFransson#&#20"},
                                              new Player(name: "RF", gamesPlayed: 3, guesses: 3));
                yield return new TestCaseData(new string[] { "RF#&#5", "RF#&#5", "RF#&#5", "Robin#&#3", "Robin#&#3", "Robin#&#3", "RobinFransson#&#2",
                                                             "RobinFransson#&#15","RobinFransson#&#20"},
                                              new Player(name: "Robin", gamesPlayed: 3, guesses: 9));
            }
        }







        [Test]
        [TestCaseSource("OrderedByAverageGuessesData")]
        public void Controller_Orders_Players_By_Average_Guesses(string[] lines, Player expected)
        {
            
            Player leader = null;
            IGameUI ui = _fakeUI.Object;
            
            _fakeUI.Setup(ui => ui.GetInput())
                   .Returns("");

            _fakeUI.Setup(ui => ui.Continue())
                   .Returns(false);

            _fakeUI.Setup(ui => ui.ShowHiscores(It.IsAny<IEnumerable<Player>>()))
                   .Callback<IEnumerable<Player>>((players) => leader = players.First());

            
            _fakeIOWrapper.Setup(wrapper => wrapper.ReadFile(It.IsAny<string>()))
                          .Returns(lines);

            
            
            _fakeGame.Setup(game => game.GetPlayers())
                     .Returns(() => _gameIO.LoadPlayerData());

            _fakeGame.Setup(game => game.GameFinished)
                     .Returns(true);

            
            
            
            var controller = new GameController(_fakeGame.Object, _fakeUI.Object);
            controller.Start();



            
            Assert.That(leader, Is.EqualTo(expected));

        }



        [Test]
        public void Hiscores_Only_Loaded_When_Game_Finished()
        {
            IGameUI ui = _fakeUI.Object;



            _fakeUI.Setup(ui => ui.GetInput())
                   .Returns("");

            _fakeUI.Setup(ui => ui.Continue())
                   .Returns(false);

            _fakeUI.Setup(ui => ui.ShowHiscores(It.IsAny<IEnumerable<Player>>()));


            _fakeIOWrapper.Setup(wrapper => wrapper.ReadFile(It.IsAny<string>()))
                          .Returns(new string[] { "RF#&#1" });



            _fakeGame.Setup(game => game.GetPlayers())
                     .Returns(_gameIO.LoadPlayerData());

            _fakeGame.Setup(game => game.GameFinished)
                     .Returns(true);




            var controller = new GameController(_fakeGame.Object, _fakeUI.Object);
            controller.Start();


            _fakeUI.Verify(ui => ui.ShowHiscores(It.IsAny<IEnumerable<Player>>()), Times.Once());
            _fakeIOWrapper.Verify(wrapper => wrapper.ReadFile(It.IsAny<string>()), Times.Once());

        }


    }
}
