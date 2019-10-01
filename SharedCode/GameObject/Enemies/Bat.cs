using Microsoft.Xna.Framework;
using SharedCode.Graphics;
using SharedCode.Misc;
using SharedCode.Physics;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedCode
{
    public enum BatStates { Wondering, Charging, Attacking }

    public class BatStateMachine : StateMachine<BatStates>
    {
        private Bat _bat;
        public BatStateMachine(Bat bat) : base(BatStates.Wondering) { _bat = bat; }

        public Vector2 targetPosition;
        Vector2 targetDir;
        float wonderingLengthMin = 10, wonderingLengthMax = 20;
        double newPositionInterval = 4;
        TaskScheduler.Task targetPosTask;

        void WonderingInit(BatStates previous)
        {
            var physics = _bat.GetComponent<APhysics>();
            physics.friction = 0.99f;
            physics.maxSpeed = _bat.baseSpeed / 4;
            physics.acceleration = physics.maxSpeed / 4;

            _bat.transform.direction = Vector2.Zero;

            //
            // Set correct animation.
            //

            _bat.RemoveComponent<AGraphics>();

            _bat.flyingAnim.Reset();
            _bat.flyingAnim.animationFrameLength = _bat.animationLength / (_bat.flyingAnim.spriteList.Count);

            _bat.AddComponent(_bat.flyingAnim);

            //
            // Chooses a new position after some time.
            //

            targetPosition = _bat.transform.position;

            Action func = () =>
            {
                var dir = GameManager.random.NextDouble() * 2 * Math.PI;
                var length = (float)GameManager.random.NextDouble() * (wonderingLengthMax - wonderingLengthMin) + wonderingLengthMin;
                targetPosition = new Vector2((float)Math.Sin(dir), (float)Math.Cos(dir)) * length + _bat.transform.position;
                targetDir = targetPosition - _bat.transform.position;
                if (targetDir != Vector2.Zero) targetDir.Normalize();
            };

            func();

            targetPosTask = TaskScheduler.AddTask(func, newPositionInterval, -1);
        }

        void WonderingState(GameTime gameTime)
        {
            _bat.transform.direction = targetPosition - _bat.transform.position;

            if (Vector2.Dot(_bat.transform.direction, targetDir) < 0)
            {
                _bat.transform.direction = Vector2.Zero;
            }

            _bat.transform.position += (
                new Vector2(
                    0,
                    _bat.amplitude * ((float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * (2 * Math.PI) / _bat.animationLength + Math.PI))
                    )
                );

            var player = GameObjectManager.playerInstance;
            if (player == null)
                return;

            if ((player.transform.position - _bat.transform.position).LengthSquared() < 1000)
            {
                Init(BatStates.Charging);
                TaskScheduler.RemoveTask(targetPosTask);
            }
        }

        void ChargingInit(BatStates previous)
        {
            TaskScheduler.AddTask(() => Init(BatStates.Attacking), 1.5f, 1.5f);

            var physics = _bat.GetComponent<APhysics>();
            physics.friction = 0.98f;
            physics.acceleration = _bat.baseSpeed / 10;
            physics.maxSpeed = _bat.baseSpeed / 5;

            //
            // Set correct animation.
            //

            //_bat.RemoveComponent<AGraphics>();

            //_bat.chargingAnim.Reset();
            //_bat.chargingAnim.animationFrameLength = _bat.animationLength;

            //_bat.AddComponent(_bat.chargingAnim);
        }

        void ChargingState(GameTime gameTime)
        {
            var player = GameObjectManager.playerInstance;

            if (player == null)
                Init(BatStates.Wondering);

            _bat.transform.direction = _bat.transform.position - player.transform.position;
        }

        Vector2 attackDir, targetPos;
        void AttackingInit(BatStates previous)
        {
            var physics = _bat.GetComponent<APhysics>();
            physics.friction = 0.98f;
            physics.acceleration = _bat.baseSpeed / 10;
            physics.maxSpeed = _bat.baseSpeed;

            var player = GameObjectManager.playerInstance;

            if (player == null)
                Init(BatStates.Wondering);

            targetPos = new Vector2(player.transform.position.X, player.transform.position.Y);
            attackDir = targetPos - _bat.transform.position;
            _bat.transform.direction = new Vector2(attackDir.X, attackDir.Y);

            //
            // Set correct animation.
            //

            _bat.RemoveComponent<AGraphics>();

            _bat.flyingAnim.Reset();
            _bat.flyingAnim.animationFrameLength = _bat.animationLength / (3 * _bat.flyingAnim.spriteList.Count);

            _bat.AddComponent(_bat.flyingAnim);
        }

        void AttackingState(GameTime gameTime)
        {
            var dir = targetPos - _bat.transform.position;
            if (Math.Sign(attackDir.X) != Math.Sign(dir.X) || Math.Sign(attackDir.Y) != Math.Sign(dir.Y))
            {
                _bat.transform.direction = Vector2.Zero;
                _bat.GetComponent<APhysics>().acceleration = _bat.baseSpeed / 150;
            }

            if (_bat.GetComponent<APhysics>().velocity == Vector2.Zero)
                Init(BatStates.Wondering);
        }
    }

    public class Bat : Enemy
    {
        BatStateMachine batStateMachine;
        public float baseSpeed = 60;
        public float amplitude = 0.3f;
        public float animationLength = 1.2f;

        public SpriteAnimation flyingAnim, chargingAnim;

        public Bat(Vector2 position) : base(position, new Box(position, new Vector2(8, 8)))
        {
            var physics = new TopDownPhysics(baseSpeed, 0, 1f);
            flyingAnim = new SpriteAnimation(new P8Sprite(53), 2, animationLength / 2);
            chargingAnim = new SpriteAnimation(new P8Sprite(55), 1, animationLength / 2);

            AddComponent(physics);

            batStateMachine = new BatStateMachine(this);
            batStateMachine.Init(BatStates.Wondering);

            List<string> prevTags = tags;
            prevTags.Add("ignore_tile");
            tags = prevTags;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            batStateMachine.StateDo(gameTime);
        }
    }
}
