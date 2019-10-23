using Microsoft.Xna.Framework;
using SharedCode.Physics;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedCode {
	public class DialogueArea : Interactable {

		private GameObject _follow;
		private Vector2 _positionOffset;
		private List<string> _messages;

		public DialogueArea(Vector2 position, Box collisionBox, GameObject follow, List<string> messages) : base(position, collisionBox) {
			_follow = follow;
			this.collisionBox.isTrigger = true;
			_messages = messages;

			if (_follow != null) {
				_positionOffset = position - _follow.transform.position;
			}
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);

			if (_follow != null) {
				if (_follow.done) {
					this.done = true;
					_follow = null;
					return;
				}

				transform.position = _follow.transform.position + _positionOffset;
				collisionBox.position = transform.position;
			}
		}

		public override void OnCollision(GameObject other) {
			base.OnCollision(other);

			if (other is Player) {
				var player = other as Player;
				player.InteractableObject = this;

				if (_follow != null) {
					_follow.transform.direction = player.transform.position - _follow.transform.position;
				}
			}
		}

		public override void OnCollisionExit(GameObject other) {
			base.OnCollisionExit(other);

			if (other is Player) {
				var player = other as Player;
				player.InteractableObject = null;
			}
		}

		public override void Interact() {
			base.Interact();

			// Instantiate Dialogue Box
			var dialogueBox = new DialogueBox(_messages);
			dialogueBox.SetEndAction(() => GameObjectManager.ResumeEveryone(null));
			GameObjectManager.AddObject(dialogueBox);

			GameObjectManager.PauseEveryone(new List<GameObject>() { dialogueBox });
		}
	}
}
