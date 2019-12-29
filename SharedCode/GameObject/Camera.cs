using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using SharedCode.Misc;

namespace SharedCode {
	public class Camera : GameObject {
		private float speed = 15.0f;
		private double _shake;
		private int shakeSize = 2;

		private Random random;

		public static Matrix TranslationMatrix { get; private set; }
		public Camera(Vector2 position) : base(position) {
			transform.position = new Vector2((float)Math.Floor(position.X / 128) * 128,
																			 (float)Math.Floor(position.Y / 128) * 128);
			tags = new List<string> { "camera" };
			random = new Random();
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);

			var pi = GameObjectManager.playerInstance;

			if (pi != null) {
				Vector2 target = new Vector2((float)Math.Floor(pi.collisionBox.middle.X / 128) * 128,
																		 (float)Math.Floor(pi.collisionBox.middle.Y / 128) * 128);
				transform.position = (Vector2.SmoothStep(transform.position, target, speed * (float)gameTime.ElapsedGameTime.TotalSeconds));
			}

			// Round up if near because if the camera goes up in value, it will never reach the desired value
			// (because of smooth lerp) and the position is rounded down to integer.
			//transform.position = new Vector2(
			//        Misc.util.RoundIfNear(transform.position.X, (float)Math.Floor(transform.position.X / 128) * 128, 10e-2f),
			//        Misc.util.RoundIfNear(transform.position.Y, (float)Math.Floor(transform.position.X / 128) * 128, 10e-2f)
			//    );

			if (_shake > 0)
				_shake -= gameTime.ElapsedGameTime.TotalSeconds;

			// Update Camera's position
			var offX = _shake > 0 ? random.NextDouble() * shakeSize : 0;
			var offY = _shake > 0 ? random.NextDouble() * shakeSize : 0;
			GameManager.Pico8.Memory.drawState.Camera((int)(transform.position.X + offX), (int)(transform.position.Y + offY));
			DrawUtility.Camera((int)(transform.position.X + offX), (int)(transform.position.Y + offY));

			TranslationMatrix = Matrix.CreateTranslation(-(int)(transform.position.X + offX), -(int)(transform.position.Y + offY), 0);
		}

		public void MoveCamera(Vector2 position) {
			transform.position = position;
		}

		public void ResetCamera() {
			GameManager.Pico8.Memory.drawState.Camera();
			DrawUtility.Camera();
			TranslationMatrix = Matrix.CreateTranslation(0, 0, 0);
			GameManager.ResetCamera();
		}

		public void RestoreCamera() {
			GameManager.Pico8.Memory.drawState.Camera((int)transform.position.X, (int)transform.position.Y);
			DrawUtility.Camera((int)transform.position.X, (int)transform.position.Y);
			TranslationMatrix = Matrix.CreateTranslation(-(int)(transform.position.X), -(int)(transform.position.Y), 0);
			GameManager.RestoreCamera();
		}

		public void AddShake(double time) {
			_shake = Math.Max(_shake, time);
		}
	}
}
