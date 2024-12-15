using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame___Final_Project
{
    internal class CutsceneCharacter
    {
        private Texture2D _idleSpriteSheet;
        private Texture2D _walkSpriteSheet;
        private Texture2D _teleportSpriteSheet;
        private List<Texture2D> _idleFrames;
        private List<Texture2D> _walkFrames;
        private List<Texture2D> _teleportFrames;
        private List<Texture2D> _currentAnimationFrames;
        private int _currentFrame;
        private int _currentTeleFrame;
        private float _frameTimer;
        private float _animationSpeed;
        private float _teleportTimer;
        private float _teleportDelay;
        private Vector2 _position;
        private Vector2 _speed;
        private bool _hasTeleported;
        private bool _isTeleporting;
        private float _teleportFrameTimer;

        public CutsceneCharacter(Texture2D idleSpriteSheet, Texture2D walkSpriteSheet, Texture2D teleportSpriteSheet, GraphicsDevice graphicsDevice, Vector2 speed)
        {
            _idleSpriteSheet = idleSpriteSheet;
            _walkSpriteSheet = walkSpriteSheet;
            _teleportSpriteSheet = teleportSpriteSheet;
            _idleFrames = new List<Texture2D>();
            _walkFrames = new List<Texture2D>();
            _teleportFrames = new List<Texture2D>();
            _currentAnimationFrames = _idleFrames;
            _currentFrame = 0;
            _currentTeleFrame = 0;
            _frameTimer = 0f;
            _animationSpeed = 0.1f;
            _speed = speed;
            _teleportTimer = 0f;
            _teleportDelay = 16.5f;
            _hasTeleported = false;
            _isTeleporting = false;
            _teleportFrameTimer = 0f;


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


            int teleportWidth = _teleportSpriteSheet.Width / 8;
            int teleportHeight = _teleportSpriteSheet.Height;

            for (int i = 0; i < 8; i++)
            {
                Rectangle sourceRect = new Rectangle(i * teleportWidth, 0, teleportWidth, teleportHeight);
                Texture2D cropTexture = new Texture2D(graphicsDevice, teleportWidth, teleportHeight);
                Color[] data = new Color[teleportWidth * teleportHeight];
                _teleportSpriteSheet.GetData(0, sourceRect, data, 0, data.Length);
                cropTexture.SetData(data);
                _teleportFrames.Add(cropTexture);
            }

            _position = new Vector2(-walkWidth * 3, 480 - (walkHeight * 3));
            
        }
        public void Update(GameTime gameTime)
        {
            _frameTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            _teleportTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            if (_teleportTimer > _teleportDelay && !_hasTeleported)
            {
                _isTeleporting = true;
            }


            if (_isTeleporting)
            {
                _teleportFrameTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                _position = new Vector2(800, 500);
                if (_teleportFrameTimer >= _animationSpeed)
                {
                    _currentTeleFrame++;
                    if (_currentTeleFrame >= _teleportFrames.Count)
                    {
                        _currentTeleFrame = 0;
                        _hasTeleported = true;
                        _isTeleporting = false;
                    }
                    _teleportFrameTimer = 0;
                }
            }

            if (_speed != Vector2.Zero)
            {
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

            if (_position.X > 200)
            {
                _speed = Vector2.Zero;
            }

        }

        public void SetAnimation(string animationName)
        {
            if ((animationName == "idle" && _currentAnimationFrames == _idleFrames) || (animationName == "walk" && _currentAnimationFrames == _walkFrames))
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
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_isTeleporting)
            {
                spriteBatch.Draw(_teleportFrames[_currentTeleFrame], new Vector2 (135, 275), null, Color.White, 0f, new Vector2(0, 0), 3, SpriteEffects.None, 0f);
            }


            if (_currentAnimationFrames.Count > 0)
            {
                spriteBatch.Draw(_currentAnimationFrames[_currentFrame], _position, null, Color.White, 0f, new Vector2(0, 0), 3, SpriteEffects.None, 0f);
            }
        }
    }
}

