using System;
using System.Collections.Generic;
using System.Text;

using SharedCode.Physics;
using SharedCode.Graphics;
using SharedCode.Misc;

using Microsoft.Xna.Framework;

namespace SharedCode
{
    public enum TimePieceStates { Exploding, Following }
    public class TimePieceStateMachine : StateMachine<TimePieceStates>
    {
        private TimePiece tp;
        public TimePieceStateMachine(TimePiece tp) : base(TimePieceStates.Exploding) { this.tp = tp; }

        void ExplodingInit(TimePieceStates previous)
        {
            //
            // Initial burst of the time piece.
            //

            tp.physics = (APhysics)tp.AddComponent(new SimpleGravity(/* friction */0.84f, /* gravity */1.5f));

            tp.physics.AddVelocity(tp.transform.direction * (300 + (float)GameManager.random.NextDouble() * 50));
            tp.transform.direction = Vector2.Zero;

            var time = 0.7 + GameManager.random.NextDouble() * 0.4;
            TaskScheduler.AddTask(() => 
            {
                if (tp.objectFollowing != null)
                    Init(TimePieceStates.Following);
                else
                    tp.done = true;
            }, time, time);
        }

        void ExplodingState(GameTime gameTime)
        {
            if(tp.physics.velocity == Vector2.Zero)
            {
                tp.physics.maxSpeed = 2;
                tp.transform.direction = new Vector2(0, 1);
            }
        }

        void FollowingInit(TimePieceStates previous)
        {
            tp.RemoveComponent<SimpleGravity>();
            tp.physics = (APhysics)tp.AddComponent(new TopDownPhysics(200, 25));
            tp.collisionBox = new Box(tp.transform.position, new Vector2(2, 2), true);
        }

        void FollowingState(GameTime gameTime)
        {
            tp.transform.direction = tp.objectFollowing.collisionBox.middle - tp.transform.position;

            if (tp.objectFollowing.done)
            {
                tp.done = true;
            }
        }
    }

    public class TimePiece : GameObject
    {
        public APhysics physics;
        public double time = 0;
        private TimePieceStateMachine stateMachine;
        public GameObject objectFollowing { get; set; }
        public TimePiece(Vector2 position, Vector2 initialDirection, double time, GameObject follow = null, byte col = 9) 
            : base(position)
        {
            AddComponent(new LineTrail(position, col));

            transform.direction = initialDirection;

            this.time = time;
            objectFollowing = follow;

            stateMachine = new TimePieceStateMachine(this);
            stateMachine.Init(TimePieceStates.Exploding);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            stateMachine.StateDo(gameTime);
        }

        public override void OnCollision(GameObject other)
        {
            base.OnCollisionEnter(other);

            if (objectFollowing != null && other.id == objectFollowing.id)
            {
                objectFollowing.lifeTime += time;
                done = true;
            }
        }

        public static List<TimePiece> SpawnParticles(int amount, Vector2 position, GameObject follow = null, byte col = 9)
        {
            List<TimePiece> l = new List<TimePiece>();
            for (int i = 0; i < amount; ++i)
            {
                var angle = GameManager.random.NextDouble() * Math.PI + Math.PI / 2;
                var dir = new Vector2((float)Math.Sin(angle), (float)Math.Cos(angle));
                var tp = new TimePiece(position, dir, 1, follow, col);
                l.Add((TimePiece)GameObjectManager.AddObject(tp));
            }

            return l;
        }
    }
}
