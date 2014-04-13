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
    /// Tämä luokka sisältää pinon aktiivisena olevia pelitiloja varten. 
    /// Pinossa päällimmäisenä näkyvä näyttö on näkyvissä kuvaruudulla.
    /// Luokka toteuttaa IGameScreenManager-rajapinnan ja perii GameComponent-luokan.
    /// </summary>
    public class GameScreenManager : Microsoft.Xna.Framework.GameComponent, IGameScreenManager
    {
        private int drawOrder;
        private int baseDrawOrder = 20;
        public event EventHandler OnScreenChange;
        private Stack<GameScreen> screenStack = new Stack<GameScreen>();

        public GameScreenManager(Game game)
            : base(game)
        {
            game.Services.AddService(typeof(IGameScreenManager), this);
            drawOrder = baseDrawOrder;
        }

        public GameScreen TopScreen
        {
            get
            {
                return (screenStack.Peek());    // Palauttaa aktiivisen näytön
            }
        }

        public bool ContainsScreen(GameScreen Screen)
        {
            return (screenStack.Contains(Screen));  // Selvittää on näyttö pinossa
        }

        
        public void PopScreen()
        {
            RemoveScreen();
            drawOrder -= 1;

            if (OnScreenChange != null)
                OnScreenChange(this, null);
        }

        // Suoritetaan näytön poistaminen pinosta.
        public void RemoveScreen()
        {
            GameScreen ScreenToPopOut = (GameScreen)screenStack.Peek();

            // Unregister the event for this screen
            OnScreenChange -= ScreenToPopOut.ScreenChanged;

            // Remove the screen from game components
            Game.Components.Remove(ScreenToPopOut);

            screenStack.Pop();
        }

        public void PushScreen(GameScreen NewScreen)
        {
            drawOrder += 1;
            NewScreen.DrawOrder = drawOrder;

            AddScreen(NewScreen);

            if (OnScreenChange != null)
                OnScreenChange(this, null);
        }

        // Suoritetaan näytön lisääminen pinoon.
        private void AddScreen(GameScreen Screen)
        {
            screenStack.Push(Screen);

            Game.Components.Add(Screen);

            // Register the event for this screen
            OnScreenChange += Screen.ScreenChanged;
        }

        // Tyhjentää koko pinon ja lisää pinoon yhden näytön.
        public void ChangeScreen(GameScreen NewScreen)
        {
            // We are changing screen, so pop everything off
            while (screenStack.Count > 0)
                RemoveScreen();

            // Changing screen, reset draw order
            NewScreen.DrawOrder = drawOrder = baseDrawOrder;
            AddScreen(NewScreen);

            // Inform everyone we just changed screen
            if (OnScreenChange != null)
                OnScreenChange(this, null);
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
