using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Helipcopter_Game_Assignment_3
{
    public static class GameController
    {
        public static int Score = 0;
        public static int HighScore = 0;
        public static bool IsGameRunning = false;

        public static void LoadHighScore()
        {
            try
            {
                if (File.Exists("highscore.txt"))
                {
                    string content = File.ReadAllText("highscore.txt");
                    if (int.TryParse(content, out int parsedHighScore))
                        HighScore = parsedHighScore;
                }
            }
            catch (Exception)
            {
                HighScore = 0; // fallback to 0 if any error occurs
            }
        }


        public static void SaveHighScore()
        {
            File.WriteAllText("highscore.txt", HighScore.ToString());
        }

        public static void UpdateScore(Label scoreLabel, Label highScoreLabel)
        {
            Score += 10 / 2;

            string newScoreText = "Score: " + Score;
            if (scoreLabel.Text != newScoreText)
                scoreLabel.Text = newScoreText;

            if (Score > HighScore)
            {
                HighScore = Score;

                // Save immediately when new high score is achieved
                SaveHighScore();

                string newHighScoreText = "High Score: " + HighScore;
                if (highScoreLabel.Text != newHighScoreText)
                    highScoreLabel.Text = newHighScoreText;
            }
        }


        public static void ResetGame()
        {
            Score = 0;
            IsGameRunning = true;
        }

        public static void GameOver()
        {
            IsGameRunning = false;
            if (Score > HighScore)
            {
                HighScore = Score;
                SaveHighScore();
            }
        }


    }

}
