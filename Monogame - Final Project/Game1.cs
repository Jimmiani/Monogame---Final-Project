using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections;
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
        KeyboardState keyboardState, prevKeyboardState;
        float forestSeconds, mansionSeconds;

        // Audio
        Song currentSong, hauntedHouseSong, spookySong;
        SoundEffect thunderEffect, summonEffect, rootEffect, teleportEffect, doorEffect, collectEffect;
        SoundEffectInstance thunderInstance;

        // Backgrounds
        Texture2D introTexture, forestTexture, blackTexture, mansion1Texture, mansion2Texture, mansion3Texture, mansion4Texture, mansion5Texture, fullMapTexture;

        // Buttons
        Texture2D playBtnTexture, backBtnTexture, mapBtnTexture;
        Rectangle playBtnRect, backBtnRect, mapBtnRect;

        // E Indicators

        // Doors
        Rectangle mansion1Door, mansion2Door1, mansion2Door3, mansion2Door4, mansion2Door5, mansion3Door, mansion4Door, mansion5Door;
        // Books
        Rectangle hintBookRect, hintBookRect2;
        string riddle1Text1, riddle1Text2;
        Texture2D book1Texture, closeUpBook1Texture, hintBookTexture, keyIndicatorTexture;
        bool hasKey, keyIsVisible, hasOpenedChest, hasMap;

        // Fonts
        SpriteFont titleFont;
        SpriteFont hintFont;

        // Images
        Texture2D eIndicatorTexture;
        Rectangle eIndicatorRect;
        bool eIsVisible;
        Texture2D hauntedStairs, hauntedRoom2Door;
        Texture2D closedChestTexture, openedChestTexture, currentChestTexture, keyTexture, groundMapTexture;
        Vector2 closedChestPos, openedChestPos, currentChestPos;
        Rectangle chestArea, groundMapRect;
        Rectangle keyRect;

        // Locations
        Vector2 mansion1Location1, mansion1Location2, mansion2Location1, mansion2Location2, mansion2Location3, mansion2Location4, mansion3Location1, mansion4Location1, mansion5Location1;

        // Sprite sheet
        CutsceneCharacter cutsceneCharacter;
        CutsceneEnemy cutsceneEnemy;
        MainCharacter mainCharacter;
        Texture2D hitTexture;
        List<Rectangle> barriers1;
        List<Rectangle> barriers2;
        List<Rectangle> barriers3;
        List<Rectangle> barriers4;
        List<Rectangle> barriers5;
        Texture2D charWalkAnimation, charIdleAnimation, enemyWalkAnimation, enemyIdleAnimation, enemyAtkAnimation, enemySmnAnimation, charTeleportAnimation, charRootAnimation, charRunAnimation;


        enum Screen
        {
            Intro,
            IntroDark,
            Forest,
            Mansion1,
            Mansion2,
            Mansion3,
            Mansion4,
            Mansion5,
            Hint1,
            KeyBook,
            Map
        }
        Screen screen;

        private Screen currentScreen;

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
            closedChestPos = new Vector2(280, 140);
            openedChestPos = new Vector2(280, 132);
            currentChestPos = closedChestPos;
            keyRect = new Rectangle(490, 99, 109, 231);
            hasKey = false;
            hasOpenedChest = false;
            hasMap = false;

            eIndicatorRect = new Rectangle(580, 310, 54, 48);
            eIsVisible = false;
            keyIsVisible = false;

            riddle1Text1 = "The key to knowledge is\n" +
                           "hard to hold,\n\n" +
                           "Hidden within where\n" +
                           "stories are told.";
                           

            riddle1Text2 = "Look for the tome that\n" +
                           "feels misplaced,\n\n" +
                           "For its spine is marked\n" +
                           "but its words erased.";



            mansion1Door = new Rectangle(580, 375, 20, 55);
            mansion2Door1 = new Rectangle(0, 228, 15, 52);
            mansion2Door3 = new Rectangle(160, 150, 60, 10);
            mansion2Door4 = new Rectangle(674, 101, 60, 8);
            mansion2Door5 = new Rectangle(650, 475, 44, 10);
            mansion3Door = new Rectangle(485, 452, 81, 10);
            mansion4Door = new Rectangle(260, 375, 80, 15);
            mansion5Door = new Rectangle(556, 66, 104, 14);

            hintBookRect = new Rectangle(502, 290, 38, 6);
            hintBookRect2 = new Rectangle(95, 165, 20, 5);
            chestArea = new Rectangle(294, 187, 22, 6);

            mansion1Location1 = new Vector2(254, 285);
            mansion1Location2 = new Vector2(518, 339);
            mansion2Location1 = new Vector2(17, 210);
            mansion2Location2 = new Vector2(160, 112);
            mansion2Location3 = new Vector2(673, 67);
            mansion2Location4 = new Vector2(640, 410);
            mansion3Location1 = new Vector2(497, 380);
            mansion4Location1 = new Vector2(270, 292);
            mansion5Location1 = new Vector2(576, 27);

            barriers1 = new List<Rectangle>();
            barriers1.Add(new Rectangle(145, 175, 55, 270));
            barriers1.Add(new Rectangle(185, 415, 83, 52));
            barriers1.Add(new Rectangle(185, 165, 55, 37));
            barriers1.Add(new Rectangle(239, 165, 106, 65));
            barriers1.Add(new Rectangle(230, 465, 375, 25));
            barriers1.Add(new Rectangle(333, 215, 106, 85));
            barriers1.Add(new Rectangle(350, 416, 50, 74));
            barriers1.Add(new Rectangle(400, 412, 131, 78));
            barriers1.Add(new Rectangle(591, 280, 39, 210));
            barriers1.Add(new Rectangle(447, 290, 163, 35));

            barriers2 = new List<Rectangle>();
            barriers2.Add(new Rectangle(0, 0, 0, 800));
            barriers2.Add(new Rectangle(0, 325, 415, 45));
            barriers2.Add(new Rectangle(0, 290, 30, 50));
            barriers2.Add(new Rectangle(31, 298, 33, 42));
            barriers2.Add(new Rectangle(63, 306, 31, 34));
            barriers2.Add(new Rectangle(95, 318, 20, 22));
            barriers2.Add(new Rectangle(415, 356, 128, 31));
            barriers2.Add(new Rectangle(500, 388, 43, 112));
            barriers2.Add(new Rectangle(520, 485, 280, 15));
            barriers2.Add(new Rectangle(768, 0, 32, 500));
            barriers2.Add(new Rectangle(736, 230, 64, 91));
            barriers2.Add(new Rectangle(752, 101, 48, 129));
            barriers2.Add(new Rectangle(514, 101, 141, 112));
            barriers2.Add(new Rectangle(620, 0, 180, 101));
            barriers2.Add(new Rectangle(576, 213, 31, 16));
            barriers2.Add(new Rectangle(256, 149, 257, 16));
            barriers2.Add(new Rectangle(159, 120, 97, 29));
            barriers2.Add(new Rectangle(84, 128, 75, 36));
            barriers2.Add(new Rectangle(50, 110, 45, 119));
            barriers2.Add(new Rectangle(0, 165, 31, 48));
            barriers2.Add(new Rectangle(32, 185, 31, 34));
            barriers2.Add(new Rectangle(96, 194, 19, 43));
            barriers2.Add(new Rectangle(312, 262, 67, 30));

            barriers3 = new List<Rectangle>();
            barriers3.Add(new Rectangle(0, 0, 800, 221));
            barriers3.Add(new Rectangle(0, 0, 166, 500));
            barriers3.Add(new Rectangle(0, 302, 406, 50));
            barriers3.Add(new Rectangle(406, 306, 39, 155));
            barriers3.Add(new Rectangle(410, 422, 75, 50));
            barriers3.Add(new Rectangle(566, 387, 160, 100));
            barriers3.Add(new Rectangle(581, 200, 100, 56));
            barriers3.Add(new Rectangle(601, 256, 100, 166));
            barriers3.Add(new Rectangle(460, 462, 140, 38));

            barriers4 = new List<Rectangle>();
            barriers4.Add(new Rectangle(0, 0, 800, 149));
            barriers4.Add(new Rectangle(0, 0, 239, 500));
            barriers4.Add(new Rectangle(0, 390, 800, 110));
            barriers4.Add(new Rectangle(560, 0, 240, 500));
            barriers4.Add(new Rectangle(415, 0, 75, 209));
            barriers4.Add(new Rectangle(495, 0, 305, 224));
            barriers4.Add(new Rectangle(450, 252, 50, 52));
            barriers4.Add(new Rectangle(500, 255, 40, 35));
            barriers4.Add(new Rectangle(240, 362, 18, 28));
            barriers4.Add(new Rectangle(340, 362, 14, 28));
            barriers4.Add(new Rectangle(280, 0, 50, 186));

            barriers5 = new List<Rectangle>();
            barriers5.Add(new Rectangle(0, 0, 800, 67));
            barriers5.Add(new Rectangle(0, 0, 555, 193));
            barriers5.Add(new Rectangle(661, 0, 139, 193));
            barriers5.Add(new Rectangle(696, 0, 104, 301));
            barriers5.Add(new Rectangle(766, 0, 34, 500));
            barriers5.Add(new Rectangle(0, 440, 800, 60));
            barriers5.Add(new Rectangle(0, 0, 34, 500));
            barriers5.Add(new Rectangle(0, 0, 452, 301));
            barriers5.Add(new Rectangle(139, 0, 208, 332));

            base.Initialize();

            currentSong = spookySong;
            thunderInstance = thunderEffect.CreateInstance();
            cutsceneCharacter = new CutsceneCharacter(charIdleAnimation, charWalkAnimation, charTeleportAnimation, charRootAnimation, GraphicsDevice, new Vector2(1, 0), rootEffect, teleportEffect);
            cutsceneEnemy = new CutsceneEnemy(enemyIdleAnimation, enemyWalkAnimation, enemyAtkAnimation, enemySmnAnimation, GraphicsDevice, new Vector2(-1, 0), summonEffect);
            mainCharacter = new MainCharacter(charIdleAnimation, charRunAnimation, GraphicsDevice, Vector2.Zero, mansion1Location1);
            cutsceneCharacter.SetAnimation("walk");
            playBtnRect = new Rectangle((window.Width / 2) - (playBtnTexture.Width / 2), 350, playBtnTexture.Width, playBtnTexture.Height);
            currentChestTexture = closedChestTexture;
            groundMapRect = new Rectangle(350, 186, groundMapTexture.Width, groundMapTexture.Height);
            mapBtnRect = new Rectangle(window.Width - mapBtnTexture.Width - 10, 10, mapBtnTexture.Width, mapBtnTexture.Height);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Audio
            hauntedHouseSong = Content.Load<Song>("Audio/hauntedHouse");
            thunderEffect = Content.Load<SoundEffect>("Audio/thunderEffect");
            spookySong = Content.Load<Song>("Audio/spookyMusic");
            summonEffect = Content.Load<SoundEffect>("Audio/summonEffect");
            rootEffect = Content.Load<SoundEffect>("Audio/rootEffect");
            teleportEffect = Content.Load<SoundEffect>("Audio/teleportEffect");
            doorEffect = Content.Load<SoundEffect>("Audio/doorEffect");
            collectEffect = Content.Load<SoundEffect>("Audio/collectEFfect");
            

            // Backgrounds
            introTexture = Content.Load<Texture2D>("Backgrounds/hauntedIntro");
            forestTexture = Content.Load<Texture2D>("Backgrounds/forestBackground");
            blackTexture = Content.Load<Texture2D>("Backgrounds/blackBackground");
            mansion1Texture = Content.Load<Texture2D>("Backgrounds/hauntedRoom1");
            mansion2Texture = Content.Load<Texture2D>("Backgrounds/hauntedRoom2");
            mansion3Texture = Content.Load<Texture2D>("Backgrounds/hauntedRoom3");
            mansion4Texture = Content.Load<Texture2D>("Backgrounds/hauntedRoom4");
            mansion5Texture = Content.Load<Texture2D>("Backgrounds/hauntedRoom5");
            fullMapTexture = Content.Load<Texture2D>("Backgrounds/mansionMap");

            // Buttons
            playBtnTexture = Content.Load<Texture2D>("Buttons/playBtn");
            backBtnTexture = Content.Load<Texture2D>("Buttons/exitBtn");

            // Images
            eIndicatorTexture = Content.Load<Texture2D>("Images/eIndicator");
            hauntedStairs = Content.Load<Texture2D>("Images/hauntedRoom2Stairs");
            hauntedRoom2Door = Content.Load<Texture2D>("Images/hauntedDoor2");
            book1Texture = Content.Load<Texture2D>("Images/book1");
            closeUpBook1Texture = Content.Load<Texture2D>("Images/closeBook1");
            hintBookTexture = Content.Load<Texture2D>("Images/hintBook");
            closedChestTexture = Content.Load<Texture2D>("Images/closedChest");
            openedChestTexture = Content.Load<Texture2D>("Images/openedChest");
            keyTexture = Content.Load<Texture2D>("Images/chestKey");
            keyIndicatorTexture = Content.Load<Texture2D>("Images/keyIndicator");
            mapBtnTexture = Content.Load<Texture2D>("Images/mapBtn");
            groundMapTexture = Content.Load<Texture2D>("Images/mapOnGround");

            // Fonts
            titleFont = Content.Load<SpriteFont>("Fonts/pixelFont");
            hintFont = Content.Load<SpriteFont>("Fonts/hintFont");

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

        public void ResizeWindow(int width, int height)
        {
            _graphics.PreferredBackBufferWidth = width;
            _graphics.PreferredBackBufferHeight = height;
            _graphics.ApplyChanges();
        }
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            prevMouseState = mouseState;
            mouseState = Mouse.GetState();
            prevKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();
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
                currentScreen = Screen.Mansion1;

                // Room 2
                if (mainCharacter.HitBox.Intersects(mansion1Door))
                {
                    eIndicatorRect = new Rectangle(580, 310, 54, 48);
                    eIsVisible = true;
                    if (keyboardState.IsKeyDown(Keys.E) && prevKeyboardState.IsKeyUp(Keys.E))
                    {
                        screen = Screen.Mansion2;
                        mainCharacter.Location = mansion2Location1;
                        doorEffect.Play();
                    }
                }
                else if (!mainCharacter.HitBox.Intersects(mansion1Door))
                    eIsVisible = false;
            }

            else if (screen == Screen.Mansion2)
            {
                mainCharacter.Update(gameTime, barriers2);
                currentScreen = Screen.Mansion2;

                if (!mainCharacter.HitBox.Intersects(mansion2Door1) || !mainCharacter.HitBox.Intersects(mansion2Door3) || !mainCharacter.HitBox.Intersects(mansion2Door4) || !mainCharacter.HitBox.Intersects(mansion2Door5))
                    eIsVisible = false;
                
                // Room 1
                if (mainCharacter.HitBox.Intersects(mansion2Door1))
                {
                    eIndicatorRect = new Rectangle(2, 150, 54, 48);
                    eIsVisible = true;
                    if (keyboardState.IsKeyDown(Keys.E) && prevKeyboardState.IsKeyUp(Keys.E))
                    {
                        screen = Screen.Mansion1;
                        mainCharacter.Location = mansion1Location2;
                        doorEffect.Play();
                    }
                }
                

                // Room 3
                if (mainCharacter.HitBox.Intersects(mansion2Door3))
                {
                    eIndicatorRect = new Rectangle(213, 59, 54, 48);
                    eIsVisible = true;
                    if (keyboardState.IsKeyDown(Keys.E) && prevKeyboardState.IsKeyUp(Keys.E))
                    {
                        screen = Screen.Mansion3;
                        mainCharacter.Location = mansion3Location1;
                        doorEffect.Play();
                    }
                }

                // Room 4
                if (mainCharacter.HitBox.Intersects(mansion2Door4))
                {
                    eIndicatorRect = new Rectangle(630, 23, 54, 48);
                    eIsVisible = true;
                    if (keyboardState.IsKeyDown(Keys.E) && prevKeyboardState.IsKeyUp(Keys.E))
                    {
                        screen = Screen.Mansion4;
                        mainCharacter.Location = mansion4Location1;
                        doorEffect.Play();
                    }
                }

                // Room 5
                if (mainCharacter.HitBox.Intersects(mansion2Door5))
                {
                    eIndicatorRect = new Rectangle(685, 405, 54, 48);
                    eIsVisible = true;
                    if (keyboardState.IsKeyDown(Keys.E) && prevKeyboardState.IsKeyUp(Keys.E))
                    {
                        screen = Screen.Mansion5;
                        mainCharacter.Location = mansion5Location1;
                        doorEffect.Play();
                    }
                }

                // Key book
                if (mainCharacter.HitBox.Intersects(hintBookRect2))
                {
                    eIndicatorRect = new Rectangle(114, 100, 27, 24);
                    eIsVisible = true;
                    if (keyboardState.IsKeyDown(Keys.E) && prevKeyboardState.IsKeyUp(Keys.E))
                    {
                        screen = Screen.KeyBook;
                    }
                }
            }

            else if (screen == Screen.Mansion3)
            {
                mainCharacter.Update(gameTime, barriers3);
                currentScreen = Screen.Mansion3;

                // Room 2
                if (mainCharacter.HitBox.Intersects(mansion3Door))
                {
                    eIndicatorRect = new Rectangle(550, 420, 54, 48);
                    eIsVisible = true;
                    if (keyboardState.IsKeyDown(Keys.E) && prevKeyboardState.IsKeyUp(Keys.E))
                    {
                        screen = Screen.Mansion2;
                        mainCharacter.Location = mansion2Location2;
                        doorEffect.Play();
                    }
                }
                else if (!mainCharacter.HitBox.Intersects(mansion3Door))
                    eIsVisible = false;
            }

            else if (screen == Screen.Mansion4)
            {
                mainCharacter.Update(gameTime, barriers4);
                currentScreen = Screen.Mansion4;

                if (!mainCharacter.HitBox.Intersects(mansion4Door) || !mainCharacter.HitBox.Intersects(hintBookRect) || !mainCharacter.HitBox.Intersects(chestArea) || mainCharacter.HitBox.Intersects(groundMapRect))
                    eIsVisible = false;

                // Room 2
                if (mainCharacter.HitBox.Intersects(mansion4Door))
                {
                    eIndicatorRect = new Rectangle(220, 330, 54, 48);
                    eIsVisible = true;
                    if (keyboardState.IsKeyDown(Keys.E) && prevKeyboardState.IsKeyUp(Keys.E))
                    {
                        screen = Screen.Mansion2;
                        mainCharacter.Location = mansion2Location3;
                        doorEffect.Play();
                    }
                }

                // Hint book
                if (mainCharacter.HitBox.Intersects(hintBookRect))
                {
                    eIndicatorRect = new Rectangle(532, 232, 27, 24);
                    eIsVisible = true;
                    if (keyboardState.IsKeyDown(Keys.E) && prevKeyboardState.IsKeyUp(Keys.E))
                    {
                        screen = Screen.Hint1;
                    }
                }

                // Chest
                if (mainCharacter.HitBox.Intersects(chestArea) && !hasKey && !hasOpenedChest)
                {
                    keyIsVisible = true;
                }
                else if (!mainCharacter.HitBox.Intersects(chestArea) && !hasKey && !hasOpenedChest)
                {
                    keyIsVisible = false;
                }
                if (mainCharacter.HitBox.Intersects(chestArea) && hasKey && !hasOpenedChest)
                {
                    eIndicatorRect = new Rectangle(300, 100, 54, 48);
                    eIsVisible = true;
                    if (keyboardState.IsKeyDown(Keys.E) && prevKeyboardState.IsKeyUp(Keys.E))
                    {
                        currentChestTexture = openedChestTexture;
                        currentChestPos = openedChestPos;
                        hasKey = false;
                        hasOpenedChest = true;
                    }
                }

                // Map
                if (mainCharacter.HitBox.Intersects(groundMapRect) && hasOpenedChest && !hasMap)
                {
                    eIndicatorRect = new Rectangle(361, 164, 27, 24);
                    eIsVisible = true;
                    if (keyboardState.IsKeyDown(Keys.E) && prevKeyboardState.IsKeyUp(Keys.E))
                    {
                        hasMap = true;
                        collectEffect.Play();
                    }
                }
                

            }
            else if (screen == Screen.Mansion5)
            {
                mainCharacter.Update(gameTime, barriers5);
                currentScreen = Screen.Mansion5;

                if (!mainCharacter.HitBox.Intersects(mansion5Door))
                    eIsVisible = false;

                // Room 2
                if (mainCharacter.HitBox.Intersects(mansion5Door))
                {
                    eIndicatorRect = new Rectangle(633, 5, 54, 48);
                    eIsVisible = true;
                    if (keyboardState.IsKeyDown(Keys.E) && prevKeyboardState.IsKeyUp(Keys.E))
                    {
                        screen = Screen.Mansion2;
                        mainCharacter.Location = mansion2Location4;
                        doorEffect.Play();
                    }
                }
            }
            else if (screen == Screen.Hint1)
            {
                // Back Button
                backBtnRect = new Rectangle(700, 20, 80, 80);

                if (backBtnRect.Contains(mouseState.Position))
                {
                    backBtnRect = new Rectangle(694, 14, 92, 92);
                    if (prevMouseState.LeftButton == ButtonState.Pressed)
                    {
                        backBtnRect = new Rectangle(706, 26, 68, 68);
                        if (mouseState.LeftButton == ButtonState.Released)
                        {
                            screen = Screen.Mansion4;
                            backBtnRect = new Rectangle(700, 20, 80, 80);
                        }
                    }
                }
            }
            else if (screen == Screen.KeyBook)
            {
                // Back Button
                backBtnRect = new Rectangle(700, 20, 80, 80);

                if (backBtnRect.Contains(mouseState.Position))
                {
                    backBtnRect = new Rectangle(694, 14, 92, 92);
                    if (prevMouseState.LeftButton == ButtonState.Pressed)
                    {
                        backBtnRect = new Rectangle(706, 26, 68, 68);
                        if (mouseState.LeftButton == ButtonState.Released)
                        {
                            screen = Screen.Mansion2;
                            backBtnRect = new Rectangle(700, 20, 80, 80);
                        }
                    }
                }

                // Key
                keyRect = new Rectangle(490, 99, 109, 231);

                if (keyRect.Contains(mouseState.Position))
                {
                    keyRect = new Rectangle(487, 94, 115, 241);
                    if (prevMouseState.LeftButton == ButtonState.Pressed)
                    {
                        keyRect = new Rectangle(493, 104, 103, 221);
                        if (mouseState.LeftButton == ButtonState.Released)
                        {
                            hasKey = true;
                            keyRect = new Rectangle(490, 99, 109, 231);
                            collectEffect.Play();
                        }
                    }
                }
            }

            else if (screen == Screen.Map)
            {
                // Back Button
                backBtnRect = new Rectangle(7, 7, 50, 50);

                if (backBtnRect.Contains(mouseState.Position))
                {
                    backBtnRect = new Rectangle(2, 2, 60, 60);
                    if (prevMouseState.LeftButton == ButtonState.Pressed)
                    {
                        backBtnRect = new Rectangle(12, 12, 40, 40);
                        if (mouseState.LeftButton == ButtonState.Released)
                        {
                            screen = currentScreen;
                            backBtnRect = new Rectangle(7, 7, 50, 50);
                        }
                    }
                }
            }

            if ((screen == Screen.Mansion1 || screen == Screen.Mansion2 || screen == Screen.Mansion3 || screen == Screen.Mansion4 || screen == Screen.Mansion5) && hasMap)
            {
                mapBtnRect = new Rectangle(window.Width - mapBtnTexture.Width - 10, 10, mapBtnTexture.Width, mapBtnTexture.Height);

                if (mapBtnRect.Contains(mouseState.Position))
                {
                    mapBtnRect = new Rectangle(window.Width - mapBtnTexture.Width - 15, 5, mapBtnTexture.Width + 10, mapBtnTexture.Height + 10);
                    if (prevMouseState.LeftButton == ButtonState.Pressed)
                    {
                        mapBtnRect = new Rectangle(window.Width - mapBtnTexture.Width - 5, 15, mapBtnTexture.Width - 10, mapBtnTexture.Height - 10);
                        if (mouseState.LeftButton == ButtonState.Released)
                        {
                            screen = Screen.Map;
                            mapBtnRect = new Rectangle(window.Width - mapBtnTexture.Width - 10, 10, mapBtnTexture.Width, mapBtnTexture.Height);
                        }
                    }
                }
            }

            if (screen != Screen.Map)
            {
                ResizeWindow(800, 500);
            }
            else if (screen == Screen.Map)
            {
                ResizeWindow(752, 736);
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
                    if (eIsVisible)
                    {
                        _spriteBatch.Draw(eIndicatorTexture, eIndicatorRect, Color.White);
                    }
                }
            }
            else if (screen == Screen.Mansion2)
            {
                _spriteBatch.Draw(blackTexture, Vector2.Zero, Color.White);
                _spriteBatch.Draw(mansion2Texture, new Vector2(0, 0), Color.White);
                _spriteBatch.Draw(book1Texture, new Vector2(103, 125), Color.White);
                mainCharacter.Draw(_spriteBatch);
                _spriteBatch.Draw(hauntedStairs, new Vector2(0, 255), Color.White);
                _spriteBatch.Draw(hauntedRoom2Door, new Vector2(640, 412), Color.White);
                _spriteBatch.Draw(hitTexture, mainCharacter.HitBox, Color.Red * 0.4f);
                if (eIsVisible)
                {
                    _spriteBatch.Draw(eIndicatorTexture, eIndicatorRect, Color.White);
                }
            }
            else if (screen == Screen.Mansion3)
            {
                _spriteBatch.Draw(blackTexture, Vector2.Zero, Color.White);
                _spriteBatch.Draw(mansion3Texture, new Vector2((window.Width / 2) - (mansion3Texture.Width / 2), (window.Height / 2) - (mansion3Texture.Height / 2)), null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
                mainCharacter.Draw(_spriteBatch);
                _spriteBatch.Draw(hitTexture, mainCharacter.HitBox, Color.Red * 0.4f);
                if (eIsVisible)
                {
                    _spriteBatch.Draw(eIndicatorTexture, eIndicatorRect, Color.White);
                }
            }
            else if (screen == Screen.Mansion4)
            {
                _spriteBatch.Draw(blackTexture, Vector2.Zero, Color.White);
                _spriteBatch.Draw(mansion4Texture, new Vector2((window.Width / 2) - (mansion4Texture.Width / 2), (window.Height / 2) - (mansion4Texture.Height / 2)), null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
                _spriteBatch.Draw(hintBookTexture, new Vector2(517, 263), Color.White);
                _spriteBatch.Draw(currentChestTexture, currentChestPos, Color.White);
                if (hasOpenedChest && !hasMap)
                {
                    _spriteBatch.Draw(groundMapTexture, new Vector2(350, 186), Color.White);
                }
                mainCharacter.Draw(_spriteBatch);
                _spriteBatch.Draw(hitTexture, mainCharacter.HitBox, Color.Red * 0.4f);
                if (eIsVisible)
                {
                    _spriteBatch.Draw(eIndicatorTexture, eIndicatorRect, Color.White);
                }
                if (keyIsVisible)
                {
                    _spriteBatch.Draw(keyIndicatorTexture, new Vector2(300, 100), Color.White);
                }
            }
            else if (screen == Screen.Mansion5)
            {
                _spriteBatch.Draw(blackTexture, Vector2.Zero, Color.White);
                _spriteBatch.Draw(mansion5Texture, new Vector2((window.Width / 2) - (mansion5Texture.Width / 2), (window.Height / 2) - (mansion5Texture.Height / 2)), null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
                mainCharacter.Draw(_spriteBatch);
                _spriteBatch.Draw(hitTexture, mainCharacter.HitBox, Color.Red * 0.4f);
                if (eIsVisible)
                {
                    _spriteBatch.Draw(eIndicatorTexture, eIndicatorRect, Color.White);
                }
            }
            else if (screen == Screen.Hint1)
            {
                _spriteBatch.Draw(blackTexture, Vector2.Zero, Color.White);
                _spriteBatch.Draw(closeUpBook1Texture, new Vector2((window.Width / 2) - (closeUpBook1Texture.Width / 2), (window.Height / 2) - (closeUpBook1Texture.Height / 2)), null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
                _spriteBatch.DrawString(hintFont, riddle1Text1, new Vector2(140, 100), Color.Black);
                _spriteBatch.DrawString(hintFont, riddle1Text2, new Vector2(430, 100), Color.Black);
                _spriteBatch.Draw(backBtnTexture, backBtnRect, Color.White);
            }
            else if (screen == Screen.KeyBook)
            {
                _spriteBatch.Draw(blackTexture, Vector2.Zero, Color.White);
                _spriteBatch.Draw(closeUpBook1Texture, new Vector2((window.Width / 2) - (closeUpBook1Texture.Width / 2), (window.Height / 2) - (closeUpBook1Texture.Height / 2)), null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
                _spriteBatch.Draw(backBtnTexture, backBtnRect, Color.White);
                if (!hasKey)
                    _spriteBatch.Draw(keyTexture, keyRect, Color.White);
            }
            else if (screen == Screen.Map)
            {
                _spriteBatch.Draw(fullMapTexture, Vector2.Zero, Color.White);
                _spriteBatch.Draw(backBtnTexture, backBtnRect, Color.White);
            }
            if (hasKey && (screen != Screen.KeyBook && screen != Screen.Hint1))
            {
                _spriteBatch.Draw(keyTexture, new Rectangle((int)mainCharacter.Location.X - 10, (int)mainCharacter.Location.Y - 10, 18, 38), Color.White);
            }
            if ((screen == Screen.Mansion1 || screen == Screen.Mansion2 || screen == Screen.Mansion3 || screen == Screen.Mansion4 || screen == Screen.Mansion5) && hasMap)
            {
                _spriteBatch.Draw(mapBtnTexture, mapBtnRect, Color.White);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
