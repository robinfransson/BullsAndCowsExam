using BullsAndCows;
using BullsAndCows.IO;
using GameEngine;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace Tests
{
    internal class WordGuessingGameTests
    {
        private IGame _game;
        private IGameIO _gameIO;
        private Mock<IGameIO> _fakeGameIO;
        private Mock<IFileIOWrapper> _fakeWrapper;
        
        
        
        
        
        [SetUp]
        public void Setup()
        {
            _fakeWrapper = new Mock<IFileIOWrapper>();
            _fakeGameIO = new Mock<IGameIO>();
            _game = new WordGuessingGame(_fakeGameIO.Object);
        }


        [Test]
        [TestCase("hej", "hejsan", false)]
        [TestCase("potais", "potatis", true)]
        [TestCase("ankodn", "anakonda", true)]
        [TestCase("rulltrappa", "rultrap", true)]
        public void Game_Is_Finished_When_Correct_Answer_Is_Given(string guess, string answer, bool expected)
        {

            _game = new WordGuessingGame(_gameIO, answer);

            _game.MakeGuess(guess);

            bool gameCompleted = _game.GameFinished;

            Assert.That(gameCompleted, Is.EqualTo(expected));

        }





        [Test]
        [TestCase("hej", "hejsan", "hej___")]
        [TestCase("potis", "potatis", "pot_tis")]
        [TestCase("nkd", "anakonda", "_n_k_nd_")]
        public void Should_Return_Part_Of_Answer(string guess, string answer, string expected)
        {

            _game = new WordGuessingGame(_gameIO, answer);

            _game.MakeGuess(guess);
            string progress = _game.GetProgress();

            Assert.That(progress, Is.EqualTo(expected));

        }





        [Test]
        [TestCase("Robin")]
        [TestCase("RF")]
        public void Save_Has_Player_Name(string playerName)
        {
            string result = "";
            _fakeGameIO.Setup(io => io.SavePlayerData(It.IsAny<string>(), It.IsAny<int>()))
                       .Callback<string, int>((player, _) => result = player);


            _game.SetPlayerName(playerName);
            _game.SaveScore();

            Assert.That(result, Is.EqualTo(playerName));
        }





        [Test]
        [TestCase("abcde", 5)]
        [TestCase("test", 4)]
        [TestCase("ghwd", 4)]
        [TestCase("ahgxjfswe", 9)]
        [TestCase("a", 1)]
        [TestCase("", 0)]
        public void Save_Should_Have_Guess_Count(string guess, int expected)
        {
            _game.SetupGame();
            _game.MakeGuess(guess);
            
            int guesses = _game.Turns;

            Assert.That(guesses, Is.EqualTo(expected));
        }




        [Test]
        [TestCase(new string[] { "RF#&#1", "Robin#&#2", "RF2#&#1" }, 3)]
        [TestCase(new string[] { "RF#&#1"}, 1)]
        [TestCase(new string[] { "" }, 0)]
        public void Load_Savefile_Returns_Players_If_File_Is_Populated(string[] lines, int expected)
        {
            _gameIO = new FileBasedGameIO("wordguess.txt", _fakeWrapper.Object);
            _game = new WordGuessingGame(_gameIO);

            _fakeWrapper.Setup(wrapper => wrapper.ReadFile(It.IsAny<string>())).Returns(lines);

            List<Player> players = _game.GetPlayers();

            Assert.That(players, Has.Count.EqualTo(expected));
        }


    }
}
