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

        public List<PlayerData> GetPlayerData()
        {
            List<PlayerData> result = new List<PlayerData>();
            var fileContents = File.ReadAllLines(_dataFile)
                                   .Where(line => !string.IsNullOrWhiteSpace(line))
                                   .ToArray();


            for (int i = 0; i < fileContents.Length; i++)
            {
                try
                {

                    string[] data = fileContents[i].Split("#&#");
                    string name = data[0];
                    int gamesPlayed = int.Parse(data[1]);
                    int totalGuesses = int.Parse(data[2]);
                    result.Add(new PlayerData(name, gamesPlayed, totalGuesses));
                }
                catch (FormatException)
                {
                    continue;
                }
            }

            return result;


        }


        public void SavePlayerData(PlayerData currentPlayer)
        {
            var fileContents = File.ReadAllLines(_dataFile).ToList();
            var playerData = fileContents.FirstOrDefault(line => line.StartsWith(currentPlayer.Name));
            
            if (playerData != null)
            {
                int lineNumber = fileContents.IndexOf(playerData);
                fileContents[lineNumber] = currentPlayer.ToString();
                File.WriteAllLines(_dataFile, fileContents);
            }
            else
            {
                File.AppendAllText(_dataFile, currentPlayer.ToString() + Environment.NewLine);
            }
        }

    }
}
