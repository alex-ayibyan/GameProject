using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Comora;
using System.Threading.Tasks.Sources;
using static System.Formats.Asn1.AsnWriter;
using System.Diagnostics;

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
        // Texture2D playerSprite;
        Texture2D walkDown;
        Texture2D walkUp;
        Texture2D walkRight;
        Texture2D walkLeft;

        Texture2D background;
        Texture2D ball;
        Texture2D enemy;

        SpriteFont menuFont;
        SpriteFont scoreFont;

        Player player = new Player();
        Controller controller = new Controller();

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
            _graphics.PreferredBackBufferHeight = 736;
            _graphics.ApplyChanges();

            this.camera = new Camera(_graphics.GraphicsDevice);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // playerSprite = Content.Load<Texture2D>("Player/Sprite");
            walkDown = Content.Load<Texture2D>("Player/Down");
            walkUp = Content.Load<Texture2D>("Player/Up");
            walkRight = Content.Load<Texture2D>("Player/Right");
            walkLeft = Content.Load<Texture2D>("Player/Left");

            background = Content.Load<Texture2D>("map");
            ball = Content.Load<Texture2D>("FireBall");
            enemy = Content.Load<Texture2D>("SlimeEnemy");

            menuFont = Content.Load<SpriteFont>("Menu");
            scoreFont = Content.Load<SpriteFont>("scoreFont");

            player.animations[0] = new SpriteAnimation(walkDown, 4 , 8);
            player.animations[1] = new SpriteAnimation(walkUp, 4, 8);
            player.animations[2] = new SpriteAnimation(walkLeft, 4, 8);
            player.animations[3] = new SpriteAnimation(walkRight, 4, 8);

            player.animation = player.animations[0];
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Controller.inGame)
            {
                player.Update(gameTime);
            }

            if (!player.dead)
            {
                Controller.Update(gameTime, enemy);
            }

            this.camera.Position = new Vector2(640, 370); //player.Position;
            this.camera.Update(gameTime);

            foreach (Bullet bullet in Bullet.bullets)
            {
                bullet.Update(gameTime);
            }

            foreach (Enemy enemy in Enemy.enemies)
            {
                enemy.Update(gameTime, player.Position, player.dead);
                int sum = 10 + enemy.radius;
                if (Vector2.Distance(player.Position, enemy.Position) < sum)
                {
                    player.dead = true;
                }
            }

            foreach (Bullet bullet in Bullet.bullets)
            {
                foreach (Enemy enemy in Enemy.enemies)
                {
                    int sum = bullet.radius + enemy.radius;
                    if (Vector2.Distance(bullet.Position, enemy.Position) < sum)
                    {
                        bullet.Collided = true;
                        enemy.Dead = true;
                        controller.UpdateScore(10);
                        Debug.WriteLine("Enemy hit");
                    }
                }
            }

            Bullet.bullets.RemoveAll(b => b.Collided);
            Enemy.enemies.RemoveAll(e => e.Dead);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(this.camera);

            _spriteBatch.Draw(background, new Vector2(0, 0), Color.White);
            _spriteBatch.DrawString(scoreFont, $"Score: {controller.Score}", new Vector2(1200, 100), Color.White);

            foreach (Enemy enemy in Enemy.enemies)
            {
                enemy.animation.Draw(_spriteBatch);
            }

            foreach (Bullet bullet in Bullet.bullets)
            {
                _spriteBatch.Draw(ball, new Vector2(bullet.Position.X - 160, bullet.Position.Y - 90), Color.White);
            }

            if (!player.dead)
            {
                player.animation.Draw(_spriteBatch);
            }

            if (Controller.inGame == false)
            {
                string menuMessage = "Press Enter To Begin";
                string characterMessage = "Player";
                Vector2 sizeOfText = menuFont.MeasureString(menuMessage);
                int halfWidth = _graphics.PreferredBackBufferWidth / 2;
                _spriteBatch.DrawString(menuFont, menuMessage, new Vector2(halfWidth - sizeOfText.X/2, 100), Color.White);
                _spriteBatch.DrawString(menuFont, characterMessage, new Vector2(5, 70), Color.White);
            }

            if (player.dead)
            {
                string endMessage = "Game Over";
                _spriteBatch.DrawString(menuFont, endMessage, new Vector2(640, 370), Color.White);

            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
