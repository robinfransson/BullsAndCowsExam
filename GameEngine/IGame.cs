using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public interface IGame
    {
        int Turns { get; }
        bool GameFinished { get; }
        void SetPlayerName(string name);
        string GetAnswer();
        string GetProgress();
        void SetupGame();
        void MakeGuess(string input);
        List<Player> GetPlayers();
        void Reset();
        void SaveScore();

    }
}
