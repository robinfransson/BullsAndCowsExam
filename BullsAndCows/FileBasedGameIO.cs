using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.IO;

namespace GameEngine
{
    public class FileBasedGameIO : IGameIO
    {
        private readonly string _dataFile;
        private readonly IFileIOWrapper _ioWrapper;

        public FileBasedGameIO(string dataFile, IFileIOWrapper wrapper)
        {
            _dataFile = dataFile;
            _ioWrapper = wrapper;

        }

        private void MakeSureFileExist()
        {
            if (!_ioWrapper.Exists(_dataFile))
            {
                _ioWrapper.Create(_dataFile);
            }
        }



        public List<PlayerData> GetPlayerData()
        {
            List<PlayerData> players = new();

            var groupedByUsername = _ioWrapper.ReadAllLines(_dataFile)
                                   .Where(line => !string.IsNullOrWhiteSpace(line))
                                   .GroupBy(Username);

                                            


            foreach(var playerData in groupedByUsername)
            {
                string name = playerData.Key;
                int gamesPlayed = playerData.Count();
                int totalGuesses = playerData.Sum(Guesses);

                var player = new PlayerData(name, gamesPlayed, totalGuesses);

                players.Add(player);
            }

            return players;


        }


        public void SavePlayerData(string name, int guesses)
        {
            var data = $"{name}#&#{guesses}";


            MakeSureFileExist();

            _ioWrapper.AppendAllText(_dataFile, data + Environment.NewLine);
        }


        private string Username(string line) => line.Split("#&#")[0];

        private int Guesses(string line)
        {
            try
            {
                string guesses = line.Split("#&#")[1];
                return int.Parse(guesses);
            }
            catch(Exception)
            {
                return 0;
            }
        }


    }
}
