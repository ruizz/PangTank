using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace PangTang
{
    class VideoAnimation
    {
        /*
         * Positions
         */
        Rectangle windowAreaRectangle;

        /*
         * Status
         */
        bool animationFinished = false;

        Video video;
        VideoPlayer videoPlayer;

        /*
         * Constructor
         */
        public VideoAnimation(Rectangle windowAreaRectangle, Video video)
        {
            this.windowAreaRectangle = windowAreaRectangle;
            this.video = video;
            videoPlayer = new VideoPlayer();
            videoPlayer.IsLooped = false;
        }

        /*
         * Returns
         */
        public bool isFinished()
        {
            return animationFinished;
        }

        /*
         * Voids
         */
        public void Start()
        {
            videoPlayer.Play(video);
        }

        public void Stop()
        {
            videoPlayer.Stop();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (videoPlayer.State != MediaState.Stopped)
            {
                Texture2D texture = videoPlayer.GetTexture();
                if (texture != null)
                {
                    spriteBatch.Draw(texture, windowAreaRectangle, Color.White);
                }
            }
            else
            {
                animationFinished = true;
            }

        }


    }
}
