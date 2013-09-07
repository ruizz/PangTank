using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.GamerServices;

namespace PangTang
{
    public class HighScores : Microsoft.Xna.Framework.Game
    {

        [Serializable]
        public struct HighScoreData
        {
            public int[] Score;
            public int Count;

            public HighScoreData(int count)
            {
                Score = new int[count];

                Count = count;
            }
        }

        HighScoreData data;
        public string HighScoresFilename = "highscores.dat";
        int PlayerScore = 0;


        /*
         * Constructor
         */
        public HighScores(int score)
        {
            Initialize();
            
            data = LoadHighScores(HighScoresFilename);
            
            PlayerScore = score;

            SaveHighScore();
        }

        protected override void Initialize()
        {
            //  Get the path of the save game
            string fullPath = "highscores.dat";

            // Check to see if the save exists
            if (!File.Exists(fullPath))
            {
                //If the file doesn't exist, make a fake one...
                // Create the data to save
                data = new HighScoreData(5);
                data.Score[4] = 5;

                data.Score[3] = 4;

                data.Score[2] = 3;

                data.Score[1] = 2;

                data.Score[0] = 1;

                SaveHighScores(data, HighScoresFilename);
            }

            base.Initialize();
        }

        /*
         * Returns
         */

        public static HighScoreData LoadHighScores(string filename)
        {
            HighScoreData highScoreData;
 
            // Get the path of the save game
            string fullPath = "highscores.dat";

            // Open the file
            FileStream loadstream = File.Open(fullPath, FileMode.Open, FileAccess.Read);
            try
            {
                // Read the data from the file
                XmlSerializer serializer = new XmlSerializer(typeof(HighScoreData));
                highScoreData = (HighScoreData)serializer.Deserialize(loadstream);
            }
            finally
            {
                // Close the file
                loadstream.Close();
            }
           
 
            return (highScoreData);
         }

        public string makeHighScoreString()
        {
            // Create the data to save
            HighScoreData data2 = LoadHighScores(HighScoresFilename);

            // Create scoreBoardString
            string scoreBoardString = "Highscores:\n\n";

            for (int i = 4; i >= 0; i--) // this part was missing (5 means how many in the list/array/Counter)
            {
                scoreBoardString = scoreBoardString + data2.Score[i] + "\n";
            }

            return scoreBoardString;
        }


        /*
         * Voids
         */

        private void SaveHighScore()
        {
            // Create the data to save
            HighScoreData data = LoadHighScores(HighScoresFilename);

            int scoreIndex = -1;

            for (int i = 0; i < data.Count; i++)
            {
                if (PlayerScore >= data.Score[i])
                    scoreIndex = i;
            }

            if (scoreIndex > -1)
            {
                // New high score found, do swaps
                for (int i = 0; i < scoreIndex; i++)
                {
                    data.Score[i] = data.Score[i+1];
                }
                data.Score[scoreIndex] = PlayerScore;

                SaveHighScores(data, HighScoresFilename);
            }


            
        }

        public static void SaveHighScores(HighScoreData highScoreData, string filename)
        {
             // Get the path of the save game
            string fullPath = "highscores.dat";
            // Open the file, creating it if necessary
            FileStream stream = File.Open(fullPath, FileMode.OpenOrCreate, FileAccess.Write);
            try
            {
                // Convert the object to XML data and put it in the stream
                XmlSerializer serializer = new XmlSerializer(typeof(HighScoreData));
                serializer.Serialize(stream, highScoreData);
            }
            finally
            {
                // Close the file
                stream.Close();
            }

        }

    }
}