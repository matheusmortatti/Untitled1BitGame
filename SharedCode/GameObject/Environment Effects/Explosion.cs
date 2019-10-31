using Microsoft.Xna.Framework;
using SharedCode.Particles;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedCode {
	public class Explosion : GameObject {
		public int particleNumber = 20;
		public Explosion(Vector2 position) : base(position) {
			for (int i = 0; i < particleNumber; i += 1) {
				var smoke = new Smoke(transform.position + new Vector2(GameManager.random.Next(6) - 3, GameManager.random.Next(6) - 3));
				smoke.SetColor(Misc.util.Choose<byte>(7, 9));
				ParticleManager.AddParticle(smoke);
			}

			done = true;

			GameManager.Pico8.Audio.Sfx(2);
		}
	}
}
