using System.Collections.Generic;

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
