using System;
using System.Collections.Generic;
using System.Text;

using SharedCode.Physics;
using SharedCode.Graphics;

using Microsoft.Xna.Framework;

namespace SharedCode.Particles
{
    public class TextParticle : Particle
    {
        public TextParticle(Vector2 position, string message, byte col) : base(new TopDownPhysics(0.8f, 0.8f), new Print(message, col), position, 0.5)
        {
            Misc.TaskScheduler.AddTask(() => {
                transform.direction = Vector2.Zero;
                fadeOut = true;
                fadeOutTime = lifetime / 2;
                }, lifetime / 2, lifetime / 2);
            transform.direction = new Vector2(0, -1);
        }
    }
}
