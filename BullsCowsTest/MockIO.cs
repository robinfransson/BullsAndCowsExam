using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullsCowsTest
{
    internal class MockIO : IGameIO
    {
        public List<PlayerData> Data = new();
        public List<PlayerData> GetPlayerData()
        {
            return Data;
        }

        public void SavePlayerData(PlayerData playerData)
        {
            Data.Add(playerData);
        }
    }
}
