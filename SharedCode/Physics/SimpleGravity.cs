using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace SharedCode.Physics
{
    public class SimpleGravity : APhysics
    {
        public float gravity;
        public SimpleGravity(float friction, float gravity) : base()
        {
            this.gravity = gravity;
            this.friction = friction;
        }
        public override void Update(GameObject gameObject, GameTime gameTime)
        {
            velocity = new Vector2(velocity.X, velocity.Y + gravity);
            velocity *= friction;

            Move(velocity * (float)gameTime.ElapsedGameTime.TotalSeconds, gameObject);
        }

        public override void AddVelocity(Vector2 velocity)
        {
            this.velocity += velocity;
        }
    }
}
