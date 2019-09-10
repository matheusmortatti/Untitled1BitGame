using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace SharedCode.Physics
{
    public class Box
    {
        private Vector2 _position;
        public Vector2 position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
                UpdateBucket();
            }
        }

        private Vector2 _size;
        public Vector2 size
        {
            get
            {
                return _size;
            }
            set
            {
                _size = value;
            }
        }

        public float left
        {
            get
            {
                return position.X;
            }
        }

        public float right
        {
            get
            {
                return position.X + size.X;
            }
        }

        public float top
        {
            get
            {
                return position.Y;
            }
        }

        public float bottom
        {
            get
            {
                return position.Y + size.Y;
            }
        }

        public Vector2 middle
        {
            get
            {
                return new Vector2(position.X + size.X / 2, position.Y + size.Y / 2);
            }
        }

        public bool isTrigger { get; set; }

        public static int gridSize { get; private set; } = 32;

        private static Dictionary<Vector2, List<Box>> buckets = new Dictionary<Vector2, List<Box>>();
        private Vector2 bucketKey;

        public Box(Vector2 position, Vector2 size)
        {
            this.position = position;
            this.size = size;
            this.isTrigger = false;

            AddToBucket();
        }

        public Box(Vector2 position, Vector2 size, bool isTrigger) : this(position, size)
        {
            this.isTrigger = isTrigger;
        }

        ~Box()
        {
            RemoveFromBucket();
        }

        public bool Collided(Box other)
        {
            return this.right > other.left   && 
                   other.right > this.left && 
                   this.bottom > other.top   && 
                   other.bottom > this.top;
        }

        public List<Box> CheckCollision()
        {
            List<Box> list = new List<Box>();
            foreach (var o in buckets[this.bucketKey])
            {
                if (o != this && Collided(o))
                    list.Add(o);
            }

            if (!isTrigger)
            {
                foreach (var o in list)
                {
                    MoveOut(o);
                }
            }

            return list;
        }

        public void MoveOut(Box other)
        {
            Vector2[] candidates = { new Vector2(other.left - this.right, 0),
                                     new Vector2(other.right - this.left, 0),
                                     new Vector2(0, other.top - this.bottom),
                                     new Vector2(0, other.bottom - this.top)};

            float ml = float.MaxValue;
            Vector2 mv = Vector2.Zero;
            for (int i = 0; i < candidates.Length; i += 1)
            {
                if (candidates[i].LengthSquared() < ml)
                {
                    ml = candidates[i].LengthSquared();
                    mv = candidates[i];
                }
            }

            this.position += mv;
        }

        private void UpdateBucket()
        {
            Vector2 newBucketKey = new Vector2((float)Math.Floor(position.X / gridSize), (float)Math.Floor(position.Y / gridSize));
            if (newBucketKey == bucketKey)
                return;

            // Remove instance from old key, update key and add it to new key.
            RemoveFromBucket();
            bucketKey = newBucketKey;
            AddToBucket();
        }

        private void AddToBucket()
        {   
            if (!buckets.ContainsKey(this.bucketKey))
            {
                buckets.Add(this.bucketKey, new List<Box>());
            }

            if (!buckets[this.bucketKey].Contains(this))
                buckets[this.bucketKey].Add(this);
        }

        private void RemoveFromBucket()
        {
            if (!buckets.ContainsKey(this.bucketKey))
            {
                return;
            }

            buckets[this.bucketKey].Remove(this);

            if (buckets[this.bucketKey].Count == 0)
            {
                buckets.Remove(this.bucketKey);
            }
        }
    }
}
