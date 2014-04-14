using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace MagicLand
{
    public class Sprite
    {
        public Vector2 Position = new Vector2(0, 0);
        //The texture object used when drawing the sprite
        public Texture2D spriteTexture;
        public string AssetName;
        public Rectangle Size;
        public bool visible = true;
        public int picture;
        public int numberOfPictures = 1;
        public SpriteEffects spriteEffects = SpriteEffects.None;
        public float scale = 1.0f;

        public int costOfCurrentSpell = 1;

        public bool goingLeft = false;

        public BulletController PlayerBullets;
        //viimenen luku on aikaväli että millon luotia luodaan
        public TimeSpan bulletDelay = new TimeSpan(0, 0, 0, 0, 440);
        public GameTime bulletCooldown = new GameTime(new TimeSpan(), new TimeSpan());

        public float Scale
        {
            get { return scale; }
            set
            {
                scale = value;
                //Recalculate the Size of the Sprite with the new scale
                // Size = new Rectangle(0, 0, (int)(mSpriteTexture.Width * Scale), (int)(mSpriteTexture.Height * Scale));
            }
        }

        public void vaihdaKuvaa(ContentManager contentManager, string assetti)
        {
            spriteTexture = contentManager.Load<Texture2D>(assetti);
            AssetName = assetti;
        }

        public void flip180degrees()
        {
            if (goingLeft == false)
            {
                goingLeft = true;
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
            else
                goingLeft = false;
        }

        public void LoadContent(ContentManager contentManager, string assetName)
        {
            spriteTexture = contentManager.Load<Texture2D>(assetName);
            AssetName = assetName;
            Size = new Rectangle(0, 0, (int)(spriteTexture.Width / numberOfPictures * Scale), (int)(spriteTexture.Height / numberOfPictures * Scale));

            PlayerBullets = new BulletController();  // Constructing a new BulletController and using the local copy of the Viewport
            PlayerBullets.LoadContent(contentManager); //Also pass along LoadContent()
        }

        public void Update(GameTime gameTime, Vector2 speed, Vector2 direction)
        {
            Position += direction * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            PlayerBullets.Update(gameTime); //update bullet flying

            if ((Keyboard.GetState().IsKeyDown(Keys.LeftControl) == true))
            {
                FireBullet(gameTime);
            }

            if ((Keyboard.GetState().IsKeyDown(Keys.LeftShift) == true))
            {
                CharacterStatusBar.lisaaHealttia(1);
                CharacterStatusBar.lisaaManaa(1);
                AudioManager.PlaySound("bubbling", false, 0.4f);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (visible == true)
            {
                int frameWidth = spriteTexture.Width / numberOfPictures;
                Rectangle sourceRect = new Rectangle(frameWidth * picture, 0, frameWidth, spriteTexture.Height);
                spriteBatch.Draw(spriteTexture, Position, sourceRect, Color.White, 0.0f, Vector2.Zero, Scale, spriteEffects, 0);

                PlayerBullets.Draw(spriteBatch); //Add our bullet drawing
            }
        }

        public void FireBullet(GameTime gameTime)
        {
            if (bulletCooldown.TotalGameTime <= gameTime.TotalGameTime)
            {
                bulletCooldown = new GameTime(gameTime.TotalGameTime.Add(bulletDelay), gameTime.ElapsedGameTime);

                if (CharacterStatusBar.mCurrentMana >= costOfCurrentSpell)
                {
                    if(goingLeft == false)
                        PlayerBullets.AddBullet(new Bullet(new Vector2(Position.X + 70, Position.Y + 60), new Vector2(10, 0), 0));
                    else
                        PlayerBullets.AddBullet(new Bullet(new Vector2(Position.X + 70, Position.Y + 60), new Vector2(-10, 0), 0));
                    
                    //CharacterStatusBar.vahennaHealttia(1);
                    CharacterStatusBar.vahennaManaa(costOfCurrentSpell);
                    AudioManager.PlaySound("splats", false, 0.3f);
                }
            }
        }
    }
}
