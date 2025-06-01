using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;

namespace Helipcopter_Game_Assignment_3
{
    enum GameState
    {
        Intro,
        Playing,
        GameOver
    }

    public partial class GameForm : Form
    {
        private Image crashImage = Image.FromFile("Resources/copter_crash.png");
        private Image copterImage = Image.FromFile("Resources/helicopter.gif");

        private GameState currentGameState = GameState.Intro;


        SoundPlayer helicopterSound;
        SoundPlayer crashSound;
        WindowsMediaPlayer backgroundMusic;


        TunnelManager tunnelManager;
        ObstacleManager obstacleManager;
        Helicopter helicopter;

        private bool isGameStarted = false;

        public GameForm()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();
            this.KeyPreview = true;
        }

        private void GameForm_Load(object sender, EventArgs e)
        {
            helicopterSound = new SoundPlayer("Resources/helicopter.wav");
            helicopterSound.Load();

            crashSound = new SoundPlayer("Resources/crash.wav");
            crashSound.Load();


            backgroundMusic = new WindowsMediaPlayer();
            backgroundMusic.URL = "Resources/background1.mp3";
            backgroundMusic.settings.setMode("loop", true);

            GameController.LoadHighScore();
            lblHighScore.Text = "High Score: " + GameController.HighScore;

            tunnelManager = new TunnelManager(this);
            obstacleManager = new ObstacleManager(this);
            helicopter = new Helicopter(copter);

            InitGame();
        }

        private void InitGame()
        {
            currentGameState = GameState.Intro;
            gameTimer.Stop();

            tunnelManager.ClearTiles();
            obstacleManager.ClearObstacles();

            copter.Image = copterImage;
            helicopter.Reset(new Point(100, this.ClientSize.Height / 2));
            GameController.ResetGame();

            lblScore.Text = "Score: 0";
            lblHighScore.Text = "High Score: " + GameController.HighScore;

            helicopterSound.Stop();
            backgroundMusic.controls.stop();

            
            Invalidate();
        }

        private void StartGame()
        {
            currentGameState = GameState.Playing;
            GameController.IsGameRunning = true;

            ResetGame();
            copter.Image = copterImage;

            helicopterSound.PlayLooping();
            backgroundMusic.controls.play();

            Invalidate();
        }


        private void EndGame()
        {
            currentGameState = GameState.GameOver;
            GameController.GameOver();
            gameTimer.Stop();

            helicopterSound.Stop();
            backgroundMusic.controls.stop();

            crashSound.Play(); 

            copter.Image = crashImage;

            MessageBox.Show("Game Over! Score: " + GameController.Score, "Game Over");
            InitGame();
        }


        private void ResetGame()
        {
            tunnelManager.ClearTiles();
            tunnelManager.InitTunnel();

            obstacleManager.ClearObstacles();
            obstacleManager.InitObstacles();

            helicopter.Reset(new Point(100, this.ClientSize.Height / 2));
            GameController.ResetGame();
            copter.Image = copterImage;

            lblScore.Text = "Score: 0";
            lblHighScore.Text = "High Score: " + GameController.HighScore;

            helicopterSound.PlayLooping();
            backgroundMusic.controls.play();
            backgroundMusic.settings.setMode("loop", true);

            gameTimer.Start();
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            if (currentGameState != GameState.Playing) return;

            helicopter.Update();
            tunnelManager.MoveTunnel();
            obstacleManager.MoveObstacles();

            if (CheckCollision())
            {
                EndGame();
                return;
            }

            GameController.UpdateScore(lblScore, lblHighScore);
            Invalidate();
        }


        private bool CheckCollision()
        {
            var heliRect = helicopter.GetBounds();

            foreach (var t in tunnelManager.TopTiles)
                if (heliRect.IntersectsWith(t.Bounds)) return true;

            foreach (var b in tunnelManager.BottomTiles)
                if (heliRect.IntersectsWith(b.Bounds)) return true;

            foreach (var o in obstacleManager.CenterObstacles)
                if (heliRect.IntersectsWith(o.Bounds)) return true;

            return (heliRect.Top < 0 || heliRect.Bottom > this.ClientSize.Height);
        }

