using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

using SharedCode.Graphics;
using SharedCode.Particles;

using Microsoft.Xna.Framework;

namespace SharedCode
{
    public class FirePit : GameObject
    {
        int particleNumber;
        public FirePit(Vector2 position) : base(position)
        {
            particleNumber = 1;

            var random = new Random();

            //
            // Create object that will instantiate smoke particles every 30 milliseconds.
            //

            Misc.TaskScheduler.AddTask(() =>
            {
                for (int i = 0; i < particleNumber; i += 1)
                {
                    var smoke = new Smoke(transform.position + new Vector2(4 + random.Next(2) - 1, 3 + random.Next(2) - 1));
                    smoke.SetColor(Misc.util.Choose<byte>(7, 9));
                    ParticleManager.AddParticle(smoke);
                }
            }, 0.04, -1);

        }
    }
}
