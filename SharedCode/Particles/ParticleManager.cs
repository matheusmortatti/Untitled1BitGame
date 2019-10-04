using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace SharedCode.Particles
{
    public static class ParticleManager
    {
        private static List<Particle> activeParticles = new List<Particle>();
        private static List<Particle> nextParticles = new List<Particle>();

        public static int numberOfParticles
        {
            get
            {
                return activeParticles.Count;
            }
        }

        public static void Update(GameTime gameTime)
        {
            Parallel.ForEach(activeParticles, (particle) => { particle.Update(gameTime); });

            for(int i = activeParticles.Count - 1; i >= 0; --i)
            {
                if (activeParticles[i].done) activeParticles.RemoveAt(i);
            }

            activeParticles.AddRange(nextParticles);
            nextParticles.Clear();
        }
        public static void Draw()
        {
            foreach (var particle in activeParticles)
            {
                particle.Draw();
            }
        }

        public static Particle AddParticle(Particle particle)
        {
            nextParticles.Add(particle);
            return particle;
        }

        public static void RemoveAllParticles()
        {
            foreach(var p in activeParticles)
            {
                p.CleanUp();
            }

            foreach (var p in nextParticles)
            {
                p.CleanUp();
            }

            activeParticles.Clear();
            nextParticles.Clear();
        }

    }
}
