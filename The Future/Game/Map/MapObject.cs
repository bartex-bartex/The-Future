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
        Dialog,
        Terminal,
        Exercise
    }

    public enum EObjectType
    {
        NotSpecial,
        Door,
        Dialog,
        Teleport,
        Exercise,
        Terminal
    }

    public class MapObject
    {
        public Rectangle Area { get; set; }
        public Rectangle SpriteAreaInAtlas { get; set; }
        public bool IsRenderable { get; set; }
        public RenderFlag RenderFlag { get; set; }
        public bool IsCollidable { get; set; } = false;
        public bool IsCollisionResponseStatic { get; set; } = false;
        public string NextLevelPath { get; set; }
        public string DialogPath { get; set; }
        public float Rotation { get; set; } = 0;
        public EObjectType ObjectType { get; set; }
        public int ObjectNumber { get; set; }
        //public int DoorConnectedToTeleport { get; set; } = -1;
        public object Instance { get; set; } //add IsActive in GameProgress
        public bool IsNegative { get; set; }

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

                case CollisionFlag.Terminal:
                    Instance = new Terminal(ObjectNumber);
                    break;

                case CollisionFlag.Exercise:
                    Instance = new Exercise(ObjectNumber);
                    break;
            }
        }

        public void SetRotation()
        {
            if(ObjectType == EObjectType.Door && GameProgress.Doors[ObjectNumber] == EDoor.Open)
            {
                if (IsNegative == true)
                    Rotation = -1.5f;
                else
                    Rotation = 1.5f;
            }
        }
    }
}