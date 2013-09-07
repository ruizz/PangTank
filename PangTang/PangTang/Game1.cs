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

        Title title; // Title screen for the game.
        StartingAnimation startingAnimation; // Starting animation for the game.
        Nozzle nozzle; // Nozzle for the game.
        Funnel funnel; // Funnel for the game.
        Water water; // Water drops for the game.
        Tank tank; // Tank for the game.

        Border border; // Border between tank and play area

        Rectangle playAreaRectangle; // Describes playing area.
        Rectangle tankAreaRectangle; // Describes the fish tank area.

        Texture2D backgroundTexture; // Background during gameplay.
        
        /*
         * Audio
         */
        // Using SoundEffect instead of Song for seamless loops.
        SoundEffect titleMusic;
        SoundEffect gameplayMusic;
        Video startingVideo;
        SoundEffectInstance musicInstance;

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

        // KeyboardState keyboardState;
        MouseState mouseState;
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

            // Assigning the background that is used during gameplay.
            backgroundTexture = Content.Load<Texture2D>("background_game");

            // Temporary texture for loading textures. e.g. The title logo
            Texture2D tempTexture;

            // Temporary texture for loading the background. (Used by Title class.)
            Texture2D tempTextureBackground;

            // Temporary texture for loading textures that are animated. In this case the funnel, nozzle, etc.
            Texture2D[] tempTextureArray;

            // Same as tempTexture, but this one is used for the tank.
            Texture2D[,] tempTexture2DArray;

            // Load all music
            titleMusic = Content.Load<SoundEffect>("titleMusic");
            gameplayMusic = Content.Load<SoundEffect>("gameplayMusic");
            musicInstance = titleMusic.CreateInstance();
            musicInstance.IsLooped = true;
            musicInstance.Play();

            // Load all video
            startingVideo = Content.Load<Video>("video_startingAnimation");

            // Load title textures
            tempTexture = Content.Load<Texture2D>("title");
            tempTextureBackground = Content.Load<Texture2D>("background_title");

            tempTextureArray = new Texture2D[3];
            tempTextureArray[0] = Content.Load<Texture2D>("tank_1_0");
            tempTextureArray[1] = Content.Load<Texture2D>("tank_1_1");
            tempTextureArray[2] = Content.Load<Texture2D>("tank_1_2");
            //new Title(tempTexture, tempTextureArray, "backgroundTexture", new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight));
            title = new Title(tempTextureBackground, tempTexture, tempTextureArray, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight));

            // Load starting animation textures
            startingAnimation = new StartingAnimation(new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), startingVideo);

            // Load the nozzle texture
            tempTextureArray = new Texture2D[3];
            tempTextureArray[0] = Content.Load<Texture2D>("nozzle_0");
            tempTextureArray[1] = Content.Load<Texture2D>("nozzle_1");
            tempTextureArray[2] = Content.Load<Texture2D>("nozzle_2");
            nozzle = new Nozzle(tempTextureArray, playAreaRectangle);

            // Load the funnel texture
            tempTextureArray = new Texture2D[3];
            tempTextureArray[0] = Content.Load<Texture2D>("funnel_0");
            tempTextureArray[1] = Content.Load<Texture2D>("funnel_1");
            tempTextureArray[2] = Content.Load<Texture2D>("funnel_2");
            funnel = new Funnel(tempTextureArray, playAreaRectangle);

            // Load the water texture
            tempTextureArray = new Texture2D[3];
            tempTextureArray[0] = Content.Load<Texture2D>("water_0");
            tempTextureArray[1] = Content.Load<Texture2D>("water_1");
            tempTextureArray[2] = Content.Load<Texture2D>("water_2");
            water = new Water(tempTextureArray, playAreaRectangle);

            // Load Tank (3x the sprites)
            tempTexture2DArray = new Texture2D[3, 3];
            tempTexture2DArray[0, 0] = Content.Load<Texture2D>("tank_0_0"); 
            tempTexture2DArray[0, 1] = Content.Load<Texture2D>("tank_0_1"); 
            tempTexture2DArray[0, 2] = Content.Load<Texture2D>("tank_0_2");

            tempTexture2DArray[1, 0] = Content.Load<Texture2D>("tank_1_0");
            tempTexture2DArray[1, 1] = Content.Load<Texture2D>("tank_1_1");
            tempTexture2DArray[1, 2] = Content.Load<Texture2D>("tank_1_2");

            tempTexture2DArray[2, 0] = Content.Load<Texture2D>("tank_2_0");
            tempTexture2DArray[2, 1] = Content.Load<Texture2D>("tank_2_1");
            tempTexture2DArray[2, 2] = Content.Load<Texture2D>("tank_2_2");

            tank = new Tank(tempTexture2DArray, tankAreaRectangle);

            // Load fonts
            dropsCaughtFont = Content.Load<SpriteFont>("dropsCaughtFont");

            // Load Border
            tempTextureArray = new Texture2D[1];
            tempTextureArray[0] = Content.Load<Texture2D>("tempBorder");
            border = new Border(tempTextureArray[0], playAreaRectangle);

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
                    this.IsMouseVisible = true;
                    //keyboardState = Keyboard.GetState();
                    mouseState = Mouse.GetState();
                    gamePadState = GamePad.GetState(PlayerIndex.One);
                    
                    if ((mouseState.LeftButton == ButtonState.Pressed) && 
                         mouseState.X > title.getTankPosition().X &&
                         mouseState.X < title.getTankPosition().X + title.getTankWidth() &&
                         mouseState.Y > title.getTankPosition().Y &&
                         mouseState.Y < title.getTankPosition().Y + title.getTankHeight())
                    {
                        this.IsMouseVisible = false;
                        musicInstance.Stop();
                        startingAnimation.Start();
                        gameState = 1;
                    }
                    break;
                case 1: // Starting animation
                    if (startingAnimation.isFinished())
                    {
                        musicInstance = gameplayMusic.CreateInstance();
                        musicInstance.IsLooped = true;
                        musicInstance.Play();
                        gameState = 2;
                    }
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
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            switch (gameState)
            {
                case 0: // Title Screen
                    
                    title.Draw(spriteBatch);

                    DrawText();
                    break;
                case 1: // Starting Animation

                    startingAnimation.Draw(spriteBatch);
                    break;
                case 2: // Game Started

                    // Drawing the background first.
                    spriteBatch.Draw(backgroundTexture, new Vector2(0, 0), Color.White);

                    funnel.Draw(spriteBatch);
                    nozzle.Draw(spriteBatch);
                    water.Draw(spriteBatch);
                    border.Draw(spriteBatch);
                    tank.Draw(spriteBatch);

                    DrawText();
                    break;
                case 3: // Game Ended
                    
                    DrawText();
                    break;
                case 4: // High Scores
                    break;
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        // Draws all text
        private void DrawText()
        {
            switch (gameState)
            {
                case 0: // Title Screen
                    spriteBatch.DrawString(dropsCaughtFont, "Click to start.", new Vector2(375, (graphics.PreferredBackBufferHeight / 10)*9), Color.Black);
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
