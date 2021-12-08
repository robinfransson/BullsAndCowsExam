using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullsCowsTest.Mocks
{
    internal class MockIO : IGameIO
    {
        public List<PlayerData> Data = new();
        public List<PlayerData> GetPlayerData()
        {
            return Data;
        }

        public void SavePlayerData(string name, int guesses)
        {
            var currentPlayer = Data.FirstOrDefault(player => player.Name == name);
            if (currentPlayer != null)
                currentPlayer.Update(guesses);
            else
                Data.Add(new PlayerData(name,1, guesses));
        }
    }
}
