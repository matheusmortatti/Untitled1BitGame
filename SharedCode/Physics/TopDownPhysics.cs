using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace SharedCode.Physics
{
    public class TopDownPhysics : APhysics
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
                {
                    value.Normalize();
                    facingDirection = value;
                }
                _movingDirection = value;
            }
        }

        /// <summary>
        /// Direction the entity is facing.
        /// </summary>
        private Vector2 _facingDirection = new Vector2(-1, 0);
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

        public Vector2 velocity { get; set; }
        public float maxSpeed { get; set; }
        public float acceleration { get; set; }
        public float friction { get; set; }

        private const int MOVE_STEP = 8;

        public TopDownPhysics(float maxSpeed, float accel)
        {
            velocity = new Vector2();
            this.maxSpeed = maxSpeed;
            acceleration = accel;
            friction = accel == 0 ? 0.5f : accel;

            collidedWith = new List<GameObject>();
        }

        public TopDownPhysics(float maxSpeed, float accel, float friction) : this (maxSpeed, accel)
        {
            this.friction = friction;
        }

        public override void Update(GameObject gameObject, GameTime gameTime)
        {
            collidedWith.Clear();
            Vector2 targetDirection = new Vector2(gameObject.transform.direction.X, gameObject.transform.direction.Y);

            if (targetDirection != Vector2.Zero)
            {
                facingDirection = targetDirection;
            }

            Vector2 step = new Vector2(Math.Sign(movingDirection.X) != Math.Sign(targetDirection.X) ? friction : acceleration,
                                       Math.Sign(movingDirection.Y) != Math.Sign(targetDirection.Y) ? friction : acceleration);
            velocity = new Vector2
            {
                X = Misc.util.Lerp(velocity.X, maxSpeed * targetDirection.X, step.X),
                Y = Misc.util.Lerp(velocity.Y, maxSpeed * targetDirection.Y, step.Y)
            };

            velocity = Misc.util.RoundIfNear(velocity, Vector2.Zero, 1e-2f);

            // Update moving direction.
            movingDirection = new Vector2(velocity.X, velocity.Y);
            movingDirection.Normalize();

            // Don't try to move if velocity is zero.
            //if (velocity == Vector2.Zero)
            //    return collidedWith;

            // Update position if there is no collision box.
            if (gameObject.collisionBox == null)
            {
                gameObject.transform.MoveTo(gameObject.transform.position + velocity);
                return;
            }

            Box col = gameObject.collisionBox;
            Vector2 amount = new Vector2(velocity.X, velocity.Y) * (float)gameTime.ElapsedGameTime.TotalSeconds;
            while (true)
            {
                // Move the collider by steps so it doesn't go through objects if
                // velocity is too high.
                Vector2 moveStep = new Vector2(amount.X > MOVE_STEP ? MOVE_STEP : amount.X, amount.Y > MOVE_STEP ? MOVE_STEP : amount.Y);
                amount -= moveStep;
                Vector2 nextPos = gameObject.transform.position + moveStep;

                // Move to the desired position, check collision and adjust the position
                // with the separation vector that was resulted.
                Vector2 sepvX;
                Vector2 sepvY;
                List<Box> others = new List<Box>();

                col.position = new Vector2(nextPos.X, col.position.Y);
                others.AddRange(col.CheckCollision(out sepvX));

                col.position = new Vector2(col.position.X, nextPos.Y);
                others.AddRange(col.CheckCollision(out sepvY));

                foreach (var box in others)
                {
                    if (!collidedWith.Contains(box.gameObject))
                        collidedWith.Add(box.gameObject);
                }

                // Zero velocity components if the entity has collided in that
                // component's direction.
                Vector2 adjustedVelocity = new Vector2(velocity.X, velocity.Y);
                if (sepvX.X != 0) { adjustedVelocity.X = 0; amount.X = 0; }
                if (sepvY.Y != 0) { adjustedVelocity.Y = 0; amount.Y = 0; }
                velocity = adjustedVelocity;

                if (amount == Vector2.Zero)
                    break;
            }

            // Move game object to collider's new position.
            gameObject.transform.MoveTo(col.position);

            return;
        }

        public override void AddVelocity(Vector2 velocity)
        {
            this.velocity += velocity;
        }
    }
}
