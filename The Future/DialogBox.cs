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
        public bool IsActive { get; private set; }
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }
        public int BorderWidth { get; set; }

        private List<string> pages;
        private int currentPage;
        private Texture2D borderTexture;
        private Color borderColor;
        private Texture2D fillTexture;
        private Color fillColor;
        private Color dialogColor;
        private Vector2 characterSize = Program.Game.DialogFont.MeasureString(new StringBuilder("W", 1));
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

            //Analyze Size and Position setting
            var sizeX = (int)(Program.Game.GraphicsDevice.Viewport.Width * 0.5);
            var sizeY = (int)(Program.Game.GraphicsDevice.Viewport.Height * 0.2);

            Size = new Vector2(sizeX, sizeY);

            var posX = Program.Game.ScreenCenter.X - (Size.X / 2f);
            var posY = Program.Game.GraphicsDevice.Viewport.Height - Size.Y - 30;

            Position = new Vector2(posX, posY);

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

        public void Draw(SpriteBatch spriteBatch)
        {
            if(IsActive == true)
            {
                foreach (Rectangle side in BorderRectangles)
                {
                    spriteBatch.Draw(borderTexture, side, borderColor);
                }

                spriteBatch.Draw(fillTexture, TextRectangle, fillColor);

                spriteBatch.DrawString(Program.Game.DialogFont, pages[currentPage], TextPosition, dialogColor);

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
