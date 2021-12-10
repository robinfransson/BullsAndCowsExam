using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GameEngine
{
    public class Player
    {
        public string Name { get; private set; }
        public int TotalGuesses { get; private set; }
        public int GamesPlayed { get; private set; }

        public double AverageGuesses => Math.Round((double)TotalGuesses / GamesPlayed, 2);




        public Player(string name, int gamesPlayed, int guesses)
        {
            Name = name;
            TotalGuesses = guesses;
            GamesPlayed = gamesPlayed;
        }

        

        public override bool Equals(object obj)
        {
            return Equals(obj as Player);

        }



        private bool Equals(Player other)
        {
            return other != null &&
                   other.TotalGuesses == this.TotalGuesses &&
                   other.GamesPlayed == this.GamesPlayed &&
                   other.Name == this.Name;
        }




        public override int GetHashCode()
        {
            return HashCode.Combine(Name, TotalGuesses, GamesPlayed);
        }
    }
}
