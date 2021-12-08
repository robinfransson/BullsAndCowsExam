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
                File.Create(_dataFile)
                    .Close();
        }
        static int Guesses(string line)
        {
            string guesses = line.Split("#&#")[1];
            if (int.TryParse(guesses, out int result))
            {
                return result;
            }
            return 0;
        }

        static string PlayerName(string line) => line.Split("#&#")[0];

        public List<PlayerData> GetPlayerData()
        {
            List<PlayerData> result = new List<PlayerData>();

            var fileContents = File.ReadAllLines(_dataFile)
                                   .Where(line => !string.IsNullOrWhiteSpace(line))
                                   .GroupBy(PlayerName);


            foreach(var group in fileContents)
            {
                string name = group.Key;
                int gamesPlayed = group.Count();
                int totalGuesses = group.Sum(x => Guesses(x));
                var player = new PlayerData(group.Key, gamesPlayed, totalGuesses);

                result.Add(player);
            }

            return result;


        }


        public void SavePlayerData(string name, int guesses)
        {
            var playerData = $"{name}#&#{guesses}";
            File.AppendAllText(_dataFile, playerData + Environment.NewLine);
        }

    }
}
