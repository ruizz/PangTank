using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PangTang
{
    class Funnel
    {
        /*
         * Positions
         */
        Vector2 position; // Current position of the funnel.
        Vector2 motion; // Defines direction. Y component ignored.
        float funnelSpeed = 12f;
        Vector2 mousePosition; // Current position of the mouse.

        /*
         * States
         */
        MouseState mouseState;
        KeyboardState keyboardState;
        GamePadState gamePadState;

        /*
         * Other
         */
        Texture2D[] texture;
        int textureStage; // Increases from 0 - 60. Determines which texture gets drawn.
        Rectangle playAreaRectangle; // Bounds for play area.

        /*
         * Constructor
         */
        public Funnel(Texture2D[] texture, Rectangle playAreaRectangle)
        {
            this.texture = texture;
            textureStage = 0;
            this.playAreaRectangle = playAreaRectangle;
            mousePosition = Vector2.Zero;
            SetInStartPosition();
        }

        /*
         * Returns
         */
        public Rectangle GetBounds()
        {
            return new Rectangle(
             (int)position.X,
             (int)position.Y,
             texture[0].Width,
             texture[0].Height);
        }

        // Same as GetBounds(), but returns the bounds of the funnel mouth instead.
        public Rectangle GetCollisionBounds()
        {
            return new Rectangle(
             (int)position.X,
             (int)position.Y,
             texture[0].Width,
             texture[0].Height / 10);
        }

        /*
         * Voids
         */
        public void Update()
        {
            motion = Vector2.Zero;
            mouseState = Mouse.GetState();
            keyboardState = Keyboard.GetState();
            gamePadState = GamePad.GetState(PlayerIndex.One);
            if (keyboardState.IsKeyDown(Keys.Left) ||
             gamePadState.IsButtonDown(Buttons.LeftThumbstickLeft) ||
             gamePadState.IsButtonDown(Buttons.DPadLeft))
                motion.X = -1;
            if (keyboardState.IsKeyDown(Keys.Right) ||
             gamePadState.IsButtonDown(Buttons.LeftThumbstickRight) ||
             gamePadState.IsButtonDown(Buttons.DPadRight))
                motion.X = 1;
            motion.X *= funnelSpeed;
            position += motion;

            if (mouseState.X != mousePosition.X)
            {
                position.X = mouseState.X;
                mousePosition.X = mouseState.X;
            }

            LockFunnel();
        }

        // Make sure the funnel doesn't go off of the screen.
        private void LockFunnel()
        {
            if (position.X < playAreaRectangle.X)
                position.X = playAreaRectangle.X;
            if (position.X + texture[0].Width > playAreaRectangle.X + playAreaRectangle.Width)
                position.X = playAreaRectangle.X + playAreaRectangle.Width - texture[0].Width;
        }

        public void SetInStartPosition()
        {
            position.X = (playAreaRectangle.Width - texture[0].Width) / 2;
            position.X += playAreaRectangle.X;
            position.Y = playAreaRectangle.Height - texture[0].Height;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (textureStage <= 4) // Draw first sprite.
                spriteBatch.Draw(texture[0], position, Color.White);

            if (textureStage > 4 && textureStage <= 8) // Draw second sprite.
                spriteBatch.Draw(texture[1], position, Color.White);

            if (textureStage > 8) // Draw third sprite.
            {
                spriteBatch.Draw(texture[2], position, Color.White);

                // Reset the texture stage once the third sprite finishes animating.
                if (textureStage >= 12)
                    textureStage = -1;
            }

            textureStage++;
        }
    }
}
