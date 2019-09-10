using System;
using System.Collections.Generic;
using System.Text;
using Pico8_Emulator;
using Microsoft.Xna.Framework;
using SharedCode.Physics;

namespace SharedCode.Graphics
{
    public class P8Sprite : IGraphics
    {
        private int _index;
        public int index
        {
            get { return _index; }
            set
            {
                _index = value < 16*16 && value >= 0 ? value : 0;
            }
        }
        public int width { get; set; }
        public int height { get; set; }

        public bool flipX { get; set; }
        public bool flipY { get; set; }

        public GraphicsUnit<Color> p8Graphics;


        public P8Sprite(in GraphicsUnit<Color> p8Graphics, int spriteIndex)
        {
            this.p8Graphics = p8Graphics;
            this.index = spriteIndex;

            width = 1;
            height = 1;
        }

        public P8Sprite(in GraphicsUnit<Color> p8Graphics, int spriteIndex, int width, int height) : this(p8Graphics, spriteIndex)
        {
            this.width = width;
            this.height = height;
        }

        public P8Sprite(in GraphicsUnit<Color> p8Graphics, int spriteIndex, int width, int height, bool flipX, bool flipY) : this(p8Graphics, spriteIndex, width, height)
        {
            this.flipX = flipX;
            this.flipY = flipY;
        }

        public void Draw(GameObject gameObject)
        {
            p8Graphics.Spr(index, (int)gameObject.transform.position.X, (int)gameObject.transform.position.Y, width, height, flipX, flipY);
        }

        public void Update(GameObject gameObject)
        {
            
        }
    }
    public class P8TopDownAnimator : IGraphics
    {
        public enum AnimationIndex
        {
            RUN_LEFT = 0,
            RUN_RIGHT = 1,
            RUN_UP = 2,
            RUN_DOWN = 3,

            IDLE_LEFT = 4,
            IDLE_RIGHT = 5,
            IDLE_UP = 6,
            IDLE_DOWN = 7,
            MAX_VALUE = 8,
        }

        public enum AnimationMode
        {
            FOUR_SIDES = 0,
            SIDES_ONLY = 1,
            MAX_VALUE = 2,
        }

        public SpriteAnimation RunLeft
        {
            get
            {
                return _animations[(int)AnimationIndex.RUN_LEFT];
            }
            set
            {
                _animations[(int)AnimationIndex.RUN_LEFT] = value;
                _animations[(int)AnimationIndex.RUN_LEFT].Reset();
            }
        }
        public SpriteAnimation RunRight
        {
            get
            {
                return _animations[(int)AnimationIndex.RUN_RIGHT];
            }
            set
            {
                _animations[(int)AnimationIndex.RUN_RIGHT] = value;
                _animations[(int)AnimationIndex.RUN_RIGHT].Reset();
            }
        }
        public SpriteAnimation RunUp
        {
            get
            {
                return _animations[(int)AnimationIndex.RUN_UP];
            }
            set
            {
                _animations[(int)AnimationIndex.RUN_UP] = value;
                _animations[(int)AnimationIndex.RUN_UP].Reset();
            }
        }
        public SpriteAnimation RunDown
        {
            get
            {
                return _animations[(int)AnimationIndex.RUN_DOWN];
            }
            set
            {
                _animations[(int)AnimationIndex.RUN_DOWN] = value;
                _animations[(int)AnimationIndex.RUN_DOWN].Reset();
            }
        }

        public SpriteAnimation IdleLeft
        {
            get
            {
                return _animations[(int)AnimationIndex.IDLE_LEFT];
            }
            set
            {
                _animations[(int)AnimationIndex.IDLE_LEFT] = value;
                _animations[(int)AnimationIndex.IDLE_LEFT].Reset();
            }
        }

        public SpriteAnimation IdleRight
        {
            get
            {
                return _animations[(int)AnimationIndex.IDLE_RIGHT];
            }
            set
            {
                _animations[(int)AnimationIndex.IDLE_RIGHT] = value;
                _animations[(int)AnimationIndex.IDLE_RIGHT].Reset();
            }
        }

        public SpriteAnimation IdleUp
        {
            get
            {
                return _animations[(int)AnimationIndex.IDLE_UP];
            }
            set
            {
                _animations[(int)AnimationIndex.IDLE_UP] = value;
                _animations[(int)AnimationIndex.IDLE_UP].Reset();
            }
        }

        public SpriteAnimation IdleDown
        {
            get
            {
                return _animations[(int)AnimationIndex.IDLE_DOWN];
            }
            set
            {
                _animations[(int)AnimationIndex.IDLE_DOWN] = value;
                _animations[(int)AnimationIndex.IDLE_DOWN].Reset();
            }
        }

        private SpriteAnimation[] _animations;
        private AnimationIndex currentlyPlaying;

