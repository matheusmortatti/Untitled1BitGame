using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace SharedCode.Physics
{
    public interface IPhysics
    {
        List<GameObject> Update(GameObject gameObject, GameTime gameTime);
    }
}
