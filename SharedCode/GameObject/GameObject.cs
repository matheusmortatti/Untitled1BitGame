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
        private List<Component> components = new List<Component>();
        public APhysics _physics { get; set; }
        public AGraphics _graphics { get; set; }
        public AInput _input { get; set; }

        public float depth { get; protected set; }
        public Transform transform { get; protected set; }

        private Box _collisionBox;
        public Box collisionBox
        {
            get { return _collisionBox; }
            protected set
            {
                _collisionBox = value;
                if (_collisionBox != null) _collisionBox.gameObject = this;
            }
        }

        /// <summary>
        /// Defines if object should stop existing or not.
        /// </summary>
        public bool done { get; protected set; }

        public List<string> tags;

        public List<GameObject> collidedLast { get; protected set; }

        /// <summary>
        /// Fade out variables.
        /// </summary>
        private double foTimePassed;
        public double fadeOutTime;
        public bool fadeOut;
        private int fillp;

        public GameObject(APhysics physics, AGraphics graphics, AInput input)
        {
            _physics = physics;
            _graphics = graphics;
            _input = input;

            AddComponent(physics);

            transform = new Transform(Vector2.Zero);
            collisionBox = null;
            collidedLast = new List<GameObject>();
            tags = new List<string>();
        }

        public GameObject(APhysics physics, AGraphics graphics, AInput input, Vector2 position) 
            : this(physics, graphics, input)
        {
            transform = new Transform(position);
            depth = position.Y;
        }

        public GameObject(APhysics physics, AGraphics graphics, AInput input, Vector2 position, Box collisionBox) 
            : this(physics, graphics, input, position)
        {
            this.collisionBox = collisionBox;
            if (collisionBox != null) this.collisionBox.position = transform.position;
        }

        public T GetComponent<T>()
        {
            foreach(var c in components)
            {
                if (c.GetType() is T)
                {
                    try
                    {
                        return (T)Convert.ChangeType(c, typeof(T));
                    }
                    catch (InvalidCastException)
                    {
                        return default;
                    }
                }
            }

            return default;
        }

        public Component AddComponent(Component component)
        {
            components.Add(component);
            return component;
        }

        public virtual void Update(GameTime gameTime)
        {
            _input?.Update(this, gameTime);
            _graphics?.Update(this, gameTime);
            _physics?.Update(this, gameTime);
            List<GameObject> collidedWith = ((TopDownPhysics)_physics)?.collidedWith;

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

            if (fadeOut)
            {
                foTimePassed += gameTime.ElapsedGameTime.TotalSeconds;
                if (foTimePassed < fadeOutTime / 2)
                {
                    fillp = 0b1010010110100101;
                }
                else
                {
                    fillp = 0b1111111111111111;
                }
            }
        }

        public virtual void Draw()
        {
            GameManager.pico8.memory.Fillp(fillp);
            _graphics?.Draw(this);
            GameManager.pico8.memory.Fillp();

            if (Debug.debugMode)
            {
                collisionBox?.DrawCollisionBox();
            }
        }

        public virtual void OnCollision(GameObject other) {  }
        public virtual void OnCollisionEnter(GameObject other) {  }
        public virtual void OnCollisionExit(GameObject other) {  }

        public void CleanUp()
        {
            _collisionBox?.CleanUp();
        }
    }
}
