using Microsoft.Xna.Framework;
using SharedCode.Graphics;
using SharedCode.Physics;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedCode {
	public class RunningBoots : GameObject {

		Vector2 _initialPosition;
		float _amplitude = 5;
		float _floatingSpeed = 1f;
		int _timeToGive = 20;

		public RunningBoots(Vector2 position) : base(position, new Box(position, new Vector2(8, 8), true)) {
			_initialPosition = new Vector2(transform.position.X, transform.position.Y);

			var anim = new SpriteAnimation(new P8Sprite(51), 2, (float)(1 / (2 * _floatingSpeed)));
			AddComponent(anim);
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);

			var sin = (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * 2 * Math.PI * _floatingSpeed + Math.PI);
			transform.position = new Vector2(
					_initialPosition.X,
					_initialPosition.Y + sin * _amplitude
			);
		}

		public override void OnCollisionEnter(GameObject other) {
			base.OnCollisionEnter(other);

			if (other is Player) {
				var player = (Player)other;

				// Kill the boots and give player run abillity
				player.EnableRunning();

				// Destroy and give seconds.
				done = true;
				TimePiece.SpawnParticles(_timeToGive, collisionBox == null ? transform.position : collisionBox.middle, GameObjectManager.playerInstance);

				// Instantiate Dialogue Box
				var dialogueBox =  new DialogueBox(new List<string>() { 
					"you've got the running boots! \n\npress [ RT / SHIFT ]\nto activate.", 
					"it will drain your time faster \nthough, so use carefully!" });
				dialogueBox.SetEndAction(() => GameObjectManager.ResumeEveryone(null));
				GameObjectManager.AddObject(dialogueBox);

				GameObjectManager.PauseEveryone(new List<GameObject>() { dialogueBox });
			}
		}

	}
}
