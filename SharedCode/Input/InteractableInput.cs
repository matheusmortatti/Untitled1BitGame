using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace SharedCode.Input {
	class InteractableInput : AInput {
		public InteractableInput() {
		}

		public override void Update(GameObject gameObject, GameTime gameTime) {
			var interactable = (Interactable)gameObject;

			//if ((bool)GameManager.Pico8.Input.Btnp(4)) {
			//	interactable.Interact();
			//	return;
			//}
		}
	}
}
