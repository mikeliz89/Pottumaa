using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace MagicLand
{

    public enum ScreenState
    {
        TransitionOn,
        Active,
        TransitionOff,
        Hidden,
    }
    /// <summary>
    /// GameScreen-luokan ja pelinäyttöjen välissä oleva apuluokka.
    /// Tähän luokkaan voidaan koota sellaisia rakenteita, joita käytetään useassa eri näyttöluokassa.
    /// Nämä rakenteet voidaan toteuttaa myös GameScreen luokassa.
    /// </summary>
    public partial class MainGameScreen : GameScreen
    {
        protected MagicGame XGame;
        protected ContentManager Content;
        static protected Boolean buttonSpeedBump;   // Näppäin painnalluksien hidastustoiminto
        Thread buttonSpeedBumpThread = null;


        #region Netistapollitty

        /// <summary>
        /// Normally when one screen is brought up over the top of another,
        /// the first screen will transition off to make room for the new
        /// one. This property indicates whether the screen is only a small
        /// popup, in which case screens underneath it do not need to bother
        /// transitioning off.
        /// </summary>
        public bool IsPopup
        {
            get { return isPopup; }
            protected set { isPopup = value; }
        }

        bool isPopup = false;


        /// <summary>
        /// Indicates how long the screen takes to
        /// transition on when it is activated.
        /// </summary>
        public TimeSpan TransitionOnTime
        {
            get { return transitionOnTime; }
            protected set { transitionOnTime = value; }
        }

        TimeSpan transitionOnTime = TimeSpan.Zero;


        /// <summary>
        /// Indicates how long the screen takes to
        /// transition off when it is deactivated.
        /// </summary>
        public TimeSpan TransitionOffTime
        {
            get { return transitionOffTime; }
            protected set { transitionOffTime = value; }
        }

        TimeSpan transitionOffTime = TimeSpan.Zero;


        /// <summary>
        /// Gets the current position of the screen transition, ranging
        /// from zero (fully active, no transition) to one (transitioned
        /// fully off to nothing).
        /// </summary>
        public float TransitionPosition
        {
            get { return transitionPosition; }
            protected set { transitionPosition = value; }
        }

        float transitionPosition = 1;


        /// <summary>
        /// Gets the current alpha of the screen transition, ranging
        /// from 1 (fully active, no transition) to 0 (transitioned
        /// fully off to nothing).
        /// </summary>
        public float TransitionAlpha
        {
            get { return 1f - TransitionPosition; }
        }


        /// <summary>
        /// Gets the current screen transition state.
        /// </summary>
        public ScreenState ScreenState
        {
            get { return screenState; }
            protected set { screenState = value; }
        }

        ScreenState screenState = ScreenState.TransitionOn;


        #endregion

        public MainGameScreen(Game game)
            : base(game)
        {
            Content = game.Content;
            XGame = (MagicGame)game;
        }

        public Boolean ButtonSpeedBump
        {
            get
            {
                return buttonSpeedBump;
            }

            set
            {
                buttonSpeedBump = value;
            }
        }

        public void StartButtonSpeedBump()
        {
            Thread.Sleep(500);

            ButtonSpeedBump = true;

            buttonSpeedBumpThread = new Thread(new ThreadStart(ButtonSpeedBumpThread));
            buttonSpeedBumpThread.Start();
        }

        protected void ButtonSpeedBumpThread()
        {
            Thread.Sleep(500);

            ButtonSpeedBump = false;
        }

        public override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back ==
                ButtonState.Pressed && ButtonSpeedBump == false)
            {
                XGame.Exit();
            }

            base.Update(gameTime);
        }
    }
}
