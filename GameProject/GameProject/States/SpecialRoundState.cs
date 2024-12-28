using Comora;
using GameProject.Controllers;
using GameProject.Entities;
using GameProject.Map;
using GameProject.States;
using GameProject.States.BaseStates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject.GameState
{
    public class SpecialRoundState : BaseGameState
    {
        private readonly Texture2D _tankEnemyTexture;
        private readonly Controller _controller;
        private readonly Vector2[] tankEnemySpawnPositions = new Vector2[]
        {
            new(64 * 32, 40 * 32),
            new(64 * 32, 58 * 32),
            new(33 * 32, 58 * 32),
            new(33 * 32, 41 * 32)
        };

        private bool transitionBackToPlaying = false;
        public double roundCounter = 0;

    public SpecialRoundState(GameWorld world, Player player, ScoreController score, Camera camera, Texture2D tankEnemyTexture)
        : base(world, player, score, camera)
        {
            _tankEnemyTexture = tankEnemyTexture;
            _controller = new Controller(_world, _mapGenerator, score);
        }

    public override void Update(GameTime gameTime)
    {
        UpdateCommon(gameTime);
        _controller.Update(gameTime, null, null, _tankEnemyTexture);
        

        if (!transitionBackToPlaying)
        {
            bool allTankEnemiesDead = Enemy.Enemies.All(e => !(e is TankEnemy) || e.Dead);
            bool noEnemiesLeft = !Enemy.Enemies.Any();

            if (allTankEnemiesDead || noEnemiesLeft || _world.Player.dead)
            {
                EndSpecialRound();
            }
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        DrawCommon(spriteBatch);
        spriteBatch.DrawString(_world.GeneralFont, "Special Round!", new Vector2(2300, 1400), Color.Yellow);
        spriteBatch.DrawString(_world.GeneralFont, $"BossEnemy Health: {roundCounter + 1}", new Vector2(2300, 1500), Color.White);
        foreach (var enemy in Enemy.Enemies)
        {
            enemy.Draw(spriteBatch);
        }
    }

    public void StartSpecialRound()
    {
        roundCounter += 0.5;

        foreach (var spawnPosition in tankEnemySpawnPositions)
        {
            var specialTank = new TankEnemy(spawnPosition, _tankEnemyTexture, _mapGenerator, _world, _controller)
            {
                IsStationary = true,
                CanShootBackAtPlayer = true,
            };
            specialTank.IncreaseHealth(roundCounter);
            Enemy.Enemies.Add(specialTank);
        }
        transitionBackToPlaying = false;
    }

    private void EndSpecialRound()
    {
        RemoveTankEnemies();
        _controller.SpecialTankRoundTriggered = false;
        _world.ChangeState(GameStates.Playing);
        transitionBackToPlaying = true;
    }

    private void RemoveTankEnemies()
    {
        Enemy.Enemies.RemoveAll(e => e is TankEnemy && e.Dead);
    }

    public void ResetRoundCounter()
    {
        roundCounter = 0;
    }

    public override Song GetBackgroundMusic()
    {
        return _world.SpecialRoundMusic;
    }
    }
}

