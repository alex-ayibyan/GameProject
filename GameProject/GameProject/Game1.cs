﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Comora;

namespace GameProject
{
    enum Direction
    {
        Down, Up, Left, Right
    }
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Texture2D playerSprite;
        Texture2D walkDown;
        Texture2D walkUp;
        Texture2D walkRight;
        Texture2D walkLeft;

        Texture2D background;
        Texture2D ball;
        Texture2D enemy;

        Player player = new Player();

        Camera camera;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.ApplyChanges();

            this.camera = new Camera(_graphics.GraphicsDevice);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            playerSprite = Content.Load<Texture2D>("Player/Sprite");
            walkDown = Content.Load<Texture2D>("Player/Down");
            walkUp = Content.Load<Texture2D>("Player/Up");
            walkRight = Content.Load<Texture2D>("Player/Right");
            walkLeft = Content.Load<Texture2D>("Player/Left");

            background = Content.Load<Texture2D>("GameBG");
            ball = Content.Load<Texture2D>("FireBall");
            enemy = Content.Load<Texture2D>("SlimeEnemy");

            player.animations[0] = new SpriteAnimation(walkDown, 4 , 8);
            player.animations[1] = new SpriteAnimation(walkUp, 4, 8);
            player.animations[2] = new SpriteAnimation(walkLeft, 4, 8);
            player.animations[3] = new SpriteAnimation(walkRight, 4, 8);

            player.animation = player.animations[0];

            Enemy.enemies.Add(new Enemy(new Vector2(500, 500), enemy));
            Enemy.enemies.Add(new Enemy(new Vector2(300, 300), enemy));
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            player.Update(gameTime);

            this.camera.Position = player.Position;
            this.camera.Update(gameTime);

            foreach (Bullet bullet in Bullet.bullets)
            {
                bullet.Update(gameTime);
            }

            foreach (Enemy enemy in Enemy.enemies)
            {
                enemy.Update(gameTime);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(this.camera);

            _spriteBatch.Draw(background, new Vector2(100, 100), Color.White);

            foreach (Enemy enemy in Enemy.enemies)
            {
                enemy.animation.Draw(_spriteBatch);
            }

            foreach (Bullet bullet in Bullet.bullets)
            {
                _spriteBatch.Draw(ball, new Vector2(bullet.Position.X - 160, bullet.Position.Y - 90), Color.White);
            }
            player.animation.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
