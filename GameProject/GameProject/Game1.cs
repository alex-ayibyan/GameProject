﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Comora;
using System.Threading.Tasks.Sources;
using static System.Formats.Asn1.AsnWriter;
using System.Diagnostics;
using GameProject.GameState;
using System;
using System.Numerics;
using GameProject.Map;
using System.Collections.Generic;
using TiledSharp;

namespace GameProject
{
    public enum Direction
    {
        Down, Up, Left, Right
    }
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private GameWorld _world;
        private TmxMap map;
        private MapGenerator mapManager;
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
            this.camera = new Camera(_graphics.GraphicsDevice);
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            var player = new Player();
            this.camera.Position = player.Position;


            var scoreFont = Content.Load<SpriteFont>("scoreFont");
            var score = new ScoreController(scoreFont);
            var menuFont = Content.Load<SpriteFont>("Menu");


            Texture2D slimeEnemy = Content.Load<Texture2D>("SlimeEnemy");

            Texture2D walkDown = Content.Load<Texture2D>("Player/Down");
            Texture2D walkUp = Content.Load<Texture2D>("Player/Up");
            Texture2D walkRight = Content.Load<Texture2D>("Player/Right");
            Texture2D walkLeft = Content.Load<Texture2D>("Player/Left");

            player.animations[0] = new SpriteAnimation(walkDown, 4, 8); // Down
            player.animations[1] = new SpriteAnimation(walkUp, 4, 8);   // Up
            player.animations[2] = new SpriteAnimation(walkLeft, 4, 8); // Left
            player.animations[3] = new SpriteAnimation(walkRight, 4, 8); // Right
            player.animation = player.animations[0];

            Texture2D bulletTexture = Content.Load<Texture2D>("FireBall");
            player.bulletTexture = bulletTexture;

            var tmxMap = new TmxMap("Content/GameMap.tmx");
            var mapGenerator = new MapGenerator(tmxMap, Content);

            _world = new GameWorld(camera, menuFont, score);

            _world.InitializeStates(player, slimeEnemy, mapGenerator);
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            
            this.camera.Update(gameTime);
            _world.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(this.camera);
            _world.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }

}
