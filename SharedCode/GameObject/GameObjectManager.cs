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

        private static Dictionary<string, List<GameObject>> taggedObjects;

        private static double _globalPause;

        public static int numberOfObjects
        {
            get
            {
                if (activeObjects == null)
                    return 0;
                return activeObjects.Count;
            }
        }

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

            taggedObjects = new Dictionary<string, List<GameObject>>();
        }

        public static GameObject AddObject(GameObject go)
        {
            nextObjects.Add(go);
            return go;
        }

        public static void UpdateObjects(GameTime gameTime)
        {
            if (_globalPause > 0)
            {
                _globalPause -= gameTime.ElapsedGameTime.TotalSeconds;
                return;
            }

            foreach(var go in activeObjects)
            {
                if (go.isPaused) continue;

                go.Update(gameTime);
            }

            // Remove all unactive objects;
            for(int i = activeObjects.Count - 1; i >= 0; i -= 1)
            {
                if (activeObjects[i].done)
                {
                    Debug.Log($"Remove instance of {activeObjects[i].GetType().FullName}");
                    activeObjects[i].OnDestroy();
                    activeObjects[i].CleanUp();
                    RemoveFromTagList(activeObjects[i]);
                    activeObjects.RemoveAt(i);
                }
            }

            activeObjects.AddRange(nextObjects);
            activeObjects.Sort((x, y) => x.depth.CompareTo(y.depth));
            nextObjects.Clear();

            //GC.Collect();
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

        public static void InsertInTagList(GameObject obj)
        {
            foreach(var t in obj.tags)
            {
                if (!taggedObjects.ContainsKey(t))
                {
                    taggedObjects[t] = new List<GameObject>();
                }

                taggedObjects[t].Add(obj);
            }
        }

        public static void RemoveFromTagList(GameObject obj)
        {
            foreach (var t in obj.tags)
            {
                if (!taggedObjects.ContainsKey(t) && taggedObjects[t] == null)
                {
                    continue;
                }

                taggedObjects[t].Remove(obj);
            }
        }

        public static GameObject FindObjectWithTag(string tag)
        {
            return taggedObjects.ContainsKey(tag) && taggedObjects[tag] != null && taggedObjects[tag].Count > 0 ? 
                taggedObjects[tag][0] : 
                null;
        }

        public static List<GameObject> FindObjectsWithTag(string tag)
        {
            return taggedObjects.ContainsKey(tag) ? taggedObjects[tag] : new List<GameObject>();
        }

        public static void RemoveObjectsWithTag(string tag)
        {
            if (!taggedObjects.ContainsKey(tag))
            {
                return;
            }

            foreach(var obj in taggedObjects[tag])
            {
                obj.done = true;
            }
        }

        public static void AddPause(double time)
        {
            _globalPause = Math.Max(_globalPause, time);
        }

        public static void RemoveAllObjects()
        {
            foreach(var obj in activeObjects)
            {
                obj.CleanUp();
            }

            foreach (var obj in nextObjects)
            {
                obj.CleanUp();
            }

            activeObjects.Clear();
            nextObjects.Clear();
            taggedObjects.Clear();
            playerInstance = null;
        }

    }
}
