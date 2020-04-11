using Microsoft.Xna.Framework;
using SharedCode.Misc;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedCode.Graphics {
	public class P8StrechedSprite : P8Sprite {
		public int screenWidth { get; set; }
		public int screenHeight { get; set; }

		public int spriteX, spriteY;

		public P8StrechedSprite(int spriteIndex) : base(spriteIndex) {
		}

		public P8StrechedSprite(int spriteIndex, int width, int height) : this(spriteIndex) {
			this.width = width;
			this.height = height;

			this.screenWidth = width;
			this.screenHeight = height;

			spriteX = spriteIndex % 16 * 8;
			spriteY = spriteIndex / 16 * 8;
		}

		public P8StrechedSprite(int spriteIndex, int width, int height, int screenWidth, int screenHeight) : this(spriteIndex, width, height) {
			this.screenWidth = screenWidth;
			this.screenHeight = screenHeight;
		}

		public P8StrechedSprite(int spriteIndex, int width, int height, int screenWidth, int screenHeight, bool flipX, bool flipY)
				: this(spriteIndex, width, height, screenWidth, screenHeight) {
			this.flipX = flipX;
			this.flipY = flipY;
		}

		public P8StrechedSprite(int spriteX, int spriteY, int width, int height) : base(spriteY / 16 * 16 + spriteX / 16) {
			this.width = width;
			this.height = height;

			this.screenWidth = width;
			this.screenHeight = height;

			this.spriteX = spriteX;
			this.spriteY = spriteY;
		}

		public P8StrechedSprite(int spriteX, int spriteY, int width, int height, int screenWidth, int screenHeight)
				: this(spriteX, spriteY, width, height) {
			this.screenWidth = screenWidth;
			this.screenHeight = screenHeight;
		}

		public P8StrechedSprite(int spriteX, int spriteY, int width, int height, int screenWidth, int screenHeight, bool flipX, bool flipY)
				: this(spriteX, spriteY, width, height, screenWidth, screenHeight) {
			this.flipX = flipX;
			this.flipY = flipY;
		}

		public override void Draw(GameObject gameObject) {
			//GameManager.Pico8.Graphics.Sspr(
			//		spriteX,
			//		spriteY,
			//		width,
			//		height,
			//		(int)gameObject.transform.position.X,
			//		(int)gameObject.transform.position.Y,
			//		screenWidth,
			//		screenHeight,
			//		flipX,
			//		flipY);

			DrawUtility.Sspr(
								spriteX,
								spriteY,
								width,
								height,
								(int)gameObject.transform.position.X,
								(int)gameObject.transform.position.Y,
								screenWidth,
								screenHeight,
								flipX,
								flipY);
		}

		public override void Update(GameObject gameObject, GameTime gameTime) {
		}
	}
}
