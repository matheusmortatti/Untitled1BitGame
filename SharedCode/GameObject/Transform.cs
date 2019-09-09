using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace SharedCode
{
    public class Transform
    {
        public Vector2 position { get; private set; }

        public Transform(Vector2 position)
        {
            this.position = position;
        }
    }
}
