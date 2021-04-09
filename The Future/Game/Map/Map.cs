using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace The_Future
{
    public class Map
    {
        public string TexturePath { get; set; }
        public Texture2D Atlas { get; set; }
        public MapObject[] MapObjects { get; set; }
        public MapAttributes MapAttributes { get; set; }

        public Map(string path, ContentManager contentManager, Player player)
        {
            try
            {
                Parser.LoadLevel(this, path);

                Atlas = contentManager.Load<Texture2D>(TexturePath);

                Vector2 playerSpawn = MapAttributes.PlayerSpawn;
                if(MapAttributes.PlayerTransitionFromLevel.ContainsKey(MapAttributes.PreviousMapName))
                {
                    playerSpawn = MapAttributes.PlayerTransitionFromLevel[MapAttributes.PreviousMapName];
                }

                 player.SetPosition(playerSpawn);
            }
             catch { }
        }

        public void DrawLevel(SpriteBatch spriteBatch, Player player)
        {
            foreach (MapObject Object in MapObjects)
            {
                if(Object.IsRenderable == true)
                {
                    switch(Object.RenderFlag)
                    {
                        case RenderFlag.Repeat:
                            DrawRepeat(spriteBatch, Object.Area, Object.SpriteAreaInAtlas);
                            break;

                        case RenderFlag.Stretch:
                            spriteBatch.Draw(Atlas, Object.Area, Object.SpriteAreaInAtlas, Color.White, Object.Rotation, Vector2.Zero, SpriteEffects.None, 0);
                            break;
                    }
                }
            }
        }

        public void DrawRepeat(SpriteBatch spriteBatch, Rectangle area, Rectangle spriteInAtlas)
        {
            int xCount = area.Width / spriteInAtlas.Width;
            int yCount = area.Height / spriteInAtlas.Height;

            for (int x = 0; x < xCount; x++)
            {
                for (int y = 0; y < yCount; y++)
                {
                    Vector2 position = new Vector2(area.X + x * spriteInAtlas.Width, area.Y + y * spriteInAtlas.Height);

                    //Debug.WriteLine(position.ToString());

                    spriteBatch.Draw(Atlas, new Rectangle((int)position.X, (int)position.Y, spriteInAtlas.Width, spriteInAtlas.Height),
                        spriteInAtlas, Color.White);
                }
            }
        }
    }
}
