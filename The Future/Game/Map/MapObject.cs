using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace The_Future
{
    public enum RenderFlag
    {
        None,
        Repeat,
        Stretch
    }

    public enum CollisionFlag
    {
        None,
        Static,
        LevelChange
    }

    public class MapObject
    {
        public Rectangle Area { get; set; }
        public Rectangle SpriteAreaInAtlas { get; set; }
        public bool IsRenderable { get; set; }
        public RenderFlag RenderFlag { get; set; }
        public bool IsCollidable { get; set; }
        public CollisionFlag CollisionFlag { get; set; }
        public bool IsCollisionResponseStatic { get; set; }
        public bool IsLevelChange { get; set; }
        public string NextLevelPath { get; set; }
        //public bool IsCollectable { get; set; }

        public MapObject(Vector2 position, Vector2 dimension)
        {
            Area = new Rectangle((int)position.X, (int)position.Y, (int)dimension.X, (int)dimension.Y);
        }

        public void AddRenderAttribute(Vector2 spritePositionInAtlas, Vector2 spriteDimensionInAtlas, RenderFlag renderFlag)
        {
            IsRenderable = true;
            RenderFlag = renderFlag;
            SpriteAreaInAtlas = new Rectangle((int)spritePositionInAtlas.X, (int)spritePositionInAtlas.Y,
                (int)spriteDimensionInAtlas.X, (int)spriteDimensionInAtlas.Y);
        }

        public void AddCollisionAttribute(CollisionFlag collisionFlag)
        {
            IsCollidable = true;

            switch (collisionFlag)
            {
                case CollisionFlag.Static:
                    IsCollisionResponseStatic = true;
                    break;

                case CollisionFlag.LevelChange:
                    IsLevelChange = true;
                    break;
            }
        }
    }
}
