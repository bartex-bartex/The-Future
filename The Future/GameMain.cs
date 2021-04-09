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
        public SpriteFont CharacterNameFont { get; private set; }
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

        public static bool IsPlayerMovementBlock = false;

        private bool IsInTerminalArea;
        private Terminal terminal;
        private bool IsInExerciseArea;
        private Exercise exercise;

        private bool IsHelloPlayerActive;
        Texture2D introduce;

        public GameMain()
        {
            graphics = new GraphicsDeviceManager(this);
            //graphics.IsFullScreen = true;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Window.Title = "The Future: Time Escaper";

            screenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            screenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;

            graphics.ApplyChanges();

            scale = (float)screenHeight / (float)desiredHeight;

            Matrix scaleMatrix;
            Matrix translateMatrix;

            if (screenWidth > screenHeight)
            {
                float scale = (float)screenHeight / (float)desiredHeight;
                scaleMatrix = Matrix.CreateScale(scale, scale, 1.0f);
                float translation = (screenWidth - (desiredWidth * scale)) / 2.0f;
                translateMatrix = Matrix.CreateTranslation(translation, 0.0f, 0.0f);
            }
            else
            {
                float scale = (float)screenWidth / (float)desiredWidth;
                scaleMatrix = Matrix.CreateScale(scale, scale, 1.0f);
                float translation = (screenHeight - (desiredHeight * scale)) / 2.0f;
                translateMatrix = Matrix.CreateTranslation(0.0f, translation, 0.0f);
            }

            globalSpriteBatchMatrix = Matrix.Multiply(scaleMatrix, translateMatrix);

            player = new Player(Content);
            map = new Map(@"../../../Content/Maps/map1.txt", Content, player);

            IsHelloPlayerActive = true;

            base.Initialize();

        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            DialogFont = Content.Load<SpriteFont>("Font_Serif26");
            dialogBox = new DialogBox();

            introduce = Content.Load<Texture2D>("HelloPlayer");
        }

        protected override void Update(GameTime gameTime)
        {
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    Exit();

            KeyboardManager.Update();

            if (IsHelloPlayerActive == false)
            {

                if (IsPlayerMovementBlock == false)
                    player.UpdateVelocity();

                HandlePlayerCollision();

                if (IsPlayerMovementBlock == false)
                    player.UpdatePosition();

                dialogBox.Update();
            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            if (IsHelloPlayerActive == true)
            {
                SpriteBatch helloSprite = new SpriteBatch(GraphicsDevice);
                helloSprite.Begin();
                helloSprite.Draw(introduce, new Rectangle(0, 0, screenWidth, screenHeight), Color.White);
                if (KeyboardManager.IsKeyPressed(Keys.Enter))
                {
                    IsHelloPlayerActive = false;
                }
                helloSprite.End();
            }
            else
            {

                spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, globalSpriteBatchMatrix);

                map.DrawLevel(spriteBatch, player);
                player.Draw(spriteBatch);
                if (terminal != null && IsInTerminalArea == true)
                {
                    if (terminal.IsActive == true)
                    {
                        terminal.DrawTerminal(spriteBatch, Content);
                        terminal.EnterCode();
                        terminal.WriteActualCode(spriteBatch, dialogBox.TextScale);
                    }
                    else if (terminal.IsActive == false)
                    {
                        terminal.WriteSpaceMessage(spriteBatch, dialogBox.TextScale);
                    }
                }

                if (exercise != null & IsInExerciseArea == true)
                {
                    if (exercise.IsActive == true)
                    {
                        exercise.DrawExercise(spriteBatch, Content);
                    }
                    else if (exercise.IsActive == false)
                    {
                        exercise.WriteSpaceMessage(spriteBatch, dialogBox.TextScale);
                    }
                }
                spriteBatch.End();

                dialogBox.Draw(GraphicsDevice);

            }
            base.Draw(gameTime);
        }

        public void HandlePlayerCollision()
        {
            IsInTerminalArea = false;
            IsInExerciseArea = false;

            foreach (MapObject Object in map.MapObjects)
            {
                if (Object.IsCollidable == true)
                {
                    if (Object.ObjectType == EObjectType.Door && GameProgress.Doors[Object.ObjectNumber] == EDoor.Open)
                    {
                        Object.SetRotation();
                        Object.IsCollidable = false;
                        Object.IsCollisionResponseStatic = false;
                        // Open/close teleport
                    }

                    if (RectangleHelper.IsCollision(player.PlayerArea, Object.Area) == true)
                    {
                        if (Object.IsCollisionResponseStatic == true)
                        {
                            player.ResolveStaticCollision(Object);
                        }

                        else if (Object.ObjectType == EObjectType.Teleport && GameProgress.AreTeleportsActive[Object.ObjectNumber] == true)
                        {
                            map = new Map(Object.NextLevelPath, Content, player);
                        }

                        else if (GameProgress.AreDialogsActive[Object.ObjectNumber] == true && Object.ObjectType == EObjectType.Dialog)
                        {
                            
                            DialogManager.DisplayDialog(Object.DialogPath, dialogBox, Object.ObjectNumber);
                            GameProgress.AreDialogsActive[Object.ObjectNumber] = false;
                        }

                        else if (Object.ObjectType == EObjectType.Terminal && GameProgress.AreTerminalActive[Object.ObjectNumber] == true)
                        {
                            terminal = (Terminal)Object.Instance;                
                            IsInTerminalArea = true;

                            if (KeyboardManager.IsKeyPressed(Keys.Space))
                            {
                                terminal.IsActive = true;
                                IsPlayerMovementBlock = true; //can cause problems
                            }
                            else if (KeyboardManager.IsKeyPressed(Keys.Escape))
                            {
                                terminal.IsActive = false;
                                IsPlayerMovementBlock = false;
                            }
                        }

                        else if (Object.ObjectType == EObjectType.Exercise)
                        {
                            exercise = (Exercise)Object.Instance;
                            IsInExerciseArea = true;

                            if (KeyboardManager.IsKeyPressed(Keys.Space))
                            {
                                exercise.IsActive = true;
                                IsPlayerMovementBlock = true; //can cause problems
                            }
                            else if (KeyboardManager.IsKeyPressed(Keys.Escape))
                            {
                                exercise.IsActive = false;
                                IsPlayerMovementBlock = false;
                            }
                        }
                    }
                }
            }
        }
    }
}
