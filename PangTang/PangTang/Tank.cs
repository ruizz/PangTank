using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PangTang
{
    class Tank
    {
        /*
         * Positions
         */
        Vector2 position; // Where the tank should be drawn

        /*
         * Objects
         */
        float currentThreshold; // 1 is full, 0 empty
        Rectangle tankAreaRectangle; // The area that is not the play area. (The left side)
        

        /*
         * Other
         */
        Texture2D[,] texture; // A 2D array, to represent  all full, 2/3 full, and 1/3 full sprites.
        int textureStage = 0;

        /*
         * Constructor
         */
        public Tank(Texture2D[,] texture, Rectangle tankAreaRectangle)
        {
            this.texture = texture;
            currentThreshold = 1.0f;
            this.tankAreaRectangle = tankAreaRectangle;

            // Position reflects the center of the tank area rectangle.
            position.X = (tankAreaRectangle.Width - texture[0, 0].Width) / 2;
            position.X += tankAreaRectangle.X;
            position.Y = (tankAreaRectangle.Height - texture[0, 0].Height) / 2;
            position.Y += tankAreaRectangle.Y;
        }

        /*
         * Returns
         */

        // 0 = full, 1 = 2/3 full, 2 = 1/3 full, 3 = empty
        public int getTankLevel()
        {
            if (currentThreshold >= 0.66f)
                return 0;

            if (currentThreshold >= 0.33f)
                return 1;

            if (currentThreshold > 0.0f)
                return 2;

            return 3;
        }

        /*
         * Voids
         */

        // Decrements the tank level
        public void updateThreshold(int levelDropRequirement, int numOfCollisions)
        {
            float amountToDecrement = (1.0f / (float) levelDropRequirement) * (float) numOfCollisions;
            currentThreshold -= amountToDecrement;

            // Resolves an odd case were you get an extremely small number instead of an expected 0
            if (currentThreshold <= 0.0001f)
                currentThreshold = 0.0f;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (textureStage <= 12) // Draw first sprite.
            {
                spriteBatch.Draw(texture[getTankLevel(), 0], position, Color.White);
                spriteBatch.Draw(texture[3, 0], new Vector2(position.X + (texture[0, 0].Width/ 2), position.Y + texture[0, 0].Height - 60), Color.White);
            }

            if (textureStage > 12) // Draw second sprite.
            {
                spriteBatch.Draw(texture[getTankLevel(), 1], position, Color.White);
                spriteBatch.Draw(texture[3, 1], new Vector2(position.X + (texture[0, 0].Width / 2) - 20, position.Y + texture[0, 0].Height - 60), Color.White);

                // Reset the texture stage once the third sprite finishes animating.
                if (textureStage >= 24)
                    textureStage = -1;
            }

            textureStage++;
        }

        public void Reset()
        {
            currentThreshold = 1.0f;

            // Position reflects the center of the tank area rectangle.
            position.X = (tankAreaRectangle.Width - texture[0, 0].Width) / 2;
            position.X += tankAreaRectangle.X;
            position.Y = (tankAreaRectangle.Height - texture[0, 0].Height) / 2;
            position.Y += tankAreaRectangle.Y;
        }

    }
}
