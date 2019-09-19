using System;
using System.Collections.Generic;
using System.Text;

using SharedCode.Physics;
using SharedCode.Graphics;
using SharedCode.Input;
using Microsoft.Xna.Framework;

namespace SharedCode
{
    public class Sword : GameObject
    {
        private double timePassed;

        private double lifetime = 0.5;
        private float repelSpeed = 10;

        public Sword(Vector2 position, Vector2 direction) : base(position, new TopDownPhysics(0, 0))
        {
            timePassed = 0;

            transform.direction = direction;

            AddComponent(new P8Sprite(direction.X != 0 ? 3 : 4, 1, 1, direction.X < 0 ? true : false, direction.Y > 0 ? true : false));

            collisionBox = new Box(position, new Vector2(8, 8), true);
            collisionBox.isTrigger = true;
            depth = 1000;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            timePassed += gameTime.ElapsedGameTime.TotalSeconds;

            if (timePassed > lifetime)
            {
                done = true;
            }

            if (timePassed > 2 * lifetime / 3 && !fadeOut)
            {
                fadeOut = true;
                fadeOutTime = lifetime - timePassed;
            }
        }

        public override void OnCollision(GameObject other)
        {
            base.OnCollision(other);

            if (other.tags.Contains("enemy"))
            {
                //
                // Add force to repel enemy.
                //

                Vector2 repelDir = transform.direction;

                other.GetComponent<APhysics>().AddVelocity(repelDir * repelSpeed);

                //
                // TODO(matheusmortatti) remove time from enemy and give to the player.
                //
            }
        }
    }
}
