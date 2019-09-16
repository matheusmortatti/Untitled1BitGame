using System;
using System.Collections.Generic;
using System.Text;

using SharedCode.Physics;
using SharedCode.Graphics;
using SharedCode.Input;
using Microsoft.Xna.Framework;

namespace SharedCode
{
    public class Player : GameObject
    {

        public Player(IPhysics physics, IGraphics graphics, IInput input, Vector2 position) 
            : base(physics, graphics, input, position, new Box(position, new Vector2(8, 8)))
        {
            tags.Add("player");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

        }

    }
}
