using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.Timers;

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

        Player player;
        Map map;
        public Matrix globalSpriteBatchMatrix;

        public static float globalScale;

        public static bool IsPlayerMovementBlock = false;

        private bool IsInTerminalArea;
        private Terminal terminal;
        private bool IsInExerciseArea;
        private Exercise exercise;

        public static bool IsHelloPlayerActive;
        public static bool IsGameEnd;
        Texture2D introduce;
        Texture2D gameEndScreen;
        bool endScreenType = true;

        public GameMain()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = true;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // generic setup

            Window.Title = "The Future: Time Escaper";

            screenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            screenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;

            graphics.ApplyChanges();

            // gui scale setup

            Matrix _scaleMatrix;
            Matrix _translateMatrix;

            // don't mess up scale when aspect ratio is x:y

            if (screenWidth > screenHeight)
            {
                globalScale = (float)screenHeight / (float)desiredHeight;
                _scaleMatrix = Matrix.CreateScale(globalScale, globalScale, 1.0f);
                float translation = (screenWidth - (desiredWidth * globalScale)) / 2.0f;
                _translateMatrix = Matrix.CreateTranslation(translation, 0.0f, 0.0f);
            }
            else
            {
                globalScale = (float)screenWidth / (float)desiredWidth;
                _scaleMatrix = Matrix.CreateScale(globalScale, globalScale, 1.0f);
                float translation = (screenHeight - (desiredHeight * globalScale)) / 2.0f;
                _translateMatrix = Matrix.CreateTranslation(0.0f, translation, 0.0f);
            }

            // finish up global sprite batch matrix

            globalSpriteBatchMatrix = Matrix.Multiply(_scaleMatrix, _translateMatrix);

            player = new Player(Content);
            map = new Map(@"../../../Content/Maps/map1.txt", Content, player);

            // initialize intro scene

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

            KeyboardManager.Update();

            // game is updated only if not in intro or outtro scene
            if (!IsHelloPlayerActive && !IsGameEnd)
            {

                if (!IsPlayerMovementBlock)
                    player.UpdateVelocity();

                HandlePlayerCollision();

                if (!IsPlayerMovementBlock)
                    player.UpdatePosition();

                dialogBox.Update();
            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // first time GUI + end screen

            if (IsHelloPlayerActive)    // intro
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

            else if (IsGameEnd) // outtro, has 2 modes
            {
                SpriteBatch endSprite = new SpriteBatch(GraphicsDevice);
                endSprite.Begin();
                if (endScreenType)  // game end 1 screen
                {
                    gameEndScreen = Content.Load<Texture2D>("GameEnd1");
                    endSprite.Draw(gameEndScreen, new Rectangle(0, 0, screenWidth, screenHeight), Color.White);

                    if (GameProgress.stopWatch.Elapsed.TotalSeconds > 2)
                    {
                        endScreenType = false;
                        GameProgress.stopWatch.Stop();
                    }
                }
                else // game end 2 screen
                {
                    gameEndScreen = Content.Load<Texture2D>("GameEnd2");
                    endSprite.Draw(gameEndScreen, new Rectangle(0, 0, screenWidth, screenHeight), Color.White);

                    if (Keyboard.GetState().IsKeyDown(Keys.Escape)) // end game
                        Exit();
                }
                endSprite.End();
            }

            // else we can draw game

            else
            {
                /*
                 *  Deferred - from back to front
                 *  PointClamp - nearest neighbour scaling (preserves pixelation, gets rid of ugly white spaces)
                 *  globalSpriteBatchMatrix - takes care of scaling and translation
                 *  
                 */
                spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, globalSpriteBatchMatrix);

                map.DrawLevel(spriteBatch, player);
                player.Draw(spriteBatch);
                // terminal handle code
                if (IsInTerminalArea && terminal != null)
                {
                    if (terminal.IsActive)
                    {
                        terminal.DrawTerminal(spriteBatch, Content);
                        terminal.EnterCode();
                        terminal.WriteActualCode(spriteBatch, dialogBox.TextScale);
                    }
                    else
                    {
                        terminal.WriteSpaceMessage(spriteBatch, dialogBox.TextScale);
                    }
                }

                // exercise handle code
                if (IsInExerciseArea && exercise != null)
                {
                    if (exercise.IsActive)
                    {
                        exercise.DrawExercise(spriteBatch, Content);
                    }
                    else
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

            // will get slow for 50+ objects, not necessary to fix

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

                    if (RectangleHelper.IsCollision(player.PlayerArea, Object.Area))
                    {
                        if (Object.IsCollisionResponseStatic)
                        {
                            player.ResolveStaticCollision(Object);
                        }

                        else
                        {
                            // more efficient than else if, could be sped up if every type had it's collision response in method

                            switch (Object.ObjectType)
                            {
                                case EObjectType.Teleport:
                                    if (GameProgress.AreTeleportsActive[Object.ObjectNumber]) map = new Map(Object.NextLevelPath, Content, player);
                                    break;
                                case EObjectType.Dialog:
                                    if (GameProgress.AreDialogsActive[Object.ObjectNumber])
                                    {
                                        DialogManager.DisplayDialog(Object.DialogPath, dialogBox, Object.ObjectNumber);
                                        GameProgress.AreDialogsActive[Object.ObjectNumber] = false;
                                    }
                                    break;
                                case EObjectType.Terminal:
                                    if (GameProgress.AreTerminalActive[Object.ObjectNumber])
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
                                    break;
                                case EObjectType.Exercise:
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
                                    break;
                            }
                        }
                    }
                }
            }
        }
    }
}
