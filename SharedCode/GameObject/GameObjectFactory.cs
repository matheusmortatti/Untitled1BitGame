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
                    return GameObjectManager.AddObject(new GameObject(null, new Graphics.P8Sprite(1), null, position));
                default:
                    return new GameObject(null, null, null);
            }
        }
    }
}
