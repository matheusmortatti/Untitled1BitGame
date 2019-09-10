using System;
using System.Collections.Generic;
using System.Text;
using Pico8_Emulator;
using Microsoft.Xna.Framework;

namespace SharedCode
{
    public static class Debug
    {
        private static Pico8<Color> _pico8;

        public static void SetPico8(Pico8<Color> pico8)
        {
            _pico8 = pico8;
        }

        public static void DrawLine(float x1, float y1, float x2, float y2, int col = 8)
        {
            _pico8.graphics.Line((int)x1, (int)y1, (int)x2, (int)y2, (byte)col);
        }

        public static void DrawRectangle(float x1, float y1, float x2, float y2, int col = 8)
        {
            _pico8.graphics.Rect((int)x1, (int)y1, (int)x2, (int)y2, (byte)col);
        }

        public static void DrawFilledRectangle(float x1, float y1, float x2, float y2, int col = 8)
        {
            _pico8.graphics.Rectfill((int)x1, (int)y1, (int)x2, (int)y2, (byte)col);
        }
    }
}
