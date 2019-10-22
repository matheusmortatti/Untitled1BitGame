using System;
using System.Collections.Generic;
using System.Text;

using SharedCode.Physics;
using SharedCode.Graphics;
using SharedCode.Input;
using Microsoft.Xna.Framework;
using SharedCode.Misc;
using SharedCode.Particles;

namespace SharedCode {
	public enum PlayerStates { Walking, Running, Still, Attacking, Dying, Dead }

	public class PlayerStateMachine : StateMachine<PlayerStates> {
		private APhysics _walkingPhysics;
		private AInput _walkingInput;
		private AGraphics _walkingGraphics;

		private Player _player;

		public PlayerStateMachine(Player player) : base(PlayerStates.Walking) {
			_player = player;

			//
			// Setup components for walking state.
			//

			_walkingInput = new PlayerInput();
			_walkingPhysics = new TopDownPhysics(_player.WalkSpeed, _player.WalkSpeed / 2, 0.9f);
			_walkingGraphics = new P8TopDownAnimator(P8TopDownAnimator.AnimationMode.SIDES_ONLY);
			((P8TopDownAnimator)_walkingGraphics).RunLeft = new SpriteAnimation(new P8Sprite(33, 1, 1, true, false), 4, 0.3f);
			((P8TopDownAnimator)_walkingGraphics).IdleLeft = new SpriteAnimation(new P8Sprite(32, 1, 1, true, false), 1, 0.3f);
			((P8TopDownAnimator)_walkingGraphics).RunRight = new SpriteAnimation(new P8Sprite(33, 1, 1, false, false), 4, 0.3f);
			((P8TopDownAnimator)_walkingGraphics).IdleRight = new SpriteAnimation(new P8Sprite(32, 1, 1, false, false), 1, 0.3f);
		}

		void WalkingInit(PlayerStates previous) {
			if (previous != PlayerStates.Running) {
				_player.RemoveComponent<AGraphics>();
				_player.RemoveComponent<APhysics>();
				_player.RemoveComponent<AInput>();

				_player.AddComponent(_walkingGraphics);
				_player.AddComponent(_walkingPhysics);
				_player.AddComponent(_walkingInput);
			}

			_walkingPhysics.maxSpeed = _player.WalkSpeed;
			_walkingPhysics.acceleration = _player.WalkSpeed / 2;
		}
		void WalkingState(GameTime gameTime) { }

		private bool _instantiatedSmoke;
		void RunningInit(PlayerStates previous) {
			if (previous != PlayerStates.Walking) {
				_player.RemoveComponent<AGraphics>();
				_player.RemoveComponent<APhysics>();
				_player.RemoveComponent<AInput>();

				_player.AddComponent(_walkingGraphics);
				_player.AddComponent(_walkingPhysics);
				_player.AddComponent(_walkingInput);
			}

			_walkingPhysics.maxSpeed = _player.RunSpeed;
			_walkingPhysics.acceleration = _player.RunSpeed / 2;

			_instantiatedSmoke = false;
		}
		void RunningState(GameTime gameTime) { 
			if (_walkingPhysics.movingDirection != Vector2.Zero) {
				if (!_instantiatedSmoke) {
					for (int i = 0; i < 8; ++i) {
						var pos = new Vector2(
							_player.collisionBox.middle.X - _walkingPhysics.movingDirection.X * 4 + (float)GameManager.random.NextDouble() * 3 - 1.5f,
							_player.collisionBox.bottom - 1 + (float)GameManager.random.NextDouble() * 3 - 1.5f);
						var s = new Smoke(pos);
						s.SetRadius(1, 1.5f);
						ParticleManager.AddParticle(s);
					}

					_instantiatedSmoke = true;
				}

				_player.timeDecrease *= 1.25f;
			} else {
				_instantiatedSmoke = false;
			}
		}

		void StillInit(PlayerStates previous) {
			var physics = _player.GetComponent<TopDownPhysics>();
			physics.velocity = Vector2.Zero;
			_player.RemoveComponent<APhysics>();
		}

		void AttackingInit(PlayerStates previous) {

			if (!_player.CanAttack) {
				Init(PlayerStates.Walking);
				return;
			}


			//
			// Remove input so it can't move while atacking
			//

			_player.RemoveComponent<AInput>();

			//
			// Stop player.
			//

			var physics = _player.GetComponent<TopDownPhysics>();
			physics.velocity = Vector2.Zero;

			//
			// Figure out sword's direction.
			//

			Vector2 facingDir = _player.transform.direction == Vector2.Zero ? physics.facingDirection : _player.transform.direction;
			if (facingDir.X != 0) facingDir.Y = 0;
			if (facingDir != Vector2.Zero) facingDir.Normalize();

			//
			// Reset player's direction so it doesn't keep moving to the previous set direction.
			//

			_player.transform.direction = Vector2.Zero;

			//
			// Instantiate sword.
			//

			_player.swordInstance = new Sword(_player.transform.position + 8 * facingDir, facingDir);
			GameObjectManager.AddObject(_player.swordInstance);
			var ratio = (_player.lifeTime / _player.initialLifetime);
			if (ratio < 0.2) {
				_player.swordInstance.timeGivenAdjustment = 1f;
			} else if(ratio < 0.8) {
				_player.swordInstance.timeGivenAdjustment = 1.7f;
			} else {
				_player.swordInstance.timeGivenAdjustment = 2.3f;
			}
		}

