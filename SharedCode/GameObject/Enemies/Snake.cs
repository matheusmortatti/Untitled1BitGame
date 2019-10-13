using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

using SharedCode.Graphics;
using SharedCode.Physics;

namespace SharedCode
{
    public class Snake : Enemy
    {
        public Snake(Vector2 position, int spriteIndex) : base (position, new Box(position, new Vector2(8, 8)), spriteIndex)
        {

            //
            // Add animator
            //

            var anim = new P8TopDownAnimator(P8TopDownAnimator.AnimationMode.SIDES_ONLY);

            anim.RunLeft = new SpriteAnimation(new P8Sprite(19), 2, 0.3f + (float)GameManager.random.NextDouble() * 0.3f);
            anim.IdleLeft = anim.RunLeft;

            anim.RunRight = new SpriteAnimation(new P8Sprite(19, 1, 1, true, false), 2, 0.3f + (float)GameManager.random.NextDouble() * 0.3f);
            anim.IdleRight = anim.RunRight;

            AddComponent(anim);

            //
            // Add physics
            //

            AddComponent(new TopDownPhysics(0, 0, 0.94f));

            //
            // Adjust life time ( 1 hit kill )
            //

            lifeTime = 5 + GameManager.random.NextDouble() * 5;

            AddComponent(new FillBar(new Vector2(0, -2), 8, 0, lifeTime));
        }
    }
}
