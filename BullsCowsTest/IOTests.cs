using BullsAndCows.IO;
using GameEngine;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    internal class IOTests
    {
        private IGameIO _gameIO;
        private Mock<IFileIOWrapper> _fakeIOWrapper;

        [SetUp]
        public void Setup()
        {
            _fakeIOWrapper = new Mock<IFileIOWrapper>();
            _gameIO = new FileBasedGameIO("bullsandcows.txt", _fakeIOWrapper.Object);
        }






        [Test]
        [TestCase("Robin", 5, "Robin#&#5")]
        [TestCase("RF", 1000, "RF#&#1000")]
        [TestCase("Robin", 123, "Robin#&#123")]
        [TestCase("RF2", 23, "RF2#&#23")]
        [TestCase("RF3", 59, "RF3#&#59")]
        public void Save_Data_To_File(string playerName, int guesses, string expected)
        {
            string result = "";


            _fakeIOWrapper.Setup(io => io.AppendToFile(It.IsAny<string>(), It.IsAny<string>()))
                          .Callback<string, string>((_, text) => result = text.Trim());


            _gameIO.SavePlayerData(playerName, guesses);

            Assert.That(result, Is.EqualTo(expected));

        }










        [Test]
        [TestCase(new string[] { "Robin#&#5", "RF#&#3", "Robban#&#2" }, 3)]
        [TestCase(new string[] { "Robin#&#5", "Robin#&#5", "Robin#&#10" }, 1)]
        [TestCase(new string[] { "Robin#&#5", "RF2#&#5", "RF#&#10", "RobinF#&#10" }, 4)]
        public void Load_Players_From_File(string[] lines, int expected)
        {


            _fakeIOWrapper.Setup(io => io.ReadFile(It.IsAny<string>()))
                          .Returns(lines);


            List<Player> players = _gameIO.LoadPlayerData();


            Assert.That(players, Has.Count.EqualTo(expected));

        }









        [Test]
        [TestCase("Robin", new string[] { "Robin#&#5", "Robin#&#3", "Robin#&#2"}, 10)]
        [TestCase("Robin", new string[] { "Robin#&#5", "Robin#&#5", "Robin#&#10"}, 20)]
        [TestCase("Robin", new string[] { "Robin#&#2", "Robin#&#4", "Robin#&#10"}, 16)]
        public void Loaded_Player_Has_Correct_Total_Guesses(string playerName, string[] saveFile, int expected)
        {
            _fakeIOWrapper.Setup(io => io.ReadFile(It.IsAny<string>()))
                          .Returns(saveFile);


            List<Player> players = _gameIO.LoadPlayerData();
            int totalGuesses = players.First(player => player.Name == playerName).TotalGuesses;

            Assert.That(totalGuesses, Is.EqualTo(expected));

        }







        [Test]
        [TestCase("Robin", new string[] {"Robin#&#2", "Robin#&#1"}, 2)]
        [TestCase("Robin", new string[] {"Robin#&#2", "Robin#&#1", "Robin#&#5" }, 3)]
        [TestCase("Robin", new string[] {"Robin#&#2", "Robin#&#1", "Robin#&#1", "Robin#&#1", "Robin#&#1" }, 5)]
        [TestCase("Robin", new string[] {"Robin#&#2", "Robin#&#1", "Robin#&#1", "Robin#&#1", "Robin#&#1", "Robin#&#1", "Robin#&#1" }, 7)]
        public void Loaded_Player_Has_Correct_Games_Played(string playerName, string[] lines, int expected)
        {
            _fakeIOWrapper.Setup(io => io.ReadFile(It.IsAny<string>()))
                          .Returns(lines);


            List<Player> players = _gameIO.LoadPlayerData();
            int totalGuesses = players.First(player => player.Name == playerName).GamesPlayed;
            

            Assert.That(totalGuesses, Is.EqualTo(expected));

        }







        [Test]
        public void Empty_Savefile_Returns_Empty_List()
        {
            _fakeIOWrapper.Setup(io => io.ReadFile(It.IsAny<string>()))
                          .Returns(new[] { "" });



            List<Player> players = _gameIO.LoadPlayerData();



            Assert.That(players, Has.Count.EqualTo(0));

        }






        [Test]
        public void File_Not_Created_If_Exists()
        {
            _fakeIOWrapper.Setup(io => io.CreateFile(It.IsAny<string>()));
            _fakeIOWrapper.Setup(io => io.FileExists(It.IsAny<string>()))
                          .Returns(true);

            _gameIO.LoadPlayerData();


            _fakeIOWrapper.Verify(mock => mock.FileExists(It.IsAny<string>()), Times.Never());



        }






        [Test]
        public void File_Created_If_Not_Existing()
        {

            _fakeIOWrapper.Setup(io => io.FileExists(It.IsAny<string>()))
                          .Returns(false);

            _fakeIOWrapper.Setup(io => io.CreateFile(It.IsAny<string>()));

            _gameIO.SavePlayerData("Robin",5);

            _fakeIOWrapper.Verify(mock => mock.CreateFile(It.IsAny<string>()), Times.Once());



        }




        [Test]
        [TestCase(new string[] { "Robin#&#", "Robin#&#", "Robin#&#"}, 0)] 
        [TestCase(new string[] { "Robin#&#2", "Robin#&#", "Robin#&#1"}, 3)] 
        [TestCase(new string[] { "RF#&#5", "RF#&#1", "Robin#&#"}, 6)] 
        [TestCase(new string[] { "RF#&#5", "RF#1", "Robin"}, 5)] 
        public void Line_Without_Guess_Count_Returns_Zero(string[] lines, int expected)
        {
            _fakeIOWrapper.Setup(wrapper => wrapper.ReadFile(It.IsAny<string>())).Returns(lines);

            var players = _gameIO.LoadPlayerData();

            int totalGuesses = players.Sum(x => x.TotalGuesses);

            Assert.That(totalGuesses, Is.EqualTo(expected));



        }


    }
}
