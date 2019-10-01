using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using SharedCode.Misc;
using SharedCode.Physics;
using SharedCode.Graphics;
using SharedCode.Particles;

namespace SharedCode
{
    public enum SpikeStates { Down, Half, Full }

    public class SpikeStateMachine : StateMachine<SpikeStates>
    {
        private Spike _spike;
        public SpikeStateMachine(Spike spike) : base(SpikeStates.Down)
        {
            _spike = spike;
            this.Init(SpikeStates.Down);
        }

        double changeStateTime = 1;

        void DownInit(SpikeStates previous)
        {
            _spike.sprite.index = 120;
            _spike.collisionBox = null;

            TaskScheduler.AddTask(() => Init(SpikeStates.Half), changeStateTime, changeStateTime);
        }

        void DownState(GameTime gameTime)
        {

        }

        void HalfInit(SpikeStates previous)
        {
            _spike.sprite.index = 121;
            _spike.collisionBox = null;

            TaskScheduler.AddTask(() => Init(SpikeStates.Full), changeStateTime, changeStateTime);
        }

        void HalfState(GameTime gameTime)
        {

        }

        int particleNumber = 10;
        void FullInit(SpikeStates previous)
        {
            _spike.sprite.index = 122;

            for (int i = 0; i < particleNumber; i += 1)
            {
                var smoke = 
                    new Smoke(_spike.transform.position + new Vector2(1 + GameManager.random.Next(6), GameManager.random.Next(6)));
                smoke.SetRadius(1, 1.5f);
                smoke.SetMaxMoveSpeed(0.2f + (float)GameManager.random.NextDouble() * 0.5f);
                smoke.SetRadiusDecreaseSpeed(3f + (float)GameManager.random.NextDouble());
                ParticleManager.AddParticle(smoke);
            }

            _spike.collisionBox = new Box(_spike.transform.position, new Vector2(8, 8));

            ((Camera)GameObjectManager.FindObjectWithTag("camera"))?.AddShake(0.1);

            TaskScheduler.AddTask(() => Init(SpikeStates.Down), changeStateTime, changeStateTime);
        }

        void FullState(GameTime gameTime)
        {

        }
    }

    public class Spike : Enemy
    {
        private SpikeStateMachine spikeStateMachine;

        public P8Sprite sprite { get; private set; }

        public Spike(Vector2 position) : base(position, null)
        {
            sprite = new P8Sprite(120);
            AddComponent(sprite);

            spikeStateMachine = new SpikeStateMachine(this);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // never dies, nor takes damage
            lifeTime = double.MaxValue;

            spikeStateMachine.StateDo(gameTime);
        }

        public override double TakeHit(double hitAmount)
        {
            return 0;
        }
    }
}
