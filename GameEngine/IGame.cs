using System.Collections.Generic;

namespace GameEngine
{
    public interface IGame
    {
        string GameName { get; }
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
