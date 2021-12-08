﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public interface IGameIO
    {
        List<PlayerData> GetPlayerData();
        void SavePlayerData(PlayerData playerData);
    }
}
