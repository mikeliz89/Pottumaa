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

//using System.Diagnostics;

namespace MagicLand
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class EnemySprite
    {
        const int ENEMY_SPEED = 250;
        const float ENEMY_SCALE = 1.0f;
        const int NUMBER_OF_PICTURE = 2;
        const float TIME_PER_PICTURE = 500.0f;

        enum State
        {
            Moves,
            Stops,
            Hides,
            Destroyed
        }

        float destroyedTime = 500.0f;

        Vector2 enemyPosition = new Vector2(0, 0);
        float turningPlace;

        Texture2D enemyTexture;
        SpriteEffects effects;
        Rectangle enemyRectangle;

        State currentState = State.Moves;
        Vector2 direction = Vector2.Zero;
        Vector2 aSpeed = Vector2.Zero;

        public Rectangle Size;

        float mScale;
        float elapsedTime;
        float elapsedTime2;

        int picture;
        byte alpha = 255;

        // Kääntöpaikkojen tutkiminen
        private Boolean end = false;
        private Boolean start = true;

        public float Scale
        {
            get { return mScale; }

            set
            {
                mScale = value;
                //Recalculate the Size of the Sprite with the new scale
                //Size = new Rectangle(0, 0, (int)(enemyTexture.Width * Scale), (int)(enemyTexture.Height * Scale));
            }
        }

        public void LoadContent(ContentManager contentManager, string assetName, Vector2 position)
        {
            enemyPosition = position;
            turningPlace = position.X;
            aSpeed = new Vector2(ENEMY_SPEED, 0);
            mScale = ENEMY_SCALE;

            enemyTexture = contentManager.Load<Texture2D>(assetName);
            Size = new Rectangle(0, 0, (int)(enemyTexture.Width / NUMBER_OF_PICTURE * Scale), (int)(enemyTexture.Height * Scale));
            effects = SpriteEffects.None;
        }

        public void Update(GameTime gameTime)
        {
            elapsedTime += (float)gameTime.ElapsedGameTime.Milliseconds;
            elapsedTime2 += (float)gameTime.ElapsedGameTime.Milliseconds;

            if (elapsedTime > TIME_PER_PICTURE) // Onko kulunut riittävästi aikaa, että vaihdetaan kuva
            {
                picture++;
                picture = picture % NUMBER_OF_PICTURE;
                elapsedTime -= TIME_PER_PICTURE;
            }

            if (currentState == State.Stops || currentState == State.Destroyed || currentState == State.Hides)
            {
                aSpeed = Vector2.Zero;
                direction = Vector2.Zero;

                if (elapsedTime2 > destroyedTime && (currentState == State.Stops || currentState == State.Destroyed))
                    alpha -= 25;
            }
            else if (currentState == State.Moves)
            {
                alpha = 255;
                if (enemyPosition.X <= turningPlace && end == false)
                {
                    enemyPosition.X = enemyPosition.X + 5;
                    Vector2 aDirection = new Vector2(-1, 0);
                    enemyPosition += aDirection * aSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (enemyPosition.X >= turningPlace - 5)
                    {
                        end = true;
                        start = false;
                        effects = SpriteEffects.FlipHorizontally;
                    }
                }
                else if (enemyPosition.X >= (turningPlace - 250) && start == false)
                {
                    enemyPosition.X = enemyPosition.X - 5;
                    Vector2 aDirection = new Vector2(1, 0);
                    enemyPosition += aDirection * aSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (enemyPosition.X <= (turningPlace - 250))
                    {
                        end = false;
                        start = true;
                        effects = SpriteEffects.None;
                    }
                }
            }

            enemyRectangle = new Rectangle((int)enemyPosition.X, (int)enemyPosition.Y, Size.Width, Size.Height - 5);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            int FrameWidth = enemyTexture.Width / NUMBER_OF_PICTURE;

            if (currentState == State.Moves || currentState == State.Stops)
            {
                spriteBatch.Draw(enemyTexture, enemyPosition,
                                 new Rectangle(FrameWidth * picture, 0, FrameWidth, enemyTexture.Height),
                                 new Color(255, 255, 255, alpha), 0.0f, Vector2.Zero, Scale, effects, 0);
            }
            else
                return;
        }

        public Rectangle getEnemyRectangle()
        {
            return enemyRectangle;
        }

        public void EnemyHitsHaisuli()
        {
            if (currentState != State.Destroyed)
                currentState = State.Stops;
            else
                direction.X = 0;
        }

        public void EnemyNoHitsHaisuli()
        {
            if (currentState != State.Destroyed)
            {
                currentState = State.Moves;
                aSpeed = new Vector2(ENEMY_SPEED, 0);
            }
            else
                aSpeed = new Vector2(ENEMY_SPEED, 0);
        }

        public void SetEnemyMove()
        {
            currentState = State.Moves;
            aSpeed = new Vector2(ENEMY_SPEED, 0);
        }

        public void EnemyDestroyed()
        {
            currentState = State.Destroyed;
            AudioManager.StopSound("Hit");
        }

        public void SetEnemySoudn()
        {
            AudioManager.LoadSounds();
            AudioManager.PlaySound("Hit", true);
        }

        public void PauseEnemySound()
        {
            AudioManager.PauseResumeSounds(false);
        }
    }
}
