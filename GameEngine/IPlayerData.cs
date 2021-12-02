using System;

namespace GameEngine
{
	public interface IPlayerData 
    {
		string Name { get; }
		int TotalGuesses { get; }
		int GamesPlayed { get; }
		double AverageGuesses { get; }
		void Update(int guesses);
		int GetHashCode();
	}
}