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


namespace PangTang
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        /*
         * Objects
         */
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Nozzle nozzle; // Nozzle for the game.
        Funnel funnel; // Funnel for the game.
        Water[] water; // Water drops for the game.

        Rectangle playAreaRectangle; // Describes playing area.
        Rectangle tankAreaRectangle; // Describes the fish tank area.
        
        /*
         * Status
         */
        double seconds; // Seconds elapsed. Uses XNA's GameTime
        int waterCounter; // Position in the water array. 

        /*
         * Constructor
         */
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 900;
            graphics.PreferredBackBufferHeight = 500;

            // Fish tank area. Takes up 300 px of the left side of the screen.
            tankAreaRectangle = new Rectangle(
                0,
                0,
                300,
                graphics.PreferredBackBufferHeight);

            // Play area. Takes up whatever the fish tank area doesn't.
            playAreaRectangle = new Rectangle(
                tankAreaRectangle.Width,
                0,
                graphics.PreferredBackBufferWidth - tankAreaRectangle.Width,
                graphics.PreferredBackBufferHeight);

            seconds = 0.0;
            waterCounter = 0;

            this.Window.Title = "PangTang";
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

            // TODO: use this.Content to load your game content here

            // Temporary texture for loading textures. In this case the funnel, nozzle, etc.
            Texture2D tempTexture;

            // Load the nozzle texture
            tempTexture = Content.Load<Texture2D>("nozzle");
            nozzle = new Nozzle(tempTexture, playAreaRectangle);

            // Load the funnel texture
            tempTexture = Content.Load<Texture2D>("funnel");
            funnel = new Funnel(tempTexture, playAreaRectangle);

            // Load the water texture
            tempTexture = Content.Load<Texture2D>("water");
            water = new Water[10];
            for (int i = 0; i < 10; i++)
            {
                water[i] = new Water(tempTexture, playAreaRectangle);
            }

            StartGame();
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            funnel.Update();
            nozzle.Update();

            seconds += gameTime.ElapsedGameTime.TotalSeconds; // Elapsed time since last update
            if (seconds > 1.0)
            {
                seconds -= 1.0;
                if (!water[waterCounter].IsActive())
                    water[waterCounter].Update(nozzle.GetBounds()); // Spit out a water droplet every second
                waterCounter++;
                if (waterCounter == 10)
                    waterCounter = 0;
            }

            for (int i = 0; i < 10; i++)
            {
                if (water[i].IsActive())
                    water[i].Update(nozzle.GetBounds()); // Water traverses down
            }

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
            spriteBatch.Begin();

            funnel.Draw(spriteBatch);
            nozzle.Draw(spriteBatch);

            for (int i = 0; i < 10; i++)
            {
                water[i].Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        // Places the funnel and ball in the start positions.
        private void StartGame()
        {
            // Set funnel in start position
            funnel.SetInStartPosition();
            nozzle.SetInStartPosition();
        }

        // End of game1
    }
}
