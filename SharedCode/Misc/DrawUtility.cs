using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedCode.Misc {
	public static class DrawUtility {

		private static SpriteBatch _sb;

		private static Vector2 camera;

		public static Color[] pico8Palette = {
            // Black
            new Color( 0, 0, 0 ),
            // Dark-blue
            new Color( 29, 43, 83 ),
            // Dark-purple
            new Color( 126, 37, 83 ),
            // Dark-green
            new Color( 0, 135, 81 ),
            // Brown
            new Color( 171, 82, 54 ),
            // Dark-gray
            new Color( 95, 87, 79 ),
            // Light-gray
            new Color( 194, 195, 199 ),
            // White
            new Color( 255, 241, 232 ),
            // Red
            new Color( 255, 0, 77 ),
            // Orange
            new Color( 255, 163, 0 ),
            // Yellow
            new Color( 255, 236, 39 ),
            // Green
            new Color( 0, 228, 54 ),
            // Blue
            new Color( 41, 173, 255 ),
            // Indigo
            new Color( 131, 118, 156 ),
            // Pink
            new Color( 255, 119, 168 ),
            // Peach
            new Color( 255, 204, 170 ),

            // Alternative Palette:

            new Color( 42, 24, 22 ),

						new Color( 17, 29, 53 ),

						new Color( 66, 33, 54 ),

						new Color( 15, 84, 91 ),

						new Color( 116, 47, 40 ),

						new Color( 72, 50, 63 ),

						new Color( 162, 136, 121 ),

						new Color( 242, 239, 124 ),

						new Color( 190, 17, 80 ),

						new Color( 255, 109, 36 ),

						new Color( 169, 231, 46 ),

						new Color( 0, 181, 68 ),

						new Color( 6, 89, 181 ),

						new Color( 117, 70, 102 ),

						new Color( 255, 110, 89 ),

						new Color( 255, 157, 128 )

				};

		public static void Init(SpriteBatch spriteBatch) {
			_sb = spriteBatch;

			camera = new Vector2();
		}

		public static void Camera(int x, int y) {
			camera.X = x;
			camera.Y = y;
		}

		public static void Pset(int x , int y, Color col) {
			LineBatch.DrawPixel(_sb, x, y, col);
		}

		public static void Pset(int x, int y, byte col) {
			LineBatch.DrawPixel(_sb, x, y, pico8Palette[col]);
		}

		public static void Line(int x1, int y1, int x2, int y2, byte col) {
			LineBatch.DrawLine(_sb, pico8Palette[col], new Vector2(x1, y1), new Vector2(x2, y2));
		}

		public static void Line(int x1, int y1, int x2, int y2, Color col) {
			LineBatch.DrawLine(_sb, col, new Vector2(x1, y1), new Vector2(x2, y2));
		}

	}

	/// <summary>
	/// Line Batch
	/// For drawing lines in a spritebatch
	/// </summary>
	static public class LineBatch {
		static private Texture2D _empty_texture;

		static public void Init(GraphicsDevice device) {
			_empty_texture = new Texture2D(device, 1, 1, false, SurfaceFormat.Color);
			_empty_texture.SetData(new[] { Color.White });
		}

		static public void DrawLine(SpriteBatch batch, Color color,
																Vector2 point1, Vector2 point2) {

			DrawLine(batch, color, point1, point2, 0);
		}

		static public void DrawPixel(SpriteBatch batch, int x, int y, Color col) {
			batch.Draw(_empty_texture, new Vector2(x, y), col);
		}

		/// <summary>
		/// Draw a line into a SpriteBatch
		/// </summary>
		/// <param name="batch">SpriteBatch to draw line</param>
		/// <param name="color">The line color</param>
		/// <param name="point1">Start Point</param>
		/// <param name="point2">End Point</param>
		/// <param name="Layer">Layer or Z position</param>
		static public void DrawLine(SpriteBatch batch, Color color, Vector2 point1,
																Vector2 point2, float Layer) {

			float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
			float length = (point2 - point1).Length();

			batch.Draw(_empty_texture, point1, null, color,
								 angle, Vector2.Zero, new Vector2(length, 1),
								 SpriteEffects.None, Layer);
		}
	}
}
