using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace SharedCode.Graphics
{
    public class LineTrail : AGraphics
    {
        public byte col { get; set; }
        private Vector2 trailPos;
        private float trailSpeed = 5f;

        public LineTrail(Vector2 pos, byte col)
        {
            this.col = col;
            trailPos = new Vector2(pos.X, pos.Y);
        }

        public override void Draw(GameObject gameObject)
        {
            trailPos.X = Misc.util.Lerp(trailPos.X, gameObject.transform.position.X, (trailPos.X - gameObject.transform.position.X) / trailSpeed);
            trailPos.Y = Misc.util.Lerp(trailPos.Y, gameObject.transform.position.Y, (trailPos.Y - gameObject.transform.position.Y) / trailSpeed);
            GameManager.pico8.graphics.Line((int)gameObject.transform.position.X,
                                            (int)gameObject.transform.position.Y,
                                            (int)trailPos.X,
                                            (int)trailPos.Y,
                                            this.col);
        }

        public override void Update(GameObject gameObject, GameTime gameTime)
        {

        }
    }
}
