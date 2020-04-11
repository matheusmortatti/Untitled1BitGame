using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace SharedCode.Misc {
	public static class util {
		public static bool NearlyEqual(float a, float b, float epsilon) {
			return Math.Abs(a - b) < epsilon;
		}

		public static float RoundIfNear(float a, float b, float epsilon) {
			if (NearlyEqual(a, b, epsilon))
				return b;
			return a;
		}

		public static Vector2 RoundIfNear(Vector2 a, Vector2 b, float epsilon) {
			return new Vector2(NearlyEqual(a.X, b.X, epsilon) ? b.X : a.X,
												 NearlyEqual(a.Y, b.Y, epsilon) ? b.Y : a.Y);
		}

		public static float Lerp(float val, float target, float step) {
			step = Math.Abs(step);
			return val < target ? Math.Min(val + step, target) : Math.Max(val - step, target);
		}

		public static T Choose<T>(params T[] p) {
			return p[GameManager.random.Next(p.Length)];
		}

		public static double PercentageOf(double value, double max) {
			return value / max;
		}

		public static Vector2 CorrespondingMapIndex(Vector2 position) {
			return new Vector2((float)Math.Floor(position.X / 128), (float)Math.Floor(position.Y / 128));
		}

		public static Vector2 CorrespondingCelIndex(Vector2 position) {
			return new Vector2((float)Math.Floor(position.X / 8), (float)Math.Floor(position.Y / 8));
		}

		public static void PrintDigits(int x1, int y1, double num, byte numCol = 7, bool border = false) {
			var digits = num.ToString().Length;
			int x2 = x1 + 3 * (int)digits + (int)digits - 1 + 3, y2 = y1 + 8;

			//GameManager.Pico8.Graphics.Rectfill(x1, y1, x2, y2, 0);

			//if (border)
				//GameManager.Pico8.Graphics.Rect(x1, y1, x2, y2, numCol);

			//GameManager.Pico8.Graphics.Print(num, x1 + 2, y1 + 2, numCol);
		}

		public static void Swap<T>(ref T lhs, ref T rhs) {
			var temp = lhs;

			lhs = rhs;
			rhs = temp;
		}

		public static List<string> ParseDialogue(string dialogue) {
			return new List<string>(dialogue.Split(';'));
		}
	}
}
