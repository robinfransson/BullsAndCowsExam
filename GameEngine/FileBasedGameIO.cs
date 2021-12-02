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
            try
            {
            var fileContents = File.ReadAllText(_dataFile);
            return JsonSerializer.Deserialize<List<PlayerData>>(fileContents);

            }
            catch(JsonException)
            {
                return new();
            }
        }


        public void SavePlayerData(IEnumerable<PlayerData> playerData)
        {
            var playerDataJson = JsonSerializer.Serialize(playerData);
            File.WriteAllText(_dataFile, playerDataJson);
        }
    }
}
