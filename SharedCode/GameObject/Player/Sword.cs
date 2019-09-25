using System;
using System.Collections.Generic;
using System.Text;

using SharedCode.Physics;
using SharedCode.Graphics;
using SharedCode.Particles;
using Microsoft.Xna.Framework;

namespace SharedCode
{
    public class Sword : GameObject
    {
        private double timePassed;

        private double lifetime = 0.5;
        private float repelSpeed = 80;

        public float damage { get; set; } = 10;

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

                this.collisionBox = null;
            }
        }

        public override void OnCollision(GameObject other)
        {
            base.OnCollision(other);

            if (other.tags.Contains("enemy"))
            {
                //
                // TODO(matheusmortatti) remove time from enemy and give to the player.
                //

                var inflicted = ((Enemy)other).TakeHit(damage);
                if (inflicted <= 0) return;

                inflicted = Math.Floor(inflicted);

                ((Camera)GameObjectManager.FindObjectWithTag("camera")).AddShake(0.1);

                TimePiece.SpawnParticles((int)Math.Ceiling(inflicted), other.collisionBox == null ? other.transform.position : other.collisionBox.middle);

#if DEBUG
                Debug.Log($"Damage Inflicted to {other.GetType().FullName} : {Math.Ceiling(inflicted).ToString()}");
#endif

                //
                // Add force to repel enemy.
                //

                Vector2 repelDir = transform.direction;

                other.GetComponent<APhysics>().velocity = (repelDir * repelSpeed);
            }
        }
    }
}
