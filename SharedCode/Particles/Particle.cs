using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using SharedCode.Physics;
using SharedCode.Graphics;

using Microsoft.Xna.Framework;

namespace SharedCode.Particles
{
    public class Particle : GameObject
    {
        public double lifetime { get; private set; }
        public double timePassed;

        public Particle(APhysics physics, AGraphics graphics, Vector2 position, double lifetime) : base(position, physics, graphics)
        {
            this.lifetime = lifetime;
            // Only set task if lifetime is greater than zero. That way we can set lifetime to be
            // "forever" and set it to done when we see fit.
            if (lifetime >= 0)
                Misc.TaskScheduler.AddTask(() => done = true, lifetime, lifetime, this.id);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
