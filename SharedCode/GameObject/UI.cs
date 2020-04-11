using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace SharedCode
{
    public class UI : GameObject
    {

        public UI() : base(Vector2.Zero)
        {
            depth = 10000;
        }

        public override void Draw()
        {
            base.Draw();

            var player = (Player)GameObjectManager.FindObjectWithTag("player");
            var camera = (Camera)GameObjectManager.FindObjectWithTag("camera");

            if (camera == null)
            {
                return;
            }

            camera.ResetCamera();

            var life = player == null ? 0 : Math.Floor(player.lifeTime);
            var digits = life.ToString().Length;
            int x1 = 4, y1 = 4, x2 = x1 + 3 * (int)digits + (int)digits - 1 + 3, y2 = y1 + 8;
            //GameManager.Pico8.Graphics.Rectfill(x1, y1, x2, y2, 0);
            //GameManager.Pico8.Graphics.Rect(x1, y1, x2, y2, 9);
            //GameManager.Pico8.Graphics.Print(life, x1 + 2, y1 + 2, 9);

            camera.RestoreCamera();
        }
    }
}
