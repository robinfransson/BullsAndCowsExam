using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public interface IGameUI
    {
        void Output(string s);
        string GetInput();
        bool Continue();
        void ShowHiscores(IEnumerable<Player> hiscores);
        void Clear();
    }
}
