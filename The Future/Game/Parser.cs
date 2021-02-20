using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace The_Future
{
    //None means input file is not created correctly
    enum RenderFlag
    {
        None,
        Repeat,
        Stretch
    }

    enum CollisionFlag
    {
        None,
        Static,
        LevelChange
    }

    class MapObject
    {
        public Vector2 Position { get; set; }
        public Vector2 Dimension { get; set; }
        public bool IsRenderable { get; set; }
        public RenderFlag RenderFlag { get; set; }
        public CollisionFlag CollisionFlag { get; set; }
        public Vector2 SpritePositionInAtlas { get; set; }
        public Vector2 SpriteDimensionInAtlas { get; set; }
        public bool IsCollidable { get; set; }
        public bool IsCollisionResponseStatic { get; set; }
        public bool IsLevelChange { get; set; }

        public MapObject(Vector2 position, Vector2 dimension)
        {
            Position = position;
            Dimension = dimension;
        }

        public void AddRenderAttribute(Vector2 spritePositionInAtlas, Vector2 spriteDimensionInAtlas, RenderFlag renderFlag)
        {
            IsRenderable = true;
            RenderFlag = renderFlag;
            SpritePositionInAtlas = spritePositionInAtlas;
            SpriteDimensionInAtlas = spriteDimensionInAtlas;
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

    class MapAttributes
    {
        public Vector2 PlayerSpawn { get; set; }
        public Dictionary<string, Vector2> PlayerTransitionFromLevel = new Dictionary<string, Vector2>();

        public MapAttributes()
        {
            PlayerSpawn = new Vector2(0, 0);
        }
    }

    class Map 
    {
        public string TexturePath { get; set; }
        public MapObject[] MapObjects { get; set; }
        public MapAttributes MapAttributes { get; set; }
    }


    class Parser
    {
        public static Map LoadLevel(string path)
        {
            StreamReader sr = new StreamReader(path);

            string currentLine;
            string[] tokens;
            string[] values;
            Map map = new Map();
            List<MapObject> mapObjects = new List<MapObject>();
            MapObject nextObject;
            MapAttributes mapAttributes = new MapAttributes();

            try
            {
                map.TexturePath = sr.ReadLine();

                do
                {
                    currentLine = sr.ReadLine();

                    if(currentLine.StartsWith('#'))
                    {
                        tokens = currentLine.Substring(1).Split(':');
                        switch(tokens[0])
                        {
                            case "PLAYER_SUMMON":
                                values = tokens[1].Split(',');

                                mapAttributes.PlayerSpawn = new Vector2(
                                    float.Parse(values[0]),
                                    float.Parse(values[1])
                                    );
                                break;

                            case "PLAYER_TRANSITION":
                                values = tokens[1].Split(',');

                                mapAttributes.PlayerTransitionFromLevel.Add
                                    (
                                    values[2],
                                    new Vector2
                                    (
                                        float.Parse(values[0]),
                                        float.Parse(values[1])
                                    )
                                );
                                break;
                        }
                    }

                    else
                    {
                        tokens = currentLine.Split('&');
                        values = tokens[0].Split(',');

                        nextObject = new MapObject
                            (
                                new Vector2
                                (
                                    float.Parse(values[0]),
                                    float.Parse(values[1])
                                ),
                                new Vector2
                                (
                                    float.Parse(values[2]),
                                    float.Parse(values[4])
                                )
                            );

                        for (int i = 1; i < tokens.Length; i++)
                        {
                            values = tokens[i].Split(',');

                            switch(values[0])
                            {
                                case "RENDER_COMPONENT":
                                    RenderFlag renderFlag;

                                    switch(values[5])
                                    {
                                        case "REPEAT":
                                            renderFlag = RenderFlag.Repeat;
                                            break;

                                        case "STRETCH":
                                            renderFlag = RenderFlag.Stretch;
                                            break;

                                        default:
                                            renderFlag = RenderFlag.None;
                                            break;
                                    }

                                    nextObject.AddRenderAttribute
                                        (
                                        new Vector2
                                        (
                                                float.Parse(values[1]),
                                                float.Parse(values[2])
                                        ),

                                        new Vector2
                                        (
                                                float.Parse(values[3]),
                                                float.Parse(values[4])
                                        ),

                                        renderFlag
                                    ) ;
                                 break;

                                case "COLLSION_COMPONENT":
                                    //nextObject.AddCollisionAttribute(string.Equals(values[1], "STATIC"));
                                    CollisionFlag collisionFlag;

                                    switch(values[1])
                                    {
                                        case "STATIC":
                                            collisionFlag = CollisionFlag.Static;
                                            break;

                                        case "LEVEL_CHANGE":
                                            collisionFlag = CollisionFlag.LevelChange;
                                            break;

                                        default:
                                            collisionFlag = CollisionFlag.None;
                                            break;
                                    }

                                    nextObject.AddCollisionAttribute(collisionFlag);

                                    break;
                            }
                        }

                        mapObjects.Add(nextObject);
                    }      

                } while (sr.Peek() >= 0);

                map.MapObjects = mapObjects.ToArray();
                map.MapAttributes = mapAttributes;

                return map;
            }
            catch
            {
                
            }
            finally
            {
                sr.Close();
            }
            return map;
        }
    }
}
