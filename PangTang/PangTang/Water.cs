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
        Texture2D[] texture;
        int textureStage;
        Rectangle playAreaRectangle;

        /*
         * Constructor
         */
        public Water(Texture2D[] texture, Rectangle playAreaRectangle)
        {
            // Establish texture, water width/height, play area, starting speed, and active state.
            this.texture = texture;
            textureStage = 0;
            bounds.Width = texture[0].Width;
            bounds.Height = texture[0].Height;
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
        private bool IsActive(int which)
        {
            return isActive[which];
        }

        // If water misses the funnel and goes out of bounds.
        private bool OffBottom(int which)
        {
            if (positions[which].Y > playAreaRectangle.Height)
                return true;

            return false;
        }

        // Returns the number of collisions with the funnel.
        public int getFunnelCollisions(Rectangle funnelMouth)
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
        private bool collidedWithFunnel(Rectangle funnelMouth, int which)
        {
            if (isActive[which])
            {
                Rectangle waterLocation = new Rectangle(
                 (int)positions[which].X,
                 (int)positions[which].Y + (texture[0].Height / 4) * 3,
                 texture[0].Width,
                 texture[0].Height / 4);

                if (funnelMouth.Intersects(waterLocation))
                {
                    isActive[which] = false;
                    return true;
                }
            }

            return false;
        }

        // Returns the number of funnel misses since the last update.
        public int getFunnelMisses()
        {
            int funnelMisses = missedCount;

            // Reset the missed count.
            missedCount = 0;

            return funnelMisses;
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
                    {
                        missedCount++;
                        isActive[i] = false;
                    }
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
            for (int i = 0; i < positions.Length; i++) // For all water droplets...
            {
                if (isActive[i]) // ... if the water droplet is active.
                {
                    if (textureStage <= 8) // Draw first sprite.
                        spriteBatch.Draw(texture[0], positions[i], Color.White);

                    if (textureStage > 8 && textureStage <= 16) // Draw second sprite.
                        spriteBatch.Draw(texture[1], positions[i], Color.White);

                    if (textureStage > 16) // Draw third sprite.
                    {
                        spriteBatch.Draw(texture[2], positions[i], Color.White);

                        // Reset the texture stage once the third sprite finishes animating.
                        if (textureStage >= 24)
                            textureStage = -1;
                    }

                    textureStage++;
                }
            }
        }
    }
}
