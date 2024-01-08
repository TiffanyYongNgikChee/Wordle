using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Wordle.ViewModel
{
    public partial class StatsViewModel : ObservableObject
    {
        // Observable properties for data binding
        [ObservableProperty]
        private int gamesWon;
        [ObservableProperty]
        private int gamesPlayed;
        [ObservableProperty]
        private float averageAttemptOfSuccess;
        [ObservableProperty]
        private string gameWinPercentage;

        // Constructor to initialize properties from saved preferences
        public StatsViewModel()
        {
            gamesWon = Preferences.Default.Get("wonGames", 0);
            gamesPlayed = Preferences.Default.Get("playedGames", 0);
            averageAttemptOfSuccess = Preferences.Default.Get("averageAttemptOfSuccess", 0f);
            // Calculate and set initial game win percentage
            gameWinPercentage = (gamesPlayed == 0 ? 0 : (float)gamesWon / gamesPlayed * 100).ToString() + "%";
        }

        // Method to increment the number of games won
        public void IncrementGamesWon()
        {
            Preferences.Default.Set("wonGames", ++gamesWon);
        }
        // Method to increment the number of games played and update win percentage
        public void IncrementGamesPlayed()
        {
            Preferences.Default.Set("playedGames", ++gamesPlayed);
            gameWinPercentage = ((float)gamesWon / gamesPlayed * 100).ToString() + "%";
        }
        // Method to update the average attempt of success
        public void UpdateAverageAttempt(int attempt)
        {
            // Update average attempt using a weighted average formula
            averageAttemptOfSuccess = (averageAttemptOfSuccess * (gamesWon - 1) + attempt) / gamesWon;
            Preferences.Default.Set("averageAttemptOfSuccess", averageAttemptOfSuccess);
        }
    }
}