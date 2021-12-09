using GameEngine;
using System.ComponentModel;

namespace BullsAndCows
{
    public class GameController
    {
        private readonly IGame _game;
        private readonly IUI _ui;
        public GameController(IGame game, IUI ui)
        {
            _game = game;
            _ui = ui;
        }

        public void Run()
        {
            _ui.Output("Enter your user name:");

            string name = _ui.GetInput();
            _game.SetPlayerName(name);
            do
            {
                _game.SetupGame();
                _ui.Output("For testing: answer is " + _game.GetAnswer());
                PlayGame();

            }
            while (Playing());
        }

        private void PlayGame()
        {
            while (!_game.GameFinished)
            {
                _ui.Output("Enter a guess: ");
                string input = _ui.GetInput();
                _game.MakeGuess(input);
                string progress = _game.GetProgress();
                _ui.Output(progress);
            }

            _ui.Output($"Completed the game in {_game.Turns} turns!");
            _game.SaveScore();

            var scores = _game.GetHiscores();
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
