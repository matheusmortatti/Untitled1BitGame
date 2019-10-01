using Microsoft.Xna.Framework;
using SharedCode.Misc;
using SharedCode.Physics;
using SharedCode.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedCode
{
    public enum TortugaStates { Wondering, Running, Vanishing }

    public class TortugaStateMachine : StateMachine<TortugaStates>
    {
        private Tortuga _tortuga;
        public TortugaStateMachine(Tortuga tortuga) : base(TortugaStates.Wondering)
        {
            _tortuga = tortuga;
        }

        public Vector2 targetPosition;
        Vector2 targetDir;
        float wonderingLengthMin = 10, wonderingLengthMax = 20;
        double newPositionInterval = 4;
        TaskScheduler.Task targetPosTask;
        void WonderingInit(TortugaStates previous)
        {
            _tortuga.GetComponent<APhysics>().maxSpeed = 10f;

            //
            // Chooses a new position after some time.
            //

            targetPosition = _tortuga.transform.position;

            Action func = () =>
            {
                var dir = GameManager.random.NextDouble() * 2 * Math.PI;
                var length = (float)GameManager.random.NextDouble() * (wonderingLengthMax - wonderingLengthMin) + wonderingLengthMin;
                targetPosition = new Vector2((float)Math.Sin(dir), (float)Math.Cos(dir)) * length + _tortuga.transform.position;
                targetDir = targetPosition - _tortuga.transform.position;
                if (targetDir != Vector2.Zero) targetDir.Normalize();
            };

            targetPosTask = TaskScheduler.AddTask(func, newPositionInterval, -1);
        }

        void WonderingState(GameTime gameTime)
        {
            _tortuga.transform.direction = targetPosition - _tortuga.transform.position;

            if (Vector2.Dot(_tortuga.transform.direction, targetDir) < 0)
            {
                _tortuga.transform.direction = Vector2.Zero;
            }

            var player = GameObjectManager.FindObjectWithTag("player");
            if (player != null && (player.transform.position - _tortuga.transform.position).LengthSquared() < 500)
            {
                Init(TortugaStates.Running);
                TaskScheduler.RemoveTask(targetPosTask);
            }
        }

        TaskScheduler.Task vanishingStateTask, directionChooserTask;
        double vanishingTime = 10, changeDirectionTime = 2;
        void RunningInit(TortugaStates previous)
        {
            _tortuga.GetComponent<APhysics>().maxSpeed = 15f;
            vanishingStateTask = TaskScheduler.AddTask(() => Init(TortugaStates.Vanishing), vanishingTime, vanishingTime);

            //
            // Choose a direction away from the player.
            //

            Action directionChooser = () =>
            {
                var player = GameObjectManager.FindObjectWithTag("player");
                if (player != null)
                {
                    var awayDir = _tortuga.transform.position - player.transform.position;

                    // Find a random direction vector that is the direction away from the player rotated
                    // by at most 90 or -90 degrees, making sure that the resulting vector is still facing
                    // away from the player.
                    var randAngle = GameManager.random.NextDouble() * Math.PI - Math.PI / 2;
                    _tortuga.transform.direction = new Vector2(
                        awayDir.X * (float)Math.Cos(randAngle) - awayDir.Y * (float)Math.Sin(randAngle),
                        awayDir.X * (float)Math.Sin(randAngle) + awayDir.Y * (float)Math.Cos(randAngle));
                }
            };

            directionChooser();

            directionChooserTask = TaskScheduler.AddTask(directionChooser, changeDirectionTime, -1);
        }

        void RunningState(GameTime gameTime)
        {
            var player = GameObjectManager.FindObjectWithTag("player");
            if (player != null && 
                Vector2.Dot(_tortuga.transform.position - player.transform.position, _tortuga.transform.direction) < 0)
            {
                var awayDir = _tortuga.transform.position - player.transform.position;

                // Find a random direction vector that is the direction away from the player rotated
                // by at most 90 or -90 degrees, making sure that the resulting vector is still facing
                // away from the player.
                var randAngle = GameManager.random.NextDouble() * Math.PI - Math.PI / 2;
                _tortuga.transform.direction = new Vector2(
                    awayDir.X * (float)Math.Cos(randAngle) - awayDir.Y * (float)Math.Sin(randAngle),
                    awayDir.X * (float)Math.Sin(randAngle) + awayDir.Y * (float)Math.Cos(randAngle));

                if ((player.transform.position - _tortuga.transform.position).LengthSquared() > 1000)
                {
                    Init(TortugaStates.Wondering);
                    TaskScheduler.RemoveTask(vanishingStateTask);
                    TaskScheduler.RemoveTask(directionChooserTask);
                }
            }
        }

        void VanishingInit(TortugaStates previous)
        {
            _tortuga.lifeTime = 0;
        }

        void VanishingState(GameTime gameTime)
        {

        }

        public void CleanUp()
        {
            TaskScheduler.RemoveTask(directionChooserTask);
            TaskScheduler.RemoveTask(vanishingStateTask);
            TaskScheduler.RemoveTask(targetPosTask);
        }
    }
    public class Tortuga : Enemy
    {
        private TortugaStateMachine _tortugaStateMachine;
        private float _animationLength = 0.3f;

        public Tortuga(Vector2 position) : base(position, new Box(position, new Vector2(8, 8)))
        {

            AddComponent(new TopDownPhysics(10, 10));

            var anim = new P8TopDownAnimator(P8TopDownAnimator.AnimationMode.SIDES_ONLY);

            anim.RunRight = new SpriteAnimation(new P8Sprite(22), 4, _animationLength);
            anim.IdleRight = new SpriteAnimation(new P8Sprite(21), 1, _animationLength);

            anim.RunLeft = new SpriteAnimation(new P8Sprite(22, 1, 1, true, false), 4, _animationLength);
            anim.IdleLeft = new SpriteAnimation(new P8Sprite(21, 1, 1, true, false), 1, _animationLength);

            AddComponent(anim);

            _tortugaStateMachine = new TortugaStateMachine(this);
            _tortugaStateMachine.Init(TortugaStates.Wondering);

            lifeTime = 200;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _tortugaStateMachine.StateDo(gameTime);
        }

        public override void OnCollision(GameObject other)
        {
            base.OnCollision(other);

            _tortugaStateMachine.targetPosition = this.transform.position;
        }

        public override void CleanUp()
        {
            base.CleanUp();

            _tortugaStateMachine.CleanUp();
            _tortugaStateMachine = null;
        }
    }
}
