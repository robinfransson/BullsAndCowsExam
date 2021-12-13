using GameEngine;
using System;
using System.Linq;

namespace BullsAndCows
{
    public class GameController
    {
        private readonly IGame _game;
        private readonly IGameUI _ui;
        public GameController(IGame game, IGameUI ui)
        {
            _game = game;
            _ui = ui;
        }

        public void Start()
        {
            _ui.Output($"Welcome to {_game.GameName}");
            SetUsername();
            do
            {
                _ui.Clear();
                _ui.Output("For testing: answer is " + _game.GetAnswer());
                PlayGame();
            }
            while (Playing());
        }

        private void SetUsername()
        {
            _ui.Output("Enter your user name:");

            string name = _ui.GetInput();
            _game.SetPlayerName(name);
            _game.SetupGame();
        }

        private void PlayGame()
        {
            while (!_game.GameFinished)
            {
                _ui.Output("Enter a guess: ");
                string guess = _ui.GetInput();
                _game.MakeGuess(guess);
                string progress = _game.GetProgress();
                _ui.Output(progress);
            }

            _ui.Output($"Completed the game in {_game.Turns} turns!");
            _game.SaveScore();
            ShowHiscores();
        }

        private void ShowHiscores()
        {
            var scores = _game.GetPlayers()
                              .OrderBy(x => x.AverageGuesses);

            _ui.ShowHiscores(scores);
        }

        private bool Playing()
        {
            if (!_ui.Continue())
                return false;

            _game.Reset();
            return true;
        }
    }
}
