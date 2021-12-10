using BullsAndCows;
using BullsAndCows.IO;
using GameEngine;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
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
        public void Save_Data_To_File()
        {
            string result = "";


            _fakeIOWrapper.Setup(io => io.AppendToFile(It.IsAny<string>(), It.IsAny<string>()))
                          .Callback<string, string>((_, text) =>
                          {
                              result = text.Trim();
                          });


            _gameIO.SavePlayerData("Robin", 1);

            Assert.That(result, Is.EqualTo("Robin#&#1"));

        }


        [Test]
        public void Load_Players_From_File()
        {


            _fakeIOWrapper.Setup(io => io.ReadFile(It.IsAny<string>()))
                          .Returns(FakeData.TextFile);


            List<Player> players = _gameIO.GetPlayerData();


            Assert.That(players, Has.Count.EqualTo(3));

        }




        [Test]
        public void Loaded_Player_Has_Correct_Total_Guesses()
        {
            _fakeIOWrapper.Setup(io => io.ReadFile(It.IsAny<string>()))
                          .Returns(FakeData.TextFile);


            List<Player> players = _gameIO.GetPlayerData();
            int totalGuesses = players.First(player => player.Name == "Robin").TotalGuesses;

            Assert.That(totalGuesses, Is.EqualTo(8));

        }


        [Test]
        public void Loaded_Player_Has_Correct_Games_Played()
        {
            _fakeIOWrapper.Setup(io => io.ReadFile(It.IsAny<string>()))
                          .Returns(FakeData.TextFile);



            List<Player> players = _gameIO.GetPlayerData();
            int totalGuesses = players.First(player => player.Name == "Robin").GamesPlayed;
            


            Assert.That(totalGuesses, Is.EqualTo(4));

        }

        [Test]
        public void File_Not_Created_If_Exists()
        {
            _fakeIOWrapper.Setup(io => io.CreateFile(It.IsAny<string>()));
            _fakeIOWrapper.Setup(io => io.FileExists(It.IsAny<string>()))
                          .Returns(true);

            _gameIO.GetPlayerData();


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


    }
}
