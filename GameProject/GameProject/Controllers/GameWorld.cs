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
using Microsoft.Xna.Framework.Audio;
using GameProject.Entities;
using GameProject.GameState;

namespace GameProject.Controllers
{
    public enum GameStates
    {
        StartScreen, Playing, GameOver, SpecialRound, ChooseDifficulty, ControlsState
    }
    public class GameWorld
    {
        public IGameState _currentState;
        private IGameState _previousState;

        private Dictionary<GameStates, IGameState> _states;

        public Camera Camera { get; private set; }
        public Player Player { get; private set; }

        private SpriteFont titleFont;
        private SpriteFont menuFont;
        public Texture2D regularEnemy;
        public Texture2D fastEnemy;
        public Texture2D tankEnemy;
        public Texture2D waterBullet;
        public Texture2D fireBullet;
        public Texture2D lifeTexture;

        public List<Enemy> Enemies { get; private set; }
        public List<Bullet> Bullets { get; private set; }
        public SpriteFont GeneralFont { get; private set; }

        public Texture2D ButtonTexture { get; private set; }

        public Texture2D Background { get; private set; }
        public ScoreController Score { get; private set; }
        public MapGenerator GameMap { get; private set; }

        private ContentManager _content;
        public Controller _controller;

        public Song startMusic;
        public Song gameOverMusic;
        public Song playMusic;
        public Song specialRoundMusic;
        public SoundEffect shootSound;

        private Song _currentMusic;

        public GraphicsDevice graphics;

        public float _stateChangeDelay = 0.2f;
        public float _elapsedTimeSinceStateChange = 0f;


        public GameWorld(Player player, Camera camera, SpriteFont generalFont, ScoreController score, MapGenerator gameMap, ContentManager content, Controller controller)
        {
            Camera = camera;
            Score = score;
            Enemies = new List<Enemy>();
            Bullets = new List<Bullet>();
            GeneralFont = generalFont;
            Player = player;

            _content = content;
            _controller = new Controller(this, gameMap, score);
            _states = new Dictionary<GameStates, IGameState>();
            GameMap = gameMap;

            InitializeStates();
            _currentState = _states[GameStates.StartScreen];

        }

        public void InitializeStates()
        {
            titleFont = _content.Load<SpriteFont>("Fonts/TitleFont");
            menuFont = _content.Load<SpriteFont>("Fonts/MenuFont");
            regularEnemy = _content.Load<Texture2D>("SlimeEnemy");
            fastEnemy = _content.Load<Texture2D>("Enemies/FastEnemy");
            tankEnemy = _content.Load<Texture2D>("Enemies/FlameEnemy");

            startMusic = _content.Load<Song>("Sounds/MenuMusic");
            gameOverMusic = _content.Load<Song>("Sounds/GameOverMusic");
            playMusic = _content.Load<Song>("Sounds/GameMusic");
            specialRoundMusic = _content.Load<Song>("Sounds/SpecialMusic");
            shootSound = _content.Load<SoundEffect>("Sounds/shootingSound");


            waterBullet = _content.Load<Texture2D>("WaterBall");
            fireBullet = _content.Load<Texture2D>("FireBall");

            lifeTexture = _content.Load<Texture2D>("Heart");

            Player.bulletTexture = waterBullet;
            Player.shootSound = shootSound;

            // Camera.Position = new Vector2(1000,100);

            _states[GameStates.StartScreen] = new StartScreenState(this, titleFont, menuFont, _controller, Camera);
            _states[GameStates.Playing] = new PlayingState(this, Player, Score, Camera, regularEnemy, fastEnemy, tankEnemy, _controller);
            _states[GameStates.SpecialRound] = new SpecialRoundState(this, Player, Score, Camera, tankEnemy);
            _states[GameStates.GameOver] = new GameOverState(this, titleFont, menuFont, _controller, Camera);
            _states[GameStates.ChooseDifficulty] = new ChooseDifficultyState(this, menuFont, _controller);
            _states[GameStates.ControlsState] = new ControlsState(this, menuFont, titleFont);
        }

        public void ChangeState(GameStates newState)
        {
            if (_currentState == _states[newState])
            {
                return;
            }

            _previousState = _currentState;
            _currentState = _states[newState];

            Debug.WriteLine($"State changed to: {newState}");

            _elapsedTimeSinceStateChange = 0f;
        }
        public void GoBackToPreviousState()
        {
            if (_previousState != null)
            {
                var previousGameState = _states.FirstOrDefault(s => s.Value == _previousState).Key;
                ChangeState(previousGameState);
            }
        }

        public void Update(GameTime gameTime)
        {

            if (_currentState is PlayingState || _currentState is SpecialRoundState)
            {
                _elapsedTimeSinceStateChange = _stateChangeDelay;
            }
            else
            {
                _elapsedTimeSinceStateChange += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (_elapsedTimeSinceStateChange >= _stateChangeDelay)
            {
                _elapsedTimeSinceStateChange = _stateChangeDelay;
                _currentState.Update(gameTime);
            }

            var newMusic = _currentState.GetBackgroundMusic();
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
            InitializeStates();
            Player.Reset();
            Score.ResetScore();
            Enemy.enemies.Clear();
            Bullet.bullets.Clear();

            if (_states[GameStates.SpecialRound] is SpecialRoundState specialRoundState)
            {
                specialRoundState.ResetRoundCounter();
            }

            Controller.timer = 2D;
        }
    }
}


