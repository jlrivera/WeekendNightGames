using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
namespace WeekendNightGames 
{
    class QuartersGame : Microsoft.Xna.Framework.Game
    {

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Background textures for the game
        Texture2D powerBar;
        Texture2D pTablebackground;
        int currentPower = 100;
        int powerPhase = -1;
        bool takeShot = false;
        SoloCup shotglass;
        int xstart = 643;
        int ycupLocations = 260;
        Player shooter;
        float playerReticleSpeed;
        int xmax = 820;
        int xmin = 500;
        int ymin = 75;
        //Number that holds the cups made
        int score;
        int shotstaken;
        // The font used to display UI elements
        SpriteFont font;

        // The sound that is played when a laser is fired
        //SoundEffect shotSound;
        // The sound used when the cup is hit
        //SoundEffect sinkSound;
        // The music played during gameplay
        Song gameplayMusic;

        bool shotxset = false;
         public QuartersGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            this.graphics.PreferredBackBufferWidth = 1280; 
            this.graphics.PreferredBackBufferHeight = 720;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            
            
            shooter = new Player();
            //Set player's score to zero
            score = 0;
            shotglass = new SoloCup();

            playerReticleSpeed = 4f;
            // player reticle speed
            
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

            gameplayMusic = Content.Load<Song>("qmusic");

            // Load the laser and explosion sound effect
            //shotSound = Content.Load<SoundEffect>("takeshot");
            //sinkSound = Content.Load<SoundEffect>("shotmade");

            // Load the score font
            font = Content.Load<SpriteFont>("GameFont");
            // Start the music right away
            PlayMusic(gameplayMusic);



            // TODO: use this.Content to load your game content here

            //Load the screen backgrounds
            pTablebackground = Content.Load<Texture2D>("KTable");
            powerBar = Content.Load<Texture2D>("powerbar") as Texture2D;
                

            // Load the player resources 
            Vector2 playerPosition = new Vector2(620, 360);
            shooter.Initialize(Content.Load<Texture2D>("crosshair"), playerPosition); 
            //load cup
            Vector2 cupPosition = new Vector2(xstart,ycupLocations);
            shotglass.Initialize(Content.Load<Texture2D>("soloCup"), cupPosition); 

        }


        private void PlayMusic(Song song)
        {
            // Due to the way the MediaPlayer plays music,
            // we have to catch the exception. Music will play when the game is not tethered
            try
            {
                // Play the music
                MediaPlayer.Play(song);
                // Loop the currently playing song
                MediaPlayer.IsRepeating = true;
            }
            catch { }
        }
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        /// 

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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (GamePad.GetState(PlayerIndex.One).Buttons.X == ButtonState.Pressed && takeShot == false && shotxset)
            {
                    takeShot = true;
                    shoot();
            }

            else if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed && takeShot == false)
            {
                if (shotxset == false)
                {
                    shotxset = true;
                }
            }

             
          
            updatePower();
            UpdatePlayerX(gameTime);
            UpdateEnemies(gameTime);
            currentPower = (int)MathHelper.Clamp(currentPower, 0, 100);
            shooter.Position.X = (int) MathHelper.Clamp(shooter.Position.X, xmin, xmax);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            DrawTable();
            // Draw the score
            spriteBatch.DrawString(font, "Quarters Made: " + score, new Vector2(60, 20), Color.White);
            spriteBatch.DrawString(font, "Shots Taken: " + shotstaken, new Vector2(60, 60), Color.White);
            //Draw the negative space for the power bar
            spriteBatch.Draw(powerBar, new Rectangle(100, 100, powerBar.Width, powerBar.Height), new Rectangle(0, 45, 0, powerBar.Height), Color.Gray);

            //Draw the current power level
            spriteBatch.Draw(powerBar, new Rectangle(100, 100, powerBar.Width, (int)(powerBar.Height * ((double)currentPower / 100))), new Rectangle(0, 45, 0, 0), Color.Red);
            
            //Draw the box around the power bar
            spriteBatch.Draw(powerBar, new Rectangle(100, 100, powerBar.Width, powerBar.Height), new Rectangle(0, 0, 44, powerBar.Height), Color.White);
            shotglass.Draw(spriteBatch);
            shooter.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void shoot()
        {

            UpdateCollision();
            takeShot = false;
            shotxset = false;
            
            shotstaken++;
        }

        private void updatePower()
        {
            if (takeShot == true)
                return;

            if (powerPhase == -1)
                currentPower--;
            else if (powerPhase == 1)
                currentPower++;

            if (currentPower == 0 || currentPower == 100)
            {
                powerPhase = powerPhase * -1;
                currentPower += powerPhase;
            }
 
        }

        private void DrawTable()
        {
            //Draw all of the elements that are part of the Controller detect screen
            spriteBatch.Draw(pTablebackground, Vector2.Zero, Color.White);
        }

        private void UpdateEnemies(GameTime gameTime)
        {

        }

        private void UpdatePlayerX(GameTime gameTime)
        {
            if (shotxset)
                return;

            else
            {
                shooter.Position.X += playerReticleSpeed;
                if (shooter.Position.X == xmax || shooter.Position.X == xmin)
                    playerReticleSpeed = playerReticleSpeed * -1;
            }
        }


        private void UpdateCollision()
        {
            // Use the Rectangle's built-in intersect function to 
            // determine if two objects are overlapping
            Rectangle rectangle1;
            Rectangle rectangle2;

            // Only create the rectangle once for the player
            rectangle1 = new Rectangle((int)shooter.Position.X + 26, (int)shooter.Position.Y+ 26,8,8);

            // Do the collision between the player and the enemies
            rectangle2 = new Rectangle((int)shotglass.Position.X + 12, (int)shotglass.Position.Y + 46, 90, 110);
            // Determine if the two objects collided with each
            // other
            if (rectangle1.Intersects(rectangle2))
            {
                //our power is off from 100, based on the negative space
                if (currentPower > 20  && currentPower < 40)
                {
                    score++;
                    if (score == 15)
                        this.Exit();
                }
            }
        }


    }
}
