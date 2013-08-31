using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PangTang
{
    class Nozzle
    {
        /*
         * Positions
         */
        Vector2 currentPosition;
        Vector2 targetPosition;
        Vector2 motion; // Defines direction. Y component ignored.

        // Vector of 7 positions that the nozzle will traverse to randomly.
        // Positions are the X values in respect to the entire window.
        int[] positions;

        float nozzleStartSpeed = 2f;
        float nozzleSpeed;

        /*
         * Other
         */
        Texture2D texture;
        Rectangle playAreaRectangle; // Bounds for play area

        /*
         * Constructor
         */
        public Nozzle(Texture2D texture, Rectangle playAreaRectangle)
        {
            // Establish texture, play area, and starting speed.
            this.texture = texture;
            this.playAreaRectangle = playAreaRectangle;
            nozzleSpeed = nozzleStartSpeed;

            // Calculate positions based on the play area.
            positions = new int[7];
            positions[0] = playAreaRectangle.X;
            for (int i = 1; i < 6; i++)
            {
                positions[i] = i * (playAreaRectangle.Width / 6);
                positions[i] += playAreaRectangle.X;
            }
            positions[6] = playAreaRectangle.X + playAreaRectangle.Width;
            positions[6] -= texture.Width * 2;

            SetInStartPosition();
        }

        /*
         * Returns
         */
        public Rectangle GetBounds()
        {
            return new Rectangle(
             (int) currentPosition.X,
             (int) currentPosition.Y,
             texture.Width,
             texture.Height);
        }

        /*
         * Voids
         */
        public void Update()
        {
            
            if ((motion.X == 1 && currentPosition.X > targetPosition.X) ||
                (motion.X == -1 && currentPosition.X < targetPosition.X))
            {
                Random rand = new Random();
                targetPosition.X = positions[rand.Next(0, 7)];

                if (currentPosition.X < targetPosition.X)
                    motion.X = 1;
                else
                    motion.X = -1;
            }

            // Multiply the direction by the speed to calculate the where the nozzle should be.
            motion.X *= nozzleSpeed;
            currentPosition += motion;

            // Re-normalize the motion vector.
            motion.Normalize();
        }

        // Increments the speed when the level changes.
        public void increaseSpeed(float speedMultiplier)
        {
            nozzleSpeed *= speedMultiplier;
        }

        public void SetInStartPosition()
        {
            currentPosition.X = (playAreaRectangle.Width - texture.Width) / 2;
            currentPosition.X += playAreaRectangle.X;
            currentPosition.Y = 0;

            Random rand = new Random();
            targetPosition.X = positions[rand.Next(0, 7)];
            targetPosition.Y = 0;

            if (currentPosition.X < targetPosition.X)
                motion.X = 1;
            else
                motion.X = -1;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
                spriteBatch.Draw(texture, currentPosition, Color.White);
        }
    }
}