        private AnimationMode _mode;

        public GraphicsUnit<Color> p8Graphics;
        private TopDownPhysics _physics;

        private float _previousSide;

        public P8TopDownAnimator(in GraphicsUnit<Color> p8Graphics, TopDownPhysics physics, AnimationMode mode)
        {
            this.p8Graphics = p8Graphics;
            _physics = physics;

            // Create animations for side, up and down directions.
            _animations = new SpriteAnimation[(int)AnimationIndex.MAX_VALUE];
            currentlyPlaying = AnimationIndex.IDLE_LEFT;

            _mode = mode;

            // Always start facing left
            _previousSide = -1;
        }

        public void Update(GameObject gameObject)
        {
            AnimationIndex nextIndex = currentlyPlaying;

            Vector2 facingDir = new Vector2(_physics.facingDirection.X, _physics.facingDirection.Y);
            // Only update the variable if the X facing side changes.
            _previousSide = facingDir.X != 0 ? facingDir.X : _previousSide;

            // Running animation.
            if (_physics.movingDirection.X < 0)
            {
                nextIndex = AnimationIndex.RUN_LEFT;
            }
            else if (_physics.movingDirection.X > 0)
            {
                nextIndex = AnimationIndex.RUN_RIGHT;
            }
            else if (_physics.movingDirection.Y < 0)
            {
                nextIndex = AnimationIndex.RUN_UP;
            }
            else if (_physics.movingDirection.Y > 0)
            {
                nextIndex = AnimationIndex.RUN_DOWN;
            }
            // Idle animation.
            else if (_physics.facingDirection.X < 0 && _physics.movingDirection.X == 0)
            {
                nextIndex = AnimationIndex.IDLE_LEFT;
            }
            else if (_physics.facingDirection.X > 0 && _physics.movingDirection.X == 0)
            {
                nextIndex = AnimationIndex.IDLE_RIGHT;
            }
            else if (_physics.facingDirection.Y < 0 && _physics.movingDirection.Y == 0)
            {
                nextIndex = AnimationIndex.IDLE_UP;
            }
            else if (_physics.facingDirection.Y > 0 && _physics.movingDirection.Y == 0)
            {
                nextIndex = AnimationIndex.IDLE_DOWN;
            }

            if (GetRealValue(nextIndex) != currentlyPlaying)
            {
                currentlyPlaying = GetRealValue(nextIndex);
                _animations[(int)GetRealValue(currentlyPlaying)]?.Reset();
            }

            _animations[(int)GetRealValue(currentlyPlaying)]?.Update();
        }

        public void Draw(GameObject gameObject)
        {
            _animations[(int)GetRealValue(currentlyPlaying)]?.GetCurrentSprite()?.Draw(gameObject);
        }

        public bool IsRunning(AnimationIndex index)
        {
            return (int)index < 4;
        }

        public AnimationIndex GetRealValue(AnimationIndex index)
        {
            if (_mode == AnimationMode.FOUR_SIDES)
                return index;

            if (IsRunning(index))
            {
                return _previousSide == -1 ? AnimationIndex.RUN_LEFT : AnimationIndex.RUN_RIGHT;
            }
            else
            {
                return _previousSide == -1 ? AnimationIndex.IDLE_LEFT : AnimationIndex.IDLE_RIGHT;
            }
        }
    }

    public class SpriteAnimation
    {
        public List<P8Sprite> spriteList { get; set; }
        public int currentIndex { get; private set; }
        public int animationFrameLength { get; set; }

        private int ticks;
        public bool isPlaying;

        public SpriteAnimation(List<P8Sprite> spriteList, int animationFrameLength)
        {
            this.spriteList = spriteList;
            this.currentIndex = 0;
            this.animationFrameLength = animationFrameLength;

            this.ticks = 0;
            this.isPlaying = true;
        }

        public SpriteAnimation(P8Sprite baseSprite, int spriteSequenceLength, int animationFrameLength)
        {
            spriteList = new List<P8Sprite>();
            for (int i = 0; i < spriteSequenceLength; i += 1)
            {
                spriteList.Add(new P8Sprite(baseSprite.p8Graphics, baseSprite.index + i, baseSprite.width, baseSprite.height, baseSprite.flipX, baseSprite.flipY));
            }

            this.animationFrameLength = animationFrameLength;
            this.isPlaying = true;
        }

        public void Update()
        {
            if (!isPlaying)
                return;

            if (ticks >= animationFrameLength)
            {
                currentIndex += 1;
                if (currentIndex >= spriteList.Count)
                    currentIndex = 0;
                ticks = 0;
            }

            ticks += 1;
        }

        public void Reset()
        {
            currentIndex = 0;
            ticks = 0;
        }

        public P8Sprite GetCurrentSprite() { return spriteList[currentIndex]; }
    }
}
