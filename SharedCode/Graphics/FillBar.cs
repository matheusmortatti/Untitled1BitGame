using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace SharedCode.Graphics
{
    public class FillBar : AGraphics
    {
        private Vector2 _offset;
        private double _min, _max;
        private int _fullSize;
        private int _size;
        public byte col { get; set; } = 9;

        public FillBar(Vector2 offset, int size, double min, double max)
        {
            _offset = offset;
            _min = min;
            _max = max;
            _fullSize = size;
        }
        public override void Draw(GameObject gameObject)
        {
            if (_size == 0)
                return;

            var x = (int)(gameObject.transform.position.X + _offset.X);
            var y = (int)(gameObject.transform.position.Y + _offset.Y);
            GameManager.pico8.Graphics.Rectfill(x, y, x + _size, y + 1, col);
            GameManager.pico8.Graphics.Rect(x - 1, y - 1, x + _size + 1, y + 1, 0);
        }

        public override void Update(GameObject gameObject, GameTime gameTime)
        {
            _size = (int)Math.Ceiling((_fullSize * (gameObject.lifeTime - _min) / (_max - _min)));
        }
    }
}
