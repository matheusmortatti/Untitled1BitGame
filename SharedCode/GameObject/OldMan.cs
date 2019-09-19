using System;
using System.Collections.Generic;
using System.Text;

using SharedCode.Physics;
using SharedCode.Graphics;

using Microsoft.Xna.Framework;

namespace SharedCode
{
    public class OldMan : GameObject
    {
        public OldMan(Vector2 position) : base(null, null, null, position, new Box(position, new Vector2(8, 8)))
        {
            _physics = new TopDownPhysics(0, 0, 10);
            _graphics = new P8TopDownAnimator((TopDownPhysics)_physics, P8TopDownAnimator.AnimationMode.SIDES_ONLY);
            ((P8TopDownAnimator)_graphics).RunLeft = new SpriteAnimation(new P8Sprite(0, 1, 1, true, false), 3, 0.4f);
            ((P8TopDownAnimator)_graphics).IdleLeft = ((P8TopDownAnimator)_graphics).RunLeft;
            ((P8TopDownAnimator)_graphics).RunRight = new SpriteAnimation(new P8Sprite(0, 1, 1, false, false), 3, 0.4f);
            ((P8TopDownAnimator)_graphics).IdleRight = ((P8TopDownAnimator)_graphics).RunRight;

            tags.Add("enemy");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            depth = collisionBox.bottom;
        }
    }
}
