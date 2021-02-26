using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
            map = new Map("no path yet", Content, player);

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
            Vector2 currentVelocity = player.Velocity;
            Vector2 currentPosition = player.Position;
            Vector2 futurePosition = player.Position + currentVelocity;
            int playerWidth = player.playerTexture.Width;
            int playerHeight = player.playerTexture.Height;

            foreach (MapObject Object in map.MapObjects)
            {
                if (Object.IsCollidable == true)
                {
                    if (Object.Area.Intersects(new Rectangle((int)futurePosition.X, (int)futurePosition.Y,
                        playerWidth, playerHeight)) == true)
                    {
                        if(Object.CollisionFlag == CollisionFlag.Static)
                        {
                            //AABB resolve
                            if(currentPosition.X + playerWidth < Object.Area.X)
                            {
                                futurePosition = new Vector2(Object.Area.X - playerWidth, futurePosition.Y);
                            }
                            else if (currentPosition.X > Object.Area.X + Object.Area.Width)
                            {
                                futurePosition = new Vector2(Object.Area.X + Object.Area.Width, futurePosition.Y);
                            }
                            else if (currentPosition.Y + playerHeight < Object.Area.Y)
                            {
                                futurePosition = new Vector2(futurePosition.X, Object.Area.Y - playerHeight);
                            }
                            else if (currentPosition.Y > Object.Area.Y + Object.Area.Height)
                            {
                                futurePosition = new Vector2(futurePosition.X, Object.Area.Y + Object.Area.Height);
                            }
                        }

                        else if(Object.CollisionFlag == CollisionFlag.LevelChange)
                        {
                            map = new Map(Object.NextLevelPath, Content, player);
                        }
                    }
                }
            }
            player.SetPosition(futurePosition);
        }
    }
}
