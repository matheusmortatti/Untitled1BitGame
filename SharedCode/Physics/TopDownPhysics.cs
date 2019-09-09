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
                Vector2 md = new Vector2(movingDirection.X, movingDirection.Y);
                md.Normalize();
                return md;
            }
            set
            {
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

        public void Update(GameObject gameObject)
        {
            
        }
    }
}
