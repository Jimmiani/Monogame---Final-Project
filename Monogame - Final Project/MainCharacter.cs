using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame___Final_Project
{
    internal class MainCharacter
    {
        private Texture2D _idleSpriteSheet;
        private Texture2D _runSpriteSheet;
        private List<Texture2D> _runFrames;
        private List<Texture2D> _idleFrames;
        private List<Texture2D> _currentAnimationFrames;
        private int _currentFrame;
        private Vector2 _speed;
        private Vector2 _location;
        private Rectangle _hitbox;
        private float _frameTimer;
        private float _animationSpeed;
        private bool _facingLeft;
        public MainCharacter(Texture2D idleSpriteSheet, Texture2D runSpriteSheet, GraphicsDevice graphicsDevice, Vector2 speed, Vector2 location)
        {
            _idleSpriteSheet = idleSpriteSheet;
            _runSpriteSheet = runSpriteSheet;
            _speed = speed;
            _location = location;
            
            _idleFrames = new List<Texture2D>();
            _runFrames = new List<Texture2D>();
            _currentAnimationFrames = _idleFrames;
            _frameTimer = 0;
            _animationSpeed = 0.1f;
            _facingLeft = false;

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


            int runWidth = _runSpriteSheet.Width / 6;
            int runHeight = _runSpriteSheet.Height;

            for (int i = 0; i < 6; i++)
            {
                Rectangle sourceRect = new Rectangle(i * runWidth, 0, runWidth, runHeight);
                Texture2D cropTexture = new Texture2D(graphicsDevice, runWidth, runHeight);
                Color[] data = new Color[runWidth * runHeight];
                _runSpriteSheet.GetData(0, sourceRect, data, 0, data.Length);
                cropTexture.SetData(data);
                _runFrames.Add(cropTexture);
            }
            _location = new Vector2(254, 285);
            _hitbox = new Rectangle();
            _hitbox.Size = new Point(runWidth, 10);
            CalculateHitbox();
        }
        public void CollisionCheck(Rectangle barrier)
        {
            
            if (_hitbox.Intersects(barrier))
            {
                UndoMove();
            }
        }
        public void SetAnimation(string animationName)
        {
            if ((animationName == "idle" && _currentAnimationFrames == _idleFrames) || (animationName == "run" && _currentAnimationFrames == _runFrames))
            {
                return;
            }

            if (animationName == "idle")
            {
                _currentAnimationFrames = _idleFrames;
                _currentFrame = 0;
            }
            else if (animationName == "run")
            {
                _currentAnimationFrames = _runFrames;
                _currentFrame = 0;
            }
        }
        private void HandleSpritesheets(GameTime gameTime)
        {
            _frameTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_speed != Vector2.Zero)
            {
                SetAnimation("run");
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
        private void Move(GameTime gametime, List<Rectangle> barriers)
        {
            

            _speed = Vector2.Zero;

            KeyboardState keyboardState = Keyboard.GetState();

            
            if (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up))
            {
                _speed.Y -= 2; 
            }
            if (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down))
            {
                _speed.Y += 2;
            }
            if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
            {
                _speed.X -= 2;
                _facingLeft = true;
            }
            if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
            {
                _speed.X += 2;
                _facingLeft = false;
            }
            _location += _speed;
            CalculateHitbox();
            foreach (Rectangle barrier in barriers)
                CollisionCheck(barrier);


        }

        private void CalculateHitbox()
        {
            _hitbox.X = (int)Math.Round(_location.X + 15);
            _hitbox.Y = (int)Math.Round(_location.Y + _currentAnimationFrames[_currentFrame].Height * 2 - _hitbox.Height);
        }

        private void UndoMove()
        {
            _location -= _speed;
            CalculateHitbox();
            
        }
        public void Update(GameTime gameTime, List<Rectangle> barriers)
        {
            Move(gameTime, barriers);
            HandleSpritesheets(gameTime);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            SpriteEffects spriteEffect = SpriteEffects.None;

            if (_speed.X < 0 || _facingLeft)
            {
                spriteEffect = SpriteEffects.FlipHorizontally;
            }

            spriteBatch.Draw(_currentAnimationFrames[_currentFrame], _location, null, Color.White, 0f, new Vector2(0, 0), 2, spriteEffect,0f);
        }

        public Rectangle HitBox
        {
            get { return _hitbox; }
        }

        public Vector2 Location
        {
            get { return _location; }
            set { _location = value; }
        }
    }
}
