using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace SharedCode.Physics
{
    public class TopDownPhysics : IPhysics
    {
        /// <summary>
        /// Direction the entity is moving.
        /// </summary>
        private Vector2 _movingDirection;
        public Vector2 movingDirection
        {
            get
            {
                Vector2 md = new Vector2(_movingDirection.X, _movingDirection.Y);
                if (md != Vector2.Zero)
                    md.Normalize();
                return md;
            }
            set
            {
                if (value != Vector2.Zero)
                    value.Normalize();
                _movingDirection = value;
            }
        }

        /// <summary>
        /// Direction the entity is facing.
        /// </summary>
        private Vector2 _facingDirection;
        public Vector2 facingDirection
        {
            get
            {
                return _facingDirection;
            }
            set
            {
                _facingDirection = value;
            }
        }

        public Vector2 velocity { get; private set; }
        public float maxSpeed { get; set; }
        public float acceleration { get; set; }
        public float friction { get; set; }

        public TopDownPhysics(float maxSpeed, float accel)
        {
            velocity = new Vector2();
            this.maxSpeed = maxSpeed;
            acceleration = accel;
            friction = accel;
        }

        public TopDownPhysics(float maxSpeed, float accel, float friction) : this (maxSpeed, accel)
        {
            this.friction = friction;
        }

        public void Update(GameObject gameObject)
        {
            Vector2 targetDirection = new Vector2(gameObject.transform.direction.X, gameObject.transform.direction.Y);

            if (targetDirection != Vector2.Zero)
            {
                facingDirection = targetDirection;
            }

            Vector2 step = new Vector2(Math.Sign(movingDirection.X) != Math.Sign(targetDirection.X) ? friction : acceleration,
                                       Math.Sign(movingDirection.Y) != Math.Sign(targetDirection.Y) ? friction : acceleration);
            velocity = new Vector2
            {
                X = MathHelper.Lerp(velocity.X, maxSpeed * targetDirection.X, step.X),
                Y = MathHelper.Lerp(velocity.Y, maxSpeed * targetDirection.Y, step.Y)
            };

            velocity = Misc.util.RoundIfNear(velocity, Vector2.Zero, 1e-2f);

            // Update moving direction.
            movingDirection = new Vector2(velocity.X, velocity.Y);
            movingDirection.Normalize();

            // Don't try to move if velocity is zero.
            if (velocity == Vector2.Zero)
                return;

            // Update position if there is no collision box.
            if (gameObject.collisionBox == null)
            {
                gameObject.transform.MoveTo(gameObject.transform.position + velocity);
                return;
            }

            Box col = gameObject.collisionBox;
            Vector2 dest = gameObject.transform.position + velocity;
            Vector2 amount = new Vector2(velocity.X, velocity.Y);
            while (true)
            {
                if (amount == Vector2.Zero)
                    break;

                Vector2 moveStep = new Vector2(amount.X % 8, amount.Y % 8);
                amount -= moveStep;
                Vector2 nextPos = gameObject.transform.position + moveStep;

                col.position = nextPos;
                if (col.CheckCollision().Count != 0)
                {
                    amount = Vector2.Zero;
                    velocity = Vector2.Zero;
                }

                gameObject.transform.MoveTo(col.position);
            }
        }
    }
}
