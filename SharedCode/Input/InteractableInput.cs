using System;
using System.Collections.Generic;
using System.Text;

using Pico8_Emulator;
using Microsoft.Xna.Framework;

namespace SharedCode.Input {
	class InteractableInput : AInput {
		public InteractableInput() {
		}

		public override void Update(GameObject gameObject, GameTime gameTime) {
			var interactable = (Interactable)gameObject;

			if ((bool)GameManager.pico8.Btnp(4)) {
				interactable.Interact();
				return;
			}
		}
	}
}
