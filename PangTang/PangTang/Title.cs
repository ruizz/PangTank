using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

        /*
         * Other
         */
        Texture2D backgroundTexture;
        Texture2D logoTexture;
        Texture2D startButtonTexture;
        Texture2D[] tankTextures;
        int textureStage;
        Rectangle windowAreaRectangle;

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
            startButtonPosition.Y = (windowAreaRectangle.Height / 5) * 4;

            // Positioning the tank
            tankPosition.X = (windowAreaRectangle.Width - tankTextures[0].Width) / 2;
            tankPosition.X += windowAreaRectangle.X;
            tankPosition.Y = (windowAreaRectangle.Height - tankTextures[0].Height) / 2;
            tankPosition.Y += windowAreaRectangle.Y;
        }

        /*
         * Returns
         */
        public Rectangle getStartButtonBounds()
        {
            return new Rectangle(
                (int) startButtonPosition.X,
                (int) startButtonPosition.Y,
                startButtonTexture.Width,
                startButtonTexture.Height);
        }

        /*
         * Voids
         */
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backgroundTexture, new Vector2(0, 0), Color.White);
            spriteBatch.Draw(logoTexture, logoPosition, Color.White);
            spriteBatch.Draw(startButtonTexture, startButtonPosition, Color.White);

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
