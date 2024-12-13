using Microsoft.Xna.Framework;
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


        // Audio
        Song hauntedHouseSong;

        // Backgrounds
        Texture2D introTexture;

        // Buttons
        Texture2D playBtnTexture;
        Rectangle playBtnRect;


        enum Screen
        {
            Intro,

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

            playBtnRect = new Rectangle((window.Width / 2) - (playBtnTexture.Width / 2), 300, playBtnTexture.Width, playBtnTexture.Height);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Audio
            hauntedHouseSong = Content.Load<Song>("Audio/hauntedHouse");

            // Backgrounds
            introTexture = Content.Load<Texture2D>("Backgrounds/hauntedIntro");

            // Buttons
            playBtnTexture = Content.Load<Texture2D>("Buttons/playBtn");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (MediaPlayer.State == MediaState.Stopped)
            {
                MediaPlayer.Play(hauntedHouseSong);
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
            }


            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
