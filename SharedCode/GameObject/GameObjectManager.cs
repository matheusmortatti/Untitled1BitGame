using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Pico8_Emulator;

namespace SharedCode
{
    public static class GameObjectManager
    {

        private static List<GameObject> activeObjects;
        private static List<GameObject> nextObjects;
        private static int MAX_OBJECTS = 200;

        public static Pico8<Color> pico8 { get; private set; }
        public static Player playerInstance { get; private set; }

        public static void Init(in Pico8<Color> p8)
        {
            if (activeObjects == null)
            {
                activeObjects = new List<GameObject>();
                activeObjects.Capacity = MAX_OBJECTS;
            }

            if (nextObjects == null)
            {
                nextObjects = new List<GameObject>();
                nextObjects.Capacity = MAX_OBJECTS / 10;
            }

            pico8 = p8;
        }

        public static GameObject AddObject(GameObject go)
        {
            nextObjects.Add(go);
            return go;
        }

        public static void UpdateObjects(GameTime gameTime)
        {
            foreach(var go in activeObjects)
            {
                go.Update(gameTime);
            }

            // Remove all unactive objects;
            for(int i = activeObjects.Count - 1; i >= 0; i -= 1)
            {
                if (activeObjects[i].done)
                {
                    activeObjects[i].CleanUp();
                    activeObjects.RemoveAt(i);
                }
            }

            activeObjects.AddRange(nextObjects);
            activeObjects.Sort((x, y) => x.depth.CompareTo(y.depth));
            nextObjects.Clear();

            GC.Collect();
        }

        public static void DrawObjects()
        {
            for (int i = 0; i < activeObjects.Count; i += 1)
            {
                activeObjects[i].Draw();
            }
        }

        public static Player InstantiatePlayer(Vector2 position)
        {
            playerInstance = new Player(position);
            AddObject(playerInstance);

            return playerInstance;
        }

    }
}
