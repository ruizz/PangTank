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

        /*
         * States
         */
        bool gameStarted = false; // true if user clicks on tank.

        /*
         * Other
         */
        Texture2D logoTexture;
        Texture2D[] tankTexture;
        Texture2D backgroundTexture;
        int textureStage;
        Rectangle windowAreaRectangle;

        /*
         * Constructor
         */
        //public Title(Texture2D logoTexture, Texture2D[] tankTexture, Texture2D backgroundTexture, Rectangle windowAreaRectangle)
        public Title(Texture2D logoTexture, Texture2D[] tankTexture, Rectangle windowAreaRectangle)
        {
            // Establish the logo, tank, and background textures and the window area
            this.logoTexture = logoTexture;
            this.tankTexture = tankTexture;
            // this.backgroundTexture = backgroundTexture;
            textureStage = 0;
            this.windowAreaRectangle = windowAreaRectangle;

            // Positioning the logo
            logoPosition.X = windowAreaRectangle.Width / 2;
            logoPosition.X -= logoTexture.Width / 2;
            logoPosition.Y = windowAreaRectangle.Height / 10;

            // Positioning the tank
            tankPosition.X = (windowAreaRectangle.Width - tankTexture[0].Width) / 2;
            tankPosition.X += windowAreaRectangle.X;
            tankPosition.Y = (windowAreaRectangle.Height - tankTexture[0].Height) / 2;
            tankPosition.Y += windowAreaRectangle.Y;
        }


        /*
         * Voids
         */
        public void Update()
        {
            // TODO  detect mouse click on on tank. Change gameStarted to true if so.
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(logoTexture, logoPosition, Color.White);

            if (textureStage <= 4) // Draw first sprite.
                spriteBatch.Draw(tankTexture[0], tankPosition, Color.White);


            if (textureStage > 4 && textureStage <= 8) // Draw second sprite.
                spriteBatch.Draw(tankTexture[1], tankPosition, Color.White);

            if (textureStage > 8) // Draw third sprite.
            {
                spriteBatch.Draw(tankTexture[2], tankPosition, Color.White);

                // Reset the texture stage once the third sprite finishes animating.
                if (textureStage >= 12)
                    textureStage = -1;
            }

            textureStage++;
        }
    }
}
