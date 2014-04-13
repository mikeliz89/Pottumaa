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
    public sealed class ToriScreen : MainGameScreen, IToriScreen
    {
        SpriteBatch spriteBatch;
        SpriteFont omaFont; //Fontti.spritefont tiedostoa varten

        Sprite tausta;
        Sprite torikoju1;
        Sprite torikoju2;
        Sprite palmu1;
        Sprite palmu2;

        NPC farmikauppias_kuuno;
        NPC tavarakauppias_tulppu;

        Character hahmo;

        static int pictureHeight = 800;
        static int pictureWidth = 1280;

        MainGameScreen mainScreen; //Näppäintoimintojen hidastamiseen

        // Alkudemo, Haisuli on metsassa hengailemassa ja morko tulee paikalla hienolla autolla. Morko tarjoaa haisulille toita.

        public ToriScreen(Game game)
            : base(game)
        {
            mainScreen = new MainGameScreen(game);

            hahmo = new Character();
            hahmo.numberOfPictures = 4;
            hahmo.Scale = 1.0f;

            torikoju1 = new Sprite();
            torikoju2 = new Sprite();
            palmu1 = new Sprite();
            palmu2 = new Sprite();
            tausta = new Sprite();

            farmikauppias_kuuno = new NPC();
            farmikauppias_kuuno.numberOfPictures = 4;
            farmikauppias_kuuno.Scale = 1.0f;

            tavarakauppias_tulppu = new NPC();
            tavarakauppias_tulppu.numberOfPictures = 4;
            tavarakauppias_tulppu.Scale = 1.0f;
        }

        public override void Initialize()
        {
            base.Initialize();
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            omaFont = Content.Load<SpriteFont>(".\\Fonts\\Fontti");

            tausta.LoadContent(this.Content, "taustaTori");
            tausta.Position = new Vector2(0, 0);

            palmu1.LoadContent(this.Content, "palmu");
            palmu1.Position = new Vector2(700, 260);

            palmu2.LoadContent(this.Content, "palmu2");
            palmu2.Position = new Vector2(1000, 260);
           
            torikoju1.LoadContent(this.Content, "torikatos");
            torikoju1.Position = new Vector2(200, -5);
            torikoju1.numberOfPictures = 1;

            torikoju2.LoadContent(this.Content, "torikatos");
            torikoju2.Position = new Vector2(770, -5);

            hahmo.LoadContent(this.Content);
            hahmo.Position = new Vector2(100, 600);

            farmikauppias_kuuno.LoadContent(this.Content, "dude");
            farmikauppias_kuuno.Position = new Vector2(830, 140);
            farmikauppias_kuuno.setStartingPosition(830, 140);
            farmikauppias_kuuno.setVaeltelu(true);
            farmikauppias_kuuno.setVaeltelunMaara(120);

            tavarakauppias_tulppu.LoadContent(this.Content, "dudeVanha");
            tavarakauppias_tulppu.Position = new Vector2(100, 260);
            tavarakauppias_tulppu.setStartingPosition(100, 260);
            tavarakauppias_tulppu.setVaeltelu(true);
            tavarakauppias_tulppu.setVaeltelunMaara(40);

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

                Enabled = false;
                Visible = false;
            }

            //hahmo törmää Kuunoon
            if (hahmo.characterRectangle.Intersects(farmikauppias_kuuno.NPCRectangle))
            {
                farmikauppias_kuuno.setVaeltelu(false);
            }
            else
                farmikauppias_kuuno.setVaeltelu(true);

            //hahmo törmää Tulppuun
            if (hahmo.characterRectangle.Intersects(tavarakauppias_tulppu.NPCRectangle))
            {
                tavarakauppias_tulppu.setVaeltelu(false);
            }
            else
                tavarakauppias_tulppu.setVaeltelu(true);

            //jos hahmo menee vasempaan alalaitaan
            if (hahmo.Position.X <= 0 && hahmo.Position.Y > 400)
            {
                hahmo.Position.X = 30;
                mainScreen.StartButtonSpeedBump();
                ScreenManager.PopScreen();
            }

            //tutkitaan törmääkö hahmo reunoihin ylös
            if (hahmo.Position.Y <= 0)
            {
                hahmo.Position.Y = 0;
            }
            //ja reunoihin alas
            else if (hahmo.Position.Y >= pictureHeight - 240)
            {
                hahmo.Position.Y = pictureHeight - 240;
            }

            hahmo.Update(gameTime);
            farmikauppias_kuuno.Update(gameTime);
            tavarakauppias_tulppu.Update(gameTime);

            //Intersects(Rectangle rectA, Rectangle rectB);
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);

            spriteBatch.Begin();
            tausta.Draw(this.spriteBatch);

            spriteBatch.DrawString(omaFont, "Tori", new Vector2(10, 20), Color.Black);
            torikoju1.Draw(this.spriteBatch);
            torikoju2.Draw(this.spriteBatch);
           
            //NPC:t
            farmikauppias_kuuno.Draw(this.spriteBatch);
            tavarakauppias_tulppu.Draw(this.spriteBatch);

            hahmo.Draw(this.spriteBatch);

            palmu1.Draw(this.spriteBatch);
            palmu2.Draw(this.spriteBatch);

            CharacterStatusBar.Draw(this.spriteBatch);  //Piirretään healthBar

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

