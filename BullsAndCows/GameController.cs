using GameEngine;

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
            _ui.PutString($"Welcome to {_game.GameName}");
            _ui.PutString("Enter your name:");

            string name = _ui.GetString();
            _game.SetupGame(name);
            _ui.PutString("Welcome " + _game.GetPlayerName());
            do
            {

                _ui.PutString("For testing: answer is " + _game.GetAnswer());

                StartGame();

                _ui.PutString($"Completed the game in {_game.Turns} turns! Well done.");
                _ui.PutString(_game.OnFinish());
            }
            while (Playing());
        }

        private void StartGame()
        {
            while (!_game.GameFinished)
            {
                _ui.PutString("Enter a guess: ");
                string guess = _ui.GetString();
                bool validInput = _game.ValidateInput(guess);

                if (!validInput)
                    continue;

                string result = _game.CheckAnswer();
                _ui.PutString(result);
            }
        }

        private bool Playing()
        {
            if (!_ui.Continue())
                return false;

            _game.ResetGame();
            return true;
        }
    }
}
