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
        protected List<PlayerData> HiScores { get; set; }
        protected PlayerData CurrentPlayer { get; set; }
        protected bool ReturningPlayer(string playerName) => HiScores is not null && 
                                                             HiScores.Any(p => p.Name == playerName);
        public GameBase(IGameIO gameIO)
        {
            IO = gameIO;
            LoadHiscores();
        }

        protected string GetTopList()
        {
            var builder = new StringBuilder();
            var playersOrderedByGuesses = HiScores.OrderByDescending(player => player.GamesPlayed)
                                                  .ThenByDescending(player => player.AverageGuesses);

            builder.AppendLine("Current leaderboard:");
            builder.AppendLine(string.Format("|{0,10}|{1,5}|{2,7}|{3,7}|", "Name", "Games", "Guesses", "Average"));

            foreach (var player in playersOrderedByGuesses)
            {
                builder.AppendLine(string.Format("|{0,10}|{1,5}|{2,7}|{3,7}|", 
                                   player.Name, player.GamesPlayed, player.TotalGuesses, player.AverageGuesses));
            }

            return builder.ToString();
        }



        protected void Setup(string playerName)
        {
            if (!ReturningPlayer(playerName))
                AddPlayer(playerName);
            else
                CurrentPlayer = HiScores.First(player => player.Name == playerName);

        }

        protected void LoadHiscores()
        {
            HiScores = IO.GetPlayerData();

            if (HiScores is null)
                HiScores = new();
        }

        protected void SaveHiscores(int guesses)
        {
            IO.SavePlayerData(CurrentPlayer.Name, guesses);
            CurrentPlayer.Update(guesses);
        }

        protected void AddPlayer(string playerName)
        {
            CurrentPlayer = new PlayerData(playerName);
            HiScores.Add(CurrentPlayer);
        }


    }
}
