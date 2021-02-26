using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace The_Future
{
    public class MapAttributes
    {
        public string PreviousMapName;
        public Vector2 PlayerSpawn { get; set; }
        public Dictionary<string, Vector2> PlayerTransitionFromLevel = new Dictionary<string, Vector2>();

        public MapAttributes()
        {
            PlayerSpawn = new Vector2(0, 0);
        }
    }
}
