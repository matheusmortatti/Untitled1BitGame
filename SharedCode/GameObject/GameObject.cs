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

        public bool isPaused { get; set; }

        public float depth { get; protected set; }
        public Transform transform { get; protected set; }

        public double lifeTime { get; set; }

        private Box _collisionBox;
        
        public int id { get; private set; }
        public Box collisionBox
        {
            get { return _collisionBox; }
            set
            {
                _collisionBox?.CleanUp();
                _collisionBox = value;
                if (_collisionBox != null) _collisionBox.gameObject = this;
            }
        }

        /// <summary>
        /// Defines if object should stop existing or not.
        /// </summary>
        public bool done { get; set; }

        private List<string> _tags;
        public List<string> tags
        {
            get
            {
                return new List<string>(_tags);
            }
            set
            {
                GameObjectManager.RemoveFromTagList(this);
                _tags = new List<string>(value);
                GameObjectManager.InsertInTagList(this);
            }
        }
        public List<string> ignoreSolidCollision;

        public List<GameObject> collidedLast { get; protected set; }

        /// <summary>
        /// Fade out variables.
        /// </summary>
        private double foTimePassed;
        public double fadeOutTime;
        public bool fadeOut;
        private int fillp;
        private static int lastId = -1;

        public GameObject(Vector2 position, params Component[] comps)
        {
            components.AddRange(comps);

            transform = new Transform(position);
            collisionBox = null;
            collidedLast = new List<GameObject>();
            ignoreSolidCollision = new List<string>();
            _tags = new List<string>();

            depth = position.Y;

            lastId += 1;
            id = lastId;
        }

        public GameObject(Vector2 position, Box collisionBox, params Component[] comps) 
            : this(position, comps)
        {
            this.collisionBox = collisionBox;
            if (collisionBox != null) this.collisionBox.position = transform.position;
        }

        public T GetComponent<T>()
        {
            foreach(var c in components)
            {
                if (c is T)
                {
                    try
                    {
                        return (T)((object)c);
                    }
                    catch (InvalidCastException)
                    {
                        return default;
                    }
                }
            }

            return default;
        }

        public List<T> GetComponents<T>()
        {
            List<T> res = new List<T>();
            foreach (var c in components)
            {
                if (c is T)
                {
                    try
                    {
                        res.Add((T)((object)c));
                    }
                    catch (InvalidCastException)
                    {
                        // Do nothing
                    }
                }
            }

            return res;
        }


        /// <summary>
        /// Returns whether or not the game object should stop moving
        /// after it collides to another object.
        /// </summary>
        public bool shouldIgnoreSolidCollision(GameObject other)
        {
            if (other == null) return false;

            foreach(var t in ignoreSolidCollision)
            {
                if (other.tags.Contains(t)) return true;
            }

            return false;
        }

        public Component AddComponent(Component component)
        {
            components.Add(component);
            return component;
        }

        public T RemoveComponent<T>()
        {
            for (int i = components.Count-1; i >= 0; i--)
            {
                if (components[i] is T)
                {
                    var comp = components[i];
                    components.RemoveAt(i);
                    try
                    {
                        return (T)((object)comp);
                    }
                    catch (InvalidCastException)
                    {
                        return default;
                    }
                }
            }

            return default;
        }

        public Component RemoveComponent(Component component)
        {
            components.Remove(component);
            return component;
        }

        public virtual void Update(GameTime gameTime)
        {
            for (int i = components.Count - 1; i >= 0; --i)
            {
                components[i].Update(this, gameTime);
            }

            List<GameObject> collidedWith = GetComponent<APhysics>()?.collidedWith;

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
            foreach (var obj in GetComponents<AGraphics>())
            {
                obj.Draw(this);
            }
            GameManager.pico8.memory.Fillp();

            if (Debug.debugMode)
            {
                collisionBox?.DrawCollisionBox();
            }
        }

        public virtual void OnCollision(GameObject other) {  }
        public virtual void OnCollisionEnter(GameObject other) {  }
        public virtual void OnCollisionExit(GameObject other) {  }

        public virtual void CleanUp()
        {
            _collisionBox?.CleanUp();
        }
    }
}
