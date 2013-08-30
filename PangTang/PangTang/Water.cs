using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PangTang
{
    class Water // INCOMPLETE
    {
        /*
         * Positions
         */
        Vector2 motion;
        Vector2 position;
        Rectangle bounds;
        float waterSpeed = 4;

        /*
         * Other
         */
        Texture2D texture;
        Rectangle screenBounds;

        /*
         * Constructor
         */
        public Water(Texture2D texture, Rectangle screenBounds)
        {
            // TODO
        }


        /*
         * Returns
         */
        public Rectangle Bounds
        {
            get
            {
                bounds.X = (int)position.X;
                bounds.Y = (int)position.Y;
                return bounds;
            }
        }

        // If water misses the funnel and goes out of bounds.
        public bool OffBottom()
        {
            if (position.Y > screenBounds.Height)
                return true;
            return false;
        }

        /*
         * Voids
         */
        public void Update()
        {
            // TODO
        }

        public void FunnelCollision(Rectangle paddleLocation)
        {
            Rectangle waterLocation = new Rectangle(
             (int)position.X,
             (int)position.Y,
             texture.Width,
             texture.Height);
            if (paddleLocation.Intersects(waterLocation))
            {
                // Currently just deflects up. Should change this, of course.
                position.Y = paddleLocation.Y - texture.Height;
                motion.Y *= -1;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }

    }
}
