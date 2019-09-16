using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace SharedCode.Graphics
{
    public interface IGraphics
    {
        void Draw(GameObject gameObject);
        void Update(GameObject gameObject, GameTime gameTime);
    }
}
