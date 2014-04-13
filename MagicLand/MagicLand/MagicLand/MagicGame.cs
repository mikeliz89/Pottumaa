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
    //määritellään rajapinta näkymän ja sen yläluokan välille
    public interface IStartGameScreen : IGameScreen { }
    public interface IHomeScreen : IGameScreen { }
    public interface IAlchemyScreen : IGameScreen { }
    public interface IForestScreen1 : IGameScreen { }
    public interface IFarmScreen : IGameScreen { }
    public interface IToriScreen : IGameScreen { }
    public interface IForestScreen2 : IGameScreen { }

    public class MagicGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        GameScreenManager screenManager;

        public IStartGameScreen StartGameScreen;
        public IGameScreen HomeScreen;
        public IAlchemyScreen AlchemyScreen;
        public IForestScreen1 ForestScreen1;
        public IFarmScreen FarmScreen;
        public IToriScreen ToriScreen;
        public IForestScreen2 AdventureScreen2;

        static int windowHeight = 800;
        static int windowWidth = 1280;

        /* 
         SqlDataAdapter adapter = new SqlDataAdapter();
        adapter.SelectCommand = new SqlCommand(
            queryString, connection);
        adapter.Fill(dataset);
        return dataset;
         */

        public MagicGame()
        {
            screenManager = new GameScreenManager(this);
            Components.Add(screenManager);

            StartGameScreen = new StartGameScreen(this);
            HomeScreen = new HomeScreen(this);
            AlchemyScreen = new AlchemyScreen(this);
            ForestScreen1 = new ForestScreen1(this);
            AdventureScreen2 = new ForestScreen2(this);
            FarmScreen = new FarmScreen(this);
            ToriScreen = new ToriScreen(this);

            AudioManager.Initialize(this);

            //first game screen           
            screenManager.ChangeScreen(StartGameScreen.Screen);

            IsMouseVisible = true;

            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferHeight = windowHeight;
            graphics.PreferredBackBufferWidth = windowWidth;
            graphics.IsFullScreen = true;

            Content.RootDirectory = "Content";
        }

        protected bool Intersects(Rectangle rectA, Rectangle rectB)
        {
            // Returns True if rectA and rectB contain any overlapping points
            return (rectA.Right > rectB.Left && rectA.Left < rectB.Right &&
                    rectA.Bottom > rectB.Top && rectA.Top < rectB.Bottom);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            AudioManager.LoadMusic();
            AudioManager.LoadSounds();

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || (Keyboard.GetState().IsKeyDown(Keys.Escape) == true))
                this.Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
