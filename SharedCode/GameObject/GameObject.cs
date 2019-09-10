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
        public Box collisionBox { get; private set; }
        

        public GameObject(IPhysics physics, IGraphics graphics, IInput input)
        {
            _physics = physics;
            _graphics = graphics;
            _input = input;

            transform = new Transform(Vector2.Zero);
            collisionBox = new Box(transform.position, new Vector2(8, 8), false);
        }

        public GameObject(IPhysics physics, IGraphics graphics, IInput input, Vector2 position) 
            : this(physics, graphics, input)
        {
            transform = new Transform(position);
            this.collisionBox.position = transform.position;
        }

        public GameObject(IPhysics physics, IGraphics graphics, IInput input, Vector2 position, Box collisionBox) 
            : this(physics, graphics, input, position)
        {
            this.collisionBox = collisionBox;
            this.collisionBox.position = transform.position;
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
            Debug.DrawRectangle(collisionBox.position.X,
                                      collisionBox.position.Y,
                                      collisionBox.position.X + collisionBox.size.X,
                                      collisionBox.position.Y + collisionBox.size.Y);
        }
    }
}
