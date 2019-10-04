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
                return position.X + offset.X;
            }
        }

        public float right
        {
            get
            {
                return position.X + size.X + offset.X;
            }
        }

        public float top
        {
            get
            {
                return position.Y + offset.Y;
            }
        }

        public float bottom
        {
            get
            {
                return position.Y + size.Y + offset.Y;
            }
        }

        public Vector2 middle
        {
            get
            {
                return new Vector2(position.X + offset.X + size.X / 2, position.Y + offset.Y + size.Y / 2);
            }
        }

        public Vector2[] corners
        {
            get
            {
                return new Vector2[]
                    {
                        new Vector2(position.X + offset.X, position.Y + offset.Y),
                        new Vector2(position.X + offset.X + size.X, position.Y + offset.Y),
                        new Vector2(position.X + offset.X, position.Y + offset.Y + size.Y),
                        new Vector2(position.X + offset.X + size.X, position.Y + offset.Y + size.Y),
                    };
            }
        }

        public bool isTrigger { get; set; }

        public Vector2 offset { get; set; } = Vector2.Zero;

        public static int gridSize { get; private set; } = 32;

        private static Dictionary<Vector2, List<Box>> buckets = new Dictionary<Vector2, List<Box>>();
        private List<Vector2> bucketKeys;

        // Reference to the GameObject that holds this box;
        public GameObject gameObject { get; set; }

        public Box(Vector2 position, Vector2 size)
        {
            bucketKeys = new List<Vector2>();

            this.size = size;
            this.position = position;
            this.isTrigger = false;
        }

        public Box(Vector2 position, Vector2 size, bool isTrigger) : this(position, size)
        {
            this.isTrigger = isTrigger;
        }

        public Box(Vector2 position, Vector2 size, bool isTrigger, Vector2 offset) : this(position, size, isTrigger)
        {
            this.offset = offset;
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
            lock (buckets)
            {
                foreach (var bk in bucketKeys)
                {
                    if (!buckets.ContainsKey(bk)) continue;
                    foreach (var o in buckets[bk])
                    {
                        if (o != this && !list.Contains(o) && Collided(o))
                            list.Add(o);
                    }
                }
            }

            return list;
        }

        public List<Box> CheckCollision(out Vector2 separationVector)
        {
            List<Box> list = CheckCollision();
            Vector2 sepv = Vector2.Zero;

            if (this.isTrigger)
                goto cleanup;

            foreach (var o in list)
            {
                if (o.isTrigger)
                    continue;

                if (gameObject.shouldIgnoreSolidCollision(o.gameObject) ||
                    o.gameObject.shouldIgnoreSolidCollision(gameObject))
                    continue;

                sepv += MoveOut(o);
            }

            //
            // Tile Collision
            //

            if (gameObject.tags.Contains("ignore_tile"))
                goto cleanup;

            Vector2[] possibleTileCollisions = corners;
            Box tileBox = new Box(Vector2.Zero, new Vector2(8, 8));
            foreach(Vector2 p in possibleTileCollisions)
            {
                tileBox.position = new Vector2((int)Math.Floor(p.X / 8) * 8, (int)Math.Floor(p.Y / 8) * 8);
                if (Map.IsSolid(new Vector2((int)Math.Floor(tileBox.position.X / 8), (int)Math.Floor(tileBox.position.Y / 8))))
                {
                    sepv += MoveOut(tileBox);
                }
            }

            tileBox.CleanUp();

            cleanup:

            separationVector = sepv;

            return list;
        }

        public Vector2 MoveOut(Box other, bool[] allowed = null)
        {
            Vector2[] candidates = { new Vector2(other.left - this.right, 0),
                                     new Vector2(other.right - this.left, 0),
                                     new Vector2(0, other.top - this.bottom),
                                     new Vector2(0, other.bottom - this.top)};
            if (allowed == null) allowed = new bool[] { true, true, true, true };

            float ml = float.MaxValue;
            Vector2 mv = Vector2.Zero;
            for (int i = 0; i < candidates.Length; i += 1)
            {
                if (allowed[i] && candidates[i].LengthSquared() < ml)
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
                new Vector2((float)Math.Floor((position.X + offset.X) / gridSize), (float)Math.Floor((position.Y + offset.Y) / gridSize)),
                new Vector2((float)Math.Floor((position.X + offset.X + size.X) / gridSize), (float)Math.Floor((position.Y + offset.Y) / gridSize)),
                new Vector2((float)Math.Floor((position.X + offset.X) / gridSize), (float)Math.Floor((position.Y + offset.Y + size.Y) / gridSize)),
                new Vector2((float)Math.Floor((position.X + offset.X + size.X) / gridSize), (float)Math.Floor((position.Y + offset.Y + size.Y) / gridSize)),
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
            lock (buckets)
            {
                if (!buckets.ContainsKey(bucketKey))
                {
                    buckets.Add(bucketKey, new List<Box>());
                }

                if (!buckets[bucketKey].Contains(this))
                    buckets[bucketKey].Add(this);
            }
        }

        private void RemoveFromBucket(Vector2 bucketKey)
        {
            lock (buckets)
            {
                List<Box> bucketBoxes;
                buckets.TryGetValue(bucketKey, out bucketBoxes);

                if (bucketBoxes == null)
                {
                    return;
                }

                if (bucketBoxes.Count == 0)
                {
                    buckets.Remove(bucketKey);
                    return;
                }

                bucketBoxes.Remove(this);

                if (bucketBoxes.Count == 0)
                {
                    buckets.Remove(bucketKey);
                }
            }
        }

        public void DrawCollisionBox()
        {
            Debug.DrawRectangle(this.position.X + offset.X, this.position.Y + offset.Y,
                                this.position.X + this.size.X + offset.X,
                                this.position.Y + this.size.Y + offset.Y);
        }
    }
}
