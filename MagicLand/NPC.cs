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
    class NPC : Sprite
    {
        const string NPC_ASSETNAME = "maagipappa";
        private int START_POSITION_X, START_POSITION_Y;
        public Rectangle NPCRectangle;
        //private Vector2 startingPosition;

        private int vaeltelu_matka_x = 50;
        public bool vaelteluPaalla = false;
        public int VAELTELU_SPEED = 1;
        Vector2 direction = Vector2.Zero;

        private bool NPC_is_going_left = true;

        public NPC()
        {
            //startingPosition = new Vector2(Position.X, Position.Y);
        }

        public void setStartingPosition(int x, int y)
        {
            START_POSITION_X = x;
            START_POSITION_Y = y;
        }

        public void setVaeltelunMaara(int maara)
        {
            vaeltelu_matka_x = maara;
        }

        public void setVaeltelu(bool vaelteluBool)
        {
            vaelteluPaalla = vaelteluBool;
        }

        public void LoadContent(ContentManager contentManager)
        {
            Position = new Vector2(START_POSITION_X, START_POSITION_Y);
            base.LoadContent(contentManager, NPC_ASSETNAME);
        }

        public void Update(GameTime gameTime)
        {
            NPCRectangle = new Rectangle((int)Position.X, (int)Position.Y,
               Size.Width, Size.Height - 1);

            //if (Position.X > START_POSITION_X - vaeltelu_matka_x && NPC_is_going_left == true)
            //    base.Update(gameTime, VAELTELU_SPEED, new Vector2(-1, 0));
            //else if (Position.X == START_POSITION_X - vaeltelu_matka_x && NPC_is_going_left == true)
            //    NPC_is_going_left = false;
            //else if (Position.X < START_POSITION_X + vaeltelu_matka_x && NPC_is_going_left == false)
            //    base.Update(gameTime, VAELTELU_SPEED, new Vector2(1, 0));
            //else if (Position.X == START_POSITION_X + vaeltelu_matka_x && NPC_is_going_left == false)
            //    NPC_is_going_left = true;

            if (vaelteluPaalla == true)
            {
                if (Position.X > START_POSITION_X - vaeltelu_matka_x && NPC_is_going_left == true)
                {
                    MOVE_NPC_LEFT(gameTime);
                    //System.Diagnostics.Debug.Write("position on" + Position.X.ToString());
                }

                else if (Position.X == START_POSITION_X - vaeltelu_matka_x && NPC_is_going_left == true)
                {
                    NPC_is_going_left = false;
                    if (picture == 1)
                        picture = 2;
                    else
                        picture = 1;
                    //System.Diagnostics.Debug.Write("position on: " + Position.X.ToString());
                }

                else if (Position.X < START_POSITION_X + vaeltelu_matka_x && NPC_is_going_left == false)
                {
                    MOVE_NPC_RIGHT(gameTime);
                }

                else if (Position.X == START_POSITION_X + vaeltelu_matka_x && NPC_is_going_left == false)
                {
                    NPC_is_going_left = true;
                    if (picture == 1)
                        picture = 2;
                    else
                        picture = 1;
                }
            }
        }

        public void MOVE_NPC_LEFT(GameTime gameTime)
        {
            Position.X -=  VAELTELU_SPEED;// *(float)gameTime.ElapsedGameTime.TotalSeconds;
            //System.Diagnostics.Debug.Write("vaeltelualeft");
        }

        public void MOVE_NPC_RIGHT(GameTime gameTime)
        {
            Position.X +=  VAELTELU_SPEED;// *(float)gameTime.ElapsedGameTime.TotalSeconds;
           // System.Diagnostics.Debug.Write("vaelteluaright");
        }
    }
}
