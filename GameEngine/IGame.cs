using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public interface IGame
    {
        string GameName { get; }
        int Turns { get; }
        bool GameFinished { get; }
        string GetAnswer();
        string GetPlayerName();
        string CheckAnswer();
        void SetupGame(string playerName);
        bool ValidateInput(string input);
        string OnFinish();
        void ResetGame();

    }
}
