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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Background textures for the various screens in the game
        Texture2D mControllerDetectScreenBackground;
        Texture2D mTitleScreenBackground;

        //Screen State variables to indicate what is the current screen
        bool mIsControllerDetectScreenShown;
        bool mIsTitleScreenShown;

        public Game1()
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
            // TODO: Add your initialization logic here
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

            // TODO: use this.Content to load your game content here

            //Load the screen backgrounds
            mControllerDetectScreenBackground = Content.Load<Texture2D>("8btitle");
            mTitleScreenBackground = Content.Load<Texture2D>("MainMenu");

            //Initialize the screen state variables
            mIsTitleScreenShown = false;
            mIsControllerDetectScreenShown = true;

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

            //Based on the screen state variables, call the            
            //Update method associated with the current screen            
            if (mIsControllerDetectScreenShown)            
            {                
                UpdateControllerDetectScreen();            
            }            
            else if (mIsTitleScreenShown)            
            {                
                UpdateTitleScreen();
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
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            // TODO: Add your drawing code here
            if (mIsControllerDetectScreenShown)
            {
                DrawControllerDetectScreen();
            }
            else if (mIsTitleScreenShown)
            {
                DrawTitleScreen();
            }

            spriteBatch.End();


            base.Draw(gameTime);
        }


        private void DrawControllerDetectScreen()
        {
            //Draw all of the elements that are part of the Controller detect screen
            spriteBatch.Draw(mControllerDetectScreenBackground, Vector2.Zero, Color.White);
        }

        private void DrawTitleScreen()
        {   //Draw all of the elements that are part of the Title screen
            spriteBatch.Draw(mTitleScreenBackground, Vector2.Zero, Color.White);
        }

        private void UpdateControllerDetectScreen()
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed)
                {
                    mIsTitleScreenShown = true;                    
                    mIsControllerDetectScreenShown = false;                    
                    return;                
                }            
        }

        private void UpdateTitleScreen()
        {
            //Move back to the Controller detect screen if the player moves
            //back (using B) from the Title screen
            if (GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed)
            {
                mIsTitleScreenShown = false;
                mIsControllerDetectScreenShown = true;
                return;
            }

           
        }

        private void StartAGame()
        {
            if (mIsTitleScreenShown == true && GamePad.GetState(PlayerIndex.One).Buttons.Y == ButtonState.Pressed)
            {
                using (QuartersGame qGame = new QuartersGame())
                {
                    qGame.Run();
                }
            }            

            {

                using (PongGame pgame = new PongGame())
                {
                    pgame.Run();
                }
            }

            if (mIsTitleScreenShown == true && GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed)
            {
                using (KingsGame kgame = new KingsGame())
                {
                    kgame.Run();
                }
            }
        }


    }
}
