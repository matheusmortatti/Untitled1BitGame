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

        public Transform transform { get; protected set; }

        private Box _collisionBox;
        public Box collisionBox { get { return _collisionBox; } private set { _collisionBox = value; _collisionBox.gameObject = this;  } }

        /// <summary>
        /// Defines if object should stop existing or not.
        /// </summary>
        public bool done { get; protected set; }

        public List<string> tags;

        public List<GameObject> collidedLast { get; protected set; }

        public GameObject(IPhysics physics, IGraphics graphics, IInput input)
        {
            _physics = physics;
            _graphics = graphics;
            _input = input;

            transform = new Transform(Vector2.Zero);
            collisionBox = new Box(transform.position, new Vector2(8, 8), false);
            collidedLast = new List<GameObject>();
            tags = new List<string>();
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
            this.collisionBox?.CleanUp();
            this.collisionBox = collisionBox;
            this.collisionBox.position = transform.position;
        }

        public virtual void Update(GameTime gameTime)
        {
            _input?.Update(this);
            _graphics?.Update(this, gameTime);
            List<GameObject> collidedWith = _physics?.Update(this, gameTime);

            if (collidedWith != null)
            {

                // Call collision code.
                foreach (var other in collidedWith)
                {
                    if (!collidedLast.Contains(other))
                    {
                        OnCollisionEnter(other);
                        other.OnCollisionEnter(this);
                    }

                    OnCollision(other);
                    other.OnCollision(this);
                }

                foreach (var other in collidedLast)
                {
                    if (!collidedWith.Contains(other))
                    {
                        OnCollisionExit(other);
                        other.OnCollisionExit(this);
                    }
                }

                collidedLast = collidedWith;
            }
        }

        public virtual void Draw()
        {
            _graphics?.Draw(this);

            if (Debug.debugMode)
            {
                Debug.DrawRectangle(_collisionBox.position.X, _collisionBox.position.Y,
                                    _collisionBox.position.X + _collisionBox.size.X,
                                    _collisionBox.position.Y + _collisionBox.size.Y);
            }
        }

        public virtual void OnCollision(GameObject other) {  }
        public virtual void OnCollisionEnter(GameObject other) {  }
        public virtual void OnCollisionExit(GameObject other) {  }

        public void CleanUp()
        {
            _collisionBox.CleanUp();
        }
    }
}
