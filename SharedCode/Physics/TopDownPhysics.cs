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
        public float speed { get; set; }
        public float acceleration { get; set; }
        public float friction { get; set; }

        public TopDownPhysics()
        {
            velocity = new Vector2();
            speed = 1;
            acceleration = 0.5f;
            friction = 0.5f;
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
                X = MathHelper.Lerp(velocity.X, speed * targetDirection.X, step.X),
                Y = MathHelper.Lerp(velocity.Y, speed * targetDirection.Y, step.Y)
            };

            velocity = Misc.util.RoundIfNear(velocity, Vector2.Zero, 1e-2f);

            // Update moving direction.
            movingDirection = velocity == Vector2.Zero ? Vector2.Zero : velocity;
            movingDirection.Normalize();

            // Update position.
            gameObject.transform.MoveTo(gameObject.transform.position + velocity);
        }
    }
}
