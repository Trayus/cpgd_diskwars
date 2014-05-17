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

using Graphics2D;
using Helpers;
using DiskWars.parser;


namespace DiskWars
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        State menu, game, replay, current;
        private static Game1 instance;
        bool tpressed = false;
        public static KnowledgeParser kp;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            instance = this;

            if (Constants.DEBUG)
            {
                graphics.PreferredBackBufferWidth = 1366;
                graphics.PreferredBackBufferHeight = 768;
                graphics.IsFullScreen = false;
            } 
            else
            {
                graphics.PreferredBackBufferWidth = 1920;
                graphics.PreferredBackBufferHeight = 1080;
                graphics.IsFullScreen = true;
            }

            new OnceInput(this);
            new Rand();
            new SoundManager(this);


            if (Constants.DEBUG)
            {
                this.Components.Add(new RenderingEngine(this, 1366, 768));
            }
            else
            {
                this.Components.Add(new RenderingEngine(this, 1920, 1080));
            }
            kp = new KnowledgeParser();
            kp.ParsePlaytestDirectory("../../../parser/dw_kb");
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
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

            current = menu = new MenuState(Content);
            game = new GameState(Content);
            replay = new ReplayState(Content);

            current.enterState();
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
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            current.update(gameTime.ElapsedGameTime.Milliseconds);

            if (Keyboard.GetState().IsKeyDown(Keys.F7) && !tpressed)
            {
                RenderingEngine.useLights = !RenderingEngine.useLights;
                tpressed = true;
            }
            if (!Keyboard.GetState().IsKeyDown(Keys.F7))
                tpressed = false;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // all drawing is done by RenderingEngine
            base.Draw(gameTime);
        }

        private void swapState(int state)
        {
            current.exitState();
            if (state == State.MENU)
                current = menu;
            else if (state == State.GAME)
                current = game;
            else
                current = replay;
            current.enterState();
        }

        public static void goToState(int state)
        {
            instance.swapState(state);
        }
    }
}
