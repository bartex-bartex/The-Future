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
        public Rectangle PlayerArea
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, playerTexture.Width, playerTexture.Height);
            }
        }
        float speed = 5.0f;

        public Player(ContentManager content)
        {
            playerTexture = content.Load<Texture2D>("player");
            Position += new Vector2(playerTexture.Width, playerTexture.Height);
        }

        /// <summary>
        /// Useful when player goes to the next map
        /// </summary>
        public void SetPosition(Vector2 position)
        {
            this.Position = position;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(playerTexture, Position, null, Color.White, 0, new Vector2(playerTexture.Width / 2, playerTexture.Height / 2), 1, SpriteEffects.None, 1);
            spriteBatch.Draw(playerTexture, Position, Color.White);
        }

        public void UpdatePosition()
        {
            Position += Velocity;

            //CheckMapBoundaries();

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
            return PlayerArea.Right + Velocity.X > objectRectangle.Left &&
              PlayerArea.Left + Velocity.X < objectRectangle.Left &&
              PlayerArea.Bottom > objectRectangle.Top &&
              PlayerArea.Top < objectRectangle.Bottom;
        }

        public bool IsTouchingRight(Rectangle objectRectangle)
        {
            return PlayerArea.Left + Velocity.X < objectRectangle.Right &&
              PlayerArea.Right + Velocity.X > objectRectangle.Right &&
              PlayerArea.Bottom > objectRectangle.Top &&
              PlayerArea.Top < objectRectangle.Bottom;
        }

        public bool IsTouchingTop(Rectangle objectRectangle)
        {
            return PlayerArea.Bottom + Velocity.Y > objectRectangle.Top &&
              PlayerArea.Top + Velocity.X < objectRectangle.Top &&
              PlayerArea.Right > objectRectangle.Left &&
              PlayerArea.Left < objectRectangle.Right;
        }

        public bool IsTouchingBottom(Rectangle objectRectangle)
        {
            return PlayerArea.Top + Velocity.Y < objectRectangle.Bottom &&
              PlayerArea.Bottom + Velocity.X > objectRectangle.Bottom &&
              PlayerArea.Right > objectRectangle.Left &&
              PlayerArea.Left < objectRectangle.Right;
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

        //public void CheckMapBoundaries()
        //{
        //    if (Position.X > GameMain.screenWidth)
        //        Position.X = GameMain.screenWidth;

        //    if (Position.X < 0)
        //        Position.X = 0;

        //    if (Position.Y > GameMain.screenHeight)
        //        Position.Y = GameMain.screenHeight;

        //    if (Position.Y < 0)
        //        Position.Y = 0;
        //}
    }
}
