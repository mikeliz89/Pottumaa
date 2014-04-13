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
    class Dialogi : Sprite
    {
        SpriteFont omaFont;
        Texture2D taustaTextuuri;
        string[] tekstitaulukko;
        public int totalSivunumero = 0;
        public int sivunumero = 0;
        int rivienMaara = 0;
        float[] rivien_Y_koordinaatit;
        float rivien_X_koordinaatti = 80.0f;

        //public int MAX_AMOUNT_OF_TEXT_LINES = 40;

        public Dialogi()
        {
            rivien_Y_koordinaatit = new float[5];

            rivien_Y_koordinaatit[0] = 40.0f;
            rivien_Y_koordinaatit[1] = 70.0f;
            rivien_Y_koordinaatit[2] = 100.0f;
            rivien_Y_koordinaatit[3] = 130.0f;
            rivien_Y_koordinaatit[4] = 160.0f;
        }

        public void LoadContent(ContentManager content)
        {
            omaFont = content.Load<SpriteFont>(".\\Fonts\\Fontti");
            taustaTextuuri = content.Load<Texture2D>("dialogiTausta");
        }

        //pakolliset tekstit, stringin koko olkoon max 50 merkkia
        public void setTekstitaulukko(string[] saapuva_tekstitaulukko)
        {
            tekstitaulukko = new string[saapuva_tekstitaulukko.Length];

            for (int i = 0; i < saapuva_tekstitaulukko.Length; i++)
            {
                tekstitaulukko[i] = saapuva_tekstitaulukko[i];
                rivienMaara++;
            }
        }

        public void kasvataSivuNumeroaYhdella()
        {
            sivunumero++;
        }

        public void resetoiDialogi()
        {
            sivunumero = 0;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(taustaTextuuri, new Rectangle(0,0, taustaTextuuri.Width, taustaTextuuri.Height),Color.White);

            if (rivienMaara > 5)
            {
                totalSivunumero = rivienMaara / 5;
                int jakojaannos = rivienMaara % 5;
                
                if(jakojaannos > 0)
                {
                    totalSivunumero++;
                }
            }
            else
                totalSivunumero = 1;

            //totalsivunumero = 1 jos rivejä vähempi tai yhtä paljon kuin 5
            //sivunumero on 1, totalSivunumero = 2 
            //-> 1 kierrosta 

            //totalsivunumero >= 2 jos rivejä enempi ku 5
            //sivunumero 0, totalSivunumero = 1
            //-> 2 kierrosta 

            ////jos rivien maara ei ole jaollinen viidella
            //if (rivienMaara % 5 != 0)
            //{

            //}

            for (int j = sivunumero; j < totalSivunumero; j++)
            {
                //RIVIEN TULOSTUS
                for (int i = 0; i < rivien_Y_koordinaatit.Length; i++)
                {
                    //int jakojaannos = rivienMaara % 5;
                    spriteBatch.DrawString(omaFont, tekstitaulukko[i + (5 * sivunumero)], new Vector2(rivien_X_koordinaatti, rivien_Y_koordinaatit[i]), Color.Black);
                }
            }

            int sivunumeroPlusOne = sivunumero + 1;
            spriteBatch.DrawString(omaFont, "Sivu: " + sivunumeroPlusOne.ToString() + " / " + totalSivunumero.ToString(), new Vector2(720.0f, rivien_Y_koordinaatit[4] + 60.0f), Color.Black);
        }
    }
}
