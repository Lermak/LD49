using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;


namespace MonoGame_Core.Scripts
{
    public static class TiledImporter
    {
        static XmlDocument myDoc;
        public static void LoadFromFile(string file)
        {
            myDoc = new XmlDocument();
            myDoc.Load(file);
        }
        public static void LoadFromContent(ContentManager c, string content)
        {
            myDoc = new XmlDocument();
            myDoc = c.Load<XmlDocument>(content);
        }
        public static void LoadFromString(string s)
        {
            myDoc = new XmlDocument();
            myDoc.LoadXml(s);
        }

        public static void BuildFromDoc()
        { 
            int width = int.Parse(myDoc.ChildNodes[1].Attributes[4].Value);
            int height = int.Parse(myDoc.ChildNodes[1].Attributes[5].Value);
            int layers = myDoc.ChildNodes[1].ChildNodes.Count - 1;
            int imageWidth = int.Parse(myDoc.ChildNodes[1].Attributes[6].Value);
            int imageHeight = int.Parse(myDoc.ChildNodes[1].Attributes[7].Value);


            CollisionManager.TileMap = new bool[width, height, myDoc.ChildNodes[1].ChildNodes.Count - 1];
            CollisionManager.TileSize = new Vector2(imageWidth, imageHeight);
            //CollisionManager.CollisionDetection = CollisionManager.CollisionType.TileMapFree;
            if (myDoc.ChildNodes[1].Attributes[2].Value == "orthogonal")
            {
                for (int l = 0; l < layers; ++l)
                {
                    string map = myDoc.ChildNodes[1].ChildNodes[l + 1].ChildNodes[0].ChildNodes[0].Value;
                    int[,] mapArr = new int[width, height];
                    string[] rows = map.Trim().Split(new char[] { '\n' });

                    for (int y = 0; y < height; ++y)
                    {
                        string[] row = rows[y].Split(new char[] { ',' });
                        for (int x = 0; x < width; ++x)
                        {
                            mapArr[x, y] = int.Parse(row[x]);
                            string name = "TileX" + x + "Y" + y + "L" + l;
                            Vector2 pos = new Vector2(imageWidth * x - width * imageWidth / 2, height * imageHeight / 2 - imageHeight * y );
                            switch (int.Parse(row[x]))
                            {
                                case 2:
                                    SceneManager.CurrentScene.GameObjects.Add(new TestStaticObject("Test", (byte)l));
                                    ((WorldObject)SceneManager.CurrentScene.GameObjects[SceneManager.CurrentScene.GameObjects.Count - 1]).Transform.Place(pos);
                                    CollisionManager.TileMap[x, y, l] = true;
                                    break;
                            }

                        }
                    }
                }
            }
            return;
        }
    }
}
