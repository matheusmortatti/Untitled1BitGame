using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace SharedCode.Graphics
{
    public abstract class AGraphics : Component
    {
        public abstract void Draw(GameObject gameObject);
    }
}
