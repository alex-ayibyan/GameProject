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
using System.Reflection.Metadata;
using System.Diagnostics;
using Microsoft.Xna.Framework.Media;

namespace GameProject.GameState
{
    public enum GameStates
    {
        StartScreen, Playing, GameOver, SpecialRound
    }
    public class GameWorld
    {
        public IGameState _currentState;
        private Dictionary<GameStates, IGameState> _states;

        public Camera Camera { get; private set; }
        public Player Player { get; private set; }

        public Texture2D TankEnemy;
        public List<Enemy> Enemies { get; private set; }
        public List<Bullet> Bullets { get; private set; }
        public SpriteFont GeneralFont { get; private set; }

        public Texture2D ButtonTexture { get; private set; }

        public Texture2D Background { get; private set; }
        public ScoreController Score { get; private set; }
        public MapGenerator GameMap { get; private set; }

        private ContentManager _content;

        public GameWorld(Camera camera, SpriteFont generalFont, ScoreController score, MapGenerator gameMap, ContentManager content)
        {
            Camera = camera;
            Score = score;
            Enemies = new List<Enemy>();
            Bullets = new List<Bullet>();
            GeneralFont = generalFont;

            _content = content;

            _states = new Dictionary<GameStates, IGameState>();

            GameMap = gameMap;
        }

        public void InitializeStates(Player player, GraphicsDevice graphicsDevice, Camera camera)
        {
            Player = player;


            SpriteFont titleFont = _content.Load<SpriteFont>("Fonts/TitleFont");
            SpriteFont menuFont = _content.Load<SpriteFont>("Fonts/MenuFont");
            Texture2D regularEnemy = _content.Load<Texture2D>("SlimeEnemy");
            Texture2D fastEnemy = _content.Load<Texture2D>("Enemies/FastEnemy");
            TankEnemy= _content.Load<Texture2D>("Enemies/TankEnemy");
            var startMusic = _content.Load<Song>("Sounds/StartScreenMusic");

            _states[GameStates.StartScreen] = new StartScreenState(this, titleFont, menuFont, camera, startMusic);
            _states[GameStates.Playing] = new PlayingState(this, Player, Score, Camera, regularEnemy, fastEnemy, TankEnemy);
            _states[GameStates.SpecialRound] = new SpecialRoundState(this, GameMap, menuFont);
            _states[GameStates.GameOver] = new GameOverState(this);

            ChangeState(GameStates.StartScreen);
        }

        public void ChangeState(GameStates newState)
        {
            _currentState = _states[newState];
            Debug.WriteLine($"State changed to: {newState}");
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
            Controller.timer = Controller.maxTime;
            Camera.Position = new Vector2(1600,1500);
        }
    }

}
