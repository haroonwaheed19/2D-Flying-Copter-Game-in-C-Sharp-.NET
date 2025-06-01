using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helipcopter_Game_Assignment_3
{
    using System.Drawing;
    using System.Windows.Forms;

    public class Helicopter
    {
        private PictureBox heli;
        private float gravitySpeed = 0f;
        public bool FlyUp { get; set; } = false;

        public Helicopter(PictureBox heli)
        {
            this.heli = heli;
        }

        public void Update()
        {
            if (!FlyUp)
            {
                gravitySpeed += 0.7f;
                gravitySpeed = System.Math.Min(gravitySpeed, 7f);
            }
            else
            {
                gravitySpeed = -4.55f;
            }

            heli.Top += (int)gravitySpeed;
        }

        public Rectangle GetBounds() => heli.Bounds;

        public void Reset(Point startPos)
        {
            heli.Location = startPos;
            gravitySpeed = 0;
            FlyUp = false;
        }
    }

}
