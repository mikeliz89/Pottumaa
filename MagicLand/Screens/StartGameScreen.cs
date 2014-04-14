using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace MagicLand
{
    public sealed class StartGameScreen : MainGameScreen, IStartGameScreen
    {
        SpriteBatch spriteBatch;
        SpriteFont omaFont; //Fontti.spritefont tiedostoa varten

        static int pictureHeight = 800;
        static int pictureWidth = 1280;
        //static int LEVELWIDTH = pictureWidth;

        Sprite tausta1;
        Sprite talo;
        Sprite kivi;
        Sprite kivi2;
        Sprite kivi3;
        Sprite kivi4;
        Sprite kivi5;
        Sprite puut;
        Container tynnyri;
        Character hahmo;

        MainGameScreen mainScreen; //Näppäintoimintojen hidastamiseen

        List<Sprite> bulletList;

        Rectangle oviRectangle;
        Texture2D testiTextuuri;

        bool oviAktiivinen = false;
        bool oviTekstiPiirretaan = false;

        // Eka screeni, hahmo ilmestyy talonsa edustalle
        public StartGameScreen(Game game)
            : base(game)
        {
            mainScreen = new MainGameScreen(game);
            hahmo = new Character();
            hahmo.numberOfPictures = 4;
            hahmo.Scale = 1.0f;
            tausta1 = new Sprite();
            tausta1.Scale = 1.0f;
            talo = new Sprite();
            talo.Scale = 1.0f;
            kivi = new Sprite();
            kivi.Scale = 1.0f;
            kivi2 = new Sprite();
            kivi3 = new Sprite();
            kivi4 = new Sprite();
            kivi5 = new Sprite();
            puut = new Sprite();
            tynnyri = new Container();
            tynnyri.numberOfPictures = 4;
        }

        public override void Initialize()
        {
            bulletList = new List<Sprite>();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            omaFont = Content.Load<SpriteFont>(".\\Fonts\\Fontti");

            tausta1.LoadContent(this.Content, "tausta1");
            tausta1.Position = new Vector2(0, 0);
           
            hahmo.LoadContent(this.Content);

            talo.LoadContent(this.Content, "house");
            talo.Position = new Vector2(0, 0);

            kivi.LoadContent(this.Content, "rock");
            kivi.Position = new Vector2(10, 20);

            kivi2.LoadContent(this.Content, "rock2");
            kivi2.Position = new Vector2(10, 180);

            kivi3.LoadContent(this.Content, "rock2");
            kivi3.Position = new Vector2(12, 260);

            kivi4.LoadContent(this.Content, "rock2");
            kivi4.Position = new Vector2(12, 340);

            kivi5.LoadContent(this.Content, "rock2");
            kivi5.Position = new Vector2(11, 420);

            puut.LoadContent(this.Content, "puut");

            tynnyri.LoadContent(this.Content, "barrelSprite");
            tynnyri.Position = new Vector2(130, 140);
            tynnyri.setStartingPosition(130, 140);
            //tynnyri.setVaeltelu(true);

            oviRectangle = new Rectangle(920, 400,
                                         200, 50);

            testiTextuuri = Content.Load<Texture2D>("tausta1");

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

                    ScreenManager.PushScreen(XGame.HomeScreen.Screen);

                    Enabled = false;
                    Visible = false;
            }

            //CharacterStatusBar.Update(gameTime); //Päivitetään healtBar

            //törmääkö hahmo kivikasoihin alhaaltapäin
            if (hahmo.Position.X <= 60 && hahmo.Position.Y <= 420)
            {
                hahmo.Position.Y = 420;
                hahmo.canJump = false;
            }
            else if (hahmo.Position.X <= 70 && hahmo.Position.Y <= 410)
            {
                hahmo.Position.X = 70;
            }

            //tutkitaan törmääkö hahmo oikealle eli meneekö FarmScreeniin
            if (hahmo.Position.X >= pictureWidth - hahmo.Size.Width)
            {
                //hahmo.Position.X = pictureWidth - hahmo.Size.Width;
                mainScreen.StartButtonSpeedBump();

                ScreenManager.PushScreen(XGame.FarmScreen.Screen);

                hahmo.Position.X = pictureWidth - hahmo.Size.Width - 20;

                Enabled = false;
                Visible = false;
            }
            //vasemmalle alalaitaan eli meneekö AdventureScreen1
            else if (hahmo.Position.X <= 0 && hahmo.Position.Y >= 400)
            {
                hahmo.Position.X = 30;

                mainScreen.StartButtonSpeedBump();

                ScreenManager.PushScreen(XGame.ForestScreen1.Screen);

                Enabled = false;
                Visible = false;
            }

            //tutkitaan törmääkö hahmo reunoihin ylös
            if (hahmo.Position.Y <= 0)
            {
                hahmo.Position.Y = 0;
                hahmo.canJump = false;
            }
            //ja reunoihin alas
            else if (hahmo.Position.Y >= pictureHeight - 240)
            {
                hahmo.Position.Y = pictureHeight - 240;
            }

            //hahmo osuu taloon sivusta
            if (hahmo.Position.Y <= 390 && hahmo.Position.X >= 360 )
            {
                hahmo.Position.X = 360; 
            }

            //hahmo osuu taloon alhaalta
            if (hahmo.Position.Y <= 400 && hahmo.Position.X >= 370)
            {
                hahmo.Position.Y = 400;
                hahmo.canJump = false;
            }

            //hahmo osuu oveen
            if (hahmo.characterRectangle.Intersects(oviRectangle) && oviAktiivinen == false)
            {
                System.Diagnostics.Debug.Write("alkemy on");
                //alkemistinPoyta.vaihdaKuvaa(this.Content, "alkemist2");
                oviAktiivinen = true;
                oviTekstiPiirretaan = true;
            }
            else if (oviAktiivinen == true && !hahmo.characterRectangle.Intersects(oviRectangle))
            {
                System.Diagnostics.Debug.Write("alkemy off");
                //alkemistinPoyta.vaihdaKuvaa(this.Content, "alkemist");
                oviAktiivinen = false;
                oviTekstiPiirretaan = false;
            }
            //enteriä painetaan kun ovi on aktiivinen
            if ((Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Enter) ||
            GamePad.GetState(PlayerIndex.One).Buttons.Y == ButtonState.Pressed) && oviAktiivinen == true)
            {
                hahmo.Position.Y += 20;

                mainScreen.StartButtonSpeedBump();

                ScreenManager.PushScreen(XGame.HomeScreen.Screen);

                Enabled = false;
                Visible = false;
            }

            //hahmo törmää Tulppuun
            if (hahmo.characterRectangle.Intersects(tynnyri.ContainerRectangle))
            {
                if ((Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Enter) ||
                     GamePad.GetState(PlayerIndex.One).Buttons.Y == ButtonState.Pressed))
                {
                    if(tynnyri.picture < tynnyri.numberOfPictures - 1)
                    tynnyri.picture++;
                    //mainScreen.StartButtonSpeedBump();
                }
            }

            tynnyri.Update(gameTime);
            hahmo.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);

            spriteBatch.Begin();
            tausta1.Draw(this.spriteBatch);
            talo.Draw(this.spriteBatch);
            kivi.Draw(this.spriteBatch);
            kivi2.Draw(this.spriteBatch);
            kivi3.Draw(this.spriteBatch);
            kivi4.Draw(this.spriteBatch);
            kivi5.Draw(this.spriteBatch);
            puut.Draw(this.spriteBatch);
            tynnyri.Draw(this.spriteBatch);
            hahmo.Draw(spriteBatch);

            CharacterStatusBar.Draw(this.spriteBatch);  //Piirretään healthBar

            //spriteBatch.Draw(testiTextuuri, oviRectangle, Color.Pink);
            
            if (oviTekstiPiirretaan == true)
            {
                spriteBatch.DrawString(omaFont, "Mene sisään enterillä", new Vector2(920,265), Color.Black);
            }
            spriteBatch.DrawString(omaFont, "Magic Land alkaa...", new Vector2(10, 20), Color.Black);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}