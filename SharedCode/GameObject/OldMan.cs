using System;
using System.Collections.Generic;
using System.Text;

using SharedCode.Physics;
using SharedCode.Graphics;

using Microsoft.Xna.Framework;

namespace SharedCode
{
    public class OldMan : Enemy
    {
        public OldMan(Vector2 position, int spritePosition) : base(position, new Box(position, new Vector2(8, 8)), spritePosition)
        {
            AddComponent(new TopDownPhysics(0, 0, 0.95f));
            var graphics = new P8TopDownAnimator(P8TopDownAnimator.AnimationMode.SIDES_ONLY);
            AddComponent(graphics);
            graphics.RunLeft = new SpriteAnimation(new P8Sprite(0, 1, 1, true, false), 3, 0.4f);
            graphics.IdleLeft = graphics.RunLeft;
            graphics.RunRight = new SpriteAnimation(new P8Sprite(0, 1, 1, false, false), 3, 0.4f);
            graphics.IdleRight = graphics.RunRight;

            doesDamage = false;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            depth = collisionBox == null ? transform.position.Y : collisionBox.bottom;
        }
    }
}
