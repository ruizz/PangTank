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
        Vector2[] positions; // Position of the water drops
        Rectangle bounds;

        /*
         * Status
         */
        int counter = 0; // Keeps track of which water drop to release.
        int missedCount = 0; // Keeps track of number of drops missed.
        bool[] isActive;
        float waterStartSpeed = 3f;
        float waterSpeed;

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
            // Establish texture, water width/height, play area, starting speed, and active state.
            this.texture = texture;
            bounds.Width = texture.Width;
            bounds.Height = texture.Height;
            this.playAreaRectangle = playAreaRectangle;
            waterSpeed = waterStartSpeed;

            positions = new Vector2[10];
            isActive = new bool[10];

            for (int i = 0; i < isActive.Length; i++)
                isActive[i] = false;
        }

        /*
         * Returns
         */

        // Returns bounds of a requested water droplet.
        public Rectangle Bounds(int which)
        {
            bounds.X = (int)positions[which].X;
            bounds.Y = (int)positions[which].Y;
            return bounds;
        }

        // If a water droplet is active or not.
        public bool IsActive(int which)
        {
            return isActive[which];
        }

        // If water misses the funnel and goes out of bounds.
        public bool OffBottom(int which)
        {
            if (positions[which].Y > playAreaRectangle.Height)
            {
                missedCount++;
                return true;
            }
            return false;
        }

        // Returns the number of collisions with the funnel.
        public int funnelCollisions(Rectangle funnelMouth)
        {
            int collisions = 0;

            for (int i = 0; i < positions.Length; i++)
            {
                if (isActive[i])
                {
                    if (collidedWithFunnel(funnelMouth, i))
                        collisions += 1;
                }
            }

            return collisions;
        }

        // If a drop has collided with the funnel or not.
        // Will deactivate a drop if a collision is detected.
        public bool collidedWithFunnel(Rectangle funnelMouth, int which)
        {
            if (isActive[which])
            {
                Rectangle waterLocation = new Rectangle(
                 (int)positions[which].X,
                 (int)positions[which].Y + texture.Height,
                 texture.Width,
                 texture.Height / 10);

                if (funnelMouth.Intersects(waterLocation))
                {
                    isActive[which] = false;
                    return true;
                }
            }

            return false;
        }

        // Returns number of missed drops
        public int missedDropCount()
        {
            return missedCount;
        }

        /*
         * Voids
         */
        public void Update()
        {
            for (int i = 0; i < positions.Length; i++)
            {
                if (isActive[i]) // Make sure the drop already squeezed out of the nozzle.
                {
                    positions[i].Y += waterSpeed;

                    // If drop falls off the bottom, it's no longer active.
                    if (OffBottom(i))
                        isActive[i] = false;
                }
            }
        }

        public void releaseDrop(Rectangle nozzleBounds)
        {
            if (!isActive[counter]) // Droplet needs to come out of nozzle
            {
                positions[counter].X = nozzleBounds.X + (nozzleBounds.Width / 2);
                positions[counter].Y = nozzleBounds.Height;
                isActive[counter] = true;
                counter++;
            }

            // Iterate back if we reached the end of the array of drops.
            if (counter >= positions.Length)
            {
                counter = 0;
            }
        }

        // Increments the speed when the level changes.
        public void increaseSpeed(float speedMultiplier)
        {
            waterSpeed *= speedMultiplier;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < positions.Length; i++)
            {
                if (isActive[i])
                    spriteBatch.Draw(texture, positions[i], Color.White);
            }
        }
    }
}
