using Microsoft.Xna.Framework;
using SharedCode.Physics;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedCode {
	public class Interactable : GameObject {

		public Interactable(Vector2 position, Box collisionBox) : base(position, collisionBox) {

		}

		public virtual void Interact() {

		}

		public virtual void SetEndAction(Action endAction) {

		}
	}
}
