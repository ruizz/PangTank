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
        Vector2 position; // Position of the border
        Rectangle bounds;

        /*
         * Other
         */
        Texture2D texture;
        Rectangle playAreaRectangle;

        /*
         * Constructor
         */
        public Border(Texture2D texture, Rectangle playAreaRectangle)
        {
            // Establish texture, water width/height, play area, starting speed, and active state.
            this.texture = texture;
            bounds.Width = texture.Width;
            bounds.Height = texture.Height;
            this.playAreaRectangle = playAreaRectangle;

            position.X = playAreaRectangle.Left - 30;
            position.Y = 0;
        }

        /*
         * Returns
         */

        // Returns bounds of a requested water droplet.
        public Rectangle Bounds(int which)
        {
            bounds.X = (int)position.X;
            bounds.Y = (int)position.Y;
            return bounds;
        }

        /*
         * Voids
         */

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
