using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace SharedCode.Misc
{
    public static class util
    {
        public static bool NearlyEqual(float a, float b, float epsilon)
        {
            return Math.Abs(a - b) < epsilon;
        }

        public static float RoundIfNear(float a, float b, float epsilon)
        {
            if (NearlyEqual(a, b, epsilon))
                return b;
            return a;
        }

        public static Vector2 RoundIfNear(Vector2 a, Vector2 b, float epsilon)
        {
            return new Vector2(NearlyEqual(a.X, b.X, epsilon) ? b.X : a.X,
                               NearlyEqual(a.Y, b.Y, epsilon) ? b.Y : a.Y);
        }
    }
}
