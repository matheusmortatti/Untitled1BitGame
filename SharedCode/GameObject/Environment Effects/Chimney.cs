using System;
using System.Collections.Generic;
using System.Text;

using System.Timers;

using SharedCode.Graphics;

using Microsoft.Xna.Framework;
using SharedCode.Particles;

namespace SharedCode
{
    public class Chimney : GameObject
    {
        Misc.TaskScheduler.Task smokeParticleTask;
        public Chimney(Vector2 position) : base(position)
        {
            //
            // Create object that will instantiate smoke particles every 30 milliseconds.
            //

            var random = new Random();

            smokeParticleTask = Misc.TaskScheduler.AddTask(() =>
            {
                var smoke = new Smoke(transform.position + new Vector2(3 + (float)random.NextDouble() + 0.5f, -1));
                smoke.SetRadius(0.95f, 1.95f);
                //smoke.SetRadiusDecreaseSpeed(0.1f);
                ParticleManager.AddParticle(smoke);
            }, 0.130, -1, this.id);
        }

        public override void CleanUp()
        {
            base.CleanUp();

            Misc.TaskScheduler.RemoveTask(smokeParticleTask);
            smokeParticleTask = null;
        }
    }
}
