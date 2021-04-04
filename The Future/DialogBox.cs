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
        public string Text { get; set; }      
        public float TextScale { get; set; }
        public bool IsActive { get; private set; }
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }
        public int BorderWidth { get; set; }
        public string CharacterName { get; set; }
        public Vector2 CharacterNamePosition { get; set; }
        public Color CharacterNameColor { get; set; }

        private List<string> pages;
        private int currentPage;
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
            if (GameMain.scale > 1.35f)
                TextScale = 1.35f;
            else { TextScale = GameMain.scale; }

            characterSize = Program.Game.DialogFont.MeasureString(new StringBuilder("w", 1)) * TextScale;

            BorderWidth = 2;
            dialogColor = Color.Black;

            fillColor = Color.LightBlue;
            borderColor = Color.Gray;

            fillTexture = new Texture2D(Program.Game.GraphicsDevice, 1, 1);
            fillTexture.SetData(new[] { fillColor });

            borderTexture = new Texture2D(Program.Game.GraphicsDevice, 1, 1);
            borderTexture.SetData(new[] { borderColor });

            pages = new List<string>();
            currentPage = 0;

            var sizeX = (int)(GameMain.screenWidth * 0.5);
            var sizeY = (int)(GameMain.screenHeight * 0.2);

            Size = new Vector2(sizeX, sizeY);

            var posX = GameMain.screenWidth/2 - (Size.X / 2);
            var posY = GameMain.screenHeight - Size.Y - 30;

            Position = new Vector2(posX, posY);

            CharacterNamePosition = new Vector2(Position.X + Size.X * 0.1f, Position.Y - characterSize.Y - 2);
        }

        public void Initialize(string text)
        {
            Text = text;
            currentPage = 0;
            Show();
        }

        public void Show()
        {
            IsActive = true;

            pages = WrapText(Text);
        }

        public void Hide()
        {
            IsActive = false;
            
        }

        public void Update()
        {
            if (IsActive)
            {
                if (KeyboardManager.IsKeyPressed(Keys.Enter) && !KeyboardManager.IsPreviousKeyPressed(Keys.Enter))
                {
                    if (currentPage >= pages.Count - 1)
                    {
                        Hide();
                    }
                    else
                    {
                        currentPage++;
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
                textSpriteBatch.DrawString(Program.Game.DialogFont, pages[currentPage], TextPosition, dialogColor, 0, new Vector2(0,0), TextScale, SpriteEffects.None, 0);

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