        private void GameForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (currentGameState == GameState.Intro)
            {
                StartGame();
            }
            else if (currentGameState == GameState.Playing)
            {
                helicopter.FlyUp = true;
            }
        }

        private void GameForm_MouseUp(object sender, MouseEventArgs e)
        {
            if (currentGameState == GameState.Playing)
                helicopter.FlyUp = false;
        }

        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                if (currentGameState == GameState.Intro)
                {
                    StartGame();
                }
                else if (currentGameState == GameState.Playing)
                {
                    helicopter.FlyUp = true;
                }
            }
        }

        private void GameForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space && currentGameState == GameState.Playing)
                helicopter.FlyUp = false;
        }

        private void GameForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            if (currentGameState == GameState.Intro)
            {
                g.Clear(Color.Black);

                string startMessage = "CLICK TO START";
                string instruction = "CLICK AND HOLD TO GO UP AND RELEASE TO GO DOWN";

                using (Font font = new Font("Arial", 24, FontStyle.Bold))
                {
                    SizeF textSize = g.MeasureString(startMessage, font);
                    float x = (ClientSize.Width - textSize.Width) / 2;
                    float y = ClientSize.Height / 2 - 40;

                    g.DrawString(startMessage, font, Brushes.Cyan, x, y);
                }

                using (Font fontSmall = new Font("Arial", 14, FontStyle.Bold))
                {
                    SizeF textSize = g.MeasureString(instruction, fontSmall);
                    float x = (ClientSize.Width - textSize.Width) / 2;
                    float y = ClientSize.Height - 60;

                    g.DrawString(instruction, fontSmall, Brushes.Cyan, x, y);
                }
            }

        }
    }
}











