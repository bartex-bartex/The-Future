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

        public Vector2 Position;
        public Vector2 Velocity;
        public Rectangle PlayerAreaCenter
        {
            get
            {
                return new Rectangle((int)Position.X - playerTexture.Width/2, (int)Position.Y - playerTexture.Height/2, playerTexture.Width, playerTexture.Height);
            }
        }
        float speed = 5.0f;

        public Player(ContentManager content)
        {
            playerTexture = content.Load<Texture2D>("player");
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
            spriteBatch.Draw(playerTexture, Position, null, Color.White, 0, new Vector2(playerTexture.Width / 2, playerTexture.Height / 2), 1, SpriteEffects.None, 1);
        }

        public void UpdatePosition()
        {
            Position += Velocity;

            CheckMapBoundaries();

            Velocity = new Vector2(0, 0);
        }

        public void UpdateVelocity()
        {
            if(KeyboardManager.IsKeyPressed(Keys.Left))
            {
                Velocity.X -= speed;
            }

            if(KeyboardManager.IsKeyPressed(Keys.Right))
            {
                Velocity.X += speed;
            }

            if(KeyboardManager.IsKeyPressed(Keys.Up))
            {
                Velocity.Y -= speed;
            }

            if(KeyboardManager.IsKeyPressed(Keys.Down))
            {
                Velocity.Y += speed;
            }
        }

        public bool IsTouchingLeft(Rectangle objectRectangle)
        {
            return PlayerAreaCenter.Right + Velocity.X > objectRectangle.Left &&
              PlayerAreaCenter.Left < objectRectangle.Left &&
              PlayerAreaCenter.Bottom > objectRectangle.Top &&
              PlayerAreaCenter.Top < objectRectangle.Bottom;
        }

        public bool IsTouchingRight(Rectangle objectRectangle)
        {
            return PlayerAreaCenter.Left + Velocity.X < objectRectangle.Right &&
              PlayerAreaCenter.Right > objectRectangle.Right &&
              PlayerAreaCenter.Bottom > objectRectangle.Top &&
              PlayerAreaCenter.Top < objectRectangle.Bottom;
        }

        public bool IsTouchingTop(Rectangle objectRectangle)
        {
            return PlayerAreaCenter.Bottom + Velocity.Y > objectRectangle.Top &&
              PlayerAreaCenter.Top < objectRectangle.Top &&
              PlayerAreaCenter.Right > objectRectangle.Left &&
              PlayerAreaCenter.Left < objectRectangle.Right;
        }

        public bool IsTouchingBottom(Rectangle objectRectangle)
        {
            return PlayerAreaCenter.Top + Velocity.Y < objectRectangle.Bottom &&
              PlayerAreaCenter.Bottom > objectRectangle.Bottom &&
              PlayerAreaCenter.Right > objectRectangle.Left &&
              PlayerAreaCenter.Left < objectRectangle.Right;
        }

        public void ResolveStaticCollision(MapObject mapObject)
        {
            if ((Velocity.X > 0 && IsTouchingLeft(mapObject.Area)) ||
            (Velocity.X < 0 && IsTouchingRight(mapObject.Area)))
                Velocity.X = 0;

            if ((Velocity.Y > 0 && IsTouchingTop(mapObject.Area)) ||
                (Velocity.Y < 0 && IsTouchingBottom(mapObject.Area)))
                Velocity.Y = 0;
        }

        public void CheckMapBoundaries()
        {
            if (Position.X > GameMain.screenWidth)
                Position.X = GameMain.screenWidth;

            if (Position.X < 0)
                Position.X = 0;

            if (Position.Y > GameMain.screenHeight)
                Position.Y = GameMain.screenHeight;

            if (Position.Y < 0)
                Position.Y = 0;
        }
    }
}
