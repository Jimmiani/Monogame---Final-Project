﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Monogame___Final_Project
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Rectangle window;
        MouseState mouseState, prevMouseState;


        // Audio
        Song hauntedHouseSong;

        // Backgrounds
        Texture2D introTexture, forestTexture;

        // Buttons
        Texture2D playBtnTexture;
        Rectangle playBtnRect;

        // Fonts
        SpriteFont titleFont;


        enum Screen
        {
            Intro,
            Forest,

        }
        Screen screen;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            screen = Screen.Intro;
            window = new Rectangle(0, 0, 800, 500);
            _graphics.PreferredBackBufferWidth = window.Width;
            _graphics.PreferredBackBufferHeight = window.Height;
            _graphics.ApplyChanges();

            base.Initialize();

            playBtnRect = new Rectangle((window.Width / 2) - (playBtnTexture.Width / 2), 350, playBtnTexture.Width, playBtnTexture.Height);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Audio
            hauntedHouseSong = Content.Load<Song>("Audio/hauntedHouse");

            // Backgrounds
            introTexture = Content.Load<Texture2D>("Backgrounds/hauntedIntro");
            forestTexture = Content.Load<Texture2D>("Backgrounds/forestBackground");

            // Buttons
            playBtnTexture = Content.Load<Texture2D>("Buttons/playBtn");

            // Fonts
            titleFont = Content.Load<SpriteFont>("Fonts/pixelFont");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            prevMouseState = mouseState;
            mouseState = Mouse.GetState();
            this.Window.Title = $"x = {mouseState.X}, y = {mouseState.Y}";

            if (MediaPlayer.State == MediaState.Stopped)
            {
                MediaPlayer.Play(hauntedHouseSong);
            }

            if (screen == Screen.Intro)
            {
                if (playBtnRect.Contains(mouseState.Position))
                {
                    playBtnRect = new Rectangle(((window.Width / 2) - (playBtnTexture.Width / 2)) - 5, 345, playBtnTexture.Width + 10, playBtnTexture.Height + 8);
                    if (prevMouseState.LeftButton == ButtonState.Pressed)
                    {
                        playBtnRect = new Rectangle(((window.Width / 2) - (playBtnTexture.Width / 2)) + 5, 355, playBtnTexture.Width - 10, playBtnTexture.Height - 8);
                        if (mouseState.LeftButton == ButtonState.Released)
                        {
                            screen = Screen.Forest;
                            playBtnRect = new Rectangle((window.Width / 2) - (playBtnTexture.Width / 2), 350, playBtnTexture.Width, playBtnTexture.Height);
                        }
                    }
                }
                else if (!playBtnRect.Contains(mouseState.Position))
                {
                    if (prevMouseState.LeftButton == ButtonState.Released)
                    {
                        playBtnRect = new Rectangle((window.Width / 2) - (playBtnTexture.Width / 2), 350, playBtnTexture.Width, playBtnTexture.Height);
                    }
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();


            if (screen == Screen.Intro)
            {
                _spriteBatch.Draw(introTexture, new Vector2(0, 0), Color.White);
                _spriteBatch.Draw(playBtnTexture, playBtnRect, Color.White);
                _spriteBatch.DrawString(titleFont, "The Elder", new Vector2(20, 20), Color.DimGray, 0, new Vector2(0, 0), 0.6f, SpriteEffects.None, 0);
                _spriteBatch.DrawString(titleFont, "Gloom", new Vector2(190, 120), Color.ForestGreen, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
            }
            else if (screen == Screen.Forest)
            {
                _spriteBatch.Draw(forestTexture, new Vector2(0, 0), Color.White);
            }


            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
