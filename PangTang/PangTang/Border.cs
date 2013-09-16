using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PangTang
{
    class Border
    {
        /*
         * Positions
         */
        Vector2 borderPosition; // Position of the border
        Vector2 hoseEndPosition;
        Rectangle bounds;

        /*
         * Other
         */
        Texture2D borderTexture;
        Texture2D borderAnimation1;
        Texture2D borderAnimation2;
        Texture2D borderAnimation3;
        Texture2D hoseEndTexture;
        Rectangle playAreaRectangle;
        int totalDropsCaught = 0;
        int animationFrame = -1;

        /*
         * Constructor
         */
        public Border(Texture2D[] textures, Rectangle playAreaRectangle)
        {
            // Establish texture, water width/height, play area, starting speed, and active state.
            borderTexture = textures[0];
            borderAnimation1 = textures[1];
            borderAnimation2 = textures[2];
            borderAnimation3 = textures[3];
            hoseEndTexture = textures[4];
            bounds.Width = borderTexture.Width;
            bounds.Height = borderTexture.Height;
            this.playAreaRectangle = playAreaRectangle;

            borderPosition.X = playAreaRectangle.Left - (borderTexture.Width / 1.8f);
            borderPosition.Y = 0;

            hoseEndPosition.X = 80;
            hoseEndPosition.Y = 0;
        }

        /*
         * Returns
         */

        // Returns bounds of a requested water droplet.
        public Rectangle Bounds(int which)
        {
            bounds.X = (int)borderPosition.X;
            bounds.Y = (int)borderPosition.Y;
            return bounds;
        }

        /*
         * Voids
         */

        public void Update(int totalDropsCaught)
        {
            if (this.totalDropsCaught < totalDropsCaught)
            {
                this.totalDropsCaught++;
                animationFrame++;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(animationFrame == -1)
                spriteBatch.Draw(borderTexture, borderPosition, Color.White);
            else if(animationFrame <= 10)
            {
                spriteBatch.Draw(borderAnimation1, borderPosition, Color.White);
                animationFrame++;
            }
            else if (animationFrame <= 20)
            {
                spriteBatch.Draw(borderAnimation2, borderPosition, Color.White);
                animationFrame++;
            }
            else if (animationFrame <= 30)
            {
                spriteBatch.Draw(borderAnimation3, borderPosition, Color.White);

                if (animationFrame == 30)
                    animationFrame = -1;
                else
                    animationFrame++;
            }

            spriteBatch.Draw(hoseEndTexture, hoseEndPosition, Color.White);
        }
    }
}
