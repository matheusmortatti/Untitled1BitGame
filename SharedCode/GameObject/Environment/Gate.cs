using Microsoft.Xna.Framework;
using SharedCode.Graphics;
using SharedCode.Physics;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedCode
{
    public class Gate : GameObject
    {
        private int keysLeft = 3;

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
