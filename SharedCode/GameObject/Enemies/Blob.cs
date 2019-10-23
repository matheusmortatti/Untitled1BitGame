using System;
using System.Collections.Generic;
using System.Text;

using SharedCode.Physics;
using SharedCode.Graphics;

using Microsoft.Xna.Framework;

namespace SharedCode
{
    public class Blob : Enemy
    {
        private Vector2 targetPosition;
        private float baseSpeed = 20f;
        public Blob(Vector2 position, int spriteIndex) : base(position, new Box(position, new Vector2(7, 4), false, new Vector2(0, 4)), spriteIndex)
        {
            var physics = new TopDownPhysics(baseSpeed, baseSpeed / 2, 0.97f);
            AddComponent(physics);

            var anim = new P8TopDownAnimator(P8TopDownAnimator.AnimationMode.SIDES_ONLY);
            anim.RunLeft = new SpriteAnimation(new P8Sprite(7), 3, 0.3f);
            anim.RunRight = anim.RunLeft;
            anim.IdleLeft = new SpriteAnimation(new P8Sprite(7), 1, 0.3f);
            anim.IdleRight = anim.IdleLeft;
            AddComponent(anim);

            targetPosition = Vector2.Zero;

            InitState("Still");

            AddComponent(new FillBar(new Vector2(0, -2), 8, 0, lifeTime));
        }

        void StillStateInit(string previous)
        {

        }

        void StillStateUpdate(GameTime gameTime)
        {
            var player = GameObjectManager.FindObjectWithTag("player");
            if (player != null && (player.transform.position - transform.position).LengthSquared() < 1000)
            {
                InitState("Following");
            }
        }

        void FollowingStateInit(string previous)
        {

        }

        void FollowingStateUpdate(GameTime gameTime)
        {
            if (GameObjectManager.playerInstance == null)
                return;

            targetPosition = GameObjectManager.playerInstance.transform.position;
            transform.direction = isInvincible ? Vector2.Zero : targetPosition - transform.position;

            var physics = GetComponent<APhysics>();

            if (physics == null)
                return;

            physics.maxSpeed = baseSpeed * ((float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * (2 * Math.PI) / 0.9f) + 1) / 2;
            physics.acceleration = physics.maxSpeed / 2;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
