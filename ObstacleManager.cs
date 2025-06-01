using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Helipcopter_Game_Assignment_3
{
    public class ObstacleManager
    {
        private Form form;
        private List<PictureBox> centerObstacles = new List<PictureBox>();
        private int centerGap = 350;
        private int obstacleWidth = 22;
        private int obstacleHeight = 60;
        private Random rand = new Random();

        private readonly int tileSpeed = 10;

        public ObstacleManager(Form form)
        {
            this.form = form;
        }

        public List<PictureBox> CenterObstacles => centerObstacles;

        public void InitObstacles()
        {
            foreach (var o in centerObstacles) form.Controls.Remove(o);
            centerObstacles.Clear();

            int x = 600;

            while (x < form.ClientSize.Width + 600)
            {
                int y = rand.Next(100, form.ClientSize.Height - 100);

                PictureBox obstacle = new PictureBox
                {
                    Size = new Size(obstacleWidth, obstacleHeight),
                    Location = new Point(x, y),
                    BackColor = Color.DarkSlateBlue
                };

                centerObstacles.Add(obstacle);
                form.Controls.Add(obstacle);

                x += centerGap;
            }
        }

        public void MoveObstacles()
        {
            if (!GameController.IsGameRunning) return;

            for (int i = 0; i < centerObstacles.Count; i++)
            {
                centerObstacles[i].Left -= tileSpeed;

                if (centerObstacles[i].Right < 0)
                {
                    int newX = GetRightmostObstacle() + centerGap;
                    int y = rand.Next(100, form.ClientSize.Height - obstacleHeight - 100);

                    centerObstacles[i].Left = newX;
                    centerObstacles[i].Top = y;
                }
            }
        }

        private int GetRightmostObstacle()
        {
            int rightMost = 0;
            foreach (var obs in centerObstacles)
                if (obs.Right > rightMost)
                    rightMost = obs.Right;
            return rightMost;
        }

        public void ClearObstacles()
        {
            foreach (var obstacle in centerObstacles)
            {
                form.Controls.Remove(obstacle);
                obstacle.Dispose();
            }
            centerObstacles.Clear();
        }


    }
}
