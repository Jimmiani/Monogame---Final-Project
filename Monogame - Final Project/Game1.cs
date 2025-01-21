using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.PortableExecutable;

namespace Monogame___Final_Project
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Rectangle window;
        MouseState mouseState, prevMouseState;
        KeyboardState keyboardState, prevKeyboardState;
        float forestSeconds, mansionSeconds, speechSeconds4, escapeSeconds, endSeconds;

        // Audio
        Song currentSong, hauntedHouseSong, spookySong;
        SoundEffect thunderEffect, summonEffect, rootEffect, teleportEffect, doorEffect, collectEffect, tensionEffect, chestEffect, biteEffect, openMapEffect, closeMapEffect, bellEffect, winEffect, breakEffect;
        SoundEffectInstance thunderInstance;
        bool playingMusic;
        Color musicColour;

        // Backgrounds
        Texture2D introTexture, menuTexture, forestTexture, blackTexture, mansion1Texture, mansion2Texture, mansion3Texture, mansion4Texture, mansion5Texture, fullMapTexture;

        // Buttons
        Texture2D playBtnTexture, backBtnTexture, mapBtnTexture, audioBtnTexture, helpBtnTexture;
        Rectangle playBtnRect, backBtnRect, mapBtnRect, audioBtnRect, helpBtnRect;

        // E Indicators

        // Doors
        Rectangle mansion1Door, mansion2Door1, mansion2Door3, mansion2Door4, mansion2Door5, mansion3Door, mansion4Door, mansion5Door;
        // Books
        Rectangle hintBookRect, hintBookRect2;
        string riddle1Text1, riddle1Text2;
        Texture2D book1Texture, closeUpBook1Texture, hintBookTexture, keyIndicatorTexture;
        bool hasKey, keyIsVisible, hasOpenedChest, hasMap, hasOrb;
        // Bells
        Rectangle bell1, bell2, bell3, bell4, orbRect, escapeRect;
        int expectedBell;

        // Fonts
        SpriteFont titleFont;
        SpriteFont hintFont;
        SpriteFont speechFont;
        SpeechManager speechManager;

        // Images
        Texture2D eIndicatorTexture, speechTexture;
        Rectangle eIndicatorRect;
        string mansion1Speech1, mansion1Speech2, mansion4Speech1, mapSpeech, orbSpeech;
        bool eIsVisible, mansion4SpeechUsed, canUseMapSpeech, hasUsedOrb, hasUsedOrbSpeech;
        Texture2D hauntedStairs, hauntedRoom2Door;
        Texture2D closedChestTexture, openedChestTexture, currentChestTexture, keyTexture, groundMapTexture, bellTexture, orbTexture;
        Rectangle chestRect;
        Rectangle chestArea, groundMapRect;
        Rectangle keyRect;
        string menuInstructions;
        Color orbColour;
        Texture2D brokenWall1Texture, brokenWall2Texture;

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

        // Magic Effect
        Texture2D magicSpritesheet, cropTexture;
        List<Texture2D> magicTextures;
        float magicSeconds;
        int magicIndex;

        // End Spritesheet
        Texture2D cropTexture2;
        List<Texture2D> runTextures;
        int runIndex;
        Rectangle runRect;
        Vector2 runSpeed;
        float runSeconds;

        // Idle Sprite sheet
        Texture2D cropTexture3;
        List<Texture2D> idleTextures;
        int idleIndex;
        float idleSeconds;

        enum Escape
        {
            Escape1,
            Escape2,
            Escape3
        }
        Escape escape;

        enum Step
        {
            Step1,
            Step2,
            Step3,
            Step4
        }
        Step step, currentStep, prevStep;
        Rectangle step1Rect, step2Rect, step3Rect;

        enum Screen
        {
            Intro,
            Menu,
            IntroDark,
            Forest,
            Mansion1,
            Mansion2,
            Mansion3,
            Mansion4,
            Mansion5,
            Hint1,
            KeyBook,
            Map,
            End,
            Win
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
            screen = Screen.Intro;
            escape = Escape.Escape1;
            currentStep = Step.Step2;
            window = new Rectangle(0, 0, 800, 500);
            _graphics.PreferredBackBufferWidth = window.Width;
            _graphics.PreferredBackBufferHeight = window.Height;
            _graphics.ApplyChanges();

            this.Window.Title = "The Eldritch Gloom";

            musicColour = Color.White;
            playingMusic = true;
            audioBtnRect = new Rectangle(20, 380, 100, 100);
            helpBtnRect = new Rectangle(680, 380, 100, 100);

            step1Rect = new Rectangle(28, 220, 1, 75);
            step2Rect = new Rectangle(60, 230, 1, 70);
            step3Rect = new Rectangle(91, 240, 1, 65);

            forestSeconds = 0;
            mansionSeconds = 0;
            speechSeconds4 = 0;
            escapeSeconds = 0;
            chestRect = new Rectangle(280, 130, 50, 50);
            keyRect = new Rectangle(490, 99, 109, 231);
            hasKey = false;
            hasOpenedChest = false;
            hasMap = false;
            mansion4SpeechUsed = false;
            canUseMapSpeech = false;
            hasOrb = false;
            hasUsedOrb = false;
            hasUsedOrbSpeech = false;
            speechManager = new SpeechManager(new Vector2(310, 20));

            eIndicatorRect = new Rectangle(580, 310, 54, 48);
            eIsVisible = false;
            keyIsVisible = false;

            menuInstructions = "Hello traveler! You are an adventurer\n" +
                               "who got cast away to the Haunted\n" +
                               "Mansion by the horrors of the Eldritch\n" +
                               "Forest, and you must find your way out!\n" +
                               "You must solve riddles and puzzles in\n" +
                               "order to escape. You better be smart or\n" +
                               "you will be stuck there for eternity!\n\n" +
                               "This game uses arrow keys or WASD for\n" +
                               "movement, you press E to interact with\n" +
                               "objects around the mansion, and I will\n" +
                               "help you out throughout your journey.\n" +
                               "Just press ENTER whenever I am talking\n" +
                               "to move onto the next part of my\n" +
                               "speech. Good luck!";

            riddle1Text1 = "The key to knowledge is\n" +
                           "hard to hold,\n\n" +
                           "Hidden within where\n" +
                           "stories are told.";
                           

            riddle1Text2 = "Look for the tome that\n" +
                           "feels misplaced,\n\n" +
                           "For its spine is marked\n" +
                           "but its words erased.";



            mansion1Speech1 = "Oh no! The Forest\n" +
                              "Monster banished us to\n" +
                              "the Haunted Mansion!\n";

            mansion1Speech2 = "We need to find a way\n" +
                              "out. Maybe check the\n" +
                              "attic?";

            mansion4Speech1 = "There must be something\n" +
                              "in here. Look around and\n" +
                              "see if you can find\n" +
                              "anything.";

            mapSpeech = "Ooh a map! That could\n" +
                        "be useful. Maybe it has\n" +
                        "hints on how we can get\n" +
                        "out! Check it out!";

            orbSpeech = "What is that? Maybe that\n" +
                        "could be our way out of\n" +
                        "here! See what it does!";


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
            chestArea = new Rectangle(294, 177, 22, 6);

            expectedBell = 1;
            bell1 = new Rectangle(720, 300, 25, 5);
            bell2 = new Rectangle(200, 200, 40, 8);
            bell3 = new Rectangle(510, 224, 20, 4);
            bell4 = new Rectangle(477, 222, 19, 9);
            orbRect = new Rectangle(431, 222, 20, 7);
            orbColour = Color.DarkGray * 0.7f;
            escapeRect = new Rectangle(230, 298, 26, 7);

            mansion1Location1 = new Vector2(254, 285);
            mansion1Location2 = new Vector2(518, 339);
            mansion2Location1 = new Vector2(20, 210);
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
            barriers4.Add(new Rectangle(280, 0, 50, 181));

            barriers5 = new List<Rectangle>();
            barriers5.Add(new Rectangle(0, 0, 800, 67));
            barriers5.Add(new Rectangle(0, 0, 555, 193));
            barriers5.Add(new Rectangle(661, 0, 139, 193));
            barriers5.Add(new Rectangle(696, 0, 104, 301));
            barriers5.Add(new Rectangle(766, 0, 34, 500));
            barriers5.Add(new Rectangle(0, 440, 800, 60));
            barriers5.Add(new Rectangle(0, 0, 34, 500));
            barriers5.Add(new Rectangle(0, 0, 452, 301));
            barriers5.Add(new Rectangle(139, 0, 68, 332));
            barriers5.Add(new Rectangle(279, 0, 68, 332));

            magicTextures = new List<Texture2D>();
            magicSeconds = 0f;
            magicIndex = 0;

            runTextures = new List<Texture2D>();
            runSeconds = 0f;
            runSpeed = new Vector2(-3, 0);
            runRect = new Rectangle(896, 380, 96, 96);
            runIndex = 0;

            idleIndex = 0;
            idleSeconds = 0f;
            idleTextures = new List<Texture2D>();

            base.Initialize();

            currentSong = spookySong;
            thunderInstance = thunderEffect.CreateInstance();
            cutsceneCharacter = new CutsceneCharacter(charIdleAnimation, charWalkAnimation, charTeleportAnimation, charRootAnimation, GraphicsDevice, new Vector2(1, 0), rootEffect, teleportEffect);
            cutsceneEnemy = new CutsceneEnemy(enemyIdleAnimation, enemyWalkAnimation, enemyAtkAnimation, enemySmnAnimation, GraphicsDevice, new Vector2(-1, 0), summonEffect, tensionEffect, biteEffect);
            mainCharacter = new MainCharacter(charIdleAnimation, charRunAnimation, GraphicsDevice, Vector2.Zero, mansion1Location1);
            cutsceneCharacter.SetAnimation("walk");
            playBtnRect = new Rectangle((window.Width / 2) - (playBtnTexture.Width / 2), 350, playBtnTexture.Width, playBtnTexture.Height);
            currentChestTexture = closedChestTexture;
            groundMapRect = new Rectangle(350, 186, groundMapTexture.Width, groundMapTexture.Height);
            mapBtnRect = new Rectangle(10, 10, mapBtnTexture.Width, mapBtnTexture.Height);
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
            collectEffect = Content.Load<SoundEffect>("Audio/collectEffect");
            chestEffect = Content.Load<SoundEffect>("Audio/chestEffect");
            tensionEffect = Content.Load<SoundEffect>("Audio/tensionEffect");
            biteEffect = Content.Load<SoundEffect>("Audio/biteEffect");
            openMapEffect = Content.Load<SoundEffect>("Audio/openMapEffect");
            closeMapEffect = Content.Load<SoundEffect>("Audio/closeMapEffect");
            bellEffect = Content.Load<SoundEffect>("Audio/bellEffect");
            winEffect = Content.Load<SoundEffect>("Audio/winEffect");
            breakEffect = Content.Load<SoundEffect>("Audio/breakEffect");


            // Backgrounds
            introTexture = Content.Load<Texture2D>("Backgrounds/hauntedIntro");
            menuTexture = Content.Load<Texture2D>("Backgrounds/menuScreen");
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
            audioBtnTexture = Content.Load<Texture2D>("Buttons/audioBtn");
            helpBtnTexture = Content.Load<Texture2D>("Buttons/helpBtn");

            // Images
            eIndicatorTexture = Content.Load<Texture2D>("Images/eIndicator");
            hauntedStairs = Content.Load<Texture2D>("Images/hauntedRoom2Stairs");
            hauntedRoom2Door = Content.Load<Texture2D>("Images/hauntedDoor2");
            book1Texture = Content.Load<Texture2D>("Images/book1");
            closeUpBook1Texture = Content.Load<Texture2D>("Images/closeBook1");
            hintBookTexture = Content.Load<Texture2D>("Images/hintBook");
            closedChestTexture = Content.Load<Texture2D>("Images/closeChest");
            openedChestTexture = Content.Load<Texture2D>("Images/openChest");
            keyTexture = Content.Load<Texture2D>("Images/chestKey");
            keyIndicatorTexture = Content.Load<Texture2D>("Images/keyIndicator");
            mapBtnTexture = Content.Load<Texture2D>("Images/mapBtn");
            groundMapTexture = Content.Load<Texture2D>("Images/mapOnGround");
            speechTexture = Content.Load<Texture2D>("Images/mainSpeech");
            bellTexture = Content.Load<Texture2D>("Images/bell");
            orbTexture = Content.Load<Texture2D>("Images/RealOrb");
            brokenWall1Texture = Content.Load<Texture2D>("Images/brokenWall1");
            brokenWall2Texture = Content.Load<Texture2D>("Images/brokenWall2");

            // Fonts
            titleFont = Content.Load<SpriteFont>("Fonts/pixelFont");
            hintFont = Content.Load<SpriteFont>("Fonts/hintFont");
            speechFont = Content.Load<SpriteFont>("Fonts/speechFont");

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
            magicSpritesheet = Content.Load<Texture2D>("Spritesheets/Magic Effects/Purple Aura");
            Rectangle sourceRect;


            int width = magicSpritesheet.Width / 8;
            int height = magicSpritesheet.Height;

            for (int y = 0; y < 1; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    sourceRect = new Rectangle(x * width, y * height, width, height);
                    cropTexture = new Texture2D(GraphicsDevice, width, height);
                    Color[] data = new Color[width * height];
                    magicSpritesheet.GetData(0, sourceRect, data, 0, data.Length);
                    cropTexture.SetData(data);
                    if (magicTextures.Count < 8)
                    {
                        magicTextures.Add(cropTexture);
                    }
                }
            }

            Rectangle sourceRect2;


            int width2 = charRunAnimation.Width / 6;
            int height2 = charRunAnimation.Height;

            for (int y = 0; y < 1; y++)
            {
                for (int x = 0; x < 6; x++)
                {
                    sourceRect2 = new Rectangle(x * width2, y * height2, width2, height2);
                    cropTexture2 = new Texture2D(GraphicsDevice, width2, height2);
                    Color[] data2 = new Color[width2 * height2];
                    charRunAnimation.GetData(0, sourceRect2, data2, 0, data2.Length);
                    cropTexture2.SetData(data2);
                    if (runTextures.Count < 6)
                    {
                        runTextures.Add(cropTexture2);
                    }
                }
            }

            Rectangle sourceRect3;


            int width3 = charIdleAnimation.Width / 4;
            int height3 = charIdleAnimation.Height;

            for (int y = 0; y < 1; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    sourceRect3 = new Rectangle(x * width3, y * height3, width3, height3);
                    cropTexture3 = new Texture2D(GraphicsDevice, width3, height3);
                    Color[] data3 = new Color[width3 * height3];
                    charIdleAnimation.GetData(0, sourceRect3, data3, 0, data3.Length);
                    cropTexture3.SetData(data3);
                    if (idleTextures.Count < 4)
                    {
                        idleTextures.Add(cropTexture3);
                    }
                }
            }
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
            speechManager.Update(keyboardState, prevKeyboardState);
            this.Window.Title = $"x = {mouseState.X}, y = {mouseState.Y}";

            if ((MediaPlayer.State == MediaState.Stopped) && playingMusic)
            {
                MediaPlayer.Play(currentSong);
            }

            if (screen == Screen.Intro)
            {
                // Play Button
                playBtnRect = new Rectangle((window.Width / 2) - (playBtnTexture.Width / 2), 350, playBtnTexture.Width, playBtnTexture.Height);

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

                // Audio Button
                audioBtnRect = new Rectangle(20, 380, 100, 100);

                if (audioBtnRect.Contains(mouseState.Position))
                {
                    audioBtnRect = new Rectangle(15, 375, 110, 110);
                    if (prevMouseState.LeftButton == ButtonState.Pressed)
                    {
                        audioBtnRect = new Rectangle(25, 385, 90, 90);
                        if (mouseState.LeftButton == ButtonState.Released)
                        {
                            if (playingMusic)
                            {
                                MediaPlayer.Pause();
                                playingMusic = false;
                                musicColour = Color.DarkGray;
                            }
                            else if (!playingMusic)
                            {
                                MediaPlayer.Resume();
                                playingMusic = true;
                                musicColour= Color.White;
                            }
                            audioBtnRect = new Rectangle(20, 380, 100, 100);
                        }
                    }
                }

                // Help Button
                helpBtnRect = new Rectangle(680, 380, 100, 100);

                if (helpBtnRect.Contains(mouseState.Position))
                {
                    helpBtnRect = new Rectangle(675, 375, 110, 110);
                    if (prevMouseState.LeftButton == ButtonState.Pressed)
                    {
                        helpBtnRect = new Rectangle(685, 385, 90, 90);
                        if (mouseState.LeftButton == ButtonState.Released)
                        {
                            screen = Screen.Menu;
                            helpBtnRect = new Rectangle(680, 380, 100, 100);
                        }
                    }
                }
            }
            else if (screen == Screen.Menu)
            {
                // Back Button
                backBtnRect = new Rectangle(580, 30, 100, 100);

                if (backBtnRect.Contains(mouseState.Position))
                {
                    backBtnRect = new Rectangle(575, 25, 110, 110);
                    if (prevMouseState.LeftButton == ButtonState.Pressed)
                    {
                        backBtnRect = new Rectangle(585, 35, 90, 90);
                        if (mouseState.LeftButton == ButtonState.Released)
                        {
                            screen = Screen.Intro;
                            backBtnRect = new Rectangle(580, 30, 100, 100);
                        }
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
                if (forestSeconds >= 6)
                {
                    MediaPlayer.Volume = 0.2f;
                }
                if (forestSeconds >= 19)
                {
                    currentSong = hauntedHouseSong;
                    if (playingMusic)
                        MediaPlayer.Play(currentSong);
                    screen = Screen.Mansion1;
                }
            }

            else if (screen == Screen.Mansion1)
            {
                mansionSeconds += (float)gameTime.ElapsedGameTime.TotalSeconds;
                mainCharacter.Update(gameTime, barriers1);
                currentScreen = Screen.Mansion1;

                if (!mainCharacter.HitBox.Intersects(mansion1Door) || !mainCharacter.HitBox.Intersects(bell2))
                    eIsVisible = false;

                // Speech
                if (mansionSeconds >= 4 && !speechManager.IsSpeechDone)
                {
                    if (!speechManager.IsSpeechVisible)
                    {
                        speechManager.StartSpeech(new List<string> { mansion1Speech1, mansion1Speech2 });
                    }
                }



                // Room 2
                if (mainCharacter.HitBox.Intersects(mansion1Door))
                {
                    eIndicatorRect = new Rectangle(580, 310, 54, 48);
                    eIsVisible = true;
                    if (keyboardState.IsKeyDown(Keys.E) && prevKeyboardState.IsKeyUp(Keys.E))
                    {
                        screen = Screen.Mansion2;
                        speechManager.EndSpeech();
                        mainCharacter.Location = mansion2Location1;
                        doorEffect.Play();
                    }
                }

                // Bell
                if (mainCharacter.HitBox.Intersects(bell2))
                {
                    eIndicatorRect = new Rectangle(217, 117, 27, 24);
                    eIsVisible = true;
                    if (keyboardState.IsKeyDown(Keys.E) && prevKeyboardState.IsKeyUp(Keys.E))
                    {
                        bellEffect.Play();
                        if (expectedBell == 2)
                        {
                            expectedBell++;
                        }
                        else if (expectedBell != 2 && !(expectedBell >= 5))
                        {
                            expectedBell = 1;
                        }
                    }
                }
            }

            else if (screen == Screen.Mansion2)
            {
                mainCharacter.Update(gameTime, barriers2);
                currentScreen = Screen.Mansion2;
                
                prevStep = currentStep;
                currentStep = step;

                if (mainCharacter.HitBox.Left < step1Rect.X)
                {
                    step = Step.Step1;
                }
                else if (mainCharacter.HitBox.Left < step2Rect.X && mainCharacter.HitBox.Left > step1Rect.X)
                {
                    step = Step.Step2;
                }
                else if (mainCharacter.HitBox.Left < step3Rect.X && mainCharacter.HitBox.Left > step2Rect.X)
                {
                    step = Step.Step3;
                }
                else
                {
                    step = Step.Step4;
                }

                if ((currentStep == Step.Step1 && prevStep == Step.Step2) || (currentStep == Step.Step2 && prevStep == Step.Step3) || (currentStep == Step.Step3 && prevStep == Step.Step4))
                {
                    mainCharacter.Y -= 7;
                }
                else if ((currentStep == Step.Step2 && prevStep == Step.Step1) || (currentStep == Step.Step3 && prevStep == Step.Step2) || (currentStep == Step.Step4 && prevStep == Step.Step3))
                {
                    mainCharacter.Y += 7;
                }

                if (mainCharacter.HitBox.Intersects(new Rectangle(383, 212, 417, 288)))
                {
                    orbColour = Color.White;
                }
                else
                {
                    orbColour = Color.DarkGray * 0.7f;
                }

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
                        speechManager.ResetSpeech();
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
                        if (!mansion4SpeechUsed)
                        {
                            speechManager.ResetSpeech();
                        }
                        
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

                if (!mainCharacter.HitBox.Intersects(mansion3Door) || !mainCharacter.HitBox.Intersects(bell3))
                    eIsVisible = false;

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
                        speechManager.EndSpeech();
                        if (expectedBell == 5)
                        {
                            hasUsedOrbSpeech = true;
                        }
                    }
                }

                // Bell
                if (mainCharacter.HitBox.Intersects(bell4))
                {
                    eIndicatorRect = new Rectangle(482, 82, 27, 24);
                    eIsVisible = true;
                    if (keyboardState.IsKeyDown(Keys.E) && prevKeyboardState.IsKeyUp(Keys.E))
                    {
                        bellEffect.Play();
                        if (expectedBell == 4)
                        {
                            expectedBell++;
                        }
                        else if (expectedBell != 4 && !(expectedBell >= 5))
                        {
                            expectedBell = 1;
                        }
                    }
                }

                if (expectedBell == 5 && !hasUsedOrbSpeech && !speechManager.IsSpeechDone)
                {
                    if (!speechManager.IsSpeechVisible)
                    {
                        speechManager.StartSpeech(new List<string> { orbSpeech });
                    }
                }
                
                if (expectedBell == 5 && !hasOrb && !hasUsedOrb)
                {
                    magicSeconds += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (magicSeconds >= 0.1f)
                    {
                        magicSeconds = 0;
                        magicIndex++;
                        if (magicIndex >= magicTextures.Count)
                        {
                            magicIndex = 0;
                        }
                    }
                    if (mainCharacter.HitBox.Intersects(orbRect))
                    {
                        eIndicatorRect = new Rectangle(448, 107, 27, 24);
                        eIsVisible = true;
                        if (keyboardState.IsKeyDown(Keys.E) && prevKeyboardState.IsKeyUp(Keys.E))
                        {
                            hasOrb = true;
                        }
                    }
                }
            }

            else if (screen == Screen.Mansion4)
            {
                mainCharacter.Update(gameTime, barriers4);
                currentScreen = Screen.Mansion4;
                speechSeconds4 += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (speechSeconds4 > 1 && !speechManager.IsSpeechDone && !mansion4SpeechUsed)
                {
                    speechManager.StartSpeech(new List<string> { mansion4Speech1 });
                }

                if (canUseMapSpeech && !speechManager.IsSpeechDone)
                {
                    speechManager.StartSpeech(new List<string> { mapSpeech });
                }

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
                        mansion4SpeechUsed = true;
                        speechManager.EndSpeech();
                    }
                }

                // Bell
                if (mainCharacter.HitBox.Intersects(bell3))
                {
                    eIndicatorRect = new Rectangle(519, 143, 27, 24);
                    eIsVisible = true;
                    if (keyboardState.IsKeyDown(Keys.E) && prevKeyboardState.IsKeyUp(Keys.E))
                    {
                        bellEffect.Play();
                        if (expectedBell == 3)
                        {
                            expectedBell++;
                        }
                        else if (expectedBell != 3 && !(expectedBell >= 5))
                        {
                            expectedBell = 1;
                        }
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
                    eIndicatorRect = new Rectangle(300, 95, 54, 48);
                    eIsVisible = true;
                    if (keyboardState.IsKeyDown(Keys.E) && prevKeyboardState.IsKeyUp(Keys.E))
                    {
                        currentChestTexture = openedChestTexture;
                        chestEffect.Play();
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
                        canUseMapSpeech = true;
                        speechManager.ResetSpeech();
                    }
                }

                

            }
            else if (screen == Screen.Mansion5)
            {
                mainCharacter.Update(gameTime, barriers5);
                currentScreen = Screen.Mansion5;
                if (mainCharacter.HitBox.Left < 450 && hasOrb)
                {
                    magicSeconds += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (magicSeconds >= 0.1f)
                    {
                        magicSeconds = 0;
                        magicIndex++;
                        if (magicIndex >= magicTextures.Count)
                        {
                            magicIndex = 0;
                        }
                    }
                }

                if (mainCharacter.HitBox.Bottom > 90)
                {
                    mainCharacter.Color = Color.DarkGray;
                }
                else
                {
                    mainCharacter.Color = Color.White;
                }

                if (!mainCharacter.HitBox.Intersects(mansion5Door) || !mainCharacter.HitBox.Intersects(bell1) || !mainCharacter.HitBox.Intersects(escapeRect))
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

                // Bell
                if (mainCharacter.HitBox.Intersects(bell1))
                {
                    eIndicatorRect = new Rectangle(730, 190, 27, 24);
                    eIsVisible = true;
                    if (keyboardState.IsKeyDown(Keys.E) && prevKeyboardState.IsKeyUp(Keys.E))
                    {
                        bellEffect.Play();
                        if (expectedBell == 1)
                        {
                            expectedBell++;
                        }
                        else if (expectedBell != 1 && !(expectedBell >= 5))
                        {
                            expectedBell = 1;
                        }
                    }
                }
                
                // Escape
                if (mainCharacter.HitBox.Intersects(escapeRect) && hasOrb && escape == Escape.Escape1)
                {
                    eIndicatorRect = new Rectangle(255, 200, 54, 48);
                    eIsVisible = true;
                    if (keyboardState.IsKeyDown(Keys.E) && prevKeyboardState.IsKeyDown(Keys.E))
                    {
                        escape = Escape.Escape2;
                        hasOrb = false;
                        hasUsedOrb = true;
                        breakEffect.Play();
                    }
                }

                if (escape == Escape.Escape2)
                {
                    escapeSeconds += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (escapeSeconds > 1.5f)
                    {
                        escape = Escape.Escape3;
                        breakEffect.Play();
                    }
                }

                if (escape == Escape.Escape3 && mainCharacter.HitBox.Intersects(escapeRect))
                {
                    eIndicatorRect = new Rectangle(250, 180, 54, 48);
                    eIsVisible = true;
                    if (keyboardState.IsKeyDown(Keys.E) && prevKeyboardState.IsKeyUp(Keys.E))
                    {
                        screen = Screen.End;
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
                if (!hasOpenedChest)
                {
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
            }

            else if (screen == Screen.Map)
            {
                // Back Button
                backBtnRect = new Rectangle(695, 7, 50, 50);

                if (backBtnRect.Contains(mouseState.Position))
                {
                    backBtnRect = new Rectangle(690, 2, 60, 60);
                    if (prevMouseState.LeftButton == ButtonState.Pressed)
                    {
                        backBtnRect = new Rectangle(700, 12, 40, 40);
                        if (mouseState.LeftButton == ButtonState.Released)
                        {
                            screen = currentScreen;
                            closeMapEffect.Play();
                            backBtnRect = new Rectangle(695, 7, 50, 50);
                        }
                    }
                }
            }

            else if (screen == Screen.End)
            {
                endSeconds += (float)gameTime.ElapsedGameTime.TotalSeconds;
                runSeconds += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (endSeconds > 2)
                {
                    runRect.X += (int)runSpeed.X;
                    runRect.Y += (int)runSpeed.Y;
                    if (runSeconds > 0.1f)
                    {
                        runIndex++;
                        runSeconds = 0;
                        if (runIndex >= runTextures.Count)
                        {
                            runIndex = 0;
                        }
                    }
                }
                if (runRect.Right < 0)
                {
                    screen = Screen.Win;
                    winEffect.Play();
                }
            }
            else if (screen == Screen.Win)
            {
                idleSeconds += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (idleSeconds > 0.1f)
                {
                    idleIndex++;
                    idleSeconds = 0;
                    if ( idleIndex >= idleTextures.Count)
                    {
                        idleIndex = 0;
                    }
                }
            }

            if ((screen == Screen.Mansion1 || screen == Screen.Mansion2 || screen == Screen.Mansion3 || screen == Screen.Mansion4 || screen == Screen.Mansion5) && hasMap)
            {
                mapBtnRect = new Rectangle(10, 10, mapBtnTexture.Width, mapBtnTexture.Height);

                if (mapBtnRect.Contains(mouseState.Position))
                {
                    mapBtnRect = new Rectangle(5, 5, mapBtnTexture.Width + 10, mapBtnTexture.Height + 10);
                    if (prevMouseState.LeftButton == ButtonState.Pressed)
                    {
                        mapBtnRect = new Rectangle(15, 15, mapBtnTexture.Width - 10, mapBtnTexture.Height - 10);
                        if (mouseState.LeftButton == ButtonState.Released)
                        {
                            screen = Screen.Map;
                            openMapEffect.Play();
                            mapBtnRect = new Rectangle(10, 10, mapBtnTexture.Width, mapBtnTexture.Height);
                        }
                    }
                }
            }

            if ((screen != Screen.Map && screen != Screen.Forest && screen != Screen.KeyBook && screen != Screen.Hint1) && playingMusic)
            {
                ResizeWindow(800, 500);
                MediaPlayer.Volume = 1;
            }
            else if ((screen == Screen.Map || screen == Screen.Hint1 || screen == Screen.KeyBook) && playingMusic)
            {
                MediaPlayer.Volume = 0.2f;
                if (screen == Screen.Map)
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
                _spriteBatch.Draw(audioBtnTexture, audioBtnRect, musicColour);
                _spriteBatch.Draw(helpBtnTexture, helpBtnRect, Color.White);
                
            }
            else if (screen == Screen.Menu)
            {
                _spriteBatch.Draw(introTexture, Vector2.Zero, Color.White);
                _spriteBatch.Draw(menuTexture, new Vector2((window.Width / 2) - (menuTexture.Width / 2), (window.Height / 2) - (menuTexture.Height / 2)), Color.White);
                _spriteBatch.Draw(backBtnTexture, backBtnRect, Color.White);
                _spriteBatch.DrawString(speechFont, menuInstructions, new Vector2(200, 105), Color.White);
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
                    _spriteBatch.Draw(bellTexture, new Vector2(208, 137), Color.White);
                    if (eIsVisible)
                    {
                        _spriteBatch.Draw(eIndicatorTexture, eIndicatorRect, Color.White);
                    }
                    speechManager.Draw(_spriteBatch, speechFont, speechTexture);
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
                _spriteBatch.Draw(bellTexture, new Vector2(475, 106), Color.White);
                if (eIsVisible)
                {
                    _spriteBatch.Draw(eIndicatorTexture, eIndicatorRect, Color.White);
                }
                if (expectedBell >= 5 && !hasOrb && !hasUsedOrb)
                {
                    _spriteBatch.Draw(magicTextures[magicIndex], new Vector2(411, 112), Color.White);
                    _spriteBatch.Draw(orbTexture, new Vector2(432, 134), Color.White);
                }
                speechManager.Draw(_spriteBatch, speechFont, speechTexture);
            }
            else if (screen == Screen.Mansion4)
            {
                _spriteBatch.Draw(blackTexture, Vector2.Zero, Color.White);
                _spriteBatch.Draw(mansion4Texture, new Vector2((window.Width / 2) - (mansion4Texture.Width / 2), (window.Height / 2) - (mansion4Texture.Height / 2)), null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
                _spriteBatch.Draw(hintBookTexture, new Vector2(517, 263), Color.White);
                _spriteBatch.Draw(currentChestTexture, chestRect, Color.White);
                _spriteBatch.Draw(bellTexture, new Vector2(511, 161), Color.White);
                if (hasOpenedChest && !hasMap)
                {
                    _spriteBatch.Draw(groundMapTexture, new Vector2(350, 186), Color.White);
                }
                mainCharacter.Draw(_spriteBatch);
                if (eIsVisible)
                {
                    _spriteBatch.Draw(eIndicatorTexture, eIndicatorRect, Color.White);
                }
                if (keyIsVisible)
                {
                    _spriteBatch.Draw(keyIndicatorTexture, new Vector2(300, 95), Color.White);
                }
                speechManager.Draw(_spriteBatch, speechFont, speechTexture);
            }
            else if (screen == Screen.Mansion5)
            {
                _spriteBatch.Draw(blackTexture, Vector2.Zero, Color.White);
                _spriteBatch.Draw(mansion5Texture, new Vector2((window.Width / 2) - (mansion5Texture.Width / 2), (window.Height / 2) - (mansion5Texture.Height / 2)), null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
                if (escape == Escape.Escape2)
                {
                    _spriteBatch.Draw(brokenWall1Texture, new Vector2(198, 213), Color.White);
                }
                if (escape == Escape.Escape3)
                {
                    _spriteBatch.Draw(brokenWall2Texture, new Vector2(175, 213), Color.White);
                }
                mainCharacter.Draw(_spriteBatch);
                _spriteBatch.Draw(bellTexture, new Vector2(723, 211), Color.DarkGray);
                if (eIsVisible)
                {
                    _spriteBatch.Draw(eIndicatorTexture, eIndicatorRect, Color.White);
                }
                if (mainCharacter.HitBox.X < 450 && hasOrb)
                {
                    _spriteBatch.Draw(magicTextures[magicIndex], new Rectangle((int)mainCharacter.Location.X - 30, (int)mainCharacter.Location.Y - 30, 65, 65), Color.White);
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
                if (!hasKey && !hasOpenedChest)
                    _spriteBatch.Draw(keyTexture, keyRect, Color.White);
            }
            else if (screen == Screen.Map)
            {
                _spriteBatch.Draw(fullMapTexture, Vector2.Zero, Color.White);
                _spriteBatch.Draw(backBtnTexture, backBtnRect, Color.White);
                _spriteBatch.DrawString(speechFont, "1", new Vector2(676, 646), Color.Red);
                _spriteBatch.DrawString(speechFont, "2", new Vector2(40, 184), Color.Red);
                _spriteBatch.DrawString(speechFont, "3", new Vector2(675, 116), Color.Red);
                _spriteBatch.DrawString(speechFont, "4", new Vector2(350, 60), Color.Red);
            }
            else if (screen == Screen.End)
            {
                _spriteBatch.Draw(forestTexture, Vector2.Zero, Color.White);
                _spriteBatch.Draw(runTextures[runIndex], runRect, null, Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
            }
            else if (screen == Screen.Win)
            {
                _spriteBatch.Draw(introTexture, Vector2.Zero, Color.White);
                _spriteBatch.Draw(idleTextures[idleIndex], new Vector2(352, 322), null, Color.White, 0, Vector2.Zero, 3, SpriteEffects.None, 0);
                _spriteBatch.DrawString(titleFont, "You ESCAPED!", new Vector2(20, 20), Color.ForestGreen, 0, Vector2.Zero, 0.75f, SpriteEffects.None, 0);
            }
            if (hasKey && (screen != Screen.KeyBook && screen != Screen.Hint1))
            {
                _spriteBatch.Draw(keyTexture, new Rectangle((int)mainCharacter.Location.X - 10, (int)mainCharacter.Location.Y - 10, 18, 38), Color.White);
            }
            if (hasOrb && (screen != Screen.KeyBook && screen != Screen.Hint1 && screen != Screen.Map))
            {
                _spriteBatch.Draw(orbTexture, new Rectangle((int)mainCharacter.Location.X - 10, (int)mainCharacter.Location.Y - 10, 25, 25), orbColour);
            }
            if ((screen == Screen.Mansion1 || screen == Screen.Mansion2 || screen == Screen.Mansion3 || screen == Screen.Mansion4 || screen == Screen.Mansion5) && hasMap)
            {
                _spriteBatch.Draw(mapBtnTexture, mapBtnRect, null, Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