		void AttackingState(GameTime gameTime) {
			if (_player.swordInstance == null || _player.swordInstance.done) {
				_player.swordInstance = null;
				base.Init(PlayerStates.Walking);
			}
		}

		private double deadTime = 1;
		void DyingInit(PlayerStates prev) {
			_player.RemoveComponent<AInput>();

			TaskScheduler.AddTask(() => {
				_player.fadeOut = true;
				_player.fadeOutTime = 2 * deadTime / 3;
			}, deadTime / 3, deadTime / 3, _player.id);

			TaskScheduler.AddTask(() => {
				Init(PlayerStates.Dead);
			}, deadTime, deadTime, _player.id);

			ControllerVibration.SetVibration(0, 1, 1, deadTime);
		}

		void DeadState(GameTime gameTime) {
			_player.done = true;
			GameManager.ResetOverworld();
		}
	}

	public class Player : GameObject {
		public Sword swordInstance { get; set; }
		public PlayerStateMachine stateMachine { get; private set; }

		public bool IsInvincible { get; private set; } = false;
		public bool CanAttack { get; private set; } = true;

		protected double invTime = 1;
		protected bool isInvisible = false;
		protected double invisibleTime = 0.1;
	
		public double initialLifetime = 100;

		public Interactable InteractableObject { get; set; }

		public float WalkSpeed { 
			get {
				return _baseSpeed;
			}
			private set { }
		}

		public float RunSpeed {
			get {
				return _baseSpeed * 1.5f;
			}
			private set { }
		}

		private float _baseSpeed = 20;

		public double timeDecrease;

		public static int spriteIndex { get; } = 32;
		public Player(Vector2 position)
				: base(position, new Box(position, new Vector2(6, 3), false, new Vector2(1, 4))) {
			tags = new List<string> { "player" };
			swordInstance = null;

			stateMachine = new PlayerStateMachine(this);
			stateMachine.Init(PlayerStates.Walking);

			lifeTime = initialLifetime;
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);

			depth = collisionBox.bottom;

			timeDecrease = gameTime.ElapsedGameTime.TotalSeconds;
			var ratio = (lifeTime / initialLifetime);
			if (ratio < 0.1) {
				timeDecrease *= (ratio / 0.1) * 0.5 + 0.5;
			} else if (ratio > 0.7) {
				timeDecrease *= (((ratio - 0.7) / 0.3) * 0.7 + 1);
			}

			stateMachine.StateDo(gameTime);

			var lastLT = lifeTime;
			lifeTime -= timeDecrease;
			if (lifeTime < 10 && Math.Floor(lastLT) != Math.Floor(lifeTime)) {
				ControllerVibration.SetVibration(0, 1, 1, 0.3);
			}

			if (lifeTime <= 0) {
				lifeTime = 0;
				stateMachine.Init(PlayerStates.Dying);
			}
		}

		public override void Draw() {
			if (isInvisible)
				return;

			base.Draw();
		}

		public double TakeHit(double hitAmount) {
			if (IsInvincible)
				return 0;

			IsInvincible = true;
			CanAttack = false;
			TaskScheduler.AddTask(() => { IsInvincible = false; isInvisible = false; }, invTime, invTime, this.id);
			TaskScheduler.AddTask(() => { CanAttack = true; }, invTime / 2, invTime / 2, this.id);
			TaskScheduler.AddTask(() => isInvisible = !isInvisible, invisibleTime, invTime - 0.1, this.id);

			lifeTime -= hitAmount;

			ControllerVibration.SetVibration(0, 1f, 1f, 0.2);
			GameManager.pico8.audio.Sfx(1);

			return lifeTime > 0 ? hitAmount : hitAmount + lifeTime;
		}

		public override void OnCollisionEnter(GameObject other) {
			base.OnCollisionEnter(other);
		}

		public void Interact() {
			if (InteractableObject == null) {
				stateMachine.Init(PlayerStates.Attacking);
			} else {
				InteractableObject.Interact();
			}
		}

		public void Run() {
			if (stateMachine.State != PlayerStates.Running) {
				stateMachine.Init(PlayerStates.Running);
			}
		}

		public void Walk() {
			if (stateMachine.State != PlayerStates.Walking) {
				stateMachine.Init(PlayerStates.Walking);
			}
		}

	}
}
