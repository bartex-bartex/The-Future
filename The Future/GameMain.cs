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

        public static readonly int desiredWidth = 800;
        public static readonly int desiredHeight = 480;

        public static float scale;
        Player player;
        Map map;
        public Matrix globalSpriteBatchMatrix;

        public GameMain()
        {
            graphics = new GraphicsDeviceManager(this);
            //graphics.IsFullScreen = true;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            screenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            screenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;

            graphics.ApplyChanges();

            scale = screenHeight / desiredHeight;

            Matrix scaleMatrix;
            Matrix translateMatrix;

            // w pełni zapełnionym wymiarem będzie ten, który jest najmniejszy
            if (screenWidth > screenHeight)
            {
                float scale = screenHeight / desiredHeight;
                scaleMatrix = Matrix.CreateScale(scale, scale, 1.0f);
                float translation = (screenWidth - (desiredWidth * scale)) / 2.0f;
                translateMatrix = Matrix.CreateTranslation(translation, 0.0f, 0.0f);
            }
            else
            {
                float scale = screenWidth / desiredWidth;
                scaleMatrix = Matrix.CreateScale(scale, scale, 1.0f);
                float translation = (screenHeight - (desiredHeight * scale)) / 2.0f;
                translateMatrix = Matrix.CreateTranslation(0.0f, translation, 0.0f);
            }

            // najpierw zeskalować, potem przenieść
            globalSpriteBatchMatrix = Matrix.Multiply(scaleMatrix, translateMatrix);

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
            dialogBox.Initialize(@"Całkowicie poglądowy *# dialog box. przez narodowe komitety olimpijskie, dlatego tez al di  fijnveari rgrs srdgs grgr sgrs grxgrdx grdxgrdxgdrx grdxgdrx gdxrxc fv gf nfd hgtrjhdg fhdfggj fdghb gfdjdfghbn vdfbgfd jdfgnbvdnyd hd rsgdgrd.");


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
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, globalSpriteBatchMatrix);

            map.DrawLevel(spriteBatch, player);
            player.Draw(spriteBatch);

            spriteBatch.End();

            dialogBox.Draw(GraphicsDevice);

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

                        else if (Object.IsDialogActive == true)
                        {
                            DialogManager.DisplayDialog(Object.DialogPath);
                        }
                    }
                }
            }
        }
    }
}
