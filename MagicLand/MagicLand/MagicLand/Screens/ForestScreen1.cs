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
    public sealed class ForestScreen1 : MainGameScreen, IForestScreen1
    {
        SpriteBatch spriteBatch;
        SpriteFont omaFont; //Fontti.spritefont tiedostoa varten

        static int pictureHeight = 800;
        static int pictureWidth = 1280;

        Character hahmo;

        Sprite tausta;
        Sprite puut;

        Sprite ruoho;
        Sprite ruoho2;
        Sprite ruoho3;
        Sprite ruoho4;

        Sprite kyltti;

        CharacterStatusBar characterStatusBar;

        MainGameScreen mainScreen; //Näppäintoimintojen hidastamiseen

        public ForestScreen1(Game game)
            : base(game)
        {
            mainScreen = new MainGameScreen(game);

            hahmo = new Character();
            hahmo.numberOfPictures = 4;
            hahmo.Scale = 1.0f;

            tausta = new Sprite();
            puut = new Sprite();

            ruoho = new Sprite();
            ruoho2 = new Sprite();
            ruoho3 = new Sprite();
            ruoho4 = new Sprite();

            kyltti = new Sprite();
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
            hahmo.Position = new Vector2(1100, 540);
            hahmo.flip180degrees();

            tausta.LoadContent(this.Content, "tausta2");
            puut.LoadContent(this.Content, "puutSprite");

            ruoho.LoadContent(this.Content, "grassSprite");
            ruoho.Position = new Vector2(900,350);

            ruoho2.LoadContent(this.Content, "grassSprite");
            ruoho2.Position = new Vector2(1000, 350);

            ruoho3.LoadContent(this.Content, "grassSprite");
            ruoho3.Position = new Vector2(1000, 450);

            ruoho4.LoadContent(this.Content, "grassSprite");
            ruoho4.Position = new Vector2(900, 450);

            kyltti.LoadContent(this.Content, "kylttiSprite");
            kyltti.Scale = 0.8f;
            kyltti.Position = new Vector2( 1000, 410);

            CharacterStatusBar.LoadContent(this.Content, pictureWidth, pictureHeight);

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

            //CharacterStatusBar.Update(gameTime); //Päivitetään healtBar

            //tutkitaan törmääkö hahmo metsänlaitaan 
            if (hahmo.Position.X <= 540)
            {
                hahmo.Position.X = 540;
            }

            //meneekö tietäpitkin ylös
            if ( (hahmo.Position.X >= 540 && hahmo.Position.X <= 610)&& hahmo.Position.Y <= -20)
            {
                mainScreen.StartButtonSpeedBump();

                ScreenManager.PushScreen(XGame.AdventureScreen2.Screen);

                hahmo.Position.Y = 20;

                Enabled = false;
                Visible = false;
            }


            //tutkitaan onko hahmo oikeassa alalaidassa
            if (hahmo.Position.X >= 1180 && hahmo.Position.Y >= 400)
            {
                mainScreen.StartButtonSpeedBump();

                ScreenManager.PopScreen();

                hahmo.Position.X = 1100;
            }
            
            //törmääkö alalaitaan
            if (hahmo.Position.Y >= pictureHeight - 240)
            {
                hahmo.Position.Y = pictureHeight - 240;
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
            kyltti.Draw(this.spriteBatch);
            hahmo.Draw(this.spriteBatch);
            puut.Draw(this.spriteBatch); 
            spriteBatch.DrawString(omaFont, "Metsä 1", new Vector2(10, 20), Color.Black);
            CharacterStatusBar.Draw(this.spriteBatch);  //Piirretään healthBar
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

