using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace SharedCode.Input {
	class PlayerInput : AInput {
		public PlayerInput() {
		}

		public override void Update(GameObject gameObject, GameTime gameTime) {
			Vector2 direction = new Vector2();
			if ((bool)GameManager.pico8.Input.Btn(0)) {
				direction.X -= 1;
			}
			if ((bool)GameManager.pico8.Input.Btn(1)) {
				direction.X += 1;
			}

			if ((bool)GameManager.pico8.Input.Btn(2)) {
				direction.Y -= 1;
			}
			if ((bool)GameManager.pico8.Input.Btn(3)) {
				direction.Y += 1;
			}

			var player = (Player)gameObject;

			if ((bool)GameManager.pico8.Input.Btnp(4)) {
				player.Interact();
				return;
			}

			var runBtnPressed = (bool)GameManager.pico8.Input.Btn(4, 1);
			if (runBtnPressed && player.stateMachine.State != PlayerStates.Running) {
				player.Run();
			} else if (!runBtnPressed && player.stateMachine.State != PlayerStates.Walking) {
				player.Walk();
			}

			gameObject.transform.direction = direction;
		}
	}
}
