using BullsAndCows;
using GameEngine;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BullsCowsTest
{
    internal class BullsAndCowsTests
    {
        private IGame _game;
        private IGameIO _gameIO;
        private GameController _controller;
        private Mock<IFileIOWrapper> _fakeIOWrapper;
        private Mock<IGameUI> _fakeUI;
        

        [SetUp]
        public void Setup()
        {
            _fakeUI = new Mock<IGameUI>();
            _fakeIOWrapper = new Mock<IFileIOWrapper>();
            _gameIO = new FileBasedGameIO("bullsandcows.txt", _fakeIOWrapper.Object);
            _game = new BullsAndCowsGame(_gameIO);
            _controller = new GameController(_game, _fakeUI.Object);

            _game.SetPlayerName("Robin");
            _game.SetupGame();
        }



        [Test] 
        public void Partial_Correct_Order_Returns_Bulls_And_Cows()
        {
            string guess = LastAndFirstCharactersWrong();
            _game.MakeGuess(guess);
            string progress = _game.GetProgress();


            Assert.That(progress, Is.EqualTo("BB,CC"));
        }





        [Test]
        public void Correct_Digits_But_Wrong_Order_Returns_Cows()
        {
            var reversed = _game.GetAnswer()
                                .Reverse();

            string guess = string.Join("", reversed);

            _game.MakeGuess(guess);

            string progress = _game.GetProgress();

            Assert.That(progress, Is.EqualTo(",CCCC"));
        }

        


        [Test]
        public void Game_Is_Completed_When_Answer_Is_Correct()
        {
            var answer = _game.GetAnswer();
            _game.MakeGuess(answer);


            Assert.That(_game.GameFinished, Is.True);
        }

        [Test]
        public void Game_Is_Resetable()
        {
            var answerBeforeReset = _game.GetAnswer();

            _game.MakeGuess(answerBeforeReset);
            string progressBeforeReset = _game.GetProgress();

            _game.Reset();
            string answer = _game.GetAnswer();
            string progress = _game.GetProgress();

            Assert.Multiple(() =>
            {
                Assert.That(answerBeforeReset, Is.Not.EqualTo(answer));
                Assert.That(progressBeforeReset, Is.Not.EqualTo(progress));
                Assert.That(_game.Turns, Is.Zero);
                Assert.That(_game.GameFinished, Is.False);
            });

        }




        [Test]
        public void Save_Data_To_File()
        {
            string result = "";

            _fakeIOWrapper.Setup(io => io.AppendAllText(It.IsAny<string>(), It.IsAny<string>()))
                          .Callback<string, string>((_, text) =>
                          {
                              result = text;
                          });


            string answer = _game.GetAnswer();
            _game.MakeGuess(answer);
            _game.SaveScore();

            string actual = result.Trim(); 

            Assert.That(actual, Is.EqualTo("Robin#&#1"));

        }


        [Test]
        public void Load_Players_From_File()
        {


            _fakeIOWrapper.Setup(io => io.ReadAllLines(It.IsAny<string>()))
                          .Returns(MockTextFile().ToArray());


            var players = _game.GetPlayers();


            Assert.That(players, Has.Count.EqualTo(2));

        }

        [Test]
        public void Loaded_Player_Has_Correct_Total_Guesses()
        {
            _fakeIOWrapper.Setup(io => io.ReadAllLines(It.IsAny<string>()))
                          .Returns(MockTextFile().ToArray());


            var players = _game.GetPlayers();
            var totalGuesses = players.First(player => player.Name == "Robin").TotalGuesses;

            Assert.That(totalGuesses, Is.EqualTo(8));

        }


        [Test]
        public void Loaded_Player_Has_Correct_Games_Played()
        {
            _fakeIOWrapper.Setup(io => io.ReadAllLines(It.IsAny<string>()))
                          .Returns(MockTextFile().ToArray());


            var players = _game.GetPlayers();
            var totalGuesses = players.First(player => player.Name == "Robin").TotalGuesses;

            Assert.That(totalGuesses, Is.EqualTo(8));

        }

        private List<string> MockTextFile()
        {
            return new()
            {
                "Robin#&#2",
                "Robin#&#2",
                "Robin#&#2",
                "Robin#&#2",
                "RF#&#2",
                "RF#&#1",
                "RF#&#2",
            };
        }



        [Test]
        public void Player_With_Lowest_Average_Is_Leader()
        {
            ConsoleUI ui = new ConsoleUI();


            _fakeIOWrapper.Setup(io => io.ReadAllLines(It.IsAny<string>()))
                             .Returns(MockTextFile().ToArray());

            var players = _game.GetPlayers();

            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                ui.ShowHiscores(players);
                string[] linesPrinted = sw.ToString().Split(Environment.NewLine);

                bool actual = linesPrinted[2].StartsWith("RF");


                Assert.That(actual, Is.True);

            }

        }



        private string LastAndFirstCharactersWrong()
        {
            string answer = _game.GetAnswer();
            StringBuilder guessBuilder = new(answer);

            char first = answer.First();
            char last = answer.Last();

            guessBuilder[^1] = first;
            guessBuilder[0] = last;

            return guessBuilder.ToString();

        }



    }
}
