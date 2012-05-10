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
    class PongGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        //Background textures for the game
        Texture2D powerBar;
        Texture2D pTablebackground;
        int currentPower = 100;
        int powerPhase = -1;
        bool takeShot = false;
        List<SoloCup> pyramid;
        int[] xstart= {545,610,675,740,578,643,698,610,675,643};
        int[] ycupLocations = {110,110,110,110,160,160,160,210,210,260};
        Player shooter;
        float playerReticleSpeed;
        int xmax = 820;
        int xmin = 500;
        int ymax = 330;
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
        bool shotyset = false;

         public PongGame()
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
            pyramid = new List<SoloCup>();

            playerReticleSpeed = 4f;
            // player reticle speed
            
            for (int i = 0; i < 10; i++)
            {
                pyramid.Add(new SoloCup());
            }
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

            gameplayMusic = Content.Load<Song>("gamemusic");

            // Load the laser and explosion sound effect
            //shotSound = Content.Load<SoundEffect>("takeshot");
            //sinkSound = Content.Load<SoundEffect>("shotmade");

            // Load the score font
            font = Content.Load<SpriteFont>("GameFont");
            // Start the music right away
            PlayMusic(gameplayMusic);



            // TODO: use this.Content to load your game content here

            //Load the screen backgrounds
            pTablebackground = Content.Load<Texture2D>("8bittable");
            powerBar = Content.Load<Texture2D>("powerbar") as Texture2D;


            // Load the player resources 
            Vector2 playerPosition = new Vector2(620, 185);
            shooter.Initialize(Content.Load<Texture2D>("crosshair"), playerPosition); 
            //load cups

            for (int i = 0; i < 10; i++)
            { 
                Vector2 cupPosition = new Vector2(xstart[i],ycupLocations[i]);
                pyramid[i].Initialize(Content.Load<Texture2D>("soloCup"), cupPosition); 
            }

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

            if (GamePad.GetState(PlayerIndex.One).Buttons.Y == ButtonState.Pressed && takeShot == false && shotxset && shotyset)
            {
                    takeShot = true;
                    shoot();
            }
            else if (GamePad.GetState(PlayerIndex.One).Buttons.X == ButtonState.Pressed && takeShot == false && shotxset)
            {
                        shotyset = true;
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
            UpdatePlayerY(gameTime);
            UpdateEnemies(gameTime);
            currentPower = (int)MathHelper.Clamp(currentPower, 0, 100);
            shooter.Position.X = (int) MathHelper.Clamp(shooter.Position.X, xmin, xmax);
            shooter.Position.Y = (int)MathHelper.Clamp(shooter.Position.Y, ymin, ymax);

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
            spriteBatch.DrawString(font, "Cups Made: " + score, new Vector2(60, 20), Color.White);
            spriteBatch.DrawString(font, "Shots Taken: " + shotstaken, new Vector2(60, 60), Color.White);
            //Draw the negative space for the power bar
            spriteBatch.Draw(powerBar, new Rectangle(100, 100, powerBar.Width, powerBar.Height), new Rectangle(0, 45, 0, powerBar.Height), Color.Gray);

            //Draw the current power level
            spriteBatch.Draw(powerBar, new Rectangle(100, 100, powerBar.Width, (int)(powerBar.Height * ((double)currentPower / 100))), new Rectangle(0, 45, 0, 0), Color.Blue);
            
            //Draw the box around the power bar
            spriteBatch.Draw(powerBar, new Rectangle(100, 100, powerBar.Width, powerBar.Height), new Rectangle(0, 0, 44, powerBar.Height), Color.White);
            foreach (SoloCup cup in pyramid) 
            { cup.Draw(spriteBatch); } 
            shooter.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void shoot()
        {

            UpdateCollision();
            takeShot = false;
            shotxset = false;
            shotyset = false;
            
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


            // Update the cups
            for (int i = pyramid.Count -1; i >= 0; i--)
            {
                if (pyramid[i].Active == false)
                {
                    pyramid.RemoveAt(i);
                }
            }
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

        private void UpdatePlayerY(GameTime gameTime)
        {
            if (shotyset)
                return;

            if (shotxset)
            {
                shooter.Position.Y += playerReticleSpeed;
                //System.Diagnostics.Debug.WriteLine(shooter.Position.Y);
                if (shooter.Position.Y < 85 || shooter.Position.Y > 315)
                {
                    playerReticleSpeed = playerReticleSpeed * -1;
                }
            }
            // Make sure that the player does not go out of bounds
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
            for (int i = 0; i < pyramid.Count; i++)
            {
                rectangle2 = new Rectangle((int)pyramid[i].Position.X + 10, (int)pyramid[i].Position.Y + 11, 40, 14);
                // Determine if the two objects collided with each
                // other
                if (rectangle1.Intersects(rectangle2))
                {
                    //our power is off from 100, based on the negative space
                    if (currentPower > 20  && currentPower < 40)
                    {
                        pyramid[i].Active = false;
                        score++;
                        if (score == 10)
                            this.Exit();
                    }
                }

            }
        }


    }
}
