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
                if (value != position)
                {
                    _position = value;
                    UpdateBucket();
                }
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

        public Vector2[] corners
        {
            get
            {
                return new Vector2[]
                    {
                        new Vector2(position.X, position.Y),
                        new Vector2(position.X + size.X, position.Y),
                        new Vector2(position.X, position.Y + size.Y),
                        new Vector2(position.X + size.X, position.Y + size.Y),
                    };
            }
        }

        public bool isTrigger { get; set; }

        public static int gridSize { get; private set; } = 32;

        private static Dictionary<Vector2, List<Box>> buckets = new Dictionary<Vector2, List<Box>>();
        private List<Vector2> bucketKeys;

        // Reference to the GameObject that holds this box;
        public GameObject gameObject { get; set; }

        public Box(Vector2 position, Vector2 size)
        {
            bucketKeys = new List<Vector2>();

            this.position = position;
            this.size = size;
            this.isTrigger = false;
        }

        public Box(Vector2 position, Vector2 size, bool isTrigger) : this(position, size)
        {
            this.isTrigger = isTrigger;
        }

        ~Box()
        {
            CleanUp();
        }

        public void CleanUp()
        {
            foreach (var bk in bucketKeys)
                RemoveFromBucket(bk);
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
            foreach (var bk in bucketKeys)
            {
                foreach (var o in buckets[bk])
                {
                    if (o != this && !list.Contains(o) && Collided(o))
                        list.Add(o);
                }
            }

            return list;
        }

        public List<Box> CheckCollision(out Vector2 separationVector)
        {
            List<Box> list = CheckCollision();
            Vector2 sepv = Vector2.Zero;

            foreach (var o in list)
            {
                if (o.isTrigger)
                    continue;

                sepv += MoveOut(o);
            }

            // Tile Collision
            Vector2[] possibleTileCollisions = corners;
            Box tileBox = new Box(Vector2.Zero, new Vector2(8, 8));
            foreach(Vector2 p in possibleTileCollisions)
            {
                tileBox.position = new Vector2((int)Math.Floor(p.X / 8) * 8, (int)Math.Floor(p.Y / 8) * 8);
                byte val = GameObjectManager.pico8.memory.Mget((int)Math.Floor(tileBox.position.X / 8), (int)Math.Floor(tileBox.position.Y / 8));
                byte flag = (byte)GameObjectManager.pico8.memory.Fget(val);

                if ((flag & 0b00000100) != 0)
                {
                    sepv += MoveOut(tileBox);
                }
            }

            tileBox.CleanUp();

            separationVector = sepv;

            return list;
        }

        public Vector2 MoveOut(Box other)
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

            position += mv;

            return mv;
        }

        private void UpdateBucket()
        {
            List<Vector2> newBucketKeys = new List<Vector2>();
            Vector2[] bkCandidates = new Vector2[]
            {
                new Vector2((float)Math.Floor(position.X / gridSize), (float)Math.Floor(position.Y / gridSize)),
                new Vector2((float)Math.Floor((position.X + size.X) / gridSize), (float)Math.Floor(position.Y / gridSize)),
                new Vector2((float)Math.Floor(position.X / gridSize), (float)Math.Floor((position.Y + size.Y) / gridSize)),
                new Vector2((float)Math.Floor((position.X + size.X) / gridSize), (float)Math.Floor((position.Y + size.Y) / gridSize)),
            };

            foreach(var c in bkCandidates)
            {
                if (!newBucketKeys.Contains(c))
                    newBucketKeys.Add(c);
            }


            // Remove instance from old key, update key and add it to new key.
            foreach (var bk in bucketKeys)
            {
                RemoveFromBucket(bk);
            }

            bucketKeys = newBucketKeys;

            foreach (var bk in bucketKeys)
            {
                AddToBucket(bk);
            }
        }

        private void AddToBucket(Vector2 bucketKey)
        {
            if (!buckets.ContainsKey(bucketKey))
            {
                buckets.Add(bucketKey, new List<Box>());
            }

            if (!buckets[bucketKey].Contains(this))
                buckets[bucketKey].Add(this);
        }

        private void RemoveFromBucket(Vector2 bucketKey)
        {
            if (!buckets.ContainsKey(bucketKey))
                return;

            buckets[bucketKey]?.Remove(this);

            if (!buckets.ContainsKey(bucketKey))
                return;

            if (buckets[bucketKey].Count == 0)
            {
                buckets.Remove(bucketKey);
            }
        }
    }
}
