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
    public sealed class HomeScreen : MainGameScreen, IHomeScreen
    {
        SpriteBatch spriteBatch;
        SpriteFont omaFont; //Fontti.spritefont tiedostoa varten

        Character hahmo;

        NPC maagipappa;
        Dialogi dialogi;

        Sprite seina;
        Sprite lattia;
        Sprite oviSeina;
        Sprite alkemistinPoyta;
        Sprite taulu;
        Sprite sanky;

        Texture2D testiTextuuri;
        Rectangle alkemyRectangle;
        Rectangle tauluRectangle;

        static int pictureHeight = 800;
        static int pictureWidth = 1280;

        bool alkemistinPoytaAktiivinen = false;
        bool alkemistinTekstiPiirretaan = false;

        bool tauluAktiivinen = false;
        bool taulunTekstiPiirretaan = false;
        bool dialogiPiirretaan = false;

        string[] dialogiTekstitaulukko;

        MainGameScreen mainScreen; //Näppäintoimintojen hidastamiseen

        // Alkudemo, Haisuli on metsassa hengailemassa ja morko tulee paikalla hienolla autolla. Morko tarjoaa haisulille toita.

        public HomeScreen(Game game)
            : base(game)
        {
            hahmo = new Character();
            hahmo.numberOfPictures = 4;
            hahmo.Scale = 1.0f;

            maagipappa = new NPC();
            maagipappa.numberOfPictures = 4;
            maagipappa.Scale = 1.0f;

            dialogi = new Dialogi();
            seina = new Sprite();
            lattia = new Sprite();
            oviSeina = new Sprite();
            alkemistinPoyta = new Sprite();
            taulu = new Sprite();
            sanky = new Sprite();

            mainScreen = new MainGameScreen(game);
        }

        public override void Initialize()
        {
            base.Initialize();
        }
        protected override void LoadContent()
        {
            //tekstia on pakko olla 5-jaollinen määrä, esim. 5, 10, 20, 50, 55
            dialogiTekstitaulukko = new string[10];

            dialogiTekstitaulukko[0] = "Hei <pelaajanimesi>.";
            dialogiTekstitaulukko[1] = "Minä, isoisäsi  arkkimaagi Metsänhönkäys,";
            dialogiTekstitaulukko[2] = "aion lähteä tästä maailmasta ja jättää talon sinulle.";
            dialogiTekstitaulukko[3] = "Olen opettanut sinulle ensimmäisen Taikasi.";
            dialogiTekstitaulukko[4] = "Vinkki: kokeile ensin taikaasi metsässä vihollisia vastaan.";
            dialogiTekstitaulukko[5] = "Niiltä saat kamaa ja mahdollisesti rahaa.";
            dialogiTekstitaulukko[6] = "Voit myös jatkaa peltomme viljelyä ja harrastaa alkemiaa.";
            dialogiTekstitaulukko[7] = "Myöhemmin opit uusia taikoja, mm. Muodonmuutoksen.";
            dialogiTekstitaulukko[8] = "Hyvästi..";
            dialogiTekstitaulukko[9] = " ";
            //dialogiTekstitaulukko[10] = "ind10..";
            //dialogiTekstitaulukko[11] = "ind11..";
            //dialogiTekstitaulukko[12] = "ind12..";
            //dialogiTekstitaulukko[13] = "ind13..";
            //dialogiTekstitaulukko[14] = "ind14..";
            //dialogiTekstitaulukko[15] = "ind15..";
            //dialogiTekstitaulukko[16] = "ind16..";
            //dialogiTekstitaulukko[17] = "ind17..";
            //dialogiTekstitaulukko[18] = "ind18..";
            //dialogiTekstitaulukko[19] = "ind19..";

            spriteBatch = new SpriteBatch(GraphicsDevice);
            omaFont = Content.Load<SpriteFont>(".\\Fonts\\Fontti");
            hahmo.LoadContent(this.Content);
            maagipappa.LoadContent(this.Content, "maagipappa");
            maagipappa.Position = new Vector2(700, 40);
            maagipappa.setStartingPosition(700, 40);
            maagipappa.setVaeltelu(true);

            dialogi.LoadContent(this.Content);
            dialogi.Position = new Vector2(20, 400);
            dialogi.setTekstitaulukko(dialogiTekstitaulukko);

            seina.LoadContent(this.Content, "wall_blue");
            seina.Position = new Vector2(0, 0);
            lattia.LoadContent(this.Content, "floor");
            lattia.Position = new Vector2(0, 0);
            oviSeina.LoadContent(this.Content, "homeDoorWall");
            oviSeina.Position = new Vector2(0, 0);

            alkemistinPoyta.LoadContent(this.Content, "alkemist");
            alkemistinPoyta.Position = new Vector2(250, 30);

            taulu.LoadContent(this.Content, "immortalpose");
            taulu.Position = new Vector2(700, 3);
            taulu.Scale = 0.9f;

            sanky.LoadContent(this.Content, "bed");
            sanky.Position = new Vector2(0, 0);
            sanky.Scale = 1.0f;

            alkemyRectangle = new Rectangle((int)alkemistinPoyta.Position.X + 45,
                                            (int)alkemistinPoyta.Position.Y + 45,
                                            200, 50);

            tauluRectangle = new Rectangle((int)taulu.Position.X + 45, 
                                           (int)taulu.Position.Y + 45,
                                           200, 50);

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
            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Enter) && alkemistinPoytaAktiivinen == true)
            {
                mainScreen.StartButtonSpeedBump();

                ScreenManager.PushScreen(XGame.AlchemyScreen.Screen);

                System.Diagnostics.Debug.Write("Systeemi");

                Enabled = false;
                Visible = false;
            }

            maagipappa.Update(gameTime);

            hahmo.Update(gameTime);

            //DIALOGI ikkuna
            if (hahmo.characterRectangle.Intersects(maagipappa.NPCRectangle))
            {
                maagipappa.setVaeltelu(false);
                dialogiPiirretaan = true;

                if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Enter))
                {
                    mainScreen.StartButtonSpeedBump();

                    //kasvata sivunumeroa jos ei olla vielä vimppa sivulla
                    if (dialogi.sivunumero + 1 < dialogi.totalSivunumero)
                        dialogi.kasvataSivuNumeroaYhdella();
                    else
                        dialogi.resetoiDialogi();
                }
            }
            else
                dialogiPiirretaan = false;

            //tutkitaan törmääkö hahmo oikeaan reunaan
            if (hahmo.Position.X >= pictureWidth - hahmo.Size.Width)
            {
                hahmo.Position.X = pictureWidth - hahmo.Size.Width;
            }
            //tutkitaan törmääkö hahmo vasempaan reunaan
            else if (hahmo.Position.X <= 0)
            {
                hahmo.Position.X = 0;
            }

            //tutkitaan törmääkö hahmo yläreunaan
            if (hahmo.Position.Y <= 40)
            {
                hahmo.Position.Y = 40;
                hahmo.canJump = false;
            }

            //tutkitaan törmääkö hahmo alareunaan muualla paitsi oven kohdalla
            if (hahmo.Position.Y >= pictureHeight - 200 && 
                (hahmo.Position.X >=0 && hahmo.Position.X <= 750) )
            {
                hahmo.Position.Y = pictureHeight - 200;
            }
            else if (hahmo.Position.Y >= pictureHeight - 200 &&
                (hahmo.Position.X >= 900 && hahmo.Position.X <= 1200))
            {
                hahmo.Position.Y = pictureHeight - 200;
            }

            //jos hahmo kävelee ovesta ulos
            if (hahmo.Position.Y >= pictureHeight - 50)
            {
                mainScreen.StartButtonSpeedBump();

                ScreenManager.PopScreen();

                hahmo.Position.Y = pictureHeight - 150;
            }

            //hahmo osuu alkemistin pöytään
            if (hahmo.characterRectangle.Intersects(alkemyRectangle) && alkemistinPoytaAktiivinen == false)
            {
                System.Diagnostics.Debug.Write("alkemy on");
                alkemistinPoyta.vaihdaKuvaa(this.Content, "alkemist2");
                alkemistinPoytaAktiivinen = true;
                alkemistinTekstiPiirretaan = true;

            }
            else if (alkemistinPoytaAktiivinen == true && !hahmo.characterRectangle.Intersects(alkemyRectangle))
            {
                System.Diagnostics.Debug.Write("alkemy off");
                alkemistinPoyta.vaihdaKuvaa(this.Content, "alkemist");
                alkemistinPoytaAktiivinen = false;

                alkemistinTekstiPiirretaan = false;
            }

            if (hahmo.characterRectangle.Intersects(tauluRectangle) && tauluAktiivinen == false)
            {
                tauluAktiivinen = true;
                taulunTekstiPiirretaan = true;
            }
            else if(tauluAktiivinen == true && !hahmo.characterRectangle.Intersects(tauluRectangle)) {
                taulunTekstiPiirretaan = false;
                tauluAktiivinen = false;
            }

            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);
            spriteBatch.Begin();
            seina.Draw(this.spriteBatch);
            lattia.Draw(this.spriteBatch);
            oviSeina.Draw(this.spriteBatch);
            alkemistinPoyta.Draw(this.spriteBatch);
            taulu.Draw(this.spriteBatch);
            sanky.Draw(this.spriteBatch);
            
            //piirretään tekstiä pöytään jos voi käyttää
            if (alkemistinTekstiPiirretaan == true)
            {
                spriteBatch.DrawString(omaFont, "Käytä enterillä", new Vector2(alkemyRectangle.X, alkemyRectangle.Y), Color.Purple);
            }

            //piirretään kiva teksti taulun päälle 
            if (taulunTekstiPiirretaan == true)
            {
                spriteBatch.DrawString(omaFont, "Kaunis taulu seinällä", new Vector2(tauluRectangle.X - 40, tauluRectangle.Y), Color.Red);
            }

            //spriteBatch.Draw(testiTextuuri, alkemyRectangle, Color.Pink);  //näytä collision rectangle
            spriteBatch.DrawString(omaFont, "Oma koti kullan kallis", new Vector2(10, 20), Color.Goldenrod);
            maagipappa.Draw(this.spriteBatch);
            hahmo.Draw(this.spriteBatch);

            if (dialogiPiirretaan == true)
            {
                dialogi.Draw(this.spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

