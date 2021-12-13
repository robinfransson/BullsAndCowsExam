using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BullsAndCows.IO
{
    public class FileBasedGameIO : IGameIO
    {
        private readonly string _saveFile;
        private readonly IFileIOWrapper _wrapper;

        public FileBasedGameIO(string saveFile, IFileIOWrapper wrapper)
        {
            _saveFile = saveFile;
            _wrapper = wrapper;

        }




        public List<Player> LoadPlayerData()
        {
            List<Player> players = new();

            var groupedByUsername = _wrapper.ReadFile(_saveFile)
                                            .Where(line => !string.IsNullOrWhiteSpace(line))
                                            .GroupBy(UsernameFromFile);

                                            


            foreach(var group in groupedByUsername)
            {
                string name = group.Key;
                int gamesPlayed = group.Count();
                int totalGuesses = group.Sum(GuessesFromFile);

                var player = new Player(name, gamesPlayed, totalGuesses);

                players.Add(player);
            }

            return players;


        }


        public void SavePlayerData(string name, int guesses)
        {
            string data = $"{name}#&#{guesses}";

            MakeSureFileExists();

            _wrapper.AppendToFile(_saveFile, data + Environment.NewLine);
        }

        private void MakeSureFileExists()
        {
            if (!_wrapper.FileExists(_saveFile))
            {
                _wrapper.CreateFile(_saveFile);
            }
        }

        private string UsernameFromFile(string line) => line.Split("#&#")[0];

        private int GuessesFromFile(string line)
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
