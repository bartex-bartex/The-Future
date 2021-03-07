using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace The_Future
{
    public static class RectangleHelper
    {
        public static bool IsCollision(Rectangle rec1, Rectangle rec2)
        {
            return rec1.X < rec2.X + rec2.Width &&
                  rec1.X + rec1.Width > rec2.X &&
                  rec1.Y < rec2.Y + rec2.Height &&
                  rec1.Y + rec1.Height > rec2.Y;
        }
    }
}
