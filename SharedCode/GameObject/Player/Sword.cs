﻿using System;
using System.Collections.Generic;
using System.Text;

using SharedCode.Physics;
using SharedCode.Graphics;
using SharedCode.Particles;
using Microsoft.Xna.Framework;

namespace SharedCode {
	public class Sword : GameObject {
		private double _timePassed;

		private float _repelSpeed = 80;

		public float timeGivenAdjustment = 1f;

		public float Damage { get; set; } = 4;

		public Sword(Vector2 position, Vector2 direction) : base(position, new TopDownPhysics(0, 0)) {
			_timePassed = 0;

			transform.direction = direction;

			AddComponent(new P8Sprite(direction.X != 0 ? 3 : 4, 1, 1, direction.X < 0 ? true : false, direction.Y > 0 ? true : false));

			collisionBox = new Box(position, new Vector2(8, 8), true);
			collisionBox.isTrigger = true;
			depth = 1000;

			List<string> newTags = new List<string>(tags);
			newTags.Add("player_attack");
			tags = newTags;

			lifeTime = 0.5;
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);

			_timePassed += gameTime.ElapsedGameTime.TotalSeconds;

			if (_timePassed > lifeTime) {
				done = true;
			}

			if (_timePassed > 2 * lifeTime / 3 && !fadeOut) {
				fadeOut = true;
				fadeOutTime = lifeTime - _timePassed;

				this.collisionBox = null;
			}
		}

		public override void OnCollision(GameObject other) {
			base.OnCollision(other);

			if (other.tags.Contains("attackable")) {
				//
				// TODO(matheusmortatti) remove time from enemy and give to the player.
				//

				var inflicted = ((Enemy)other).TakeHit(Damage);
				if (inflicted <= 0) return;

				//
				// Camera shake, brief pause and time particles.
				//

				((Camera)GameObjectManager.FindObjectWithTag("camera"))?.AddShake(0.1);

				if (other.lifeTime <= 0) {
					GameObjectManager.AddPause(0.2f);
				}

				TimePiece.SpawnParticles((int)Math.Ceiling(inflicted / timeGivenAdjustment), other.collisionBox == null ? other.transform.position : other.collisionBox.middle, GameObjectManager.playerInstance);

				Debug.Log($"Damage Inflicted to {other.GetType().FullName} : {Math.Ceiling(inflicted).ToString()}");

				//
				// Add force to repel enemy.
				//

				Vector2 repelDir = transform.direction;

				var physics = other.GetComponent<APhysics>();
				if (physics == null)
					Debug.Log($"{other.GetType().FullName} does not have a physics component. Attacking it does not send it back.");
				else
					physics.velocity = (repelDir * _repelSpeed);
			}
		}
	}
}
