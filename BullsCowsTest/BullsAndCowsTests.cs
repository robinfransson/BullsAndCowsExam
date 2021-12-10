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

            Assert.That(answerBeforeReset, Is.Not.EqualTo(answer));
            Assert.That(progressBeforeReset, Is.Not.EqualTo(progress));
            Assert.That(_game.Turns, Is.Zero);
            Assert.That(_game.GameFinished, Is.False);

        }




        [Test]
        public void Save_Data_To_File()
        {
            string result = "";

            _fakeIOWrapper.Setup(io => io.AppendAllText(It.IsAny<string>(), It.IsAny<string>()))
                          .Callback<string, string>((_, text) =>
                          {
                              result = text.Trim();
                          });


            string answer = _game.GetAnswer();
            _game.MakeGuess(answer);
            _game.SaveScore();

            Assert.That(result, Is.EqualTo("Robin#&#1"));

        }


        [Test]
        public void Load_Players_From_File()
        {


            _fakeIOWrapper.Setup(io => io.ReadAllLines(It.IsAny<string>()))
                          .Returns(FakeTextFile().ToArray());


            var players = _game.GetPlayers();


            Assert.That(players, Has.Count.EqualTo(3));

        }




        [Test]
        public void Loaded_Player_Has_Correct_Total_Guesses()
        {
            _fakeIOWrapper.Setup(io => io.ReadAllLines(It.IsAny<string>()))
                          .Returns(FakeTextFile().ToArray());


            var players = _game.GetPlayers();
            var totalGuesses = players.First(player => player.Name == "Robin").TotalGuesses;

            Assert.That(totalGuesses, Is.EqualTo(8));

        }


        [Test]
        public void Loaded_Player_Has_Correct_Games_Played()
        {
            _fakeIOWrapper.Setup(io => io.ReadAllLines(It.IsAny<string>()))
                          .Returns(FakeTextFile().ToArray());


            var players = _game.GetPlayers();
            var totalGuesses = players.First(player => player.Name == "Robin").GamesPlayed;

            Assert.That(totalGuesses, Is.EqualTo(4));

        }

        private List<string> FakeTextFile()
        {
            return new()
            {
                "Robin#&#2",
                "Robin#&#2",
                "Robin#&#2",
                "Robin#&#2",
                "RF#&#1",
                "RF#&#1",
                "RF#&#1",
                "RF2#&#10",
                "RF2#&#20",
                "RF2#&#15",
                "RF2#&#15",
            };
        }

        private GameController CreateController(IGame game, IGameUI ui) => new(game, ui);

        [Test]
        public void Player_With_Lowest_Average_Is_Leader()
        {
            PlayerData expected = new("RF", 3, 3);
            PlayerData leader = null;
            IGameUI ui = _fakeUI.Object;
            
            _fakeUI.Setup(ui => ui.GetInput()).Returns("");
            _fakeUI.Setup(ui => ui.Continue()).Returns(false);

            _fakeUI.Setup(ui => ui.ShowHiscores(It.IsAny<IEnumerable<PlayerData>>()))
                   .Callback<IEnumerable<PlayerData>>((players) => leader = players.First());

            _fakeIOWrapper.Setup(io => io.ReadAllLines(It.IsAny<string>()))
                          .Returns(FakeTextFile().ToArray());

            _fakeGame.Setup(game => game.GetPlayers()).Returns(() => _gameIO.GetPlayerData());
            _fakeGame.Setup(game => game.GameFinished).Returns(true);

            var controller = CreateController(_fakeGame.Object, _fakeUI.Object);

            controller.Run();


            Assert.That(leader, Is.EqualTo(expected));

        }

        [Test]
        public void Can_Play_One_Round()
        {
            var fakeFileContents = FakeTextFile();
            string answer = "1234";
            string guess = "";
            string playerName = "";
            string output = "";


            _fakeGame.Setup(game => game.SetPlayerName(It.IsAny<string>()))
                     .Callback<string>((name) => playerName = name);


            _fakeGame.Setup(game => game.MakeGuess(It.IsAny<string>()))
                     .Callback<string>((input) => guess = input);
            
            
            _fakeGame.Setup(game => game.GetAnswer())
                     .Returns(answer);
            
            
            _fakeGame.Setup(game => game.GameFinished)
                     .Returns(() => guess == "1234");
            
            
            _fakeGame.Setup(game => game.SetPlayerName(It.IsAny<string>()))
                     .Callback<string>((name) => playerName = name);

            
            _fakeGame.Setup(game => game.GetProgress())
                     .Returns("");
            
            _fakeGame.Setup(game => game.GetPlayers())
                     .Returns(_game.GetPlayers());

            _fakeUI.Setup(ui => ui.Output(It.IsAny<string>())).Callback<string>((line) => output += line);

            _fakeUI.Setup(ui => ui.ShowHiscores(It.IsAny<IEnumerable<PlayerData>>()));


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
