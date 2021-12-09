﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public interface IUI
    {
        void Output(string s);
        string GetInput();
        bool Continue();
        void ShowHiscores(IEnumerable<PlayerData> hiscores);
        void Clear();
    }
}
