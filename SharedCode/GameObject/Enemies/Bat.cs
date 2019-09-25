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

        void WonderingInit(BatStates previous)
        {
            var physics = _bat.GetComponent<APhysics>();
            physics.friction = 1;
            physics.acceleration = _bat.baseSpeed / 2;
        }

        void WonderingState(GameTime gameTime)
        {
            var physics = _bat.GetComponent<APhysics>();

            physics.AddVelocity(
                new Vector2(
                    0, 
                    _bat.amplitude * ((float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * (2 * Math.PI) / _bat.animationLength)) / 2
                    )
                );
        }

        void ChargingInit(BatStates previous)
        {

        }

        void ChargingState(GameTime gameTime)
        {

        }

        void AttackingInit(BatStates previous)
        {

        }

        void AttackingState(GameTime gameTime)
        {

        }
    }

    public class Bat : Enemy
    {
        BatStateMachine batStateMachine;
        public float baseSpeed = 10;
        public float amplitude = 15;
        public float animationLength = 1.2f;
        public Bat(Vector2 position) : base(position, new Box(position, new Vector2(8, 8)))
        {
            var physics = new TopDownPhysics(baseSpeed, 0, 1f);
            var anim = new SpriteAnimation(new P8Sprite(53), 2, animationLength / 2);

            AddComponent(physics);
            AddComponent(anim);

            batStateMachine = new BatStateMachine(this);
            batStateMachine.Init(BatStates.Wondering);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            batStateMachine.StateDo(gameTime);
        }
    }
}
