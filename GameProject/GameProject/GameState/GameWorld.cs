using Comora;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameProject.Map;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace GameProject.GameState
{
    public enum GameStates
    {
        Playing, GameOver, Restart
    }
    public class GameWorld
    {
        private IGameState _currentState;
        private Dictionary<GameStates, IGameState> _states;

        public Camera Camera { get; private set; }
        public Player Player { get; private set; }

        private Texture2D _enemyTexture;

        public List<Enemy> Enemies { get; private set; }
        public List<Bullet> Bullets { get; private set; }
        public SpriteFont MenuFont { get; private set; }
        public ScoreController Score { get; private set; }
        public MapGenerator GameMap { get; private set; }


        public GameWorld(Camera camera, SpriteFont menuFont, ScoreController score, MapGenerator gameMap)
        {
            Camera = camera;
            MenuFont = menuFont;
            Score = score;
            Enemies = new List<Enemy>();
            Bullets = new List<Bullet>();

            _states = new Dictionary<GameStates, IGameState>();

            GameMap = gameMap;
        }

        public void InitializeStates(Player player, Texture2D enemyTexture)
        {
            Player = player;
            _enemyTexture = enemyTexture;

            _states[GameStates.Playing] = new PlayingState(this, Player, Score, Camera, enemyTexture);
            _states[GameStates.GameOver] = new GameOverState(this);

            ChangeState(GameStates.Playing);
        }

        public void ChangeState(GameStates newState)
        {
            _currentState = _states[newState];
        }

        public void Update(GameTime gameTime)
        {
            _currentState.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _currentState.Draw(spriteBatch);
        }

        public void Reset()
        {
            Player.Reset();
            Score.ResetScore();
            Enemy.enemies.Clear();
            Bullet.bullets.Clear();
            Controller.timer = Controller.maxTime; // reset enemy spawner
        }
    }

}