/*namespace Helipcopter_Game_Assignment_3
{
    enum GameState
    {
        Intro,  
        Playing,   
        GameOver    
    }



    public partial class GameForm : Form
    {
        private Image crashImage = Image.FromFile("Resources/copter_crash.png");
        private Image copterImage = Image.FromFile("Resources/Halicopter.gif");
        private bool showCrashImage = false;
        private Point crashPosition; // Position where the crash image appears



        private GameState currentGameState = GameState.Intro;
        SoundPlayer helicopterSound;
        WindowsMediaPlayer backgroundMusic;

        TunnelManager tunnelManager;
        ObstacleManager obstacleManager;
        Helicopter helicopter;

        private bool isGameStarted = false;

        public GameForm()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();

            this.KeyPreview = true;
        }

        private void GameForm_Load(object sender, EventArgs e)
        {
            helicopterSound = new SoundPlayer("Resources/helicopter.wav");
            helicopterSound.Load();

            backgroundMusic = new WindowsMediaPlayer();
            backgroundMusic.URL = "Resources/background1.mp3";
            backgroundMusic.settings.setMode("loop", true);

            GameController.LoadHighScore();
            lblHighScore.Text = "High Score: " + GameController.HighScore;

            tunnelManager = new TunnelManager(this);
            obstacleManager = new ObstacleManager(this);
            helicopter = new Helicopter(copter);

            InitGame();
        }

        private void InitGame()
        {
            currentGameState = GameState.Intro;

            gameTimer.Stop(); // Stop timer here

            tunnelManager.ClearTiles();
           

            obstacleManager.ClearObstacles();
        

            helicopter.Reset(new Point(100, this.ClientSize.Height / 2));
            GameController.ResetGame();

            lblScore.Text = "Score: 0";
            lblHighScore.Text = "High Score: " + GameController.HighScore;

            helicopterSound.Stop();
            backgroundMusic.controls.stop();

            Invalidate(); // Force redraw to show intro screen
        }

        private void StartGame()
        {
            currentGameState = GameState.Playing;
            GameController.IsGameRunning = true;

            ResetGame();

            helicopterSound.PlayLooping();
            backgroundMusic.controls.play();

            //gameTimer.Start();
            Invalidate();
        }

        private void EndGame()
        {
            currentGameState = GameState.Intro;
            GameController.GameOver();
            gameTimer.Stop();

            helicopterSound.Stop();
            backgroundMusic.controls.stop();

            // Set crash image position at helicopter's last location
            crashPosition = helicopter.GetBounds().Location;
            showCrashImage = true;
            copter.Image = crashImage;
            Invalidate();

            MessageBox.Show("Game Over! Score: " + GameController.Score, "Game Over");

            InitGame(); // Show intro again after game over
            Invalidate();
        }

        private void ResetGame()
        {
            //copter.Image = copterImage;
            tunnelManager.ClearTiles();
            tunnelManager.InitTunnel();

            obstacleManager.ClearObstacles();
            obstacleManager.InitObstacles();

            helicopter.Reset(new Point(100, this.ClientSize.Height / 2));
            GameController.ResetGame();

            lblScore.Text = "Score: 0";
            lblHighScore.Text = "High Score: " + GameController.HighScore;

            helicopterSound.PlayLooping();
            backgroundMusic.controls.play();
            backgroundMusic.settings.setMode("loop", true);

            gameTimer.Start();
        }


        private void gameTimer_Tick(object sender, EventArgs e)
        {
            if (currentGameState != GameState.Playing) return;

            helicopter.Update();
            tunnelManager.MoveTunnel();
            obstacleManager.MoveObstacles();

            if (CheckCollision())
            {
                EndGame();
                return;
            }

            GameController.UpdateScore(lblScore, lblHighScore);
        }


        private bool CheckCollision()
        {
            var heliRect = helicopter.GetBounds();

            foreach (var t in tunnelManager.TopTiles)
                if (heliRect.IntersectsWith(t.Bounds)) return true;

            foreach (var b in tunnelManager.BottomTiles)
                if (heliRect.IntersectsWith(b.Bounds)) return true;

            foreach (var o in obstacleManager.CenterObstacles)
                if (heliRect.IntersectsWith(o.Bounds)) return true;

            return (heliRect.Top < 0 || heliRect.Bottom > this.ClientSize.Height);
        }




        private void GameForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (currentGameState == GameState.Intro)
            {
                StartGame();
            }
            else if (currentGameState == GameState.Playing)
            {
                helicopter.FlyUp = true;
            }
        }

        private void GameForm_MouseUp(object sender, MouseEventArgs e)
        {
            if (currentGameState == GameState.Playing)
                helicopter.FlyUp = false;
        }

        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                if (currentGameState == GameState.Intro)
                {
                    StartGame();
                }
                else if (currentGameState == GameState.Playing)
                {
                    helicopter.FlyUp = true;
                }
            }
        }

        private void GameForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space && currentGameState == GameState.Playing)
                helicopter.FlyUp = false;
        }


        private void GameForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            if (currentGameState == GameState.Intro)
            {
                g.Clear(Color.Black);

                string startMessage = "CLICK TO START";
                string instruction = "CLICK AND HOLD TO GO UP AND RELEASE TO GO DOWN";

                using (Font font = new Font("Arial", 24, FontStyle.Bold))
                {
                    SizeF textSize = g.MeasureString(startMessage, font);
                    float x = (ClientSize.Width - textSize.Width) / 2;
                    float y = ClientSize.Height / 2 - 40;

                    g.DrawString(startMessage, font, Brushes.Cyan, x, y);
                }

                using (Font fontSmall = new Font("Arial", 14, FontStyle.Bold))
                {
                    SizeF textSize = g.MeasureString(instruction, fontSmall);
                    float x = (ClientSize.Width - textSize.Width) / 2;
                    float y = ClientSize.Height - 60;

                    g.DrawString(instruction, fontSmall, Brushes.Cyan, x, y);
                }
            }
        }

    }
}
*/