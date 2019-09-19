using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace SharedCode
{
    public abstract class Component
    {
        public abstract void Update(GameObject gameObject, GameTime gameTime);
    }
}
