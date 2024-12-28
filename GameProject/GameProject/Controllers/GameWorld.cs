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
using GameProject.States;

namespace GameProject.Controllers
{
    
    public class GameWorld
    {
        public IGameState CurrentState;
        public IGameState PreviousState;
        private readonly Dictionary<GameStates, IGameState> _states;

        public Camera Camera { get; private set; }
        public Player Player { get; private set; }
        private readonly ContentManager _content;
        public Controller Controller;

        private SpriteFont _titleFont;
        private SpriteFont _menuFont;
        public Texture2D RegularEnemy;
        public Texture2D FastEnemy;
        public Texture2D TankEnemy;
        public Texture2D WaterBullet;
        public Texture2D FireBullet;
        public Texture2D LifeTexture;

        public List<Enemy> Enemies { get; private set; }
        public List<Bullet> Bullets { get; private set; }
        public SpriteFont GeneralFont { get; private set; }
        public Texture2D Background { get; private set; }
        public ScoreController Score { get; private set; }
        public MapGenerator GameMap { get; private set; }

        public Song StartMusic { get; private set; }
        public Song GameOverMusic { get; private set; }
        public Song PlayMusic { get; private set; }
        public Song SpecialRoundMusic { get; private set; }
        public SoundEffect ShootSound { get; private set; }
        private Song _currentMusic;

        public float StateChangeDelay = 0.2f;
        public float ElapsedTimeSinceStateChange = 0f;

        public GameWorld(Player player, Camera camera, SpriteFont generalFont, ScoreController score, MapGenerator gameMap, ContentManager content, Controller controller)
        {
            Camera = camera;
            Score = score;
            Enemies = new List<Enemy>();
            Bullets = new List<Bullet>();
            GeneralFont = generalFont;
            Player = player;

            _content = content;
            Controller = new Controller(this, gameMap, score);
            _states = new Dictionary<GameStates, IGameState>();
            GameMap = gameMap;

            InitializeStates();
            CurrentState = _states[GameStates.StartScreen];

        }

        public void InitializeStates()
        {
            _titleFont = _content.Load<SpriteFont>("Fonts/TitleFont");
            _menuFont = _content.Load<SpriteFont>("Fonts/MenuFont");
            RegularEnemy = _content.Load<Texture2D>("SlimeEnemy");
            FastEnemy = _content.Load<Texture2D>("Enemies/FastEnemy");
            TankEnemy = _content.Load<Texture2D>("Enemies/FlameEnemy");

            StartMusic = _content.Load<Song>("Sounds/MenuMusic");
            GameOverMusic = _content.Load<Song>("Sounds/GameOverMusic");
            PlayMusic = _content.Load<Song>("Sounds/GameMusic");
            SpecialRoundMusic = _content.Load<Song>("Sounds/SpecialMusic");
            ShootSound = _content.Load<SoundEffect>("Sounds/shootingSound");


            WaterBullet = _content.Load<Texture2D>("WaterBall");
            FireBullet = _content.Load<Texture2D>("FireBall");

            LifeTexture = _content.Load<Texture2D>("Heart");

            Player.bulletTexture = WaterBullet;
            Player.shootSound = ShootSound;

            _states[GameStates.StartScreen] = new StartScreenState(this, _titleFont, _menuFont, Controller, Camera);
            _states[GameStates.Playing] = new PlayingState(this, Player, Score, Camera, RegularEnemy, FastEnemy, TankEnemy, Controller);
            _states[GameStates.SpecialRound] = new SpecialRoundState(this, Player, Score, Camera, TankEnemy);
            _states[GameStates.GameOver] = new GameOverState(this, _titleFont, _menuFont, Controller, Camera);
            _states[GameStates.ChooseDifficulty] = new ChooseDifficultyState(this, _titleFont, _menuFont, Controller, Camera);
            _states[GameStates.ControlsState] = new ControlsState(this, _menuFont, _titleFont);
        }

        public void ChangeState(GameStates newState)
        {
            if (CurrentState == _states[newState])
            {
                return;
            }

            PreviousState = CurrentState;
            CurrentState = _states[newState];

            Debug.WriteLine($"State changed to: {newState}");

            ElapsedTimeSinceStateChange = 0f;
        }
        public void GoBackToPreviousState()
        {
            if (PreviousState != null)
            {
                var previousGameState = _states.FirstOrDefault(s => s.Value == PreviousState).Key;
                ChangeState(previousGameState);
            }
        }

        public void Update(GameTime gameTime)
        {

            if (CurrentState is PlayingState || CurrentState is SpecialRoundState)
            {
                ElapsedTimeSinceStateChange = StateChangeDelay;
            }
            else
            {
                ElapsedTimeSinceStateChange += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (ElapsedTimeSinceStateChange >= StateChangeDelay)
            {
                ElapsedTimeSinceStateChange = StateChangeDelay;
                CurrentState.Update(gameTime);
            }

            var newMusic = CurrentState.GetBackgroundMusic();
            if (newMusic != _currentMusic)
            {
                MediaPlayer.Stop();
                MediaPlayer.Play(newMusic);
                _currentMusic = newMusic;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            CurrentState.Draw(spriteBatch);
        }

        public void Reset()
        {
            InitializeStates();
            Player.Reset();
            Score.ResetScore();
            Enemy.Enemies.Clear();
            Bullet.bullets.Clear();

            if (_states[GameStates.SpecialRound] is SpecialRoundState specialRoundState)
            {
                specialRoundState.ResetRoundCounter();
            }

            Controller.Timer = 2D;
        }
    }
}


