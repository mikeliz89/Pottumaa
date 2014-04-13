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
    public sealed class FarmScreen : MainGameScreen, IFarmScreen
    {
        SpriteBatch spriteBatch;
        SpriteFont omaFont; //Fontti.spritefont tiedostoa varten

        static int pictureHeight = 800;
        static int pictureWidth = 1280;

        Sprite tausta;
        Sprite aidat;

        Sprite[] kasvit;

        Character hahmo;

        Texture2D testiTextuuri;
        Texture2D testiTextuuri2;

        bool hahmoAidanTakana = false;

        const int MAX_RECTANGLE_COUNT = 36;
        Rectangle[] rectangleTaulukko;
        bool[] rectangleTextuurit;

        int yKoordinaattiRuudulle = -70;
        int xKoordinaattiRuudulle = 150;
        int ruudunLeveys = 110;
        int ruudunKorkeus = 120;

        public enum kasvilajit { kurpitsa, tomaatti, vilja, manakasvi }

        MainGameScreen mainScreen; //Näppäintoimintojen hidastamiseen

        public FarmScreen(Game game)
            : base(game)
        {
            mainScreen = new MainGameScreen(game);

            hahmo = new Character();
            hahmo.numberOfPictures = 4;
            hahmo.Scale = 1.0f;

            rectangleTaulukko = new Rectangle[MAX_RECTANGLE_COUNT];
            rectangleTextuurit = new bool[MAX_RECTANGLE_COUNT];

            kasvit = new Sprite[MAX_RECTANGLE_COUNT];

            aidat = new Sprite();
            tausta = new Sprite();

            yKoordinaattiRuudulle = -70;

            int rivilaskuri = 0;

            for (int i = 0; i < rectangleTaulukko.Length; i++)
            {
                //joka 9:nnen jälkeen lisätään rivilaskuria 
                if (i % 9 == 0)
                {
                    rivilaskuri++;
                    //"nollataan" x-koordinaatti alkuarvoonsa 100
                    xKoordinaattiRuudulle = 150;
                    yKoordinaattiRuudulle += ruudunKorkeus + 10;
                }
                rectangleTaulukko[i] = new Rectangle(xKoordinaattiRuudulle,
                                                     yKoordinaattiRuudulle,
                                                     ruudunLeveys,
                                                     ruudunKorkeus);

                rectangleTextuurit[i] = false;

                kasvit[i] = new Sprite();
                kasvit[i].Position.X = xKoordinaattiRuudulle;
                kasvit[i].Position.Y = yKoordinaattiRuudulle;

                //joka kierros kasvatetaan x-koordinaattia ruudun leveyden verran
                xKoordinaattiRuudulle += ruudunLeveys + 10;

            }
        }

        public override void Initialize()
        {
            testiTextuuri = Content.Load<Texture2D>("multaTextuuri");
            testiTextuuri2 = Content.Load<Texture2D>("multaTextuuriActive");

            base.Initialize();
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            omaFont = Content.Load<SpriteFont>(".\\Fonts\\Fontti");

            hahmo.LoadContent(this.Content);
            hahmo.Position = new Vector2(100, 540);

            tausta.LoadContent(this.Content, "taustaFarm");
            tausta.Position = new Vector2(0, 0);

            aidat.LoadContent(this.Content, "spriteAidat");
            aidat.Position = new Vector2(0, 0);

            CharacterStatusBar.LoadContent(this.Content, pictureWidth, pictureHeight);

            //kasvimaan kasvien alustus
            for(int indx = 0 ; indx < kasvit.Length; indx++)
            {
                kasvit[indx].LoadContent(this.Content, "kurpitsaSprite");
                kasvit[indx].numberOfPictures = 4;
                kasvit[indx].picture = 0;
            }

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

            //jos hahmo menee vasempaan alalaitaan
            if (hahmo.Position.X <= 0 && hahmo.Position.Y > 390)
            {
                hahmo.Position.X = 30;
                mainScreen.StartButtonSpeedBump();
                ScreenManager.PopScreen();
            }
            //tutkitaan onko hahmo oikeassa alalaidassa
            else if (hahmo.Position.X >= 1180 && hahmo.Position.Y >= 400)
            {
                mainScreen.StartButtonSpeedBump();

                ScreenManager.PushScreen(XGame.ToriScreen.Screen);

                Enabled = false;
                Visible = false;

                hahmo.Position.X = 1100;
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

            //törmääkö Talon nurkkaan oikealta
            if (hahmo.Position.Y <= 390 && hahmo.Position.X <= 81)
            {
                hahmo.Position.X = 81;
            }

            //törmääkö Talon nurkkaan alhaaltapäin
            if (hahmo.Position.Y <= 400 && hahmo.Position.X <= 80)
            {
                hahmo.Position.Y = 400;
            }

            //kasvin istuttaminen kyseiseen farmiruutuun (eli kohtaan jossa rectangleTextuurit == true
            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Enter))
            {
                mainScreen.StartButtonSpeedBump();

                for (int i = 0; i < rectangleTextuurit.Length; i++)
                {
                    if (rectangleTextuurit[i] == true)
                    {
                        System.Diagnostics.Debug.Write("Enteri painettu ruudun "+ i.ToString() + " kohdalla");
                        kasvit[i].picture += 1;
                    }
                }
            }

            //tää indeksoi farmiruutuja
            int indexiLaskuri = 0;

            //kahtotaan osuuko hahmo farmiruutuihin
            foreach (Rectangle rect in rectangleTaulukko)
            {
                if (rect.Intersects(hahmo.farmRectangle))
                {
                    rectangleTextuurit[indexiLaskuri] = true;
                }
                else
                {
                    rectangleTextuurit[indexiLaskuri] = false;
                }
                indexiLaskuri++;
            }

            //jos hahmo ylittää y 500 muutetaan hahmon ja aidan piirtojärjestystä
            //niin että hahmo on aidan takana
            if (hahmo.Position.Y <= 500)
            {
                hahmoAidanTakana = true;
            }
            //muussa tapauksessa hahmo on aidan edessä
            else
            {
                hahmoAidanTakana = false;
            }

            //CharacterStatusBar.Update(gameTime); //Päivitetään healtBar

            hahmo.Update(gameTime);

            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);

            spriteBatch.Begin();
            tausta.Draw(this.spriteBatch);

            int indexiLaskuri = 0;

            //draw every rectangle
            foreach (Rectangle rect in rectangleTaulukko)
            {
                if (rectangleTextuurit[indexiLaskuri] == false)
                {
                    spriteBatch.Draw(testiTextuuri, rect, Color.White);
                }
                else
                {
                    spriteBatch.Draw(testiTextuuri2, rect, Color.White);
                }

                indexiLaskuri++;
            }

            foreach (Sprite plant in kasvit)
            {
                plant.Draw(this.spriteBatch);
            }

            if (hahmoAidanTakana == true)
            {
                hahmo.Draw(this.spriteBatch);
                aidat.Draw(this.spriteBatch);
            }
            else
            {
                aidat.Draw(this.spriteBatch);
                hahmo.Draw(this.spriteBatch);
            }

            spriteBatch.DrawString(omaFont, "Farm", new Vector2(10, 20), Color.White);
            CharacterStatusBar.Draw(this.spriteBatch);  //Piirretään healthBar
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

