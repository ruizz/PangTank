using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PangTang
{
    class Tank
    {
        /*
         * Objects
         */
        
        float currentThreshold;


        /*
         * Other
         */

        Texture2D texture;

        /*
         * Constructor
         */

        public Tank(Texture2D texture)
        {
            this.texture = texture;
            currentThreshold = 1.0f;
        }

        /*
         * Returns
         */

        public float getThreshold()
        {
            return currentThreshold;
        }

        /*
         * Voids
         */

        public void updateThreshold(int levelDropRequirement)
        {
            // TODO
        }

    }
}
