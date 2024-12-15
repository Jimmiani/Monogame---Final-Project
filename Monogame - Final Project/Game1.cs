using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
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
        Song hauntedHouseSong;

        // Backgrounds
        Texture2D introTexture, forestTexture, blackTexture, mansion1Texture;

        // Buttons
        Texture2D playBtnTexture;
        Rectangle playBtnRect;

        // Fonts
        SpriteFont titleFont;

        // Sprite sheet
        CutsceneCharacter cutsceneCharacter;
        CutsceneEnemy cutsceneEnemy;
        Texture2D charWalkAnimation, charIdleAnimation, enemyWalkAnimation, enemyIdleAnimation, enemyAtkAnimation, enemySmnAnimation, charTeleportAnimation;


        enum Screen
        {
            Intro,
            Forest,
            Mansion1,
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
            forestSeconds = 0;
            mansionSeconds = 0;

            

            base.Initialize();

            cutsceneCharacter = new CutsceneCharacter(charIdleAnimation, charWalkAnimation, charTeleportAnimation, GraphicsDevice, new Vector2(1, 0));
            cutsceneEnemy = new CutsceneEnemy(enemyIdleAnimation, enemyWalkAnimation, enemyAtkAnimation, enemySmnAnimation, GraphicsDevice, new Vector2(-1, 0));
            cutsceneCharacter.SetAnimation("walk");
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
            blackTexture = Content.Load<Texture2D>("Backgrounds/blackBackground");
            mansion1Texture = Content.Load<Texture2D>("Backgrounds/hauntedRoom1");

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
            charTeleportAnimation = Content.Load<Texture2D>("Spritesheets/Magic Effects/Teleport");


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

            if (screen == Screen.Forest)
            {
                forestSeconds += (float)gameTime.ElapsedGameTime.TotalSeconds;
                cutsceneCharacter.Update(gameTime);
                cutsceneEnemy.Update(gameTime);
                if (forestSeconds >= 19)
                {
                    screen = Screen.Mansion1;
                }
            }

            if (screen == Screen.Mansion1)
            {
                mansionSeconds += (float)gameTime.ElapsedGameTime.TotalSeconds;
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
                    _spriteBatch.Draw(mansion1Texture, new Vector2((window.Width / 2) - (mansion1Texture.Width / 2), (window.Height / 2) - (mansion1Texture.Height / 2)), Color.White);
                }
            }


            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
