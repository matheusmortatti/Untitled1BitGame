using System;
using System.Collections.Generic;
using System.Text;

using SharedCode.Physics;

using Microsoft.Xna.Framework;

using SharedCode.Misc;
using SharedCode.Physics;

namespace SharedCode
{

    public enum EnemyStates { Alive, Dead };

    public class EnemyStateMachine : StateMachine<EnemyStates>
    {
        private Enemy _enemy;
        public EnemyStateMachine(Enemy enemy) : base(EnemyStates.Alive) { _enemy = enemy; }

        void AliveInit(EnemyStates previous) { }

        void AliveState(GameTime gameTime)
        {
            if (_enemy.lifeTime <= 0)
            {
                Init(EnemyStates.Dead);
            }
        }

        private double deadTime = 1;
        void DeadInit(EnemyStates prev)
        {
            TaskScheduler.AddTask(() => _enemy.done = true, deadTime, deadTime);
            TaskScheduler.AddTask(() => {
                _enemy.fadeOut = true;
                _enemy.fadeOutTime = 2 * deadTime / 3;
            }, deadTime / 3, deadTime / 3);

            var p = _enemy.GetComponent<APhysics>();
            if (p == null)
                return;

            p.maxSpeed /= 2;

            //_enemy.collisionBox = null;
        }

        void DeadState(GameTime gameTime)
        {

        }
    }

    public class Enemy : GameObject
    {
        protected bool isInvincible = false;
        protected double invTime = 1;
        protected bool isInvisible = false;
        protected double invisibleTime = 0.2;

        protected float repelSpeed = 200;
        protected double damage = 5;

        protected EnemyStateMachine stateMachine;

        protected Vector2 startPosition;

        public Enemy(Vector2 position, Box collisionBox) : base(position, collisionBox)
        {
            tags = new List<string> { "enemy", "nonpersistent" };
            ignoreSolidCollision.Add("player");

            stateMachine = new EnemyStateMachine(this);
            stateMachine.Init(EnemyStates.Alive);

            ignoreSolidCollision.Add("enemy");

            startPosition = new Vector2(position.X, position.Y);

            lifeTime = 30 + (new Random()).NextDouble() * 5;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            lifeTime -= gameTime.ElapsedGameTime.TotalSeconds;
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
        }

        public override void Draw()
        {
            if (isInvisible)
                return;

            base.Draw();
        }

        public virtual double TakeHit(double hitAmount)
        {
            if (isInvincible)
                return 0;

            isInvincible = true;
            TaskScheduler.AddTask(() => isInvincible = false, invTime, invTime);
            TaskScheduler.AddTask(() => isInvisible = !isInvisible, invisibleTime, invTime);

            lifeTime -= hitAmount;

            return Math.Ceiling(lifeTime > 0 ? hitAmount : hitAmount + lifeTime);
        }

        public override void OnCollision(GameObject other)
        {
            base.OnCollisionEnter(other);

            if (other.tags.Contains("player"))
            {
                var inflicted = ((Player)other).TakeHit(damage);
                if (inflicted <= 0) return;

                var repelDir = Vector2.Zero;

                if (transform.direction == Vector2.Zero)
                {
                    // if enemy is still, try to use the other GO's direction.
                    if (other.transform.direction == Vector2.Zero)
                    {
                        var dir = other.transform.position - transform.position;

                        // First try to use the direction between the two GOs
                        // positions. If that is zero, use the other GO's
                        // facing direction.
                        if (dir == Vector2.Zero)
                        {
                            var physics = other.GetComponent<APhysics>();
                            if (physics != null)
                            {
                                repelDir = -physics.facingDirection;
                            }
                        }
                        else
                        {
                            dir.Normalize();
                            repelDir = dir;
                        }
                    }
                    else
                    {
                        repelDir = -other.transform.direction;
                    }
                }
                else
                {
                    repelDir = transform.direction;
                }

                other.GetComponent<APhysics>().velocity = repelDir * repelSpeed;
                var tps = TimePiece.SpawnParticles(
                    (int)inflicted, other.collisionBox == null ? other.transform.position : other.collisionBox.middle, 
                    null,
                    8);

                ((Camera)GameObjectManager.FindObjectWithTag("camera")).AddShake(0.1);
            }
        }
    }
}
