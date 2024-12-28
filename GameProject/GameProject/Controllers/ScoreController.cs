using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameProject.Controllers
{
    public class ScoreController
    {
        private static ScoreController _instance;

        private int score = 0;
        internal SpriteFont Font;

        public int Score
        {
            get
            {
                return score;
            }
        }

        private ScoreController(SpriteFont font)
        {
            Font = font;
        }

        public static ScoreController GetInstance(SpriteFont font = null)
        {
            if (_instance == null)
            {
                if (font == null)
                {
                    throw new ArgumentNullException(nameof(font), "Font must be provided.");
                }
                _instance = new ScoreController(font);
            }

            return _instance;
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
