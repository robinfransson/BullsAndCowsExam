using System.Collections.Generic;

namespace GameEngine
{
    public interface IGameIO
    {
        List<Player> LoadPlayerData();
        void SavePlayerData(string name, int guesses);
    }
}
