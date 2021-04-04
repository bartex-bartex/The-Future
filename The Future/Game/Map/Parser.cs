using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace The_Future
{
    public class Parser
    {
        public static string currentLevelName { get; private set; } = "";

        public static Map LoadLevel(Map map, string path)
        {
            StreamReader sr = new StreamReader(path);

            string currentLine;
            string[] tokens;
            string[] values;
            List<MapObject> mapObjects = new List<MapObject>();
            MapObject nextObject;
            MapAttributes mapAttributes = new MapAttributes();

            mapAttributes.PreviousMapName = currentLevelName;
            currentLevelName = Path.GetFileName(path);

            try
            {
                map.TexturePath = sr.ReadLine();

                do
                {
                    currentLine = sr.ReadLine();
                    currentLine = Regex.Replace(currentLine, @"[ ]*\(.+\)[ ]*", "");

                    if (currentLine.StartsWith('#'))
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
                                    float.Parse(values[3])
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

                                case "COLLISION_COMPONENT":
                                    CollisionFlag collisionFlag;

                                    switch(values[1])
                                    {
                                        case "STATIC":
                                            collisionFlag = CollisionFlag.Static;
                                            break;

                                        case "LEVEL_CHANGE":
                                            collisionFlag = CollisionFlag.LevelChange;
                                            nextObject.NextLevelPath = values[2].ToString();
                                            break;

                                        case "DIALOG":
                                            collisionFlag = CollisionFlag.Dialog;
                                            nextObject.DialogPath = values[2].ToString();
                                            nextObject.IsDialogActive = true;
                                            break;

                                        default:
                                            collisionFlag = CollisionFlag.None;
                                            break;
                                    }

                                    nextObject.AddCollisionAttribute(collisionFlag);

                                    break;
                            }

                            if (currentLine.EndsWith('#'))
                            {

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

        private static string RemoveCommentaries(string levelText)
        {
            return Regex.Replace(levelText, "(.+)", "");
        }
    }
}
