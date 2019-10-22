using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using SharedCode.Misc;
using SharedCode.Particles;
using SharedCode.Graphics;

namespace SharedCode {
	public class Altar : GameObject {

		private Key _key;
		public Altar(Vector2 position) : base(position) {
			_key = new Key(position + new Vector2(4, -2));
			GameObjectManager.AddObject(_key);

			TaskScheduler.AddTask(() => {
				var smoke = new Smoke(position + new Vector2(4 + (float)GameManager.random.NextDouble() * 8, 8));
				smoke.SetColor(9);
				smoke.SetRadius(1, 1.5f);
				ParticleManager.AddParticle(smoke);
			}, 0.1f, -1, this.id);

			AddComponent(new P8Sprite(78, 2, 2));

			depth = -1000;
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);

			if (_key.currentState != "Floating") {
				TaskScheduler.RemoveTasksUnderId(this.id);
			}
		}

		public override void OnDestroy() {
			base.OnDestroy();

			_key = null;
		}
	}
}
