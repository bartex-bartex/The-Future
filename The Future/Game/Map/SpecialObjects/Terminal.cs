using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace The_Future
{
    public class Terminal
    {
        public bool IsActive { get; set; }
        public static Vector2 TextPosition;
        Texture2D terminalTexture;
        string userInput = "";
        int terminalNumber;
        int framesCountFail;
        Vector2 CodeText;
        float XterminalScale;
        float YterminalScale;

        public Terminal(int terminalNumber)
        {
            TextPosition = new Vector2(GameMain.desiredWidth / 2 - Program.Game.DialogFont.MeasureString("Spacja").X / 2,
                GameMain.desiredHeight * 0.8f);

            this.terminalNumber = terminalNumber;


        }

        public void DrawTerminal(SpriteBatch spriteBatch, ContentManager content)
        {
            if (terminalTexture == null)
            {
                terminalTexture = content.Load<Texture2D>("Terminal");
            }
            spriteBatch.Draw(terminalTexture, new Rectangle(20, 20, 760, 440), Color.White);

            if (CodeText == Vector2.Zero)
            {
                XterminalScale = (float)760 / (float)terminalTexture.Width;
                YterminalScale = (float)440 / (float)terminalTexture.Height;

                //Can be rendered with not the best positioning
                CodeText = new Vector2((175 * XterminalScale + 20) + (GameMain.screenWidth - GameMain.desiredWidth * GameMain.scale) / 2,
                    (175 * YterminalScale + 20));
            }
        }

        public void WriteSpaceMessage(SpriteBatch spriteBatch, float scale) //scale after calculation
        {
            spriteBatch.DrawString(Program.Game.DialogFont, "Spacja", TextPosition, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
        }

        public void WriteActualCode(SpriteBatch spriteBatch, float scale)
        {
            spriteBatch.DrawString(Program.Game.DialogFont, userInput, CodeText, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);

            if(framesCountFail >= 0)
            {
                spriteBatch.DrawString(Program.Game.DialogFont, userInput, CodeText, Color.Red, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
                framesCountFail--;
            }
        }

        public void EnterCode()
        {
            if (userInput.Length < 4)
            {
                if (KeyboardManager.IsKeyPressed(Keys.D0) && !KeyboardManager.IsPreviousKeyPressed(Keys.D0)) { userInput += "0"; }
                else if (KeyboardManager.IsKeyPressed(Keys.D1) && !KeyboardManager.IsPreviousKeyPressed(Keys.D1)) { userInput += "1"; }
                else if (KeyboardManager.IsKeyPressed(Keys.D2) && !KeyboardManager.IsPreviousKeyPressed(Keys.D2)) { userInput += "2"; }
                else if (KeyboardManager.IsKeyPressed(Keys.D3) && !KeyboardManager.IsPreviousKeyPressed(Keys.D3)) { userInput += "3"; }
                else if (KeyboardManager.IsKeyPressed(Keys.D4) && !KeyboardManager.IsPreviousKeyPressed(Keys.D4)) { userInput += "4"; }
                else if (KeyboardManager.IsKeyPressed(Keys.D5) && !KeyboardManager.IsPreviousKeyPressed(Keys.D5)) { userInput += "5"; }
                else if (KeyboardManager.IsKeyPressed(Keys.D6) && !KeyboardManager.IsPreviousKeyPressed(Keys.D6)) { userInput += "6"; }
                else if (KeyboardManager.IsKeyPressed(Keys.D7) && !KeyboardManager.IsPreviousKeyPressed(Keys.D7)) { userInput += "7"; }
                else if (KeyboardManager.IsKeyPressed(Keys.D8) && !KeyboardManager.IsPreviousKeyPressed(Keys.D8)) { userInput += "8"; }
                else if (KeyboardManager.IsKeyPressed(Keys.D9) && !KeyboardManager.IsPreviousKeyPressed(Keys.D9)) { userInput += "9"; }
            }
            if (KeyboardManager.IsKeyPressed(Keys.Back) && !KeyboardManager.IsPreviousKeyPressed(Keys.Back))
            {
                if (userInput.Length >= 1)
                {
                    userInput = userInput.Substring(0, userInput.Length - 1);
                }
            }
            if (KeyboardManager.IsKeyPressed(Keys.Enter) && !KeyboardManager.IsPreviousKeyPressed(Keys.Enter)) 
            {
                if(GameProgress.TerminalCodes[terminalNumber] == userInput)
                {
                    GameProgress.SetTerminalValues(0);
                }
                else
                {
                    framesCountFail = 60;
                }
            }
        }
    }
}
