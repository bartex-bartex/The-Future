using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace The_Future
{
    public class Player
    {
        public Texture2D playerTexture;

        private Vector2 position;
        public Vector2 Position { get { return velocity; } private set { velocity = value; } }
        private Vector2 velocity;
        public Vector2 Velocity { get { return velocity; } private set { velocity = value; } }
        float speed = 5.0f;

        public Player(ContentManager content)
        {
            playerTexture = content.Load<Texture2D>("sprite");
        }

        /// <summary>
        /// If player goes to next map
        /// </summary>
        public void SetPosition(Vector2 position)
        {
            this.Position = position;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(playerTexture, position, null, Color.White, 0, new Vector2(playerTexture.Width / 2, playerTexture.Height / 2), 1, SpriteEffects.None, 1);
        }

        public void UpdatePosition()
        {
            //UpdateVelocity();

            position += velocity;

            velocity = new Vector2(0, 0);
        }

        public void UpdateVelocity()
        {
            if(KeyboardManager.IsKeyPressed(Keys.Left))
            {
                velocity.X -= speed;
            }

            if(KeyboardManager.IsKeyPressed(Keys.Right))
            {
                velocity.X += speed;
            }

            if(KeyboardManager.IsKeyPressed(Keys.Up))
            {
                velocity.Y -= speed;
            }

            if(KeyboardManager.IsKeyPressed(Keys.Down))
            {
                velocity.Y += speed;
            }
        }

        //public void UpdateAnimation()
        //{

        //}
    }
}
