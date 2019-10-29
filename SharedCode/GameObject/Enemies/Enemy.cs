using System;
using System.Collections.Generic;
using System.Text;

using SharedCode.Physics;

using Microsoft.Xna.Framework;

using SharedCode.Misc;
using SharedCode.Physics;
using SharedCode.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SharedCode {

	public enum EnemyStates { Alive, Dead };

	public class EnemyStateMachine : StateMachine<EnemyStates> {
		private Enemy _enemy;
		public EnemyStateMachine(Enemy enemy) : base(EnemyStates.Alive) { _enemy = enemy; }

		void AliveInit(EnemyStates previous) { }

		void AliveState(GameTime gameTime) {
			if (_enemy.lifeTime <= 0) {
				Init(EnemyStates.Dead);
			}
		}

		private double deadTime = 0.7;
		void DeadInit(EnemyStates prev) {
			TaskScheduler.AddTask(() => _enemy.done = true, deadTime, deadTime, _enemy.id);

			_enemy.fadeOut = true;
			_enemy.fadeOutTime = 2 * deadTime / 3;

			var p = _enemy.GetComponent<APhysics>();
			if (p == null)
				return;

			p.maxSpeed /= 2;

			if (_enemy.deathType == Enemy.DeathType.HIT)
				_enemy.collisionBox = null;
			_enemy.doesDamage = false;
		}

		void DeadState(GameTime gameTime) {

		}
	}

	public class Enemy : GameObject {
		protected bool isInvincible = false;
		protected double invTime = 1;
		protected bool isInvisible = false;
		protected double invisibleTime = 0.2;
		public bool doesDamage = true;

		protected float repelSpeed = 200;
		protected double damage = 5;

		protected EnemyStateMachine stateMachine;

		protected Vector2 startPosition;

		protected int spriteIndex;

		protected double respawnTime = 40;

		protected double timePassageAdjustment = 0.75;

		public enum DeathType { TIME, HIT }
		public DeathType deathType { get; private set; }

		public Enemy(Vector2 position, Box collisionBox, int spriteIndex) : base(position, collisionBox) {
			tags = new List<string> { "enemy", "nonpersistent", "attackable" };
			ignoreSolidCollision.Add("player");

			stateMachine = new EnemyStateMachine(this);
			stateMachine.Init(EnemyStates.Alive);

			ignoreSolidCollision.Add("enemy");

			startPosition = new Vector2(position.X, position.Y);

			lifeTime = 15 + GameManager.random.NextDouble() * 5;

			this.spriteIndex = spriteIndex;
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);

			lifeTime -= gameTime.ElapsedGameTime.TotalSeconds * timePassageAdjustment;

			if (lifeTime <= 0)
				deathType = DeathType.TIME;

			stateMachine.StateDo(gameTime);

			//
			// Cap position to corners of the screen.
			//

			transform.position = new Vector2(
					MathHelper.Clamp(
							transform.position.X,
							(float)Math.Floor(startPosition.X / 128) * 128,
							(float)Math.Floor(startPosition.X / 128) * 128 + 120),
					MathHelper.Clamp(
							transform.position.Y,
							(float)Math.Floor(startPosition.Y / 128) * 128,
							(float)Math.Floor(startPosition.Y / 128) * 128 + 120)
					);

			depth = transform.position.Y;
		}

		public override void Draw() {
			if (isInvisible)
				return;

			base.Draw();
		}

		public virtual double TakeHit(double hitAmount) {
			if (isInvincible)
				return 0;

			isInvincible = true;
			TaskScheduler.AddTask(() => { isInvincible = false;  }, invTime, invTime, this.id);
			TaskScheduler.AddTask(() => isInvisible = !isInvisible, invisibleTime, invTime, this.id);

			if (doesDamage) {
				doesDamage = false;
				TaskScheduler.AddTask(() => { doesDamage = true; }, invTime / 2, invTime / 2, this.id);
			}

			lifeTime -= hitAmount;

			// If enemy is very close to being dead, kill it anyway
			if (lifeTime < 0.5) {
				lifeTime = 0;
			}

			if (lifeTime <= 0)
				deathType = DeathType.HIT;

			//
			// Play audio effect
			//

			GameManager.pico8.Audio.Sfx(0);

			//
			// Give a bonus time in the last hit.
			//

			var timeAmount = hitAmount;
			if (lifeTime <= 0.5) {
				ControllerVibration.SetVibration(0, 1f, 1f, 0.1);
				timeAmount *= 1.5f;
			}

			return Math.Ceiling(timeAmount);
		}

		protected List<TimePiece> timePiecesSpawned;
		public override void OnCollision(GameObject other) {
			base.OnCollisionEnter(other);

			if (doesDamage && other.tags.Contains("player")) {
				var inflicted = ((Player)other).TakeHit(damage);
				if (inflicted <= 0) return;

				var repelDir = Vector2.Zero;

				if (transform.direction == Vector2.Zero) {
					// if enemy is still, try to use the other GO's direction.
					if (other.transform.direction == Vector2.Zero) {
						var dir = other.transform.position - transform.position;

						// First try to use the direction between the two GOs
						// positions. If that is zero, use the other GO's
						// facing direction.
						if (dir == Vector2.Zero) {
							var physics = other.GetComponent<APhysics>();
							if (physics != null) {
								repelDir = -physics.facingDirection;
							}
						}
						else {
							dir.Normalize();
							repelDir = dir;
						}
					}
					else {
						repelDir = -other.transform.direction;
					}
				}
				else {
					repelDir = transform.direction;
				}

				other.GetComponent<APhysics>().velocity = repelDir * repelSpeed;
				timePiecesSpawned = TimePiece.SpawnParticles(
						(int)inflicted, other.collisionBox == null ? other.transform.position : other.collisionBox.middle,
						null,
						8);

				((Camera)GameObjectManager.FindObjectWithTag("camera")).AddShake(0.1);
			}
		}

		public override void OnDestroy() {
			base.OnDestroy();

			//
			// Schedule a respawn only if it has previously died.
			//

			if (lifeTime <= 0) {
				var celPos = new Vector2((float)Math.Floor(startPosition.X / 8), (float)Math.Floor(startPosition.Y / 8));
				GameManager.pico8.Memory.Mset((int)celPos.X, (int)celPos.Y, 0);
				TaskScheduler.AddTask(() => {
					GameManager.pico8.Memory.Mset((int)celPos.X, (int)celPos.Y, (byte)spriteIndex);
				}, respawnTime, respawnTime, -1
				);
			}
		}
	}
}
