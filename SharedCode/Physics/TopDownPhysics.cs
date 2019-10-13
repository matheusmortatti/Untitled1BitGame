using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace SharedCode.Physics
{
    public class TopDownPhysics : APhysics
    {
        public TopDownPhysics(float maxSpeed, float accel) : base()
        {
            velocity = new Vector2();
            this.maxSpeed = maxSpeed;
            acceleration = accel;
            friction = 0.2f;
        }

        public TopDownPhysics(float maxSpeed, float accel, float friction) : this (maxSpeed, accel)
        {
            this.friction = friction;
        }

        public override void Update(GameObject gameObject, GameTime gameTime)
        {
            Vector2 targetDirection = new Vector2(gameObject.transform.direction.X, gameObject.transform.direction.Y);

            if (targetDirection != Vector2.Zero)
            {
                facingDirection = targetDirection;
            }

            Vector2 newVelocity = new Vector2
            {
                X = Misc.util.Lerp(velocity.X, maxSpeed * targetDirection.X, acceleration),
                Y = Misc.util.Lerp(velocity.Y, maxSpeed * targetDirection.Y, acceleration)
            };

            Vector2 newaAddedVelocity = new Vector2(addedVelocity.X, addedVelocity.Y);

            if (Math.Sign(movingDirection.X) != Math.Sign(targetDirection.X) || targetDirection.X == 0)
            {
                newVelocity.X *= friction;
                newaAddedVelocity.X *= friction;
            }

            if (Math.Sign(movingDirection.Y) != Math.Sign(targetDirection.Y) || targetDirection.Y == 0)
            {
                newVelocity.Y *= friction;
                newaAddedVelocity.Y *= friction;
            }

            velocity = Misc.util.RoundIfNear(newVelocity, Vector2.Zero, 1e-2f);
            addedVelocity = newaAddedVelocity;

            // Update moving direction.
            movingDirection = new Vector2(velocity.X, velocity.Y);
            movingDirection.Normalize();

            Vector2 amount = new Vector2(velocity.X + addedVelocity.X, velocity.Y + addedVelocity.Y) * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Move(amount, gameObject);
        }
    }
}
