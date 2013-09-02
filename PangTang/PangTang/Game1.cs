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
        Water water; // Water drops for the game.

        Border border; // Border between tank and play area

        Rectangle playAreaRectangle; // Describes playing area.
        Rectangle tankAreaRectangle; // Describes the fish tank area.
        
        /*
         * States
         */

        enum GameState  // State of the game
        {
            TitleScreen         = 0,
            StartingAnimation   = 1,
            GameStarted         = 2,
            GameEnded           = 3,
            HighScores          = 4,
        }

        KeyboardState keyboardState;
        GamePadState gamePadState;
        int gameState;  // The current game state


        /*
         * Status
         */

        
        double waterInterval; // Interval water drops to come out of nozzle.
        double seconds; // Seconds elapsed. Uses XNA's GameTime
        int totalDropsCaught; // Total drops caught.
        int currentLevel;
        int levelDropRequirement; // Drops required to compelete level. +5 drops for each level.
        int levelDropsCaught; // Number of drops caught in the level.
        float speedMultiplier; // How much to speed up the game by whenever level changes.

        int gameOverNumber; // If missed this number of drops, game over

        /*
         * Fonts
         */
        SpriteFont dropsCaughtFont;
        SpriteFont titleScreenFont;

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

            // Set some starting values.
            seconds = 0.0f;
            totalDropsCaught = 0;
            levelDropsCaught = 0;
            currentLevel = 1;
            gameOverNumber = 3;

            gameState = 0;

            // -- CHANGE ANY DEFAULT STARTING VALUES HERE --
            waterInterval = 2.0f;
            levelDropRequirement = 5;
            speedMultiplier = 1.4f;
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
            water = new Water(tempTexture, playAreaRectangle);

            // Load fonts
            dropsCaughtFont = Content.Load<SpriteFont>("dropsCaughtFont");
            titleScreenFont = Content.Load<SpriteFont>("titleScreenFont");

            // Load Border
            tempTexture = Content.Load<Texture2D>("tempBorder");
            border = new Border(tempTexture, playAreaRectangle);

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
            switch (gameState)
            {
                case 0:
                    keyboardState = Keyboard.GetState();
                    gamePadState = GamePad.GetState(PlayerIndex.One);
                    if (keyboardState.IsKeyDown(Keys.S))
                        gameState = 2;
                    break;
                case 1:
                    break;
                case 2:
                    // Allows the game to exit
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                        this.Exit();

                    // TODO: Add your update logic here

                    // Update all objects to new positions
                    funnel.Update();
                    nozzle.Update();
                    water.Update();

                    // Release a drop during every specified interval
                    seconds += gameTime.ElapsedGameTime.TotalSeconds; // seconds += Elapsed time since last update
                    if (seconds > waterInterval)
                    {
                        seconds = 0.0;
                        water.releaseDrop(nozzle.GetBounds());
                    }

                    // Increase counters for any water-funnel collisions.
                    int collisions = water.funnelCollisions(funnel.GetCollisionBounds());
                    totalDropsCaught += collisions;
                    levelDropsCaught += collisions;

                    // Next level!
                    if (levelDropsCaught == levelDropRequirement)
                    {
                        currentLevel += 1;
                        levelDropRequirement += 5;
                        levelDropsCaught = 0;

                        // Increase speeds
                        nozzle.increaseSpeed(speedMultiplier);
                        water.increaseSpeed(speedMultiplier);
                        waterInterval /= speedMultiplier;

                        // Decrease the multiplier so that the speed increases per level don't become drastic.
                        if (speedMultiplier > 1.0f)
                            speedMultiplier *= 0.98f;
                    }

                    // Game Over
                    if (water.missedDropCount() == gameOverNumber) 
                        gameState = 3;

                    base.Update(gameTime);
                    break;
                case 3:
                    break;
                case 4:
                    break;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            switch (gameState)
            {
                case 0:
                    spriteBatch.Begin();
                    
                    DrawText();

                    spriteBatch.End();

                    base.Draw(gameTime);
                    break;
                case 1:
                    break;
                case 2:
                    // TODO: Add your drawing code here
                    spriteBatch.Begin();

                    funnel.Draw(spriteBatch);
                    nozzle.Draw(spriteBatch);
                    water.Draw(spriteBatch);
                    border.Draw(spriteBatch);

                    DrawText();

                    spriteBatch.End();

                    base.Draw(gameTime);
                    break;
                case 3:
                    spriteBatch.Begin();
                    
                    DrawText();

                    spriteBatch.End();

                    base.Draw(gameTime);
                    break;
                case 4:
                    break;
            }
        }

        // Draws all text
        private void DrawText()
        {
            switch (gameState)
            {
                case 0:
                    spriteBatch.DrawString(titleScreenFont, "Temporary Title", new Vector2(350, 100), Color.White);
                    spriteBatch.DrawString(dropsCaughtFont, "Press S to Start", new Vector2(350, 150), Color.White);
                    break;
                case 1:
                    break;
                case 2:
                    spriteBatch.DrawString(dropsCaughtFont, "Level: " + currentLevel, new Vector2(20, 20), Color.White);
                    spriteBatch.DrawString(dropsCaughtFont, "Level Drops Caught: " + levelDropsCaught, new Vector2(20, 40), Color.White);
                    spriteBatch.DrawString(dropsCaughtFont, "Total Drops Caught: " + totalDropsCaught, new Vector2(20, 60), Color.White);
                    break;
                case 3:
                    spriteBatch.DrawString(dropsCaughtFont, "Game Over", new Vector2(350, 100), Color.White);
                    spriteBatch.DrawString(dropsCaughtFont, "Your Score: " + totalDropsCaught, new Vector2(350, 200), Color.White);
                    break;
                case 4:
                    break;
            }
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
