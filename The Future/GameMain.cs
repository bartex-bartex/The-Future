using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;

namespace The_Future
{
    public class GameMain : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public static int screenHeight;
        public static int screenWidth;
        Player player;
        Map map;

        public GameMain()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            screenWidth = graphics.PreferredBackBufferWidth;
            screenHeight = graphics.PreferredBackBufferHeight;

            Window.Title = "The Future: Time Escaper";

            player = new Player(Content);
            map = new Map(@"../../../Content/map1.txt", Content, player);
            var path = Directory.GetCurrentDirectory();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyboardManager.Update();
            
            player.UpdateVelocity();
            HandlePlayerCollision();
            player.UpdatePosition();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            map.DrawLevel(spriteBatch, player);
            player.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void HandlePlayerCollision()
        {
            foreach (MapObject Object in map.MapObjects)
            {
                if (Object.IsCollidable == true)
                {
                    if (RectangleHelper.IsCollision(player.PlayerAreaCenter, Object.Area) == true)
                    {
                        if (Object.IsCollisionResponseStatic == true)
                        {
                            player.ResolveStaticCollision(Object);
                        }

                        else if (Object.IsLevelChange == true)
                        {
                            map = new Map(Object.NextLevelPath, Content, player);
                        }
                    }
                }
            }
        }
    }
}
