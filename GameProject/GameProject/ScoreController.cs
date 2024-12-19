using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameProject
{
    public class ScoreController
    {
        private int score = 0;
        internal SpriteFont Font;

        public int Score
        {
            get
            {
                return score;
            }
        }

        public ScoreController(SpriteFont font)
        {
            Font = font;
        }
        public void UpdateScore(int points)
        {
            score += points;
        }

        public void ResetScore()
        {
            score = 0;
        }
    }
}
