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
        float nozzleSpeed = 8f;

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
            this.texture = texture;
            this.playAreaRectangle = playAreaRectangle;

            // Calculate positions based on the play area.
            positions = new int[7];
            positions[0] = playAreaRectangle.X;
            for (int i = 1; i < 6; i++)
            {
                positions[i] = i * (playAreaRectangle.Width / 6);
                positions[i] += playAreaRectangle.X;
            }
            positions[6] = playAreaRectangle.X + playAreaRectangle.Width - texture.Width;

            SetInStartPosition();
        }

        /*
         * Returns
         */
        // (None)

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

            motion.X *= nozzleSpeed;
            currentPosition += motion;
            motion.Normalize();
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
