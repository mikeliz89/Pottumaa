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
    /// <summary>
    /// Pelinäyttöjen perusluokka eli kaikki pelinäytöt periytyvät tästä luokasta.
    /// Toteuttaa IGameScreen-rajapinnan ja perii DrawableGameComponentin eli tässä 
    /// voidaan toteuttaa myös draw-metodi.
    /// </summary>
    public abstract partial class GameScreen : Microsoft.Xna.Framework.DrawableGameComponent, IGameScreen
    {
        protected Rectangle TitleSafeArea; // Tämä tarvitaan lähinna XBOX-peleissä.
        protected IGameScreenManager ScreenManager;

        public GameScreen(Game game)
            : base(game)
        {
            // Luodaan ScreenManager, jonka avulla pelinäytöt voivat käyttää GameScreenManager-luokan palveluja.
            ScreenManager = (IGameScreenManager)game.Services.GetService(typeof(IGameScreenManager));
        }

        public GameScreen Screen
        {
            get
            {
                return (this);
            }
        }

        protected override void LoadContent()
        {
            TitleSafeArea = GraphicsDevice.Viewport.TitleSafeArea;
        }

        /// <summary>
        /// OnScreenChange-eventin tapahtumakäsittelijä. 
        /// Tutkitaan onko näyttö päällimmäisenä pinossa, jos on se tuodaan näkyviin ja aktivoidaan.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal protected virtual void ScreenChanged(object sender, EventArgs e)
        {
            if (ScreenManager.TopScreen == this.Screen)
                Visible = Enabled = true;
            else
                Visible = Enabled = false;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);
        }
    }
}
