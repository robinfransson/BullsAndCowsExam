using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GameEngine
{
    public class PlayerData : IPlayerData
    {
        [JsonInclude]
        public string Name { get; private set; }

        [JsonInclude]
        public int TotalGuesses { get; private set; }

        [JsonInclude]
        public int GamesPlayed { get; private set; }

        public double AverageGuesses => (double)TotalGuesses / GamesPlayed;

        public PlayerData()
        {

        }


        public PlayerData(string name)
        {
            Name = name;
        }

        public void Update(int guesses)
        {
            TotalGuesses += guesses;
            GamesPlayed++;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as PlayerData);

        }

        private bool Equals(PlayerData otherData)
        {
            return otherData != null &&
                   otherData.TotalGuesses == this.TotalGuesses &&
                   otherData.GamesPlayed == this.GamesPlayed &&
                   otherData.Name == this.Name;
        }


        public override int GetHashCode()
        {
            return HashCode.Combine(Name, TotalGuesses, GamesPlayed);
        }
    }
}
