using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace SharedCode.Graphics
{
    public class Circfill : AGraphics
    {
        public byte col { get; set; }
        public float radius { get; set; }

        public Circfill(float radius, byte col)
        {
            this.radius = radius;
            this.col = col;
        }

        public override void Draw(GameObject gameObject)
        {
            GameManager.pico8.Graphics.Circfill((int)gameObject.transform.position.X,
                                                (int)gameObject.transform.position.Y,
                                                this.radius,
                                                this.col);
        }

        public override void Update(GameObject gameObject, GameTime gameTime)
        {

        }
    }
}
