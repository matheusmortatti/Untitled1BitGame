using Microsoft.Xna.Framework;
using SharedCode.Graphics;
using SharedCode.Physics;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedCode {
	public class Stairs : GameObject {
		
		public Stairs(Vector2 position, int spriteIndex) : base(position, new Box(position, new Vector2(16, 8))) {
			AddComponent(new P8Sprite(spriteIndex, 2, 1));
		}

		public override void OnCollisionEnter(GameObject other) {
			base.OnCollisionEnter(other);

			GameObjectManager.AddObject(new ScreenTransition(1, () => GameManager.ResetOverworld(), ScreenTransition.TransitionEffect.FadeOut));
			var p = GameObjectManager.FindObjectOfType<Player>();
			if (p != null) {
				p.isPaused = true;
			}
		}

	}
}
