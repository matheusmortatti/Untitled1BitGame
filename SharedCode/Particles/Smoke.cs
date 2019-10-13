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

        public Smoke(Vector2 position) : base(new TopDownPhysics(30, 30, 0.95f), new Circfill(2, 8), position, -1)
        {
            radiusDecreaseSpeed = 2f + (float)GameManager.random.NextDouble() * 2;
            maxRadius = 2.5f;
            minRadius = 1.8f;

            random = new Random();

            transform.direction = new Vector2((float)random.NextDouble()*0.4f-0.2f, -((float)random.NextDouble()+0.5f));

            GetComponent<TopDownPhysics>().maxSpeed += random.Next(1);

            GetComponent<Circfill>().col = 7;
            GetComponent<Circfill>().radius = minRadius + (float)random.NextDouble() * (maxRadius - minRadius);
        }

        public void SetColor(byte col)
        {
            GetComponent<Circfill>().col = col;
        }

        public void SetRadius(float min, float max)
        {
            GetComponent<Circfill>().radius = min + (float)random.NextDouble() * (max - min);
        }

        public void SetMaxMoveSpeed(float speed)
        {
            GetComponent<TopDownPhysics>().maxSpeed = speed;
        }

        public void SetRadiusDecreaseSpeed(float speed)
        {
            radiusDecreaseSpeed = speed;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            GetComponent<Circfill>().radius -= radiusDecreaseSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (GetComponent<Circfill>().radius <= 0)
                this.done = true;
        }
    }
}
