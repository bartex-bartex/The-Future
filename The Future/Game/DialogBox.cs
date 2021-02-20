//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace The_Future.Game
//{
//    public static class DialogBox
//    {
//        List<string> lines = new List<string>();

//        public static void Draw(string text, SpriteBatch spriteBatch, SpriteFont font, Vector2 position, Rectangle textBoxSize)
//        {
//            //spriteBatch.Draw()
//            font.MeasureString(text);  

//        }

//        private static void PrepareText(string text, SpriteFont font, Rectangle textBoxSize)
//        {
//            int width = textBoxSize.X;
//            int height = textBoxSize.Y;
//            string[] textArray = text.Split(' ');

//            int i = 0;
//            int sum = 0;
//            string currentText;
//            while(true)
//            {
//                i++;
//                sum += textArray[i].Length + i;
//                if(font.MeasureString(text.Substring(0, sum)).X < width)
//                {
//                    i--;
//                    break;
//                }

//                currentText += textArray[i] + " ";
//            }
//            lines.Add()
            
//        }


//    }
//}
