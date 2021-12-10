using BullsAndCows;
using BullsAndCows.IO;
using GameEngine;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitTests;

namespace BullsCowsTest
{
    internal class BullsAndCowsTests
    {
        private IGame _game;
        private IGameIO _gameIO;
        private GameController _controller;
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
            _controller = new GameController(_game, _fakeUI.Object);

            _game.SetupGame();

        }



        [Test]
        public void Partial_Correct_Order_Returns_Bulls_And_Cows()
        {
            string answer = _game.GetAnswer();
            string guess = SwitchPlaceLastAndFirstCharacter(answer);


            _game.MakeGuess(guess);
            string progress = _game.GetProgress();


            Assert.That(progress, Is.EqualTo("BB,CC"));
        }





        [Test]
        public void Correct_Digits_But_Wrong_Order_Returns_Cows()
        {
            IEnumerable<char> answerReversed = _game.GetAnswer()
                                                    .Reverse();
            string guess = string.Join("", answerReversed);


            _game.MakeGuess(guess);


            string progress = _game.GetProgress();
            Assert.That(progress, Is.EqualTo(",CCCC"));
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






        [Test]
        public void Player_With_Lowest_Average_Is_Leader()
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
                          .Returns(FakeData.TextFile);

            
            
            _fakeGame.Setup(game => game.GetPlayers())
                     .Returns(() => _gameIO.GetPlayerData());

            _fakeGame.Setup(game => game.GameFinished)
                     .Returns(true);

            
            
            
            var controller = new GameController(_fakeGame.Object, _fakeUI.Object);
            controller.Start();



            Player expected = new(name: "RF", gamesPlayed: 3, guesses: 3);
            Assert.That(leader, Is.EqualTo(expected));

        }


        private string SwitchPlaceLastAndFirstCharacter(string answer)
        {
            StringBuilder guessBuilder = new(answer);

            char first = answer.First();
            char last = answer.Last();

            guessBuilder[^1] = first;
            guessBuilder[0] = last;

            return guessBuilder.ToString();

        }


        [Test]
        public void Hiscores__Only_Loaded_When_Game_Finished()
        {
            IGameUI ui = _fakeUI.Object;



            _fakeUI.Setup(ui => ui.GetInput())
                   .Returns("");

            _fakeUI.Setup(ui => ui.Continue())
                   .Returns(false);

            _fakeUI.Setup(ui => ui.ShowHiscores(It.IsAny<IEnumerable<Player>>()));


            _fakeIOWrapper.Setup(wrapper => wrapper.ReadFile(It.IsAny<string>()))
                          .Returns(FakeData.TextFile);



            _fakeGame.Setup(game => game.GetPlayers())
                     .Returns(_gameIO.GetPlayerData());

            _fakeGame.Setup(game => game.GameFinished)
                     .Returns(true);




            var controller = new GameController(_fakeGame.Object, _fakeUI.Object);
            controller.Start();


            _fakeUI.Verify(ui => ui.ShowHiscores(It.IsAny<IEnumerable<Player>>()), Times.Once());
            _fakeIOWrapper.Verify(wrapper => wrapper.ReadFile(It.IsAny<string>()), Times.Once());

        }


    }
}
