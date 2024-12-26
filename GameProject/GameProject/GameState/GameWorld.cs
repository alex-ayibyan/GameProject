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

        private SpriteFont titleFont;
        private SpriteFont menuFont;
        private Texture2D regularEnemy;
        private Texture2D fastEnemy;
        private Texture2D tankEnemy;

        public List<Enemy> Enemies { get; private set; }
        public List<Bullet> Bullets { get; private set; }
        public SpriteFont GeneralFont { get; private set; }

        public Texture2D ButtonTexture { get; private set; }

        public Texture2D Background { get; private set; }
        public ScoreController Score { get; private set; }
        public MapGenerator GameMap { get; private set; }

        private ContentManager _content;

        private Song startMusic;
        private Song gameOverMusic;
        private Song playMusic;
        private Song specialRoundMusic;

        private Song _currentMusic;

        private GraphicsDevice graphics;

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

            titleFont = _content.Load<SpriteFont>("Fonts/TitleFont");
            menuFont = _content.Load<SpriteFont>("Fonts/MenuFont");
            regularEnemy = _content.Load<Texture2D>("SlimeEnemy");
            fastEnemy = _content.Load<Texture2D>("Enemies/FastEnemy");
            tankEnemy = _content.Load<Texture2D>("Enemies/TankEnemy");

            startMusic = _content.Load<Song>("Sounds/StartScreenMusic");
            gameOverMusic = _content.Load<Song>("Sounds/GameOverMusic");
            playMusic = _content.Load<Song>("Sounds/MainGameMusic");
            specialRoundMusic = _content.Load<Song>("Sounds/SpecialRoundMusic");

            _states[GameStates.StartScreen] = new StartScreenState(this, titleFont, menuFont, camera);
            _states[GameStates.Playing] = new PlayingState(this, Player, Score, Camera, regularEnemy, fastEnemy, tankEnemy);
            _states[GameStates.SpecialRound] = new SpecialRoundState(this, Player, Score, Camera, tankEnemy);
            _states[GameStates.GameOver] = new GameOverState(this);

            _currentState = _states[GameStates.StartScreen];
        }

        public void ChangeState(GameStates newState)
        {

            if (_currentState == _states[newState])
            {
                return;
            }

            _currentState = _states[newState];
            Debug.WriteLine($"State changed to: {newState}");

        }

        public void Update(GameTime gameTime)
        {
            _currentState.Update(gameTime);

            Song newMusic = null;

            if (_currentState is StartScreenState)
            {
                newMusic = startMusic;
            }
            else if (_currentState is PlayingState)
            {
                newMusic = playMusic;
            }
            else if (_currentState is SpecialRoundState)
            {
                newMusic = specialRoundMusic;
            }
            else if (_currentState is GameOverState)
            {
                newMusic = gameOverMusic;
            }

            if (newMusic != _currentMusic)
            {
                MediaPlayer.Stop();
                MediaPlayer.Play(newMusic);
                _currentMusic = newMusic;
            }
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

            InitializeStates(Player, graphics, Camera);
            if (_currentState != _states[GameStates.GameOver])
            {
                ChangeState(GameStates.GameOver);
            }
        }
    }
}


