using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MagicLand
{
    class Container : Sprite
    {
        const string CONTAINER_ASSETNAME = "barrelSprite";
        private int START_POSITION_X, START_POSITION_Y;
        public Rectangle ContainerRectangle;
        //private Vector2 startingPosition;

        public int VAELTELU_SPEED = 1;
        Vector2 direction = Vector2.Zero;

        public Container()
        {
            //startingPosition = new Vector2(Position.X, Position.Y);
        }

        public void setStartingPosition(int x, int y)
        {
            START_POSITION_X = x;
            START_POSITION_Y = y;
        }

        public new void LoadContent(ContentManager contentManager, string AssetName)
        {
            Position = new Vector2(START_POSITION_X, START_POSITION_Y);

            //jos asset nimeä ei ole annettu, asetetaan vakionimi "barrelSprite"
            if (AssetName.Length <= 0)
                base.LoadContent(contentManager, CONTAINER_ASSETNAME);
            else
                base.LoadContent(contentManager, AssetName);
        }

        public void Update(GameTime gameTime)
        {
            ContainerRectangle = new Rectangle((int)Position.X, (int)Position.Y,
                                                Size.Width, Size.Height - 1);
        }
    }
}
