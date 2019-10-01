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
        public OldMan(Vector2 position) : base(position, new Box(position, new Vector2(8, 8)))
        {
            AddComponent(new TopDownPhysics(0, 0, 0.95f));
            var graphics = new P8TopDownAnimator(P8TopDownAnimator.AnimationMode.SIDES_ONLY);
            AddComponent(graphics);
            graphics.RunLeft = new SpriteAnimation(new P8Sprite(0, 1, 1, true, false), 3, 0.4f);
            graphics.IdleLeft = graphics.RunLeft;
            graphics.RunRight = new SpriteAnimation(new P8Sprite(0, 1, 1, false, false), 3, 0.4f);
            graphics.IdleRight = graphics.RunRight;

            tags = new List<string>() { "enemy", "nonpersistent" };
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            depth = collisionBox.bottom;
        }
    }
}
