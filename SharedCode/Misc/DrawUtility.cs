using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedCode.Misc {
	public static class DrawUtility {

		private static SpriteBatch _sb;
		private static GraphicsDevice _gd;

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

		private static Color _drawColor;
		public static Color DrawColor {
			get { return _drawColor; }
			private set { if (value != null) _drawColor = value; }
		}

		public static Texture2D spriteSheet;

		private static ContentManager _content;
		private static Effect ditheringEffect;

		public static void Init(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, ContentManager content) {
			LineBatch.Init(graphicsDevice);

			_sb = spriteBatch;
			_content = content;
			_gd = graphicsDevice;

			spriteSheet = _content.Load<Texture2D>("Maps/1bitSheet");

			camera = new Vector2();
		}

		public static void Camera(int x = 0, int y = 0) {
			//camera.X = x;
			//camera.Y = y;
		}

		public static void Palt(byte? col = null, bool t = false) {
			if (!col.HasValue && !t) {
				SetTransparent(0);

				for (byte i = 1; i < 16; i++) {
					ResetTransparent(i);
				}

				return;
			}

			if (t) {
				SetTransparent(col.Value);
			}
			else {
				ResetTransparent(col.Value);
			}
		}

		private static void ResetTransparent(byte c) {
			pico8Palette[c].A = 255;
		}

		private static void SetTransparent(byte c) {
			pico8Palette[c].A = 0;
		}

		public static void Spr(int n, int x = 0, int y = 0, int w = 1, int h = 1, bool flipX = false, bool flipY = false) {
			var sprX = (n & 0x0f) << 3;
			var sprY = (n >> 4) << 3;

			x -= (int)camera.X;
			y -= (int)camera.Y;

			var spriteEffects = SpriteEffects.None;
			if (flipX) spriteEffects |= SpriteEffects.FlipHorizontally;
			if (flipY) spriteEffects |= SpriteEffects.FlipVertically;

			_sb.Draw(
				spriteSheet,
				new Rectangle(x, y, w * 8, h * 8), 
				new Rectangle(sprX, sprY, w * 8, h * 8), 
				Color.White, 
				0, 
				Vector2.Zero, 
				spriteEffects, 
				0);
		}

		public static void Sspr(int sx, int sy, int sw, int sh, int dx, int dy, int? dw = null, int? dh = null,
			bool flipX = false, bool flipY = false) {

			if (!dw.HasValue) {
				dw = sw;
			}
			if (!dh.HasValue) {
				dh = sh;
			}

			dx -= (int)camera.X;
			dy -= (int)camera.Y;

			var spriteEffects = SpriteEffects.None;
			if (flipX) spriteEffects |= SpriteEffects.FlipHorizontally;
			if (flipY) spriteEffects |= SpriteEffects.FlipVertically;

			_sb.Draw(
				spriteSheet,
				new Rectangle(dx, dy, dw.Value, dh.Value),
				new Rectangle(sx, sy, sw, sh),
				Color.White,
				0,
				Vector2.Zero,
				spriteEffects,
				0);
		}

		public static void Pset(int x, int y, Color? col = null) {
			x -= (int)camera.X;
			y -= (int)camera.Y;

			DrawColor = col ?? DrawColor;

			LineBatch.DrawPixel(_sb, x, y, DrawColor);
		}

		public static void Pset(int x, int y, byte? col = null) {
			x -= (int)camera.X;
			y -= (int)camera.Y;

			if (col != null) {
				DrawColor = pico8Palette[col.Value];
			}

			LineBatch.DrawPixel(_sb, x, y, DrawColor);
		}

		public static void Line(int x1, int y1, int x2, int y2, byte? col) {
			x1 -= (int)camera.X;
			y1 -= (int)camera.Y;
			x2 -= (int)camera.X;
			y2 -= (int)camera.Y;

			if (col != null) {
				DrawColor = pico8Palette[col.Value];
			}

			LineBatch.DrawLine(_sb, DrawColor, new Vector2(x1, y1), new Vector2(x2, y2));
		}

		public static void Line(int x1, int y1, int x2, int y2, Color? col = null) {
			x1 -= (int)camera.X;
			y1 -= (int)camera.Y;
			x2 -= (int)camera.X;
			y2 -= (int)camera.Y;

			DrawColor = col ?? DrawColor;

			LineBatch.DrawLine(_sb, DrawColor, new Vector2(x1, y1), new Vector2(x2, y2));
		}

		public static void Rect(int x0, int y0, int x1, int y1, byte? col = null) {
			Line(x0, y0, x1, y0, col);
			Line(x0, y0, x0, y1, col);
			Line(x1, y1, x1, y0, col);
			Line(x1, y1, x0, y1, col);
		}

		public static void Rectfill(int x0, int y0, int x1, int y1, byte? col = null) {
			if (y0 > y1) {
				util.Swap(ref y0, ref y1);
			}

			for (var y = y0; y <= y1; y++) {
				Line(x0, y, x1, y, col);
			}
		}

		public static void Circ(int x, int y, double r, byte? col = null) {
			if (col != null) {
				Circ(x, y, r, pico8Palette[col.Value]);
			}
			else {
				Circ(x, y, r, DrawColor);
			}
		}

		public static void Circ(int x, int y, double r, Color? col = null) {
			DrawCircle(x, y, (int)Math.Ceiling(r), DrawColor, false);
		}

		public static void Circfill(int x, int y, double r, byte? col = null) {
			if (col != null) {
				Circfill(x, y, r, pico8Palette[col.Value]);
			}
			else {
				Circfill(x, y, r, DrawColor);
			}
		}

		public static void Circfill(int x, int y, double r, Color col) {
			DrawColor = col;

			DrawCircle(x, y, (int)(r), DrawColor, true);
		}

		//
		// Pure draw functions.
		//

		private static void Plot4(int x, int y, int offX, int offY, Color c, bool fill) {
			if (fill) {
				Line((x - offX), (y + offY), (x + offX + 1), (y + offY), c);

				if (offY != 0) {
					Line((x - offX), (y - offY), (x + offX + 1), (y - offY), c);
				}
			}
			else {
				Pset((x - offX), (y + offY), c);
				Pset((x + offX), (y + offY), c);

				if (offY != 0) {
					Pset((x - offX), (y - offY), c);
					Pset((x + offX), (y - offY), c);
				}
			}
		}

		private static void DrawCircle(int posX, int posY, int r, Color col, bool fill) {
			var x = r;
			var y = 0;
			double err = 1 - r;

			while (y <= x) {
				Plot4(posX, posY, x, y, col, fill);

				if (err < 0) {
					err = err + 2 * y + 3;
				}
				else {
					if (x != y) {
						Plot4(posX, posY, y, x, col, fill);
					}

					x = x - 1;
					err = err + 2 * (y - x) + 3;
				}

				y = y + 1;
			}
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

			if (length < 1) length = 1;

			batch.Draw(_empty_texture, point1, null, color,
								 angle, Vector2.Zero, new Vector2(length, 1),
								 SpriteEffects.None, Layer);
		}
	}
}
