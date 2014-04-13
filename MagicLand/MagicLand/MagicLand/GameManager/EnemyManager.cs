using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace MagicLand
{
    class EnemyManager : GameScreen
    {
        protected MagicGame XGame;
        protected ContentManager Content;

        //EnemySprite enemy;

        public EnemyManager(Game game, Vector2 position, string texture)  
            : base(game)
        {
            Content = game.Content;
            XGame = (MagicGame)game;

          //  enemy = new EnemySprite(XGame, position, texture);
          //  XGame.Components.Add(enemy);
          //  enemy.Enabled = false;
          //  enemy.Visible = false;
        }

        public void SetEnemyVisible(bool value)
        {
            if (value == true)
            {
            //    enemy.Enabled = true;
            //    enemy.Visible = true;
            }
            else
            {
            //    enemy.Enabled = false;
            //    enemy.Visible = false;
            }
        }
    }
}
