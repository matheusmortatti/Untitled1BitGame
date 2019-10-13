using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace SharedCode.Graphics
{
    public class Print : AGraphics
    {
        public byte col { get; set; }
        public string message { get; set; }

        public Print(string message, byte col)
        {
            this.message = message;
            this.col = col;
        }

        public override void Draw(GameObject gameObject)
        {
            Vector2[] offsets = new Vector2[] {
                new Vector2(-1, -1),
                new Vector2(-1, 0),
                new Vector2(-1, 1),
                new Vector2(0, -1),
                new Vector2(0, 0),
                new Vector2(0, 1),
                new Vector2(1, -1),
                new Vector2(1, 0),
                new Vector2(1, 1),

            };
            foreach(var off in offsets)
            {
                GameManager.pico8.graphics.Print(message,
                                            (int)(gameObject.transform.position.X + off.X),
                                            (int)(gameObject.transform.position.Y + off.Y),
                                            0);
            }

            GameManager.pico8.graphics.Print(message, 
                                            (int)gameObject.transform.position.X,
                                            (int)gameObject.transform.position.Y,
                                            col);
        }

        public override void Update(GameObject gameObject, GameTime gameTime)
        {

        }
    }
}
