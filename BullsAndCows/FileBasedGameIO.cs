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


        public FileBasedGameIO(string dataFile)
        {
            _dataFile = dataFile;
            MakeSureFileExist();

        }

        private void MakeSureFileExist()
        {
            if (!File.Exists(_dataFile))
            {
                File.Create(_dataFile)
                    .Close();
            }
        }



        public List<PlayerData> GetPlayerData()
        {
            List<PlayerData> players = new();

            var fileContents = File.ReadAllLines(_dataFile);

            var groupedByName = fileContents.Where(line => !string.IsNullOrWhiteSpace(line))
                                            .GroupBy(GetPlayerName);


            foreach(var group in groupedByName)
            {
                string name = group.Key;
                int gamesPlayed = group.Count();
                int totalGuesses = group.Sum(x => GetGuesses(x));

                var player = new PlayerData(group.Key, gamesPlayed, totalGuesses);

                players.Add(player);
            }

            return players;


        }


        public void SavePlayerData(string name, int guesses)
        {
            var playerData = $"{name}#&#{guesses}";
            File.AppendAllText(_dataFile, playerData + Environment.NewLine);
        }


        private string GetPlayerName(string line) => line.Split("#&#")[0];

        private int GetGuesses(string line)
        {
            string guesses = line.Split("#&#")[1];

            if (int.TryParse(guesses, out int result))
            {
                return result;
            }

            return 0;
        }


    }
}
