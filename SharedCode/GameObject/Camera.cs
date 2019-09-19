using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace SharedCode
{
    public class Camera : GameObject
    {
        private float speed = 5.0f;
        public Camera(Vector2 position) : base(position)
        {
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            var pi = GameObjectManager.playerInstance;

            if (pi != null)
            {
                Vector2 target = new Vector2((float)Math.Floor(pi.transform.position.X / 128) * 128,
                                             (float)Math.Floor(pi.transform.position.Y / 128) * 128);
                transform.position = (Vector2.Lerp(transform.position, target, speed * (float)gameTime.ElapsedGameTime.TotalSeconds));
            }

            // Round up if near because if the camera goes up in value, it will never reach the desired value
            // (because of smooth lerp) and the position is rounded down to integer.
            transform.position = new Vector2(
                    Misc.util.RoundIfNear(transform.position.X, (float)Math.Floor(transform.position.X / 128) * 128, 10e-2f),
                    Misc.util.RoundIfNear(transform.position.Y, (float)Math.Floor(transform.position.X / 128) * 128, 10e-2f)
                );

            // Update Camera's position
            GameObjectManager.pico8.Camera((int)transform.position.X, (int)transform.position.Y);
        }
    }
}
