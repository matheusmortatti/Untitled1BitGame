using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace SharedCode {
	public class Camera : GameObject {
		private float speed = 15.0f;
		private double _shake;
		private int shakeSize = 2;

		private Random random;
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
			GameManager.pico8.Camera((int)(transform.position.X + offX), (int)(transform.position.Y + offY));
		}

		public void MoveCamera(Vector2 position) {
			transform.position = position;
		}

		public void ResetCamera() {
			GameManager.pico8.memory.Camera();
		}

		public void RestoreCamera() {
			GameObjectManager.pico8.Camera((int)transform.position.X, (int)transform.position.Y);
		}

		public void AddShake(double time) {
			_shake = Math.Max(_shake, time);
		}
	}
}
