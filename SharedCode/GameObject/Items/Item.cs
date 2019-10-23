using Microsoft.Xna.Framework;
using SharedCode.Physics;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedCode {
	public class Item : GameObject {

		public Item(Vector2 position, Box collisionBox) : base(position, collisionBox) {

		}

		public virtual void ExecuteItem(Player player) {

		}

	}
}
