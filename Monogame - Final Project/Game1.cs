using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System.Diagnostics;

namespace Monogame___Final_Project
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Rectangle window;
        MouseState mouseState, prevMouseState;
        float forestSeconds, mansionSeconds;

        // Audio
        Song currentSong, hauntedHouseSong, spookySong;
        SoundEffect thunderEffect;
        SoundEffectInstance thunderInstance;

        // Backgrounds
        Texture2D introTexture, forestTexture, blackTexture, mansion1Texture, mansion2Texture, mansion3Texture, mansion4Texture, mansion5Texture;

        // Buttons
        Texture2D playBtnTexture;
        Rectangle playBtnRect;

        // Fonts
        SpriteFont titleFont;

        // Sprite sheet
        CutsceneCharacter cutsceneCharacter;
        CutsceneEnemy cutsceneEnemy;
        MainCharacter mainCharacter;
        Texture2D hitTexture;
        List<Rectangle> barriers1;
        Texture2D charWalkAnimation, charIdleAnimation, enemyWalkAnimation, enemyIdleAnimation, enemyAtkAnimation, enemySmnAnimation, charTeleportAnimation, charRootAnimation, charRunAnimation;


        enum Screen
        {
            Intro,
            IntroDark,
            Forest,
            Mansion1,
            Mansion2
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
            screen = Screen.Mansion1;
            window = new Rectangle(0, 0, 800, 500);
            _graphics.PreferredBackBufferWidth = window.Width;
            _graphics.PreferredBackBufferHeight = window.Height;
            _graphics.ApplyChanges();
            forestSeconds = 0;
            mansionSeconds = 0;
            barriers1 = new List<Rectangle>();
            barriers1.Add(new Rectangle(145, 175, 55, 270));
            barriers1.Add(new Rectangle(185, 415, 83, 52));
            barriers1.Add(new Rectangle(185, 165, 55, 30));
            barriers1.Add(new Rectangle(239, 165, 106, 65));
            barriers1.Add(new Rectangle(230, 465, 375, 25));
            barriers1.Add(new Rectangle(333, 215, 106, 85));
            barriers1.Add(new Rectangle(350, 416, 50, 74));
            barriers1.Add(new Rectangle(400, 412, 131, 78));
            barriers1.Add(new Rectangle(591, 280, 39, 210));
            barriers1.Add(new Rectangle(447, 290, 163, 35));
            

            base.Initialize();

            currentSong = spookySong;
            thunderInstance = thunderEffect.CreateInstance();
            cutsceneCharacter = new CutsceneCharacter(charIdleAnimation, charWalkAnimation, charTeleportAnimation, charRootAnimation, GraphicsDevice, new Vector2(1, 0));
            cutsceneEnemy = new CutsceneEnemy(enemyIdleAnimation, enemyWalkAnimation, enemyAtkAnimation, enemySmnAnimation, GraphicsDevice, new Vector2(-1, 0));
            mainCharacter = new MainCharacter(charIdleAnimation, charRunAnimation, GraphicsDevice, Vector2.Zero, new Vector2(300, 150));
            cutsceneCharacter.SetAnimation("walk");
            playBtnRect = new Rectangle((window.Width / 2) - (playBtnTexture.Width / 2), 350, playBtnTexture.Width, playBtnTexture.Height);
            

        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Audio
            hauntedHouseSong = Content.Load<Song>("Audio/hauntedHouse");
            thunderEffect = Content.Load<SoundEffect>("Audio/thunderEffect");
            spookySong = Content.Load<Song>("Audio/spookyMusic");
            

            // Backgrounds
            introTexture = Content.Load<Texture2D>("Backgrounds/hauntedIntro");
            forestTexture = Content.Load<Texture2D>("Backgrounds/forestBackground");
            blackTexture = Content.Load<Texture2D>("Backgrounds/blackBackground");
            mansion1Texture = Content.Load<Texture2D>("Backgrounds/hauntedRoom1");
            mansion2Texture = Content.Load<Texture2D>("Backgrounds/hauntedRoom2");

            // Buttons
            playBtnTexture = Content.Load<Texture2D>("Buttons/playBtn");

            // Fonts
            titleFont = Content.Load<SpriteFont>("Fonts/pixelFont");

            // Sprite sheets
            charWalkAnimation = Content.Load<Texture2D>("Spritesheets/Main Character/Owlet_Monster_Walk");
            charIdleAnimation = Content.Load<Texture2D>("Spritesheets/Main Character/Owlet_Monster_Idle");
            enemyIdleAnimation = Content.Load<Texture2D>("Spritesheets/Enemy/Idle");
            enemyWalkAnimation = Content.Load<Texture2D>("Spritesheets/Enemy/Walk");
            enemyAtkAnimation = Content.Load<Texture2D>("Spritesheets/Enemy/Attack");
            enemySmnAnimation = Content.Load<Texture2D>("Spritesheets/Magic Effects/Summon");
            charTeleportAnimation = Content.Load<Texture2D>("Spritesheets/Magic Effects/Blink");
            charRunAnimation = Content.Load<Texture2D>("Spritesheets/Main Character/Owlet_Monster_Run");
            charRootAnimation = Content.Load<Texture2D>("Spritesheets/Magic Effects/Root");
            hitTexture = Content.Load<Texture2D>("Spritesheets/Main Character/rectangle");
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
                MediaPlayer.Play(currentSong);
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
                            thunderInstance.Play();
                            screen = Screen.IntroDark;
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
            else if (screen == Screen.IntroDark)
            {
                if (thunderInstance.State == SoundState.Stopped)
                {
                    screen = Screen.Forest;
                }
            }

            else if (screen == Screen.Forest)
            {
                forestSeconds += (float)gameTime.ElapsedGameTime.TotalSeconds;
                cutsceneCharacter.Update(gameTime);
                cutsceneEnemy.Update(gameTime);
                if (forestSeconds >= 19)
                {
                    currentSong = hauntedHouseSong;
                    MediaPlayer.Play(currentSong);
                    screen = Screen.Mansion1;
                }
            }

            else if (screen == Screen.Mansion1)
            {
                mansionSeconds += (float)gameTime.ElapsedGameTime.TotalSeconds;
                mainCharacter.Update(gameTime, barriers1);
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
                _spriteBatch.DrawString(titleFont, "The Eldritch", new Vector2(20, 20), Color.DimGray, 0, new Vector2(0, 0), 0.55f, SpriteEffects.None, 0);
                _spriteBatch.DrawString(titleFont, "Gloom", new Vector2(190, 120), Color.ForestGreen, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
                
            }
            else if (screen == Screen.IntroDark)
                _spriteBatch.Draw(blackTexture, new Vector2(0, 0), Color.White);
            else if (screen == Screen.Forest)
            {
                _spriteBatch.Draw(forestTexture, new Vector2(0, 0), Color.White);
                cutsceneCharacter.Draw(_spriteBatch);
                cutsceneEnemy.Draw(_spriteBatch);
            }
            else if (screen == Screen.Mansion1)
            {
                _spriteBatch.Draw(blackTexture, Vector2.Zero, Color.White);
                if (mansionSeconds > 3)
                {
                    _spriteBatch.Draw(mansion1Texture, new Vector2((window.Width / 2) - (mansion1Texture.Width / 2), (window.Height / 2) - (mansion1Texture.Height / 2)), null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
                    mainCharacter.Draw(_spriteBatch);
                    _spriteBatch.Draw(hitTexture, mainCharacter.HitBox, Color.Red * 0.4f);
                }
            }
            else if (screen == Screen.Mansion2)
            {
                _spriteBatch.Draw(mansion2Texture, new Vector2((window.Width / 2) - (mansion1Texture.Width / 2), (window.Height / 2) - (mansion1Texture.Height / 2)), Color.White);
            }


            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
