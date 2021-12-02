using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public abstract class GameBase
    {
        protected IGameIO IO { get; set; }
        protected List<PlayerData> PlayerData { get; set; }

        protected PlayerData CurrentPlayer { get; set; }

        public GameBase(IGameIO gameIO)
        {
            IO = gameIO;
            LoadHiscores();
        }

        protected string GetTopList()
        {
            var builder = new StringBuilder();
            var orderedByGuesses = PlayerData.OrderByDescending(player => player.AverageGuesses);

            builder.AppendLine("Current leaderboard:");
            builder.AppendLine(string.Format("|{0,10}|{1,5}|{2,7}|{3,7}|", "Name", "Games", "Guesses", "Average"));

            foreach (var stats in orderedByGuesses)
            {
                builder.AppendLine(string.Format("|{0,10}|{1,5}|{2,7}|{3,7}|", stats.Name, stats.GamesPlayed, stats.TotalGuesses, stats.AverageGuesses));
            }

            return builder.ToString();
        }

        protected void LoadHiscores()
        {
            PlayerData = IO.GetPlayerData();

            if(PlayerData is null)
                PlayerData = new();
        }

        protected void Setup(string playerName)
        {
            if (!ReturnedPlayer(playerName))
                AddPlayer(playerName);
            else
                CurrentPlayer = PlayerData.First(player => player.Name == playerName);

        }

        private bool ReturnedPlayer(string playerName) => PlayerData is not null && PlayerData.Any(p => p.Name == playerName);

        protected void SaveHiscores()
        {
            IO.SavePlayerData(PlayerData);
        }

        protected void AddPlayer(string playerName)
        {
            var player = new PlayerData(playerName);
            CurrentPlayer = player;
            PlayerData.Add(player);
        }


    }
}
