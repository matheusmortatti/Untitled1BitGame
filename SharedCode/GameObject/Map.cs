using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace SharedCode
{
    public class Map : GameObject
    {
        private Vector2 currentIndex;
        public Map(Vector2 position) : base(null, null, null, position)
        {
            currentIndex = new Vector2((float)Math.Floor(position.X / 128),
                                            (float)Math.Floor(position.Y / 128));
            InstantiateEntities(currentIndex);

            depth = -1000;
        }

        public override void Update(GameTime gameTime)
        {
            var pi = GameObjectManager.playerInstance;

            if (pi == null)
                return;

            Vector2 nextIndex = new Vector2((float)Math.Floor(pi.transform.position.X / 128),
                                            (float)Math.Floor(pi.transform.position.Y / 128));

            if (currentIndex.X != nextIndex.X || currentIndex.Y != nextIndex.Y)
            {
                currentIndex = nextIndex;
                InstantiateEntities(currentIndex);
            }
        }

        public override void Draw()
        {
            GameObjectManager.pico8.graphics.Map(0, 0, 0, 0, 64, 128, 0x1);
        }

        public void InstantiateEntities(Vector2 screenIndex)
        {
            Vector2 celPos = new Vector2((int)screenIndex.X, (int)screenIndex.Y) * 16;

            for (int i = 0; i < 16; i += 1)
            {
                for (int j = 0; j < 16; j += 1)
                {
                    byte val = GameObjectManager.pico8.memory.Mget((int)celPos.X + i, (int)celPos.Y + j);
                    byte flag = (byte)GameObjectManager.pico8.memory.Fget(val);

                    if ((flag & 0b00000010) != 0)
                    {
                        GameObjectManager.pico8.memory.Mset((int)celPos.X + i, (int)celPos.Y + j, 0);
                    }

                    if ((flag & 0b00001000) != 0)
                    {
                        GameObjectFactory.CreateGameObject(val, new Vector2((int)celPos.X + i, (int)celPos.Y + j) * 8);
                    }
                }
            }
        }

        public static bool IsSolid(Vector2 celPos)
        {
            byte val = GameObjectManager.pico8.memory.Mget((int)celPos.X, (int)celPos.Y);
            byte flag = (byte)GameObjectManager.pico8.memory.Fget(val);

            return (flag & 0b00000100) != 0;
        }
    }
}
