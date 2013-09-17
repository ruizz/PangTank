using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PangTang
{
    class Title
    {
        /*
         * Position
         */
        Vector2 logoPosition;
        Vector2 tankPosition;
        Vector2 startButtonPosition;
        Vector2 muteButtonPosition;

        /*
         * Other
         */
        Texture2D backgroundTexture;
        Texture2D logoTexture;
        Texture2D startButtonTexture;
        Texture2D muteButtonTexture;
        Texture2D[] tankTextures;
        int textureStage;
        Rectangle windowAreaRectangle;
        Rectangle startButton;
        Rectangle muteButton;
        bool buttonHeldDown;

        /*
         * Constructor
         */

        // Textures for titleTextures
        // 0 - background
        // 1 - logo
        // 2 - start button
        public Title(Texture2D[] titleTextures, Texture2D[] tankTextures, Rectangle windowAreaRectangle)
        {
            // Setting the window area
            this.windowAreaRectangle = windowAreaRectangle;

            // Setting the logo, tank, and start button textures
            backgroundTexture = titleTextures[0];
            logoTexture = titleTextures[1];
            startButtonTexture = titleTextures[2];
            muteButtonTexture = titleTextures[3];

            // Setting the tank tank textures
            this.tankTextures = tankTextures;
            textureStage = 0;

            // Positioning the logo
            logoPosition.X = windowAreaRectangle.Width / 2;
            logoPosition.X -= logoTexture.Width / 2;
            logoPosition.Y = windowAreaRectangle.Height / 30;

            // Positioning the start button
            startButtonPosition.X = windowAreaRectangle.Width / 2;
            startButtonPosition.X -= startButtonTexture.Width / 2;
            startButtonPosition.Y = (windowAreaRectangle.Height / 6) * 5;

            // Positioning the mute button
            muteButtonPosition.X = windowAreaRectangle.Width / 8;
            muteButtonPosition.X -= muteButtonTexture.Width / 2;
            muteButtonPosition.Y = (windowAreaRectangle.Height / 6) * 5;

            // Creating rectangle for start button
            startButton = new Rectangle((int)startButtonPosition.X, (int)startButtonPosition.Y,
                startButtonTexture.Width, startButtonTexture.Height);

            muteButton = new Rectangle((int)muteButtonPosition.X, (int)muteButtonPosition.Y,
                muteButtonTexture.Width, muteButtonTexture.Height);

            // Positioning the tank
            tankPosition.X = (windowAreaRectangle.Width - tankTextures[0].Width) / 2;
            tankPosition.X += windowAreaRectangle.X;
            tankPosition.Y = (windowAreaRectangle.Height - tankTextures[0].Height) / 2;
            tankPosition.Y += 25; // Impromptu fix to conform to the logo change.
            tankPosition.Y += windowAreaRectangle.Y;

            // Setting a flag
            buttonHeldDown = false;
        }

        /*
         * Returns
         */
        public bool isStartButtonPressed(MouseState mouseState)
        {
            if ((mouseState.LeftButton == ButtonState.Pressed) &&
                         mouseState.X > startButtonPosition.X &&
                         mouseState.X < startButtonPosition.X + startButton.Width &&
                         mouseState.Y > startButtonPosition.Y &&
                         mouseState.Y < startButtonPosition.Y + startButton.Height)
                return true;
            else
                return false;
        }

        public bool isMuteButtonPressed(MouseState mouseState)
        {
            if (mouseState.LeftButton == ButtonState.Released)
            {
                buttonHeldDown = false;
                return false;
            }

            if (buttonHeldDown)
                return false;

            if (!buttonHeldDown &&
                        (mouseState.LeftButton == ButtonState.Pressed) &&
                         mouseState.X > muteButtonPosition.X &&
                         mouseState.X < muteButtonPosition.X + muteButton.Width &&
                         mouseState.Y > muteButtonPosition.Y &&
                         mouseState.Y < muteButtonPosition.Y + muteButton.Height)
            {
                buttonHeldDown = true;
                return true;
            }
            else
                return false;
        }

        /*
         * Voids
         */
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backgroundTexture, new Vector2(0, 0), Color.White);
            spriteBatch.Draw(logoTexture, logoPosition, Color.White);
            spriteBatch.Draw(startButtonTexture, startButtonPosition, Color.White);
            spriteBatch.Draw(muteButtonTexture, muteButtonPosition, Color.White);

            if (textureStage <= 6) // Draw first sprite.
                spriteBatch.Draw(tankTextures[0], tankPosition, Color.White);


            if (textureStage > 6) // Draw second sprite.
            {
                spriteBatch.Draw(tankTextures[1], tankPosition, Color.White);

                // Reset the texture stage once the third sprite finishes animating.
                if (textureStage >= 12)
                    textureStage = -1;
            }

            textureStage++;
        }
    }
}
