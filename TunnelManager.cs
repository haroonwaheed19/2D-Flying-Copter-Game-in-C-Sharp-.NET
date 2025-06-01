using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Helipcopter_Game_Assignment_3
{
    public class TunnelManager
    {
        private Form form;
        private List<PictureBox> topTiles = new List<PictureBox>();
        private List<PictureBox> bottomTiles = new List<PictureBox>();

        private int tileWidth = 40;
        private int tileSpeed = 13;
        private int gap = 275;
        private int waveChange = 28;

        private int previousTopHeight = 180;
        private Random rand = new Random();

        public TunnelManager(Form form)
        {
            this.form = form;
        }

        public List<PictureBox> TopTiles => topTiles;
        public List<PictureBox> BottomTiles => bottomTiles;

        public void InitTunnel()
        {
            ClearTiles();

            int x = 0;
            previousTopHeight = 180;
            while (x < form.ClientSize.Width + tileWidth)
            {
                CreateTileAt(x);
                x += tileWidth;
            }
        }

        public void MoveTunnel()
        {
            if (!GameController.IsGameRunning) return;

            for (int i = 0; i < topTiles.Count; i++)
            {
                topTiles[i].Left -= tileSpeed;
                bottomTiles[i].Left -= tileSpeed;
            }

            if (topTiles[0].Right < 0)
            {
                form.Controls.Remove(topTiles[0]);
                form.Controls.Remove(bottomTiles[0]);
                topTiles.RemoveAt(0);
                bottomTiles.RemoveAt(0);

                int newX = topTiles[topTiles.Count - 1].Right;
                CreateTileAt(newX);
            }
        }

        private void CreateTileAt(int x)
        {
            int newTopHeight = previousTopHeight + rand.Next(-waveChange, waveChange + 1);
            newTopHeight = Math.Max(50, Math.Min(newTopHeight, form.ClientSize.Height - gap - 50));
            previousTopHeight = newTopHeight;

            int bottomHeight = form.ClientSize.Height - newTopHeight - gap;

            PictureBox top = new PictureBox
            {
                Size = new Size(tileWidth, newTopHeight),
                Location = new Point(x, 0),
                BackColor = Color.DarkSlateBlue
            };
            topTiles.Add(top);
            form.Controls.Add(top);

            PictureBox bottom = new PictureBox
            {
                Size = new Size(tileWidth, bottomHeight),
                Location = new Point(x, form.ClientSize.Height - bottomHeight),
                BackColor = Color.DarkSlateBlue
            };
            bottomTiles.Add(bottom);
            form.Controls.Add(bottom);
        }

        public void ClearTiles()
        {
            foreach (var t in topTiles) form.Controls.Remove(t);
            foreach (var b in bottomTiles) form.Controls.Remove(b);
            topTiles.Clear();
            bottomTiles.Clear();
        }
    }
}
