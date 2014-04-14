using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MagicLand
{
    public class Bullet
    {
        public Vector2 Position, Motion;
        public int BulletType;
        public Rectangle bulletRectangle;

        public Bullet(Vector2 Position, Vector2 Motion, int BulletType)
        {
            this.Position = Position;
            this.Motion = Motion;
            this.BulletType = BulletType;

            bulletRectangle = new Rectangle(0, 0, 18, 10);
        }
    }
}
