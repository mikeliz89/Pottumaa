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
    public sealed class AlchemyScreen : MainGameScreen, IAlchemyScreen
    {
        SpriteBatch spriteBatch;
        SpriteFont omaFont; //Fontti.spritefont tiedostoa varten
        SpriteFont isoFontti;

        Sprite tausta;

        Texture2D testiTextuuri;

        MainGameScreen mainScreen; //Näppäintoimintojen hidastamiseen

        public AlchemyScreen(Game game)
            : base(game)
        {
            tausta = new Sprite();

            mainScreen = new MainGameScreen(game);
        }

        public override void Initialize()
        {

            base.Initialize();
        }
        protected override void LoadContent()
        {

            spriteBatch = new SpriteBatch(GraphicsDevice);
            omaFont = Content.Load<SpriteFont>(".\\Fonts\\Fontti");
            isoFontti = Content.Load<SpriteFont>(".\\Fonts\\Isofontti");

            tausta.LoadContent(this.Content, "alkemistScreen");

            testiTextuuri = Content.Load<Texture2D>("tausta1");
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
            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.C) ||
                GamePad.GetState(PlayerIndex.One).Buttons.Y == ButtonState.Pressed)
            {
                mainScreen.StartButtonSpeedBump();

                ScreenManager.PopScreen();
            }
           
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);
            spriteBatch.Begin();
            tausta.Draw(this.spriteBatch);
            spriteBatch.DrawString(omaFont, "Alchemy mesta", new Vector2(10, 20), Color.Goldenrod);
            spriteBatch.DrawString(isoFontti, "Paina C poistuaksesi", new Vector2(400, 700), Color.Purple);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

