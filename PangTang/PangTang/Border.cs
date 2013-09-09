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
        Texture2D hoseEndTexture;
        Rectangle playAreaRectangle;

        /*
         * Constructor
         */
        public Border(Texture2D[] textures, Rectangle playAreaRectangle)
        {
            // Establish texture, water width/height, play area, starting speed, and active state.
            borderTexture = textures[0];
            hoseEndTexture = textures[1];
            bounds.Width = borderTexture.Width;
            bounds.Height = borderTexture.Height;
            this.playAreaRectangle = playAreaRectangle;

            borderPosition.X = playAreaRectangle.Left - borderTexture.Width;
            borderPosition.Y = 0;

            hoseEndPosition.X = 40;
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

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(borderTexture, borderPosition, Color.White);
            spriteBatch.Draw(hoseEndTexture, hoseEndPosition, Color.White);
        }
    }
}
