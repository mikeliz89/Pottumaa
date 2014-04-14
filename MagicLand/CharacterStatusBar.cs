using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace MagicLand
{
    public class CharacterStatusBar : DrawableGameComponent
    {
        private Vector2 healthPosition = new Vector2(0, 0);
        private const int MAX_HEALTPOTION_COUNT = 8, MAX_MANAPOTION_COUNT = 8;
        public static int healthpicture = 0,  manapicture = 0;
        static int healthPositionX, healthPositionY, manaPositionX, manaPositionY;
        static Texture2D mHealthBar, mManaBar, infoBarBackground;

        private static SpriteFont omaFont;

        public static int mCurrentHealth = 40, mCurrentMana = 30;
        public static int totalHealth = 40, totalMana = 30;
        const float HEALT_TIME = 1500.0f; // Pakoaika ennen kuin lähtee health

        private CharacterStatusBar(Game game)
            : base(game)
        {
              
        }

        public static void LoadContent(ContentManager content, int screenWidth, int screenHeight)
        {
            mHealthBar = content.Load<Texture2D>("healthpotions");
            mManaBar = content.Load<Texture2D>("manapotions");
            infoBarBackground = content.Load<Texture2D>("infobar");
            omaFont = content.Load<SpriteFont>(".\\Fonts\\Fontti");

            healthPositionX = 20;
            healthPositionY = screenHeight - 120;

            manaPositionX = screenWidth - 120;
            manaPositionY = screenHeight - 120;
        }

        public static void lisaaHealttia(int healthAdd)
        {
            if (mCurrentHealth < totalHealth)
            {
                mCurrentHealth += healthAdd;
            }
        }


        public static void lisaaManaa(int manaAdd)
        {
            if (mCurrentMana < totalMana)
            {
                mCurrentMana += manaAdd;
            }
        }

        public static void vahennaHealttia(int healthCost)
        {
            if (mCurrentHealth == 0)
            {
                //TODO: kuolema()
            }
            else if(mCurrentHealth > 0)
            {
                if (mCurrentHealth >= healthCost)
                {
                    mCurrentHealth -= healthCost;
                }
            }
        }

        public static void vahennaManaa(int manaCost)
        {
            if (mCurrentMana >= manaCost)
            {
                mCurrentMana -= manaCost;
            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            //tässä lasketaan paljonko manaa on prosentuaalisesti jäljellä kokonaismanaan nähden
            double manaprosentteja = ((double)mCurrentMana / (double)totalMana) * 100;

            if (manaprosentteja == 0.0)
            {
                manapicture = 7;
            }
            else if (12.5 <= manaprosentteja && manaprosentteja < 25.0)
            {
                manapicture = 6;
            }
            else if (25.0 <= manaprosentteja && manaprosentteja < 37.5)
            {
                manapicture = 5;
            }
            else if (37.5 <= manaprosentteja && manaprosentteja < 50.0)
            {
                manapicture = 4;
            }
            else if (50.0 <= manaprosentteja && manaprosentteja < 62.5)
            {
                manapicture = 3;
            }
            else if (62.5 <= manaprosentteja && manaprosentteja < 75.0)
            {
                manapicture = 2;
            }
            else if (75.0 <= manaprosentteja && manaprosentteja < 87.5)
            {
                manapicture = 1;
            }
            else if (87.5 <= manaprosentteja && manaprosentteja < 100.0)
            {
                manapicture = 0;
            }

            double helaprosentteja = ((double)mCurrentHealth / (double)totalHealth) * 100;

            if (helaprosentteja == 0.0)
            {
                healthpicture = 7;
            }
            else if (12.5 <= helaprosentteja && helaprosentteja < 25.0)
            {
                healthpicture = 6;
            }
            else if (25.0 <= helaprosentteja && helaprosentteja < 37.5)
            {
                healthpicture = 5;
            }
            else if (37.5 <= helaprosentteja && helaprosentteja < 50.0)
            {
                healthpicture = 4;
            }
            else if (50.0 <= helaprosentteja && helaprosentteja < 62.5)
            {
                healthpicture = 3;
            }
            else if (62.5 <= helaprosentteja && helaprosentteja < 75.0)
            {
                healthpicture = 2;
            }
            else if (75.0 <= helaprosentteja && helaprosentteja < 87.5)
            {
                healthpicture = 1;
            }
            else if (87.5 <= helaprosentteja && helaprosentteja < 100.0)
            {
                healthpicture = 0;
            }

            //infobarin TAUSTA
            spriteBatch.Draw(infoBarBackground, new Rectangle(0, 0, infoBarBackground.Width, infoBarBackground.Height), Color.White);

            ////HEALTH
            int frameWidthHealth = mHealthBar.Width / MAX_HEALTPOTION_COUNT;
            Rectangle sourceRect = new Rectangle(frameWidthHealth * healthpicture, 0, frameWidthHealth, mHealthBar.Height);
            spriteBatch.Draw(mHealthBar, new Vector2(healthPositionX, healthPositionY), sourceRect, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
            //health teksti
            spriteBatch.DrawString(omaFont, mCurrentHealth.ToString() + " / " + totalHealth.ToString(), new Vector2(healthPositionX + frameWidthHealth / 2 - 25, healthPositionY + 90), Color.White);

            ////MANA
            int frameWidthMana = mManaBar.Width / MAX_MANAPOTION_COUNT;
            Rectangle sourceRectMana = new Rectangle(frameWidthMana * manapicture, 0, frameWidthMana, mManaBar.Height);
            spriteBatch.Draw(mManaBar, new Vector2(manaPositionX, manaPositionY), sourceRectMana, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
            //mana teksti
            spriteBatch.DrawString(omaFont, mCurrentMana.ToString() + " / " + totalMana.ToString(), new Vector2(manaPositionX + frameWidthMana / 2 - 25, manaPositionY + 90), Color.White);
        }
    }
}
