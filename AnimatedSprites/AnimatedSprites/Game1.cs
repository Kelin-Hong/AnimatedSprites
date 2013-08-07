using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

namespace AnimatedSprites
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteManager spriteManager;
        
        //SoundEffectInstance startSound;
        //SoundEffectInstance trackSound;
        //AudioEngine audioEngine;
        //WaveBank waveBank;
        //SoundBank soundBank;
        //Cue trackCue;

        // Random number generator
        public Random rnd { get; private set; }

        // Score stuff
        int currentScore = 0;
        SpriteFont scoreFont;

        // Background
        Texture2D backgroundTexture;

        // Game states
        enum GameState { Start, InGame, GameOver };
        GameState currentGameState = GameState.Start;

        // Lives remaining
        int numberLivesRemaining = 3;

        List<AutomatedSprite> spritesList1 = new List<AutomatedSprite>();
        public int NumberLivesRemaining
        {
            get { return numberLivesRemaining; }
            set
            {
                numberLivesRemaining = value;
                if (numberLivesRemaining == 0)
                {
                    currentGameState = GameState.GameOver;
                    spriteManager.Enabled = false;
                    spriteManager.Visible = false;
                }
            }
        }
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            rnd = new Random();
            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            // Extend battery life under lock.
            InactiveSleepTime = TimeSpan.FromSeconds(1);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            spriteManager = new SpriteManager(this);
            Components.Add(spriteManager);
            spriteManager.Enabled = false;
            spriteManager.Visible = false;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            scoreFont = Content.Load<SpriteFont>(@"fonts\SpriteFont");
            //soundLoad
            //SoundEffect start= Content.Load<SoundEffect>(@"Audio\start");
            //SoundEffect track= Content.Load<SoundEffect>(@"Audio\track");
            //startSound = start.CreateInstance();
            //trackSound = track.CreateInstance();
            // Load the background
            backgroundTexture = Content.Load<Texture2D>(@"Images\background1");
            // TODO: use this.Content to load your game content here
            spritesList1.Add(new AutomatedSprite(
                   Content.Load<Texture2D>(@"images\bluehead"),
                   new Vector2(150 - 50, 350 - 50), new Point(45, 90), 10,
                   new Point(0, 0), new Point(3, 4), Vector2.Zero,
                   null, 0, .5f));
            spritesList1.Add(new AutomatedSprite(
                   Content.Load<Texture2D>(@"images\redhead"),
                   new Vector2(250 - 50, 350 - 50), new Point(45, 90), 10,
                   new Point(0, 0), new Point(3,4), Vector2.Zero,
                   null, 0, .5f));
            spritesList1.Add(new AutomatedSprite(
                   Content.Load<Texture2D>(@"images\fourblades"),
                   new Vector2(350 - 50, 350 - 50), new Point(75, 75), 10,
                   new Point(0, 0), new Point(6, 8), Vector2.Zero,
                   null, 0, .3f));
            spritesList1.Add(new AutomatedSprite(
                   Content.Load<Texture2D>(@"images\threeblades"),
                   new Vector2(450 - 50, 350 - 50), new Point(75, 75), 10,
                   new Point(0, 0), new Point(6, 8), Vector2.Zero,
                   null, 0, .3f));
            spritesList1.Add(new AutomatedSprite(
                   Content.Load<Texture2D>(@"images\bolt"),
                   new Vector2(550 - 50, 350 - 50), new Point(75, 75), 10,
                   new Point(0, 0), new Point(6, 8), Vector2.Zero,
                   null, 0, .3f));
            spritesList1.Add(new AutomatedSprite(
                 Content.Load<Texture2D>(@"images\plus"),
                 new Vector2(650 - 50, 350 - 50), new Point(75, 75), 10,
                 new Point(0, 0), new Point(6, 8), Vector2.Zero,
                 null, 0, .3f));
            spritesList1.Add(new AutomatedSprite(
                 Content.Load<Texture2D>(@"images\skullball"),
                 new Vector2(750 - 50, 350 - 50), new Point(75, 75), 10,
                 new Point(0, 0), new Point(6, 8), Vector2.Zero,
                 null, 0, .3f));
            
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            //    this.Exit();
            switch (currentGameState)
            {
                case GameState.Start:
                   // startSound.Play();
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                    this.Exit();
                    TouchCollection touchLocations = TouchPanel.GetState(); 
                    foreach (TouchLocation touchLocation in touchLocations)
                     {
                         if (touchLocation.State == TouchLocationState.Pressed)
                         {
                             currentGameState = GameState.InGame;
                             spriteManager.Enabled = true;
                             spriteManager.Visible = true;
                             break;
                         }
                      //{
                      //Vector2 touchPosition = touchLocation.Position;
                      // if (touchPosition.X >= textPosition.X &&
                      // touchPosition.X < textPosition.X + textSize.X &&
                      // touchPosition.Y >= textPosition.Y &&
                      // touchPosition.Y < textPosition.Y + textSize.Y)
                      //    {
                      //    textColor = new Color((byte)rand.Next(256),
                      //    (byte)rand.Next(256),
                      //    (byte)rand.Next(256));
                      //     }
                      //  }
                    }
                    //if (Keyboard.GetState().GetPressedKeys().Length > 0)
                    //{
                    //    currentGameState = GameState.InGame;
                    //    spriteManager.Enabled = true;
                    //    spriteManager.Visible = true;
                    //}
                    break;
                case GameState.InGame:
                    {
                       // startSound.Stop();
                       //trackSound.Play();
                        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                        {
                            currentGameState = GameState.Start;
                            spriteManager.reSetGame();
                            spriteManager.Enabled = false;
                            spriteManager.Visible = false;
                            NumberLivesRemaining = 3;
                            currentScore = 0;
                            break;
                        }
                        currentGameState = GameState.InGame;
                        spriteManager.Enabled = true;
                        spriteManager.Visible = true;
                      
                    }
                    break;
                case GameState.GameOver:
                    //if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    //    Exit();
                     TouchCollection touchLocationss = TouchPanel.GetState();
                     foreach (TouchLocation touchLocation in touchLocationss)
                     {
                         if (touchLocation.State == TouchLocationState.Pressed)
                         {
                             currentGameState = GameState.Start;
                             spriteManager.reSetGame();
                             spriteManager.Enabled = false;
                             spriteManager.Visible = false;
                             NumberLivesRemaining = 3;
                             currentScore = 0;
                             break;
                         }
                     }
                     if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                     {
                         currentGameState = GameState.Start;
                         spriteManager.reSetGame();
                         spriteManager.Enabled = false;
                         spriteManager.Visible = false;
                         NumberLivesRemaining = 3;
                         currentScore = 0;
                         break;
                     }
                    break;
            }
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            switch (currentGameState)
            {
                case GameState.Start:
                    GraphicsDevice.Clear(Color.MediumSlateBlue);

                    // Draw text for intro splash screen
                    spriteBatch.Begin();
                    string text = "��Ϸ˵��";
                    spriteBatch.DrawString(scoreFont, text,
                        new Vector2((GraphicsDevice.PresentationParameters.BackBufferWidth / 2)
                        - (scoreFont.MeasureString(text).X / 2),
                        (GraphicsDevice.PresentationParameters.BackBufferHeight /8 )
                        - (scoreFont.MeasureString(text).Y / 2)),
                        Color.SaddleBrown);

                    text = "��Ϸ�����ͨ��������Ӧ���ƾ�����ͬ�ķ����ƶ�\n"
                            +"���˻������˷�������ҵ���Ҫ�����Ƕ��������\n"
                            +"��˷��ĵ��˺ͳԵ����Լ��������Ʒ�����в�ͬ��\n"
                            +"�࣬�еĻ������Ѫ���еĻ�������٣�����������\n"
                            +"��ܵ��ˣ��еĻ����������������������������\n"
                            +"����Щ���˻�׷����ҪС��Ŷ������Ч�����ۼӻ���\n"
                            +"һ����Ʒ���������������ƶ��ٶȻ��Ѫ�������ڿ�\n"
                            +"����ʱ��������㾡���ܵĶ�ܵ��˻�ø��ߵķ֡�\n";
                    spriteBatch.DrawString(scoreFont, text,
                        new Vector2((GraphicsDevice.PresentationParameters.BackBufferWidth / 2)
                        - (scoreFont.MeasureString(text).X / 2),
                        (GraphicsDevice.PresentationParameters.BackBufferHeight / 2)
                        - (scoreFont.MeasureString(text).Y / 2)-50),
                        Color.SaddleBrown);
                   
                    foreach (Sprite item in spritesList1)
                    {
                       // spriteBatch.Draw(item,new Vector2(100 + i * 30,350),Color.Wheat);
                        item.Draw(gameTime, spriteBatch);
                    }
                    text = "  �ӷ�500+��С �ӷ�200+���� ��Ѫ    ��Ѫ  ����+�ӷ�100  ���     ����";
                    spriteBatch.DrawString(scoreFont, text,
                       new Vector2((GraphicsDevice.PresentationParameters.BackBufferWidth / 2-40)
                       - (scoreFont.MeasureString(text).X / 2),
                      360- (scoreFont.MeasureString(text).Y / 2)),
                       Color.SaddleBrown);
                    text = "�����Ļ��ʼ";
                    spriteBatch.DrawString(scoreFont, text,
                        new Vector2((GraphicsDevice.PresentationParameters.BackBufferWidth / 2)
                        - (scoreFont.MeasureString(text).X / 2),
                       400
                        - (scoreFont.MeasureString(text).Y / 2)),
                        Color.SaddleBrown);
                    spriteBatch.End();
                    break;

                case GameState.InGame:
                    GraphicsDevice.Clear(Color.Wheat);
                    spriteBatch.Begin();

                    // Draw background image
                    spriteBatch.Draw(backgroundTexture,
                        new Rectangle(0, 0, GraphicsDevice.PresentationParameters.BackBufferWidth,
                         GraphicsDevice.PresentationParameters.BackBufferHeight), null,
                        Color.White, 0, Vector2.Zero,
                        SpriteEffects.None, 0);

                    // Draw fonts
                    spriteBatch.DrawString(scoreFont,
                        "�÷�: " + currentScore,
                        new Vector2(10, 10), Color.DarkBlue,
                        0, Vector2.Zero,
                        1, SpriteEffects.None, 1);

                    spriteBatch.End();
                    break;

                case GameState.GameOver:
                    GraphicsDevice.Clear(Color.MediumSlateBlue);

                    spriteBatch.Begin();
                    string gameover = "��Ϸʧ��!";
                    spriteBatch.DrawString(scoreFont, gameover,
                        new Vector2((GraphicsDevice.PresentationParameters.BackBufferWidth / 2)
                        - (scoreFont.MeasureString(gameover).X / 2),
                        (GraphicsDevice.PresentationParameters.BackBufferHeight / 2)
                        - (scoreFont.MeasureString(gameover).Y / 2)),
                        Color.SaddleBrown);

                    gameover = "�÷�: " + currentScore;
                    spriteBatch.DrawString(scoreFont, gameover,
                        new Vector2((GraphicsDevice.PresentationParameters.BackBufferWidth / 2)
                        - (scoreFont.MeasureString(gameover).X / 2),
                        (GraphicsDevice.PresentationParameters.BackBufferHeight / 2)
                        - (scoreFont.MeasureString(gameover).Y / 2) + 30),
                        Color.SaddleBrown);

                    gameover = "�����Ļ������Ϸ";
                    spriteBatch.DrawString(scoreFont, gameover,
                        new Vector2((GraphicsDevice.PresentationParameters.BackBufferWidth / 2)
                        - (scoreFont.MeasureString(gameover).X / 2),
                        (GraphicsDevice.PresentationParameters.BackBufferHeight / 2)
                        - (scoreFont.MeasureString(gameover).Y / 2) + 60),
                        Color.SaddleBrown);

                    spriteBatch.End();
                    break;
            }
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
        public void AddScore(int score)
        {
            currentScore += score;
        }
    }
}
