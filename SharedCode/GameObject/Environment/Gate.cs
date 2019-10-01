using Microsoft.Xna.Framework;
using SharedCode.Graphics;
using SharedCode.Physics;
using SharedCode.Misc;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedCode
{
    public class Gate : GameObject
    {
        private int keysLeft = 0;

        public Gate(Vector2 position) : base(position, new Box(position, new Vector2(16, 16)))
        {
            InitState("Closed");
        }

        void ClosedStateInit(string previous)
        {
            AddComponent(new P8Sprite(11, 2, 2));
        }

        void ClosedStateUpdate(GameTime gameTime)
        {
            if (keysLeft <= 0)
                InitState("Opened");
        }

        void OpenedStateInit(string previous)
        {
            TaskScheduler.AddTask(() => 
            {
                GameObjectManager.AddObject(
                    new Explosion(
                        this.transform.position + new Vector2(
                            8 + (float)GameManager.random.NextDouble() * 8 -4, 
                            8 + (float)GameManager.random.NextDouble() * 8 - 4)));
                ((Camera)GameObjectManager.FindObjectWithTag("camera"))?.AddShake(0.1);
            },
            0.3, 1.8
            );

            TaskScheduler.AddTask(() =>
            {
                done = true;
                for (int i = 0; i < 4; i += 1)
                {
                    GameObjectManager.AddObject(
                    new Explosion(
                        this.transform.position + new Vector2(
                            8 + (float)GameManager.random.NextDouble() * 8 - 4,
                            8 + (float)GameManager.random.NextDouble() * 8 - 4)));
                }
                ((Camera)GameObjectManager.FindObjectWithTag("camera"))?.AddShake(0.4);
            },
            2, 2
            );
        }

        void OpenedStateUpdate(GameTime gameTime)
        {
           
        }

        public override void Draw()
        {
            base.Draw();

            switch(keysLeft)
            {
                case 3:
                    GameManager.pico8.graphics.Spr(13, (int)transform.position.X - 1, (int)transform.position.Y + 6);
                    goto case 2;
                case 2:
                    GameManager.pico8.graphics.Spr(13, (int)transform.position.X + 1 + 8, (int)transform.position.Y + 6);
                    goto case 1;
                case 1:
                    GameManager.pico8.graphics.Spr(13, (int)transform.position.X + 4, (int)transform.position.Y + 6);
                    break;
            }
        }

        public override void OnCollision(GameObject other)
        {
            base.OnCollision(other);

            if (other.tags.Contains("key"))
            {
                keysLeft -= 1;
            }
        }
    }
}
