﻿using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace SharedCode.Physics
{
    public abstract class APhysics : Component
    {
        public abstract void AddVelocity(Vector2 velocity);
    }
}
