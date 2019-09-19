using System;
using System.Collections.Generic;
using System.Text;

using SharedCode.Physics;
using SharedCode.Graphics;

using Microsoft.Xna.Framework;

namespace SharedCode.Particles
{
    public class Smoke : Particle
    {
        private float radiusDecreaseSpeed;
        private float maxRadius, minRadius;
        private Random random;

        public Smoke(Vector2 position) : base(new TopDownPhysics(0.5f, 0.5f), new Circfill(2, 8), position, -1)
        {
            radiusDecreaseSpeed = 3f;
            maxRadius = 2.5f;
            minRadius = 1.8f;

            random = new Random();

            transform.direction = new Vector2((float)random.NextDouble()*0.4f-0.2f, -((float)random.NextDouble()+0.5f));

            ((TopDownPhysics)_physics).maxSpeed += random.Next(1);

            ((Circfill)_graphics).col = 7;
            ((Circfill)_graphics).radius = minRadius + (float)random.NextDouble() * (maxRadius - minRadius);
        }

        public void SetColor(byte col)
        {
            ((Circfill)_graphics).col = col;
        }

        public void SetRadius(float min, float max)
        {
            ((Circfill)_graphics).radius = min + (float)random.NextDouble() * (max - min);
        }

        public void SetMaxMoveSpeed(float speed)
        {
            ((TopDownPhysics)_physics).maxSpeed = speed;
        }

        public void SetRadiusDecreaseSpeed(float speed)
        {
            radiusDecreaseSpeed = speed;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            ((Circfill)_graphics).radius -= radiusDecreaseSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (((Circfill)_graphics).radius <= 0)
                this.done = true;
        }
    }
}
