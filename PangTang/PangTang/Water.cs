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
        Vector2 position;
        Rectangle bounds;
        float waterSpeed = 4f;

        /*
         * Status
         */
        bool isActive;

        /*
         * Other
         */
        Texture2D texture;
        Rectangle playAreaRectangle;

        /*
         * Constructor
         */
        public Water(Texture2D texture, Rectangle playAreaRectangle)
        {
            this.texture = texture;
            this.playAreaRectangle = playAreaRectangle;
            isActive = false;
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
            if (position.Y > playAreaRectangle.Height)
                return true;
            return false;
        }

        public bool IsActive()
        {
            return isActive;
        }

        /*
         * Voids
         */
        public void Update(Rectangle nozzleBounds)
        {
            if (isActive) // Droplet is currently falling down
            {
                position.Y += waterSpeed;

                if (OffBottom())
                    isActive = false;
            } 
            else // Droplet needs to come out of nozzle
            {
                position.X = nozzleBounds.X + (nozzleBounds.Width / 2);
                position.Y = nozzleBounds.Height;
                isActive = true;
            }
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
                //motion.Y *= -1;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (isActive)
                spriteBatch.Draw(texture, position, Color.White);
        }

    }
}
