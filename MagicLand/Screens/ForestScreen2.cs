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
    public sealed class ForestScreen2 : MainGameScreen, IForestScreen2
    {
        SpriteBatch spriteBatch;
        SpriteFont omaFont; //Fontti.spritefont tiedostoa varten

        static int pictureHeight = 800;

        Character hahmo;

        Sprite tausta;

        Sprite ruoho;
        Sprite ruoho2;
        Sprite ruoho3;
        Sprite ruoho4;

        MainGameScreen mainScreen; //Näppäintoimintojen hidastamiseen

        // Alkudemo, Haisuli on metsassa hengailemassa ja morko tulee paikalla hienolla autolla. Morko tarjoaa haisulille toita.

        public ForestScreen2(Game game)
            : base(game)
        {
            mainScreen = new MainGameScreen(game);

            hahmo = new Character();
            hahmo.numberOfPictures = 4;
            hahmo.Scale = 1.0f;

            tausta = new Sprite();

            ruoho = new Sprite();
            ruoho2 = new Sprite();
            ruoho3 = new Sprite();
            ruoho4 = new Sprite();
        }

        public override void Initialize()
        {
            base.Initialize();
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            omaFont = Content.Load<SpriteFont>(".\\Fonts\\Fontti");
            hahmo.LoadContent(this.Content);
            hahmo.Position = new Vector2(620, 550);
            hahmo.flip180degrees();

            tausta.LoadContent(this.Content, "tausta3");

            ruoho.LoadContent(this.Content, "grassSprite");
            ruoho.Position = new Vector2(900, 350);

            ruoho2.LoadContent(this.Content, "grassSprite");
            ruoho2.Position = new Vector2(1000, 350);

            ruoho3.LoadContent(this.Content, "grassSprite");
            ruoho3.Position = new Vector2(1000, 450);

            ruoho4.LoadContent(this.Content, "grassSprite");
            ruoho4.Position = new Vector2(900, 450);

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

            //tutkitaan onko hahmo oikeassa laidassa
            if (hahmo.Position.X >= 1180)
            {
                mainScreen.StartButtonSpeedBump();

                //ScreenManager.PopScreen();

                hahmo.Position.X = 1100;
            }

            //alalaitaan törmäys, takas forestScreen1
            if (hahmo.Position.Y >= pictureHeight - hahmo.Size.Height)
            {
                hahmo.Position.Y = pictureHeight - hahmo.Size.Height - 50;
                ScreenManager.PopScreen();
            }

            hahmo.Update(gameTime);

            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);

            spriteBatch.Begin();
            tausta.Draw(this.spriteBatch);
            ruoho.Draw(this.spriteBatch);
            ruoho2.Draw(this.spriteBatch);
            ruoho3.Draw(this.spriteBatch);
            ruoho4.Draw(this.spriteBatch);
            hahmo.Draw(this.spriteBatch);

            spriteBatch.DrawString(omaFont, "Metsä 2", new Vector2(10, 20), Color.Black);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

