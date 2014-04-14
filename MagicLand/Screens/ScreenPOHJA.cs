using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace MagicLand
{
    public sealed class ScreenPOHJA : MainGameScreen//, IScreenPOHJA
    {
        SpriteBatch spriteBatch;
        SpriteFont omaFont; //Fontti.spritefont tiedostoa varten

        MainGameScreen mainScreen; //Näppäintoimintojen hidastamiseen

        public ScreenPOHJA(Game game)
            : base(game)
        {
            mainScreen = new MainGameScreen(game);
        }

        public override void Initialize()
        {
            base.Initialize();
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            omaFont = Content.Load<SpriteFont>("Fontti");

            base.LoadContent();
        }
        protected override void UnloadContent()
        {
            base.UnloadContent();
        }
        protected internal override void ScreenChanged(object sender, EventArgs e)
        {
            base.ScreenChanged(sender, e);

            if (ScreenManager.TopScreen != this.Screen)
            {
                Visible = true;
            }
        }
        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.H))
            {
                mainScreen.StartButtonSpeedBump();

                //ScreenManager.PushScreen(XGame.HomeScreen.Screen);

                Enabled = false;
                Visible = false;
            }

            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);

            spriteBatch.Begin();
            spriteBatch.DrawString(omaFont, "test", new Vector2(10, 20), Color.Black);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

