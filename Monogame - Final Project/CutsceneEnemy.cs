﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame___Final_Project
{
    internal class CutsceneEnemy
    {
        private Texture2D _idleSpriteSheet;
        private Texture2D _walkSpriteSheet;
        private Texture2D _attackSpriteSheet;
        private Texture2D _summonSpriteSheet;
        private List<Texture2D> _idleFrames;
        private List<Texture2D> _walkFrames;
        private List<Texture2D> _attackFrames;
        private List<Texture2D> _summonFrames;
        private List<Texture2D> _currentAnimationFrames;
        private int _currentFrame;
        private int _currentSmnFrame;
        private float _frameTimer;
        private float _animationSpeed;
        private Vector2 _position;
        private Vector2 _speed;
        private Vector2 _summonLocation;
        private bool _isAttacking;
        private float _attackDuration;
        private float _attackTimer;
        private float _spawnTimer;
        private float _attackDelay;
        private bool _hasAttacked;
        private float _summonTimer;
        private bool _isSummoning;
        private float _summonDelay;
        private float _summonDelayTimer;
        private float _summonerDelayTimer;
        private float _summonIdleTimer;
        private SoundEffect _summonEffect;
        private bool _hasPlayedSummon;
        private SoundEffect _tensionEffect;
        private bool _hasPlayedTension;
        private SoundEffect _biteEffect;
        private bool _hasBitten;

        public CutsceneEnemy(Texture2D idleSpriteSheet, Texture2D walkSpriteSheet, Texture2D attackSpriteSheet, Texture2D summonSpriteSheet, GraphicsDevice graphicsDevice, Vector2 speed, SoundEffect summonEffect, SoundEffect tensionEffect, SoundEffect biteEffect)
        {
            _idleSpriteSheet = idleSpriteSheet;
            _walkSpriteSheet = walkSpriteSheet;
            _attackSpriteSheet = attackSpriteSheet;
            _summonSpriteSheet = summonSpriteSheet;
            _summonEffect = summonEffect;
            _tensionEffect = tensionEffect;
            _biteEffect = biteEffect;

            _idleFrames = new List<Texture2D>();
            _walkFrames = new List<Texture2D>();
            _attackFrames = new List<Texture2D>();
            _summonFrames = new List<Texture2D>();
            _currentAnimationFrames = _idleFrames;
            _currentFrame = 0;
            _currentSmnFrame = 0;
            _frameTimer = 0f;
            _animationSpeed = 0.1f;
            _isAttacking = false;
            _attackDuration = 1.2f;
            _attackTimer = 0f;
            _speed = speed;
            _spawnTimer = 0f;
            _attackDelay = 5f;
            _hasAttacked = false;
            _summonTimer = 0f;
            _summonDelayTimer = 0f;
            _summonDelay = 6;
            _summonLocation = new Vector2(420, 200);
            _summonerDelayTimer = 0f;
            _summonIdleTimer = 0f;
            _hasPlayedSummon = false;
            _hasPlayedTension = false;
            _hasBitten = false;

            int idleWidth = _idleSpriteSheet.Width / 4;
            int idleHeight = _idleSpriteSheet.Height;

            for (int i = 0; i < 4; i++)
            {
                Rectangle sourceRect = new Rectangle(i * idleWidth, 0, idleWidth, idleHeight);
                Texture2D cropTexture = new Texture2D(graphicsDevice, idleWidth, idleHeight);
                Color[] data = new Color[idleWidth * idleHeight];
                _idleSpriteSheet.GetData(0, sourceRect, data, 0, data.Length);
                cropTexture.SetData(data);
                _idleFrames.Add(cropTexture);
            }


            int walkWidth = _walkSpriteSheet.Width / 6;
            int walkHeight = _walkSpriteSheet.Height;

            for (int i = 0; i < 6; i++)
            {
                Rectangle sourceRect = new Rectangle(i * walkWidth, 0, walkWidth, walkHeight);
                Texture2D cropTexture = new Texture2D(graphicsDevice, walkWidth, walkHeight);
                Color[] data = new Color[walkWidth * walkHeight];
                _walkSpriteSheet.GetData(0, sourceRect, data, 0, data.Length);
                cropTexture.SetData(data);
                _walkFrames.Add(cropTexture);
            }


            int attackWidth = _attackSpriteSheet.Width / 12;
            int attackHeight = _attackSpriteSheet.Height;

            for (int i = 0; i < 12; i++)
            {
                 Rectangle sourceRect = new Rectangle(i * attackWidth, 0, attackWidth, attackHeight);
                 Texture2D cropTexture = new Texture2D(graphicsDevice, attackWidth, attackHeight);
                 Color[] data = new Color[attackWidth * attackHeight];
                 _attackSpriteSheet.GetData(0, sourceRect, data, 0, data.Length);
                 cropTexture.SetData(data);
                 if (_attackFrames.Count < 12) 
                     _attackFrames.Add(cropTexture);
            }


            int summonWidth = _summonSpriteSheet.Width / 8;
            int summonHeight = _summonSpriteSheet.Height;

            for (int i = 0; i < 8; i++)
            {
                Rectangle sourceRect = new Rectangle(i * summonWidth, 0, summonWidth, summonHeight);
                Texture2D cropTexture = new Texture2D(graphicsDevice, summonWidth, summonHeight);
                Color[] data = new Color[summonWidth * summonHeight];
                _summonSpriteSheet.GetData(0, sourceRect, data, 0, data.Length);
                cropTexture.SetData(data);
                if (_summonFrames.Count < 8)
                    _summonFrames.Add(cropTexture);
            }
            _isSummoning = true;

            _position = new Vector2(750, 480 - (walkHeight * 3));

        }

        public void Attack()
        {
            if (!_isAttacking && !_hasAttacked)
            {
                _isAttacking = true;  
                _attackTimer = 0f;
                _spawnTimer = 0f;
                if (!_hasBitten)
                {
                    _biteEffect.Play();
                    _hasBitten = true;
                }
            }
        }

        public void Update(GameTime gameTime)
        {

            if (!_isSummoning)
            {
                _frameTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                _spawnTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                _summonIdleTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (_isSummoning && _summonerDelayTimer > 3f && _hasPlayedSummon == false)
            {
                _summonEffect.Play();
                _hasPlayedSummon = true;
            }

                if (_isSummoning)
                {
                    _summonTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    _summonDelayTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    _summonerDelayTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (_summonTimer >= 0.1f)
                    {
                        _currentSmnFrame++;
                        _summonTimer = 0f;
                        if (_currentSmnFrame >= _summonFrames.Count)
                        {
                            _currentSmnFrame = 0;
                        }
                    }
                    if (_summonDelayTimer >= _summonDelay)
                    {
                        _isSummoning = false;
                        _currentSmnFrame = 0;
                        _position = new Vector2(290, 100);
                        if (!_hasPlayedTension)
                        {
                            _tensionEffect.Play();
                            _hasPlayedTension = true;
                        }
                    }
                }

            if (_spawnTimer >= _attackDelay && !_isAttacking && !_hasAttacked)
            {
                Attack();
            }

            if (_isAttacking)
            {
                if (_currentAnimationFrames != _attackFrames)
                {
                    SetAnimation("attack");
                    _currentFrame = 0;
                }

                _attackTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                _speed = Vector2.Zero;


                if (_frameTimer >= _animationSpeed)
                {
                    _currentFrame++;
                    if (_currentFrame >= _currentAnimationFrames.Count)
                    {
                        _currentFrame = 0;
                    }
                    _frameTimer = 0f;
                }

                if (_attackTimer >= _attackDuration)
                {
                    _isAttacking = false;
                    _hasAttacked = true;
                    _attackTimer = 0f;
                    SetAnimation("idle");
                    _currentFrame = 0;
                }
            }
            else
            {
                if (_position.X < 220)
                {
                    _speed = Vector2.Zero;
                    SetAnimation("idle");
                }
                else if (_speed != Vector2.Zero && _summonIdleTimer > 2)
                {
                    if (!_isSummoning)
                        _position += _speed;
                    SetAnimation("walk");
                }
                else
                {
                    SetAnimation("idle");
                }
                if (_frameTimer >= _animationSpeed)
                {
                    _currentFrame++;
                    if (_currentFrame >= _currentAnimationFrames.Count)
                    {
                        _currentFrame = 0;
                    }
                    _frameTimer = 0f;
                }
            }

        }

        public void SetAnimation(string animationName)
        {
            if ((animationName == "idle" && _currentAnimationFrames == _idleFrames) ||
            (animationName == "walk" && _currentAnimationFrames == _walkFrames) ||
            (animationName == "attack" && _currentAnimationFrames == _attackFrames))
            {
                return;
            }

            if (animationName == "idle")
            {
                _currentAnimationFrames = _idleFrames;
                _currentFrame = 0;
            }
            else if (animationName == "walk")
            {
                _currentAnimationFrames = _walkFrames;
                _currentFrame = 0;
            }
            else if (animationName == "attack")
            {
                _currentAnimationFrames = _attackFrames;
                _currentFrame = 0;
            }
            _currentFrame = 0;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_isSummoning && _summonerDelayTimer > 3f)
            {
                spriteBatch.Draw(_summonFrames[_currentSmnFrame], _summonLocation, null, Color.White, 0, new Vector2(0, 0), 4, SpriteEffects.None, 0);
            }


            if (_currentAnimationFrames.Count > 0)
            {
                spriteBatch.Draw(_currentAnimationFrames[_currentFrame], _position, null, Color.White, 0, new Vector2(0, 0), 4, SpriteEffects.None, 0);
            }

        }

    }
}
    

