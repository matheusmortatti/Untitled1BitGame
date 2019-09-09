using System;
using System.Collections.Generic;
using System.Text;
using SharedCode.Physics;
using SharedCode.Graphics;
using SharedCode.Input;
using Microsoft.Xna.Framework;

namespace SharedCode
{
    public class GameObject
    {
        private IPhysics _physics;
        private IGraphics _graphics;
        private IInput _input;

        public Transform transform { get; private set; }
        

        public GameObject(IPhysics physics, IGraphics graphics, IInput input)
        {
            _physics = physics;
            _graphics = graphics;
            _input = input;

            transform = new Transform(Vector2.Zero);
        }

        public GameObject(IPhysics physics, IGraphics graphics, IInput input, Vector2 position) : this(physics, graphics, input)
        {
            transform = new Transform(position);
        }

        public virtual void Update(GameTime gameTime)
        {
            _physics?.Update(this);
            _input?.Update(this);
            _graphics?.Update(this);
        }

        public virtual void Draw()
        {
            _graphics?.Draw(this);
        }
    }
}
