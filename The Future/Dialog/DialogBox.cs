using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace The_Future
{
    public class DialogBox
    {
        public float TextScale { get; set; }
        public bool IsActive { get; private set; }
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }
        public int BorderWidth { get; set; }
        public Vector2 CharacterNamePosition { get; set; }

        List<PlayerSentence> playerSentences;
        int currentPlayerSentence;
        private int currentPage;
        private int dialogNumber = -1;

        private Texture2D borderTexture;
        private Color borderColor;
        private Texture2D fillTexture;
        private Color fillColor;
        private Color dialogColor;
        private Vector2 characterSize;
        private const float DialogBoxMargin = 24f;
        private int MaxCharsPerLine() => (int) Math.Floor((Size.X - DialogBoxMargin) / characterSize.X);
        private int MaxLinesCount() => (int) Math.Floor((Size.Y - DialogBoxMargin) / characterSize.Y) - 1;

        private Rectangle TextRectangle => new Rectangle(Position.ToPoint(), Size.ToPoint());
        private Vector2 TextPosition => new Vector2(TextRectangle.X + 1, TextRectangle.Y + 1);

        private List<Rectangle> BorderRectangles => new List<Rectangle>
        {
            new Rectangle(TextRectangle.X - BorderWidth, TextRectangle.Y - BorderWidth,
                TextRectangle.Width + BorderWidth*2, BorderWidth),

            new Rectangle(TextRectangle.X + TextRectangle.Size.X, TextRectangle.Y, BorderWidth, TextRectangle.Height),

            new Rectangle(TextRectangle.X - BorderWidth, TextRectangle.Y + TextRectangle.Size.Y,
                TextRectangle.Width + BorderWidth*2, BorderWidth),

            new Rectangle(TextRectangle.X - BorderWidth, TextRectangle.Y, BorderWidth, TextRectangle.Height)
        };

        public DialogBox()
        {
            TextScale = GameMain.scale / 2;

            //fillColor = new Color(248, 214, 142);

            characterSize = Program.Game.DialogFont.MeasureString(new StringBuilder("u", 1)) * TextScale;

            BorderWidth = 2;
            dialogColor = Color.Black;

            fillColor = Color.LightBlue;
            borderColor = Color.Gray;

            fillTexture = new Texture2D(Program.Game.GraphicsDevice, 1, 1);
            fillTexture.SetData(new[] { fillColor });

            borderTexture = new Texture2D(Program.Game.GraphicsDevice, 1, 1);
            borderTexture.SetData(new[] { borderColor });

            playerSentences = new List<PlayerSentence>();
            currentPage = 0;
            currentPlayerSentence = 0;
            dialogNumber = 0;

            var sizeX = (int)(GameMain.screenWidth * 0.5);
            var sizeY = (int)(GameMain.screenHeight * 0.2);

            Size = new Vector2(sizeX, sizeY);

            var posX = GameMain.screenWidth/2 - (Size.X / 2);
            var posY = GameMain.screenHeight - Size.Y - 40;

            Position = new Vector2(posX, posY);

            CharacterNamePosition = new Vector2(Position.X, Position.Y - characterSize.Y * 2 - 2);
        }

        public void Initialize(string[] dialogs)
        {
            string[] sentences;

            for (int i = 0; i < dialogs.Length; i++)
            {
                sentences = dialogs[i].Split('&');
                PlayerSentence playerSentence = new PlayerSentence();

                playerSentence.characterName = sentences[0];

                switch (sentences[0])
                {
                    case "You":
                        playerSentence.color = Color.Blue;
                        break;

                    case "???":
                        playerSentence.color = Color.Red;
                        break;

                    case "King":
                        playerSentence.color = Color.Red;
                        break;

                    default:
                        playerSentence.color = Color.White;
                        break;
                }

                playerSentence.pages = WrapText(sentences[1]);

                playerSentences.Add(playerSentence);
            }

            currentPage = 0;
            currentPlayerSentence = 0;
            Show();
        }

        public void Initialize(string[] dialogs, int dialogNumber)
        {
            string[] sentences;

            for (int i = 0; i < dialogs.Length; i++)
            {
                sentences = dialogs[i].Split('&');
                PlayerSentence playerSentence = new PlayerSentence();

                playerSentence.characterName = sentences[0];

                switch (sentences[0])
                {
                    case "You":
                        playerSentence.color = Color.Blue;
                        break;

                    case "???":
                        playerSentence.color = Color.Red;
                        break;

                    case "King":
                        playerSentence.color = Color.Red;
                        break;

                    default:
                        playerSentence.color = Color.White;
                        break;
                }

                playerSentence.pages = WrapText(sentences[1]);

                playerSentences.Add(playerSentence);
            }

            currentPage = 0;
            currentPlayerSentence = 0;
            this.dialogNumber = dialogNumber;
            Show();
        }

        public void Show()
        {
            IsActive = true;
            GameMain.IsPlayerMovementBlock = true;
        }

        public void Hide()
        {
            IsActive = false;
            GameMain.IsPlayerMovementBlock = false;

            if(dialogNumber > 0)
            {
                
                GameProgress.SetDialogsValue(dialogNumber);
            }

            playerSentences = new List<PlayerSentence>();
            currentPage = 0;
            currentPlayerSentence = 0;
            dialogNumber = 0;

        }

        public void Update()
        {
            if (IsActive)
            {
                if (KeyboardManager.IsKeyPressed(Keys.Enter) && !KeyboardManager.IsPreviousKeyPressed(Keys.Enter))
                {
                    if (currentPage < playerSentences[currentPlayerSentence].pages.Count - 1)
                    {
                        currentPage++;
                    }
                    else
                    {
                        if (currentPlayerSentence < playerSentences.Count - 1)
                        {
                            currentPlayerSentence++;
                            currentPage = 0;
                        }
                        else
                        {
                            Hide();
                        }
                    }
                }
            }
        }

        public void Draw(GraphicsDevice graphicsDevice)
        {
            if (IsActive == true)
            {
                SpriteBatch textSpriteBatch = new SpriteBatch(graphicsDevice);

                textSpriteBatch.Begin();

                foreach (Rectangle side in BorderRectangles)
                {
                    textSpriteBatch.Draw(borderTexture, side, borderColor);
                }
                textSpriteBatch.Draw(fillTexture, TextRectangle, fillColor);
                textSpriteBatch.DrawString(Program.Game.DialogFont, playerSentences[currentPlayerSentence].pages[currentPage], TextPosition, 
                    dialogColor, 0, new Vector2(0,0), TextScale, SpriteEffects.None, 0);

                textSpriteBatch.DrawString(Program.Game.DialogFont, playerSentences[currentPlayerSentence].characterName, CharacterNamePosition,
                    playerSentences[currentPlayerSentence].color, 0, Vector2.Zero, 2* TextScale, SpriteEffects.None, 0);

                textSpriteBatch.End();
                //Blinking
            }
        }

        public List<string> WrapText(string text)
        {
            var pages = new List<string>();

            var capacity = MaxCharsPerLine() * MaxLinesCount() > text.Length ? text.Length : MaxCharsPerLine() * MaxLinesCount();

            var result = new StringBuilder(capacity);
            var resultLines = 0;

            var currentWord = new StringBuilder();
            var currentLine = new StringBuilder();

            for (var i = 0; i < text.Length; i++)
            {
                var currentChar = text[i];
                var isNewLine = text[i] == '\n';
                var isLastChar = i == text.Length - 1;

                currentWord.Append(currentChar);

                if (char.IsWhiteSpace(currentChar) || isLastChar)
                {
                    var potentialLength = currentLine.Length + currentWord.Length;

                    if (potentialLength > MaxCharsPerLine())
                    {
                        result.AppendLine(currentLine.ToString());

                        currentLine.Clear();

                        resultLines++;
                    }

                    currentLine.Append(currentWord);

                    currentWord.Clear();

                    if (isLastChar || isNewLine)
                    {
                        result.AppendLine(currentLine.ToString());
                    }

                    if (resultLines > MaxLinesCount() || isLastChar || isNewLine)
                    {
                        pages.Add(result.ToString());

                        result.Clear();

                        resultLines = 0;

                        if (isNewLine)
                        {
                            currentLine.Clear();
                        }
                    }
                }
            }

            return pages;
        }
    }
}
