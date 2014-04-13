using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MagicLand
{
    public class BulletController
    {
        public struct BulletTex
        {
            public Texture2D Texture;
            public Vector2 Origin;

            public BulletTex(Texture2D Texture, Vector2 Origin)
            {
                this.Texture = Texture;
                this.Origin = Origin;
            }
        }

        public List<Bullet> Bullets;
        public List<Sprite> Enemies;
        public BulletTex[] texBullet;
        public Rectangle screenEdges;

        public BulletController(/*Viewport viewport*/)
        {
            Bullets = new List<Bullet>();
            Enemies = new List<Sprite>();
           // edges = new Rectangle(0, 0, viewport.Width, viewport.Height);
            screenEdges = new Rectangle(0, 0, 1280, 800);
        }

        public bool Intersects(Rectangle rectA, Rectangle rectB)
        {
            return (rectA.Right > rectB.Left && rectA.Left < rectB.Right &&
                       rectA.Bottom > rectB.Top && rectA.Top < rectB.Bottom);
        }

        public void LoadContent(ContentManager Content)
        {
            //Allocate room for the new BulletTex
            texBullet = new BulletTex[1];

            //Assign the bullet texture and mark the origin
            texBullet[0] = new BulletTex(Content.Load<Texture2D>("bullet"), new Vector2(10, 4));
        }

        public void Update(GameTime gameTime)
        {
            List<Bullet> DeleteList = new List<Bullet>();

            int jotain = this.Enemies.Count;

            foreach (Bullet bullet in Bullets)
            {
                bullet.Position += bullet.Motion;

                // Check if they are on the screen still
                if (!screenEdges.Contains((int)bullet.Position.X, (int)bullet.Position.Y))
                {
                    DeleteList.Add(bullet);
                    //System.Diagnostics.Debug.Write("LUOTI POISTETAAN:");
                }
                
            } //foreach bullet

            //TODO:
            //foreach (Sprite enemyLocal in Enemies)
            //{   
                
                //Tutkitaan osuuko luoti johonkin vihollisiin tai muihin Spriteihin
                //if (Intersects(bullet.bulletRectangle,
                //                enemyLocal.Size))
                //{

                //    //DestroyEnemy(x);
                //    DeleteList.Add(bullet);
                //}
            //}
            
            foreach (Bullet bullet in DeleteList)
            {
                Bullets.Remove(bullet);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Bullet bullet in Bullets)
            {
                spriteBatch.Draw(texBullet[bullet.BulletType].Texture, bullet.Position, null, Color.White, 0.0f,
                                 texBullet[bullet.BulletType].Origin, 1.0f, SpriteEffects.None, 0.0f);
            }
        }

        public void DestroyBullet(Bullet incoming_bullet)
        {
            List<Bullet> DeleteList = new List<Bullet>();
            DeleteList.Add(incoming_bullet);

            //foreach (Bullet bullet in DeleteList)
            //{
                Bullets.Remove(incoming_bullet);
            //}
        }

        public void AddBullet(Bullet bullet)
        {
            Bullets.Add(bullet);
        }

        public void addEnemy(Sprite enemy)
        {
            Enemies.Add(enemy);
            System.Diagnostics.Debug.Write("Enemy Created!:" + Enemies.Count);
        }
    }
}
