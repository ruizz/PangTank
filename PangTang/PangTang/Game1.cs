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
using System.Xml.Serialization;




namespace PangTang
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        /*
         * Game Objects
         */
        Title title; // Title screen for the game.
        VideoAnimation startingAnimation; // Starting animation for the game.
        VideoAnimation endingAnimation; // Ending animation for the game.
        Nozzle nozzle; // Nozzle for the game.
        Funnel funnel; // Funnel for the game.
        Water water; // Water drops for the game.
        Tank tank; // Tank for the game.
        HighScores highScores; // High scores for the game.

        Border border; // Border between tank and play area

        Rectangle windowAreaRectangle; // Describes the entire window.
        Rectangle playAreaRectangle; // Describes playing area.
        Rectangle tankAreaRectangle; // Describes the fish tank area.

        Texture2D gameplayBackgroundTexture; // Background during gameplay.
        Texture2D titleBackgroundTexture; // Background during title and highscores.
        Texture2D highScoresBackgroundTexture;
        Texture2D blackTexture; // Used as a black screen.

        Texture2D waterDropUITexture; // Water drople used for the UI.
        
        /*
         * Audio
         */
        // Using SoundEffect instead of Song for seamless loops.
        Song titleMusic;
        Song gameplayMusic;
        Video startingVideo;
        Video endingVideo;

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
         * High Scores
         */
        Texture2D highScoresTitle; // Because we can.
        Rectangle orderingRectangle; // Rectangle for 1st, 2nd, etc.
        Rectangle highScoresVals;  // Rectangle for the high scores
        Texture2D retryButtonTexture;  // Retry button texture
        Rectangle retryButton; // The retry button

        /*
         * Fonts
         */
        SpriteFont dropsCaughtFont;
        SpriteFont levelFont;
        SpriteFont highScoresFont;

        /*
         * Constructor
         */
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 900;
            graphics.PreferredBackBufferHeight = 500;

            // Window area rectangle.
            windowAreaRectangle = new Rectangle(
                tankAreaRectangle.Width,
                0,
                graphics.PreferredBackBufferWidth,
                graphics.PreferredBackBufferHeight);

            // Fish tank area. Takes up 350 px of the left side of the screen.
            tankAreaRectangle = new Rectangle(
                0,
                0,
                350,
                graphics.PreferredBackBufferHeight);

            // Play area. Takes up whatever the fish tank area doesn't.
            playAreaRectangle = new Rectangle(
                tankAreaRectangle.Width,
                0,
                graphics.PreferredBackBufferWidth - tankAreaRectangle.Width,
                graphics.PreferredBackBufferHeight);

            // High scores ordering rectangle
            orderingRectangle = new Rectangle(
                0,
                0,
                (windowAreaRectangle.Width / 2) - 10,
                graphics.PreferredBackBufferHeight);

            // High scores rectangle
            highScoresVals = new Rectangle(
                (windowAreaRectangle.Width / 2) + 10,
                0,
                (windowAreaRectangle.Width / 2) - 10,
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
            this.Window.Title = "PangTank";
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

            // Assigning the black screen and background that is used during gameplay.
            titleBackgroundTexture = Content.Load<Texture2D>("background_title");
            highScoresBackgroundTexture = Content.Load<Texture2D>("background_highScores");
            gameplayBackgroundTexture = Content.Load<Texture2D>("background_game");
            blackTexture = Content.Load<Texture2D>("background_black");
            waterDropUITexture = Content.Load<Texture2D>("water_0");
            

            // Temporary texture arrays for loading batches of textures or textures that are animated. 
            Texture2D[] tempTextureArray;
            Texture2D[] tempTextureArray2;

            // Same as tempTexture, but this one is used for the tank.
            Texture2D[,] tempTexture2DArray;

            // Load all music
            titleMusic = Content.Load<Song>("titleMusic");
            gameplayMusic = Content.Load<Song>("gameplayMusic");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(titleMusic);

            // Load all video
            startingVideo = Content.Load<Video>("video_startingAnimation");
            endingVideo = Content.Load<Video>("video_endingAnimation");

            // Load title textures
            tempTextureArray = new Texture2D[4];
            tempTextureArray[0] = titleBackgroundTexture;
            tempTextureArray[1] = Content.Load<Texture2D>("title");
            tempTextureArray[2] = Content.Load<Texture2D>("button_start");
            tempTextureArray[3] = Content.Load<Texture2D>("button_mute");

            tempTextureArray2 = new Texture2D[3];
            tempTextureArray2[0] = Content.Load<Texture2D>("title_tank_0");
            tempTextureArray2[1] = Content.Load<Texture2D>("title_tank_1");
            title = new Title(tempTextureArray, tempTextureArray2, windowAreaRectangle);

            // Load starting and ending animation objects
            startingAnimation = new VideoAnimation(windowAreaRectangle, startingVideo);
            endingAnimation = new VideoAnimation(windowAreaRectangle, endingVideo);

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
            tempTexture2DArray = new Texture2D[4, 2];
            tempTexture2DArray[0, 0] = Content.Load<Texture2D>("tank_0_0"); 
            tempTexture2DArray[0, 1] = Content.Load<Texture2D>("tank_0_1"); 

            tempTexture2DArray[1, 0] = Content.Load<Texture2D>("tank_1_0");
            tempTexture2DArray[1, 1] = Content.Load<Texture2D>("tank_1_1");

            tempTexture2DArray[2, 0] = Content.Load<Texture2D>("tank_2_0");
            tempTexture2DArray[2, 1] = Content.Load<Texture2D>("tank_2_1");

            tempTexture2DArray[3, 0] = Content.Load<Texture2D>("leak_0");
            tempTexture2DArray[3, 1] = Content.Load<Texture2D>("leak_1");

            tank = new Tank(tempTexture2DArray, tankAreaRectangle);

            // Load fonts
            dropsCaughtFont = Content.Load<SpriteFont>("dropsCaughtFont");
            highScoresFont = Content.Load<SpriteFont>("highScoresFont");
            levelFont = Content.Load<SpriteFont>("levelFont");

            // Load Border
            tempTextureArray = new Texture2D[5];
            tempTextureArray[0] = Content.Load<Texture2D>("divider_0");
            tempTextureArray[1] = Content.Load<Texture2D>("divider_1");
            tempTextureArray[2] = Content.Load<Texture2D>("divider_2");
            tempTextureArray[3] = Content.Load<Texture2D>("divider_3");
            tempTextureArray[4] = Content.Load<Texture2D>("hoseEnd_0");
            border = new Border(tempTextureArray, playAreaRectangle);

            // Load High Scores title
            highScoresTitle = Content.Load<Texture2D>("title_highScores");

            //Load retry button
            retryButtonTexture = Content.Load<Texture2D>("button_retry");
            retryButton = new Rectangle(
                100,
                400,
                retryButtonTexture.Width,
                retryButtonTexture.Height);

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
            mouseState = Mouse.GetState();
            switch (gameState)
            {
                    
                case 0: // Title screen
                    
                    this.IsMouseVisible = true;
                    gamePadState = GamePad.GetState(PlayerIndex.One);
                    
                    // Start the game
                    if (title.isStartButtonPressed(mouseState))
                    {
                        this.IsMouseVisible = false;
                        MediaPlayer.Stop();
                        startingAnimation.Start();
                        gameState = 1;
                    }

                    if (title.isMuteButtonPressed(mouseState))
                    {
                        if (MediaPlayer.IsMuted == false)
                            MediaPlayer.IsMuted = true;
                        else
                            MediaPlayer.IsMuted = false;

                        startingAnimation.ChangeMute();
                        endingAnimation.ChangeMute();

                    }


                    break;
                case 1: // Starting animation
                    if (startingAnimation.isFinished() || mouseState.LeftButton == ButtonState.Pressed)
                    {
                        startingAnimation.Stop();
                        
                        MediaPlayer.Play(gameplayMusic);
                        gameState = 2;
                    }

                    break;
                case 2: // Game Started

                    // Allows the game to exit
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                        this.Exit();

                    // Take care of the fade from black first
                    if (!FadeInAnimation.IsFadeCompleted())
                        break;

                    if (!LevelChangeAnimation.IsAnimationCompleted())
                    {
                        funnel.Update();
                        border.Update(totalDropsCaught);
                        break;
                    }
                        
                    // Update all objects to new positions
                    funnel.Update();
                    nozzle.Update();
                    water.Update();
                    border.Update(totalDropsCaught);

                    // Release a drop during every specified interval
                    seconds += gameTime.ElapsedGameTime.TotalSeconds; // seconds += Elapsed time since last update
                    if (seconds > waterInterval)
                    {
                        seconds = 0.0;
                        water.releaseDrop(nozzle.GetBounds());
                    }

                    // Increase counters for any water-funnel collisions.
                    // From level 8 onward, return the entire funnel collision bounds because the water
                    // droplets move too fast between frames for collisions to be detected.
                    int collisions;
                    if (currentLevel < 8)
                        collisions = water.getFunnelCollisions(funnel.GetCollisionBounds());
                    else 
                        collisions = water.getFunnelCollisions(funnel.GetBounds());
                    
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

                        LevelChangeAnimation.Reset();
                    }

                    // Game Over
                    if (tank.getTankLevel() == 3)
                    {
                        MediaPlayer.Stop();
                        endingAnimation.Start();
                        gameState = 3;
                    }

                    base.Update(gameTime);
                    break;
                case 3: // Game Ended
                    if (endingAnimation.isFinished() || mouseState.LeftButton == ButtonState.Pressed)
                    {
                        endingAnimation.Stop();
                        FadeInAnimation.Reset();
                        MediaPlayer.Play(gameplayMusic);
                        highScores = new HighScores(totalDropsCaught);
                        gameState = 4;
                    }
                    break;
                case 4: // High Scores
                    this.IsMouseVisible = true;
                    //keyboardState = Keyboard.GetState();

                    if ((mouseState.LeftButton == ButtonState.Pressed) &&
                         mouseState.X > retryButton.X &&
                         mouseState.X < retryButton.X + retryButton.Width &&
                         mouseState.Y > retryButton.Y &&
                         mouseState.Y < retryButton.Y + retryButton.Height)
                    {
                        MediaPlayer.Stop();
                        gameState = 0;
                        ResetValues();
                    }
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
                    spriteBatch.Draw(gameplayBackgroundTexture, new Vector2(0, 0), Color.White);

                    funnel.Draw(spriteBatch);
                    nozzle.Draw(spriteBatch);
                    water.Draw(spriteBatch);
                    border.Draw(spriteBatch);
                    tank.Draw(spriteBatch);

                    DrawText();

                    FadeInAnimation.Draw(blackTexture, spriteBatch, windowAreaRectangle);

                    if (FadeInAnimation.IsFadeCompleted())
                    {
                        LevelChangeAnimation.Draw(spriteBatch, levelFont, currentLevel);
                    }
                    break;
                case 3: // Ending Animation

                    endingAnimation.Draw(spriteBatch);
                    break;
                case 4: // High Scores
                    spriteBatch.Draw(highScoresBackgroundTexture, new Vector2(0, 0), Color.White);
                    DrawText();

                    FadeInAnimation.Draw(blackTexture, spriteBatch, windowAreaRectangle);
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
                    break;
                case 1: // Starting Animation
                    break;
                case 2: // Game Started
                    spriteBatch.DrawString(dropsCaughtFont, "Level: " + currentLevel, new Vector2(23, 18), new Color(236, 166, 32));
                    spriteBatch.DrawString(dropsCaughtFont, "Total     : " + totalDropsCaught, new Vector2(23, 53), new Color(236, 166, 32));

                    spriteBatch.DrawString(dropsCaughtFont, "Level: " + currentLevel, new Vector2(20, 15), new Color(201, 110, 0));
                    spriteBatch.DrawString(dropsCaughtFont, "Total     : " + totalDropsCaught, new Vector2(20, 50), new Color(201, 110, 0));

                    spriteBatch.Draw(waterDropUITexture, new Vector2(73, 50), Color.White);
                    break;
                case 3: // Ending Animation
                    break;
                case 4: // High Scores

                    spriteBatch.Draw(highScoresTitle, new Vector2((windowAreaRectangle.Width / 2) - (highScoresTitle.Width / 2), windowAreaRectangle.Height / 30), Color.White);
                    spriteBatch.Draw(retryButtonTexture, new Vector2(100, 400), Color.White);
                    string first, second, third, fourth, fifth;
                    first = "" + highScores.getHighScore(1);
                    second = "" + highScores.getHighScore(2);
                    third = "" + highScores.getHighScore(3);
                    fourth = "" + highScores.getHighScore(4);
                    fifth = "" + highScores.getHighScore(5);

                    DrawScore1(highScoresFont, "1st:", orderingRectangle, 2, new Color(236, 166, 32));
                    DrawScore1(highScoresFont, "\n\n2nd:", orderingRectangle, 2, new Color(236, 166, 32));
                    DrawScore1(highScoresFont, "\n\n\n\n3rd:", orderingRectangle, 2, new Color(236, 166, 32));
                    DrawScore1(highScoresFont, "\n\n\n\n\n\n4th:", orderingRectangle, 2, new Color(236, 166, 32));
                    DrawScore1(highScoresFont, "\n\n\n\n\n\n\n\n5th:", orderingRectangle, 2, new Color(236, 166, 32));
                    DrawScore1(highScoresFont, first, highScoresVals, 1, new Color(236, 166, 32));
                    DrawScore1(highScoresFont, "\n\n" + second, highScoresVals, 1, new Color(236, 166, 32));
                    DrawScore1(highScoresFont, "\n\n\n\n" + third, highScoresVals, 1, new Color(236, 166, 32));
                    DrawScore1(highScoresFont, "\n\n\n\n\n\n" + fourth, highScoresVals, 1, new Color(236, 166, 32));
                    DrawScore1(highScoresFont, "\n\n\n\n\n\n\n\n" + fifth, highScoresVals, 1, new Color(236, 166, 32));

                    DrawScore2(highScoresFont, "1st:", orderingRectangle, 2, new Color(201, 110, 0));
                    DrawScore2(highScoresFont, "\n\n2nd:", orderingRectangle, 2, new Color(201, 110, 0));
                    DrawScore2(highScoresFont, "\n\n\n\n3rd:", orderingRectangle, 2, new Color(201, 110, 0));
                    DrawScore2(highScoresFont, "\n\n\n\n\n\n4th:", orderingRectangle, 2, new Color(201, 110, 0));
                    DrawScore2(highScoresFont, "\n\n\n\n\n\n\n\n5th:", orderingRectangle, 2, new Color(201, 110, 0));
                    DrawScore2(highScoresFont, first, highScoresVals, 1, new Color(201, 110, 0));
                    DrawScore2(highScoresFont, "\n\n" + second, highScoresVals, 1, new Color(201, 110, 0));
                    DrawScore2(highScoresFont, "\n\n\n\n" + third, highScoresVals, 1, new Color(201, 110, 0));
                    DrawScore2(highScoresFont, "\n\n\n\n\n\n" + fourth, highScoresVals, 1, new Color(201, 110, 0));
                    DrawScore2(highScoresFont, "\n\n\n\n\n\n\n\n" + fifth, highScoresVals, 1, new Color(201, 110, 0));
                    break;
            }
        }


        //Drawing the Strings for high scores
        public void DrawScore1(SpriteFont font, string text, Rectangle bounds, int align, Color color)
        {
            Vector2 size = font.MeasureString(text);
            Vector2 pos = new Vector2(bounds.Center.X, bounds.Center.Y - 50);
            Vector2 origin = size * 0.5f;

            if (align == 1)
                origin.X += bounds.Width / 2 - size.X / 2;

            if (align == 2)
                origin.X -= bounds.Width / 2 - size.X / 2;

            spriteBatch.DrawString(font, text, pos, color, 0, origin, 1, SpriteEffects.None, 0);
        }

        // Adds the shadow effect to the scores
        public void DrawScore2(SpriteFont font, string text, Rectangle bounds, int align, Color color)
        {
            Vector2 size = font.MeasureString(text);
            Vector2 pos = new Vector2(bounds.Center.X - 3, bounds.Center.Y - 53);
            Vector2 origin = size * 0.5f;

            if (align == 1)
                origin.X += bounds.Width / 2 - size.X / 2;

            if (align == 2)
                origin.X -= bounds.Width / 2 - size.X / 2;

            spriteBatch.DrawString(font, text, pos, color, 0, origin, 1, SpriteEffects.None, 0);
        }

        // Places the funnel and ball in the start positions.
        private void StartGame()
        {
            // Set funnel in start position
            funnel.SetInStartPosition();
            nozzle.SetInStartPosition();
        }

        // Resets the starting values
        private void ResetValues()
        {
            funnel.Reset();
            nozzle.Reset();
            water.Reset();
            tank.Reset();

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

            startingAnimation.Reset();
            endingAnimation.Reset();
            FadeInAnimation.Reset();
            LevelChangeAnimation.Reset();
            MediaPlayer.Play(titleMusic);
        }

        private class FadeInAnimation
        {
            private static int currentFrame = 255;
            private static bool fadeCompleted = false;

            public static void Draw(Texture2D blackTexture, SpriteBatch spriteBatch, Rectangle windowAreaRectangle)
            {
                if (currentFrame > 0)
                {
                    spriteBatch.Draw(blackTexture, windowAreaRectangle, new Color(255, 255, 255, currentFrame));
                    currentFrame -= 8;
                }
                else
                    fadeCompleted = true;
            }

            public static bool IsFadeCompleted()
            {
                return fadeCompleted;
            }

            public static void Reset()
            {
                fadeCompleted = false;
                currentFrame = 255;
            }
        }

        private class LevelChangeAnimation
        {
            private static int currentFrame = 600;
            private static bool animationCompleted = false;

            public static void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont, int level)
            {
                if (currentFrame > 0)
                {
                    spriteBatch.DrawString(spriteFont, "Level " + level, new Vector2(530, 205), new Color(236, 166, 32));
                    spriteBatch.DrawString(spriteFont, "Level " + level, new Vector2(525, 200), new Color(201, 110, 0));
                    currentFrame -= 8;
                }
                else
                    animationCompleted = true;
                
            }

            public static bool IsAnimationCompleted()
            {
                return animationCompleted;
            }

            public static void Reset()
            {
                animationCompleted = false;
                currentFrame = 600;
            }
        }

        // End of game1
    }
}
