using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace SharedCode
{
    public class Transform
    {
        public Vector2 position { get; set; }

        private Vector2 _direction;
        public Vector2 direction
        {
            get
            {
                return _direction;
            }
            set
            {
                _direction = value;
                if (_direction != Vector2.Zero)
                    _direction.Normalize();
            }
        }

        public Transform(Vector2 position)
        {
            this.position = position;
            this.direction = new Vector2();
        }

        public void MoveTo(Vector2 position)
        {
            this.position = position;
        }
    }
}
