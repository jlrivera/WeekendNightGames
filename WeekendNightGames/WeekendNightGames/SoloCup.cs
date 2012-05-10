
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace WeekendNightGames
{
    class SoloCup
    {

        // Animation representing the player
        public Texture2D CupTexture;

        // Position of the Player relative to the upper left side of the screen
        public Vector2 Position;

        // State of the cup
        public bool Active;

        // Amount of cups left 

        public void Initialize(Texture2D texture, Vector2 position)
        {
            CupTexture = texture;

            // Set the starting position of the player around the middle of the screen and to the back
            Position = position;

            // Set the player to be active
            Active = true;


        } 

        public void Update()
        {
       
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(CupTexture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        } 
    }

}
