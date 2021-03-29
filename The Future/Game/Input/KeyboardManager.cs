using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace The_Future
{
    public static class KeyboardManager
    {
        private static KeyboardState keyboardState;

        private static List<Keys> pressedKeys = new List<Keys>();
        private static List<Keys> previousPressedKeys = new List<Keys>();

        public static void Update()
        {
            keyboardState = Keyboard.GetState();

            UpdatePressedKeys();
        }

        public static bool IsKeyPressed(Keys key)
        {
            for (int i = 0; i < pressedKeys.Count; i++)
            {
                if (pressedKeys[i] == key)
                    return true;
            }
            return false;
        }

        public static bool IsPreviousKeyPressed(Keys key)
        {
            for (int i = 0; i < previousPressedKeys.Count; i++)
            {
                if (previousPressedKeys[i] == key)
                    return true;
            }
            return false;
        }


        private static void UpdatePressedKeys()
        {
            previousPressedKeys = pressedKeys;

            //pressedKeys.Clear();

            pressedKeys = keyboardState.GetPressedKeys().ToList();
        }
    }
}
