using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace SharedCode
{
    public class Transform
    {
        public Vector2 position { get; private set; }
        public Vector2 direction { get; set; }

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
