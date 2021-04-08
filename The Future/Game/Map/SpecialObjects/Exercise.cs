using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace The_Future
{
    public class Exercise
    {
        public bool IsActive { get; set; }
        Texture2D exerciseTexture;
        int exerciseNumber;
        public static Vector2 TextPosition;

        public Exercise(int exerciseNumber)
        {
            TextPosition = new Vector2(GameMain.desiredWidth / 2 - Program.Game.DialogFont.MeasureString("Spacja").X / 2,
                GameMain.desiredHeight * 0.8f);

            this.exerciseNumber = exerciseNumber;
        }

        public void DrawExercise(SpriteBatch spriteBatch, ContentManager content)
         {
            if (exerciseTexture == null)
            {
                exerciseTexture = content.Load<Texture2D>(GameProgress.PathsToExercises[exerciseNumber]);
            }
            spriteBatch.Draw(exerciseTexture, new Rectangle(20, 20, 760, 440), Color.White);
        }

        public void WriteSpaceMessage(SpriteBatch spriteBatch, float scale) //scale after calculation
        {
            spriteBatch.DrawString(Program.Game.DialogFont, "Spacja", TextPosition, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
        }
    }
}
