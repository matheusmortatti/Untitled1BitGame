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
                        new OldMan(position, spriteValue)
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
                case 7:
                    return GameObjectManager.AddObject(new Blob(position, spriteValue));
                case 55:
                    return GameObjectManager.AddObject(new Bat(position, spriteValue));
                case 21:
                    return GameObjectManager.AddObject(new Tortuga(position, spriteValue));
                case 122:
                    return GameObjectManager.AddObject(new Spike(position, spriteValue));
                case 42:
                    return GameObjectManager.AddObject(new Goose(position, spriteValue));
                case 11:
                    return GameObjectManager.AddObject(new Gate(position));
                case 12:
                    return GameObjectManager.AddObject(new Gate(position, Gate.OpenCondition.NO_ENEMIES));
                case 19:
                    return GameObjectManager.AddObject(new Snake(position, spriteValue));
                case 105:
                    return GameObjectManager.AddObject(new Key(position));
                case 78:
                    return GameObjectManager.AddObject(new Altar(position));
                default:
                    return GameObjectManager.AddObject(new GameObject(position, new Graphics.P8Sprite(spriteValue)));
            }
        }
    }
}
