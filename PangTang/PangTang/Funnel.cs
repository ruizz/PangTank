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
        Vector2 position;
        Vector2 motion; // Defines direction. Y component ignored.
        float funnelSpeed = 8f;

        /*
         * States
         */
        KeyboardState keyboardState;
        GamePadState gamePadState;

        /*
         * Other
         */
        Texture2D texture;
        Rectangle playAreaRectangle; // Bounds for play area.

        /*
         * Constructor
         */
        public Funnel(Texture2D texture, Rectangle playAreaRectangle)
        {
            this.texture = texture;
            this.playAreaRectangle = playAreaRectangle;
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
             texture.Width,
             texture.Height);
        }

        /*
         * Voids
         */
        public void Update()
        {
            motion = Vector2.Zero;
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
            LockFunnel();
        }

        // Make sure the funnel doesn't go off of the screen.
        private void LockFunnel()
        {
            if (position.X < playAreaRectangle.X)
                position.X = playAreaRectangle.X;
            if (position.X + texture.Width > playAreaRectangle.X + playAreaRectangle.Width)
                position.X = playAreaRectangle.X + playAreaRectangle.Width - texture.Width;
        }

        public void SetInStartPosition()
        {
            position.X = (playAreaRectangle.Width - texture.Width) / 2;
            position.X += playAreaRectangle.X;
            position.Y = playAreaRectangle.Height - texture.Height;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
