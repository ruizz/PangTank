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
        Tank tank; // Tank for the game.

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
            Texture2D[] tempTexture;

            // Same as tempTexture, but this one is used for the tank.
            Texture2D[,] temp2DTexture;

            // Load the nozzle texture
            tempTexture = new Texture2D[3];
            tempTexture[0] = Content.Load<Texture2D>("nozzle_0");
            tempTexture[1] = Content.Load<Texture2D>("nozzle_1");
            tempTexture[2] = Content.Load<Texture2D>("nozzle_2");
            nozzle = new Nozzle(tempTexture, playAreaRectangle);

            // Load the funnel texture
            tempTexture = new Texture2D[3];
            tempTexture[0] = Content.Load<Texture2D>("funnel_0");
            tempTexture[1] = Content.Load<Texture2D>("funnel_1");
            tempTexture[2] = Content.Load<Texture2D>("funnel_2");
            funnel = new Funnel(tempTexture, playAreaRectangle);

            // Load the water texture
            tempTexture = new Texture2D[3];
            tempTexture[0] = Content.Load<Texture2D>("water_0");
            tempTexture[1] = Content.Load<Texture2D>("water_1");
            tempTexture[2] = Content.Load<Texture2D>("water_2");
            water = new Water(tempTexture, playAreaRectangle);

            // Load Tank (3x the sprites)
            temp2DTexture = new Texture2D[3, 3];
            temp2DTexture[0, 0] = Content.Load<Texture2D>("tank_0_0"); 
            temp2DTexture[0, 1] = Content.Load<Texture2D>("tank_0_1"); 
            temp2DTexture[0, 2] = Content.Load<Texture2D>("tank_0_2");

            temp2DTexture[1, 0] = Content.Load<Texture2D>("tank_1_0");
            temp2DTexture[1, 1] = Content.Load<Texture2D>("tank_1_1");
            temp2DTexture[1, 2] = Content.Load<Texture2D>("tank_1_2");

            temp2DTexture[2, 0] = Content.Load<Texture2D>("tank_2_0");
            temp2DTexture[2, 1] = Content.Load<Texture2D>("tank_2_1");
            temp2DTexture[2, 2] = Content.Load<Texture2D>("tank_2_2");

            tank = new Tank(temp2DTexture, tankAreaRectangle);

            // Load fonts
            dropsCaughtFont = Content.Load<SpriteFont>("dropsCaughtFont");
            titleScreenFont = Content.Load<SpriteFont>("titleScreenFont");

            // Load Border
            tempTexture = new Texture2D[1];
            tempTexture[0] = Content.Load<Texture2D>("tempBorder");
            border = new Border(tempTexture[0], playAreaRectangle);

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
                case 0: // Title screen
                    keyboardState = Keyboard.GetState();
                    gamePadState = GamePad.GetState(PlayerIndex.One);
                    if (keyboardState.IsKeyDown(Keys.S))
                        gameState = 2;
                    break;
                case 1: // Starting animation
                    break;
                case 2: // Game Started

                    // Allows the game to exit
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                        this.Exit();

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
                    int collisions = water.getFunnelCollisions(funnel.GetCollisionBounds());
                    totalDropsCaught += collisions;
                    levelDropsCaught += collisions;

                    // Update the tank threshold
                    int misses = water.getFunnelMisses();
                    tank.updateThreshold(levelDropRequirement, misses);

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
                    if (tank.getTankLevel() == 3) 
                        gameState = 3;

                    base.Update(gameTime);
                    break;
                case 3: // Game Ended
                    break;
                case 4: // High Scores
                    break;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            switch (gameState)
            {
                case 0: // Title Screen
                    spriteBatch.Begin();
                    
                    DrawText();

                    spriteBatch.End();

                    base.Draw(gameTime);
                    break;
                case 1: // Starting Animation
                    break;
                case 2: // Game Started
                    spriteBatch.Begin();

                    funnel.Draw(spriteBatch);
                    nozzle.Draw(spriteBatch);
                    water.Draw(spriteBatch);
                    border.Draw(spriteBatch);
                    tank.Draw(spriteBatch);

                    DrawText();

                    spriteBatch.End();

                    base.Draw(gameTime);
                    break;
                case 3: // Game Ended
                    spriteBatch.Begin();
                    
                    DrawText();

                    spriteBatch.End();

                    base.Draw(gameTime);
                    break;
                case 4: // High Scores
                    break;
            }
        }

        // Draws all text
        private void DrawText()
        {
            switch (gameState)
            {
                case 0: // Title Screen
                    spriteBatch.DrawString(titleScreenFont, "Temporary Title", new Vector2(350, 100), Color.Black);
                    spriteBatch.DrawString(dropsCaughtFont, "Press S to Start", new Vector2(350, 150), Color.Black);
                    break;
                case 1: // Starting Animation
                    break;
                case 2: // Game Started
                    spriteBatch.DrawString(dropsCaughtFont, "Level: " + currentLevel, new Vector2(20, 20), Color.Black);
                    spriteBatch.DrawString(dropsCaughtFont, "Level Drops Caught: " + levelDropsCaught, new Vector2(20, 50), Color.Black);
                    spriteBatch.DrawString(dropsCaughtFont, "Total Drops Caught: " + totalDropsCaught, new Vector2(20, 80), Color.Black);
                    break;
                case 3: // Game Ended
                    spriteBatch.DrawString(dropsCaughtFont, "Game Over", new Vector2(350, 100), Color.Black);
                    spriteBatch.DrawString(dropsCaughtFont, "Your Score: " + totalDropsCaught, new Vector2(350, 200), Color.Black);
                    break;
                case 4: // High Scores
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
