using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GameEngine
{
    public class PlayerData
    {
        public string Name { get; private set; }
        public int TotalGuesses { get; private set; }
        public int GamesPlayed { get; private set; }

        public double AverageGuesses => Math.Round((double)TotalGuesses / GamesPlayed, 2);


        public PlayerData(string name)
        {
            Name = name;
        }

        public PlayerData(string name, int gamesPlayed, int totalGuesses)
        {
            Name = name;
            GamesPlayed = gamesPlayed;
            TotalGuesses = totalGuesses;
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

        private bool Equals(PlayerData other)
        {
            return other != null &&
                   other.TotalGuesses == this.TotalGuesses &&
                   other.GamesPlayed == this.GamesPlayed &&
                   other.Name == this.Name;
        }

        public override string ToString()
        {
            return $"{Name}#&#{TotalGuesses}";
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, TotalGuesses, GamesPlayed);
        }
    }
}
