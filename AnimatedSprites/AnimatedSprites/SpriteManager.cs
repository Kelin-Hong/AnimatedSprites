using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace AnimatedSprites
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class SpriteManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        // SpriteBatch for drawing
        SpriteBatch spriteBatch;

        // A sprite for the player and a list of automated sprites
         UserControlledSprite player;
         List<Sprite> spriteList = new List<Sprite>();

        // Variables for spawning new enemies
        int enemySpawnMinMilliseconds = 1000;
        int enemySpawnMaxMilliseconds = 2000;
        int enemyMinSpeed = 2;
        int enemyMaxSpeed = 6;
        int nextSpawnTime = 0;

        // Chance of spawning different enemies
        int likelihoodAutomated = 70;
        int likelihoodChasing = 20;
        int likelihoodEvading = 10;

        // Scoring
        int automatedSpritePointValue = 10;
        int chasingSpritePointValue = 20;
        int evadingSpritePointValue = 0;

        // Lives
         List<AutomatedSprite> livesList = new List<AutomatedSprite>();

        //Spawn time variables
        int nextSpawnTimeChange = 5000;
        int timeSinceLastSpawnTimeChange = 0;

        // Powerup stuff
        int powerUpExpiration = 0;

        //Sound
        //SoundEffectInstance sound1;
        //SoundEffectInstance sound2;
        //SoundEffectInstance sound3;
        //SoundEffectInstance sound4;
        //SoundEffectInstance sound5;
        public SpriteManager(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // Initialize spawn time
            ResetSpawnTime();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            //Loadsound
           //SoundEffect sound11= Game.Content.Load<SoundEffect>(@"Audio\boltcollision");
           //sound1 = sound11.CreateInstance();
           //SoundEffect sound22 = Game.Content.Load<SoundEffect>(@"Audio\fourbladescollision");
           //sound2 = sound22.CreateInstance();
           //SoundEffect sound33 = Game.Content.Load<SoundEffect>(@"Audio\pluscollision");
           //sound3 = sound33.CreateInstance();
           //SoundEffect sound44 = Game.Content.Load<SoundEffect>(@"Audio\skullcollision");
           //sound4 = sound44.CreateInstance();
           //SoundEffect sound55 = Game.Content.Load<SoundEffect>(@"Audio\threebladescollision");
           //sound5 = sound55.CreateInstance();
            player = new UserControlledSprite(
                Game.Content.Load<Texture2D>(@"Images/threerings"),
                new Vector2(GraphicsDevice.PresentationParameters.BackBufferWidth / 2,
                    GraphicsDevice.PresentationParameters.BackBufferHeight / 2),
                new Point(75, 75), 10, new Point(0, 0),
                new Point(6, 8), new Vector2(3, 3));


            // Load player lives list
            for (int i = 0; i < ((Game1)Game).NumberLivesRemaining; ++i)
            {
                int offset = 10 + i * 40;
                livesList.Add(new AutomatedSprite(
                    Game.Content.Load<Texture2D>(@"images\threerings"),
                    new Vector2(offset, 35), new Point(75, 75), 10,
                    new Point(0, 0), new Point(6, 8), Vector2.Zero,
                    null, 0, .5f));
            }

            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // Time to spawn enemy?
            
            nextSpawnTime -= gameTime.ElapsedGameTime.Milliseconds;
            if (nextSpawnTime < 0)
            {
                SpawnEnemy();

                // Reset spawn timer
                ResetSpawnTime();
            }

            UpdateSprites(gameTime);

            // Adjust sprite spawn times
            AdjustSpawnTimes(gameTime);

            // Expire Powerups?
            CheckPowerUpExpiration(gameTime);

            base.Update(gameTime);
        }

        public void reSetGame()
        {
            livesList.Clear();
            for (int i = 0; i < 3; ++i)
                             {
                                 int offset = 10 + i * 40;
                                 livesList.Add(new AutomatedSprite(
                                     Game.Content.Load<Texture2D>(@"images\threerings"),
                                     new Vector2(offset, 35), new Point(75, 75), 10,
                                     new Point(0, 0), new Point(6, 8), Vector2.Zero,
                                     null, 0, .5f));
                             }

             
             player = new UserControlledSprite(
             Game.Content.Load<Texture2D>(@"Images/threerings"),
             new Vector2(GraphicsDevice.PresentationParameters.BackBufferWidth / 2,
             GraphicsDevice.PresentationParameters.BackBufferHeight / 2),
             new Point(75, 75), 10, new Point(0, 0),
             new Point(6, 8), new Vector2(3, 3));
             spriteList.Clear();
             
             enemySpawnMinMilliseconds = 1000;
             enemySpawnMaxMilliseconds = 2000;
        }
        protected void UpdateSprites(GameTime gameTime)
        {
            // Update player
            player.Update(gameTime, GraphicsDevice.PresentationParameters.Bounds);

            // Update all non-player sprites
            for (int i = 0; i < spriteList.Count; ++i)
            {
                Sprite s = spriteList[i];

                s.Update(gameTime, GraphicsDevice.PresentationParameters.Bounds);

                // Check for collisions
                if (s.collisionRect.Intersects(player.collisionRect))
                {
                    // Play collision sound
                    //if (s.collisionCueName != null)
                    //    ((Game1)Game).PlayCue(s.collisionCueName);
                   
                    // If collided with AutomatedSprite
                    // remove a life from the player
                    if (s is AutomatedSprite)
                    {
                        // sound2.Play();
                        if (livesList.Count > 0)
                        {
                            livesList.RemoveAt(livesList.Count - 1);
                            --((Game1)Game).NumberLivesRemaining;
                        }
                    }
                    else if (s.collisionCueName == "pluscollision")
                    {
                        // Collided with plus - start plus power-up
                        //sound3.Play();
                        powerUpExpiration = 5000;
                        player.ModifyScale(2);
                    }
                    else if (s.collisionCueName == "skullcollision")
                    {
                        // Collided with skull - start skull power-up
                        //sound4.Play();
                        powerUpExpiration = 5000;
                        player.ModifySpeed(.5f);
                    }
                    else if (s.collisionCueName == "boltcollision")
                    {
                        // Collided with bolt - start bolt power-up
                       // sound1.Play();
                        powerUpExpiration = 5000;
                        player.ModifySpeed(2);
                        ((Game1)Game).AddScore(100);
                    }
                    else if (s.collisionCueName == "boltcollision1")
                    {
                        // Collided with bolt - start bolt power-up
                       // sound5.Play();
                        powerUpExpiration = 8000;
                        player.ModifyScale(0.6f);
                        ((Game1)Game).AddScore(500);
                        
                    }
                    else if (s.collisionCueName == "boltcollision2")
                    {
                        // Collided with bolt - start bolt power-up
                       // sound5.Play();
                        powerUpExpiration = 1000;
                       // player.ModifySpeed(2);
                        if (livesList.Count <8)
                        {
                            int offset = 10 + ((Game1)Game).NumberLivesRemaining*40;
                            livesList.Add(new AutomatedSprite(
                                Game.Content.Load<Texture2D>(@"images\threerings"),
                                new Vector2(offset, 35), new Point(75, 75), 10,
                                new Point(0, 0), new Point(6, 8), Vector2.Zero,
                                null, 0, .5f));
                            ++((Game1)Game).NumberLivesRemaining;
                            ((Game1)Game).AddScore(200);
                        }
                    }

                    // Remove collided sprite from the game
                    spriteList.RemoveAt(i);
                    --i;
                }

                // Remove object if it is out of bounds
                if (s.IsOutOfBounds(GraphicsDevice.PresentationParameters.Bounds))
                {
                    ((Game1)Game).AddScore(spriteList[i].scoreValue);
                    spriteList.RemoveAt(i);
                    --i;
                }

            }

            // Update lives-list sprites
            foreach (Sprite sprite in livesList)
                sprite.Update(gameTime, Game.Window.ClientBounds);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

            // Draw the player
            player.Draw(gameTime, spriteBatch);

            // Draw all sprites
            foreach (Sprite s in spriteList)
                s.Draw(gameTime, spriteBatch);

            // Draw player lives
            foreach (Sprite sprite in livesList)
                sprite.Draw(gameTime, spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void ResetSpawnTime()
        {
            // Set the next spawn time for an enemy
            nextSpawnTime = ((Game1)Game).rnd.Next(
                enemySpawnMinMilliseconds,
                enemySpawnMaxMilliseconds);
        }

        private void SpawnEnemy()
        {
            Vector2 speed = Vector2.Zero;
            Vector2 position = Vector2.Zero;

            // Default frame size
            Point frameSize = new Point(75, 75);

            // Randomly choose which side of the screen to place enemy,
            // then randomly create a position along that side of the screen
            // and randomly choose a speed for the enemy
            switch (((Game1)Game).rnd.Next(4))
            {
                case 0: // LEFT to RIGHT
                    position = new Vector2(
                        -frameSize.X, ((Game1)Game).rnd.Next(0,
                        Game.GraphicsDevice.PresentationParameters.BackBufferHeight
                        - frameSize.Y));
                    speed = new Vector2(((Game1)Game).rnd.Next(
                        enemyMinSpeed,
                        enemyMaxSpeed), 0);
                    break;
                case 1: // RIGHT to LEFT
                    position = new
                        Vector2(
                        Game.GraphicsDevice.PresentationParameters.BackBufferWidth,
                        ((Game1)Game).rnd.Next(0,
                        Game.GraphicsDevice.PresentationParameters.BackBufferHeight
                        - frameSize.Y));

                    speed = new Vector2(-((Game1)Game).rnd.Next(
                        enemyMinSpeed, enemyMaxSpeed), 0);
                    break;
                case 2: // BOTTOM to TOP
                    position = new Vector2(((Game1)Game).rnd.Next(0,
                    Game.GraphicsDevice.PresentationParameters.BackBufferWidth
                        - frameSize.X),
                        Game.GraphicsDevice.PresentationParameters.BackBufferHeight);

                    speed = new Vector2(0,
                        -((Game1)Game).rnd.Next(enemyMinSpeed,
                        enemyMaxSpeed));
                    break;
                case 3: // TOP to BOTTOM
                    position = new Vector2(((Game1)Game).rnd.Next(0,
                        Game.GraphicsDevice.PresentationParameters.BackBufferWidth
                        - frameSize.X), -frameSize.Y);

                    speed = new Vector2(0,
                        ((Game1)Game).rnd.Next(enemyMinSpeed,
                        enemyMaxSpeed));
                    break;
            }

            // Get random number between 0 and 99
            int random = ((Game1)Game).rnd.Next(100);
            if (random < likelihoodAutomated)
            {
                // Create an AutomatedSprite.
                // Get new random number to determine whether to
                // create a three-blade or four-blade sprite.
                if (((Game1)Game).rnd.Next(2) == 0)
                {
                    // Create a four-blade enemy
                    spriteList.Add(
                    new AutomatedSprite(
                        Game.Content.Load<Texture2D>(@"images\fourblades"),
                        position, new Point(75, 75), 10, new Point(0, 0),
                        new Point(6, 8), speed, "fourbladescollision",
                        automatedSpritePointValue));
                 
                }
                else
                {
                    // Create a three-blade enemy
                    spriteList.Add(
                    new AutomatedSprite(
                        Game.Content.Load<Texture2D>(@"images\threeblades"),
                        position, new Point(75, 75), 10, new Point(0, 0),
                        new Point(6, 8), speed, "threebladescollision",
                        automatedSpritePointValue));
                }
            }
            else if (random < likelihoodAutomated +
            likelihoodChasing)
            {
                // Create a ChasingSprite.
                // Get new random number to determine whether
                // to create a skull or a plus sprite.
                if (((Game1)Game).rnd.Next(2) == 0)
                {
                    // Create a skull
                    spriteList.Add(
                    new ChasingSprite(
                        Game.Content.Load<Texture2D>(@"images\skullball"),
                        position, new Point(75, 75), 10, new Point(0, 0),
                        new Point(6, 8), speed, "skullcollision", this,
                        chasingSpritePointValue));
                }
                else
                {
                    // Create a plus
                    spriteList.Add(
                    new ChasingSprite(
                        Game.Content.Load<Texture2D>(@"images\plus"),
                        position, new Point(75, 75), 10, new Point(0, 0),
                        new Point(6, 4), speed, "pluscollision", this,
                        chasingSpritePointValue));
                }
            }
            else
            {
                // Create an EvadingSprite
                if (((Game1)Game).rnd.Next(4) == 0)
                {
                    if (((Game1)Game).rnd.Next(2) == 0)
                    {
                        EvadingSprite es = new EvadingSprite(Game.Content.Load<Texture2D>(@"images\bluehead"),
                          position, new Point(45, 90), 10, new Point(0, 0),
                          new Point(3, 4), speed, 100, "boltcollision1", this,
                          .75f, 60, evadingSpritePointValue);
                        spriteList.Add(es);
                    }
                    else
                    {
                        EvadingSprite es = new EvadingSprite(Game.Content.Load<Texture2D>(@"images\redhead"),
                        position, new Point(45, 95), 10, new Point(0, 0),
                        new Point(3, 4), speed, 100, "boltcollision2", this,
                        .75f, 60, evadingSpritePointValue);
                        spriteList.Add(es);
                    }
                }
                else
                {
                   // spriteList.Add(
                   //new EvadingSprite(
                   //Game.Content.Load<Texture2D>(@"images\bluehead"),
                   //position, new Point(45, 90), 10, new Point(0, 0),
                   //new Point(3, 4), speed, 100, "threebladescollision",
                   //automatedSpritePointValue));
                  
                    spriteList.Add(
                    new EvadingSprite(
                        Game.Content.Load<Texture2D>(@"images\bolt"),
                        position, new Point(75, 75), 10, new Point(0, 0),
                        new Point(6, 8), speed * 0.9f, "boltcollision", this,
                        .75f, 40, evadingSpritePointValue));
                    
                }
            }
        }

        // Return current position of the player sprite
        public Vector2 GetPlayerPosition()
        {
            return player.GetPosition;
        }

        protected void AdjustSpawnTimes(GameTime gameTime)
        {
            // If the spawn max time is > 500 milliseconds
            // decrease the spawn time if it is time to do
            // so based on the spawn-timer variables
            if (enemySpawnMaxMilliseconds > 500)
            {
                timeSinceLastSpawnTimeChange += gameTime.ElapsedGameTime.Milliseconds;
                if (timeSinceLastSpawnTimeChange > nextSpawnTimeChange)
                {
                    timeSinceLastSpawnTimeChange -= nextSpawnTimeChange;
                    if (enemySpawnMaxMilliseconds > 1000)
                    {
                        enemySpawnMaxMilliseconds -= 100;
                        enemySpawnMinMilliseconds -= 100;
                    }
                    else
                    {
                        enemySpawnMaxMilliseconds -= 10;
                        enemySpawnMinMilliseconds -= 10;
                    }
                }
            }
        }

        protected void CheckPowerUpExpiration(GameTime gameTime)
        {
            // Is a power-up active?
            if (powerUpExpiration > 0)
            {
                // Decrement power-up timer
                powerUpExpiration -= gameTime.ElapsedGameTime.Milliseconds;
                if (powerUpExpiration <= 0)
                {
                    // If power-up timer has expired, end all power-ups
                    powerUpExpiration = 0;
                    player.ResetScale();
                    player.ResetSpeed();
                }
            }
        }
    }
}
