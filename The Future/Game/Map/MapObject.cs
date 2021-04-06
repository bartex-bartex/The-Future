using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

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
        LevelChange,
        Dialog
    }

    public enum EObjectType
    {
        NotSpecial,
        Door,
        Dialog,
        Teleport
    }

    public class MapObject
    {
        public Rectangle Area { get; set; }
        public Rectangle SpriteAreaInAtlas { get; set; }
        public bool IsRenderable { get; set; }
        public RenderFlag RenderFlag { get; set; }
        public bool IsCollidable { get; set; } = false;
        public bool IsCollisionResponseStatic { get; set; } = false;
        //public bool IsLevelChange { get; set; }
        //public bool IsLevelChangeActive { get; set; }
        public string NextLevelPath { get; set; }
        public string DialogPath { get; set; }
        //public bool IsDialogActive { get; set; } --> Wypisane w GameProgess
        //public bool IsDoor { get; set; }
        //public EDoor Door { get; set; }
        public float Rotation { get; set; } = 0;
        public EObjectType ObjectType { get; set; }
        public int ObjectNumber { get; set; }
        public int DoorConnectedToTeleport { get; set; } = -1;

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
                    
                    break;

                case CollisionFlag.Dialog:
                    ObjectNumber = int.Parse(Regex.Match(DialogPath, "[0-9]+").Value);
                    break;
            }
        }

        public void SetRotation()
        {
            if(ObjectType == EObjectType.Door && GameProgress.Doors[ObjectNumber] == EDoor.Open)
            {
                Rotation = 1.5f;
            }
        }
    }
}