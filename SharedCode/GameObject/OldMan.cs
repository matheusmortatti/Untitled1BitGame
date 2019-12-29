using System;
using System.Collections.Generic;
using System.Text;

using SharedCode.Physics;
using SharedCode.Graphics;

using Microsoft.Xna.Framework;
using SharedCode.Misc;

namespace SharedCode {
	public class OldMan : Enemy {
		public OldMan(Vector2 position, int spritePosition, Dictionary<string, string> properties) : base(position, new Box(position, new Vector2(8, 8)), spritePosition) {
			AddComponent(new TopDownPhysics(0, 0, 0.95f));
			var graphics = new P8TopDownAnimator(P8TopDownAnimator.AnimationMode.SIDES_ONLY);
			AddComponent(graphics);
			graphics.RunLeft = new SpriteAnimation(new P8Sprite(0, 1, 1, true, false), 3, 0.4f);
			graphics.IdleLeft = graphics.RunLeft;
			graphics.RunRight = new SpriteAnimation(new P8Sprite(0, 1, 1, false, false), 3, 0.4f);
			graphics.IdleRight = graphics.RunRight;

			doesDamage = false;

			if (properties != null && properties.ContainsKey("dialogue")) {
				var dialoguePos = position - new Vector2(8, 0);
				var dialogue = util.ParseDialogue(properties["dialogue"]);
				GameObjectManager.AddObject(new DialogueArea(dialoguePos, new Box(dialoguePos, new Vector2(24, 16)), this, dialogue));
			}
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);

			depth = collisionBox == null ? transform.position.Y : collisionBox.bottom;
		}
	}
}
