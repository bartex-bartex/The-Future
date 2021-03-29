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
        private DialogBox dialogBox;
        public SpriteFont DialogFont { get; private set; }
        public Vector2 ScreenCenter => new Vector2(screenWidth / 2, screenHeight / 2);

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


            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            DialogFont = Content.Load<SpriteFont>("Font_Arial");
            dialogBox = new DialogBox();
            //dialogBox.Initialize("Byly sobie kotki dwa ale to juz bylo i nie wroci wiecej i choc tyle sie zdazylo to juz nigdy fa;wknuejujujujujujujujujujujujujujujujujujujujuvuuuuuuuuuuuuuuuvuvuvuvuvuvuvuvuvuvuvuvuvuvuvuvuvuvuvuvuvuvuvuvuvuvuvuvuvuvuvucuuuuuuuuulouuuuuuuuuiuuuuuuuuuuuuuuuuujuuuuuuuuuuuuuuuujujujujujuuuuuuuuuuuuuuuuuujjjjjjjjjjjjjjjjjjjjjbrhfvllllllllllllllllllllllllllllllllll dfsaaaa sdfa ds asd asd fas afs asf sda fasd fasd fasd fasd fasf sad asd asdf asd fasd asd ftak nie stanie sie");


        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyboardManager.Update();
            
            player.UpdateVelocity();
            HandlePlayerCollision();
            player.UpdatePosition();

            dialogBox.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            map.DrawLevel(spriteBatch, player);
            player.Draw(spriteBatch);
            dialogBox.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void HandlePlayerCollision()
        {
            foreach (MapObject Object in map.MapObjects)
            {
                if (Object.IsCollidable == true)
                {
                    if (RectangleHelper.IsCollision(player.PlayerArea, Object.Area) == true)
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
