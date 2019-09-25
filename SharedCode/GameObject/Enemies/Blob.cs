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
        public Blob(Vector2 position) : base(position, new Box(position, new Vector2(7, 4), false, new Vector2(0, 4)))
        {
            var physics = new TopDownPhysics(baseSpeed, baseSpeed / 2, 0.95f);
            AddComponent(physics);

            var anim = new P8TopDownAnimator(physics, P8TopDownAnimator.AnimationMode.SIDES_ONLY);
            anim.RunLeft = new SpriteAnimation(new P8Sprite(7), 3, 0.3f);
            anim.RunRight = anim.RunLeft;
            anim.IdleLeft = new SpriteAnimation(new P8Sprite(7), 1, 0.3f);
            anim.IdleRight = anim.IdleLeft;
            AddComponent(anim);

            targetPosition = Vector2.Zero;
        }

        public override void Update(GameTime gameTime)
        {
            if (GameObjectManager.playerInstance == null)
                return;

            targetPosition = GameObjectManager.playerInstance.transform.position;
            transform.direction = isInvincible ? Vector2.Zero : targetPosition - transform.position;

            var physics = GetComponent<APhysics>();

            physics.maxSpeed = baseSpeed * ((float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * (2 * Math.PI) / 0.9f ) + 1) / 2;
            physics.acceleration = physics.maxSpeed / 2;

            base.Update(gameTime);
        }
    }
}
