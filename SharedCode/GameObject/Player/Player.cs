using System;
using System.Collections.Generic;
using System.Text;

using SharedCode.Physics;
using SharedCode.Graphics;
using SharedCode.Input;
using Microsoft.Xna.Framework;
using SharedCode.Misc;

namespace SharedCode
{
    public enum PlayerStates { Walking, Attacking }

    public class PlayerStateMachine : StateMachine<PlayerStates>
    {
        private APhysics walkingPhysics;
        private AInput walkingInput;
        private AGraphics walkingGraphics;

        private Player _player;

        public PlayerStateMachine(Player player) : base(PlayerStates.Walking)
        {
            _player = player;

            //
            // Setup components for walking state.
            //

            walkingInput = new PlayerInput();
            walkingPhysics = new TopDownPhysics(20, 10, 0.9f);
            walkingGraphics = new P8TopDownAnimator((TopDownPhysics)walkingPhysics, P8TopDownAnimator.AnimationMode.SIDES_ONLY);
            ((P8TopDownAnimator)walkingGraphics).RunLeft = new SpriteAnimation(new P8Sprite(33, 1, 1, true, false), 4, 0.3f);
            ((P8TopDownAnimator)walkingGraphics).IdleLeft = new SpriteAnimation(new P8Sprite(32, 1, 1, true, false), 1, 0.3f);
            ((P8TopDownAnimator)walkingGraphics).RunRight = new SpriteAnimation(new P8Sprite(33, 1, 1, false, false), 4, 0.3f);
            ((P8TopDownAnimator)walkingGraphics).IdleRight = new SpriteAnimation(new P8Sprite(32, 1, 1, false, false), 1, 0.3f);
        }

        void WalkingInit(PlayerStates previous)
        {
            _player.RemoveComponent<AGraphics>();
            _player.RemoveComponent<APhysics>();
            _player.RemoveComponent<AInput>();

            _player.AddComponent(walkingGraphics);
            _player.AddComponent(walkingPhysics);
            _player.AddComponent(walkingInput);
        }
        void WalkingState(GameTime gameTime) { }

        void AttackingInit(PlayerStates previous)
        {
            _player.RemoveComponent<AInput>();

            _player.transform.direction = Vector2.Zero;
            var physics = _player.GetComponent<TopDownPhysics>();
            physics.velocity = Vector2.Zero;

            Vector2 facingDir = physics.facingDirection;
            if (facingDir.X != 0) facingDir.Y = 0;
            if (facingDir != Vector2.Zero) facingDir.Normalize();

            _player.swordInstance = new Sword(_player.transform.position + 8 * facingDir, facingDir);
            GameObjectManager.AddObject(_player.swordInstance);
        }

        void AttackingState(GameTime gameTime)
        {
            if (_player.swordInstance == null || _player.swordInstance.done)
            {
                _player.swordInstance = null;
                base.Init(PlayerStates.Walking);
            }
        }
    }

    public class Player : GameObject
    {
        public Sword swordInstance { get; set; }
        public PlayerStateMachine stateMachine { get; private set; }
        public double lifeTime { get; set; } = 120;

        protected bool isInvincible = false;
        protected double invTime = 1;
        protected bool isInvisible = false;
        protected double invisibleTime = 0.2;

        public static int spriteIndex { get; } = 32;
        public Player(Vector2 position)
            : base(position, new Box(position, new Vector2(8, 4), false, new Vector2(0, 4)))
        {
            tags = new List<string>{"player"};
            swordInstance = null;

            stateMachine = new PlayerStateMachine(this);
            stateMachine.Init(PlayerStates.Walking);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            depth = collisionBox.bottom;

            stateMachine.StateDo(gameTime);

            var decrease = lifeTime < 10 ? gameTime.ElapsedGameTime.TotalSeconds / 1.5 : gameTime.ElapsedGameTime.TotalSeconds;
            lifeTime -= decrease;
        }

        public override void Draw()
        {
            if (isInvisible)
                return;

            base.Draw();
        }

        public double TakeHit(double hitAmount)
        {
            if (isInvincible)
                return 0;

            isInvincible = true;
            TaskScheduler.AddTask(() => isInvincible = false, invTime, invTime);
            TaskScheduler.AddTask(() => isInvisible = !isInvisible, invisibleTime, invTime);

            lifeTime -= hitAmount;

            return lifeTime > 0 ? hitAmount : hitAmount + lifeTime;
        }

    }
}
