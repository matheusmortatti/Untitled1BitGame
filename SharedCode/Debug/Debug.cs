using System;
using System.Collections.Generic;
using System.Text;
using Pico8_Emulator;
using Microsoft.Xna.Framework;
using System.IO;

namespace SharedCode
{
    public static class Debug
    {
        private static Pico8<Color> _pico8;
        public static bool debugMode = false;
        public static string logPath;

        public static void Init(Pico8<Color> pico8)
        {
            _pico8 = pico8;

            if (!Directory.Exists("Debug"))
            {
                Directory.CreateDirectory("Debug/");
            }
            logPath = "Debug/log_" + Math.Floor(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds).ToString() + ".txt";
            if (!File.Exists(logPath))
            {
                File.Create(logPath).Close();
            }
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

        public static void Log(string message)
        {
            using (var s = File.AppendText(logPath))
            {
                s.WriteLine(message);
                s.Close();
            }
        }
    }
}
