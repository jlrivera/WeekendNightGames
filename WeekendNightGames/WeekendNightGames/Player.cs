
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace WeekendNightGames
{
    class Player
    {

        // Animation representing the player
        public Texture2D PlayerTexture;

        // Position of the Player relative to the upper left side of the screen
        public Vector2 Position;

        // State of the player
        public bool Active;


        public void Initialize(Texture2D texture, Vector2 position)
        {
            PlayerTexture = texture;

            // Set the starting position of the player around the middle of the screen and to the back
            Position = position;

            // Set the player to be active
            Active = true;
        } 

        public void Update()
        {
        }

        public int Height
        {
            get { return PlayerTexture.Height / 2; }
        }

        public int Width
        {
            get { return PlayerTexture.Width / 2; }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(PlayerTexture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        } 
    }

}
