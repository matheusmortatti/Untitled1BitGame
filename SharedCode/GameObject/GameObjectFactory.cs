using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace SharedCode
{
    public static class GameObjectFactory
    {
        public static GameObject CreateGameObject(int spriteValue, Vector2 position)
        {
            switch(spriteValue)
            {
                case 32:
                    return GameObjectManager.InstantiatePlayer(position);
                case 1:
                    return GameObjectManager.AddObject(
                        new OldMan(position)
                        );
                case 98:
                    return GameObjectManager.AddObject(
                        new FirePit(
                            position
                            )
                        );
                case 76:
                    return GameObjectManager.AddObject(
                        new Chimney(
                            position
                            )
                        );
                default:
                    return GameObjectManager.AddObject(new GameObject(position, new Graphics.P8Sprite(spriteValue)));
            }
        }
    }
}
