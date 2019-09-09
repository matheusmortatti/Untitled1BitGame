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
    public class P8TopDownSpr : IGraphics
    {
        public enum AnimationIndex
        {
            LEFT = 0,
            RIGHT = 1,
            UP = 2,
            DOWN = 3,
        }

        public SpriteAnimation animLeft
        {
            get
            {
                return _animations[(int)AnimationIndex.LEFT];
            }
            set
            {
                _animations[(int)AnimationIndex.LEFT] = value;
                _animations[(int)AnimationIndex.LEFT].Reset();
            }
        }
        public SpriteAnimation animRight
        {
            get
            {
                return _animations[(int)AnimationIndex.RIGHT];
            }
            set
            {
                _animations[(int)AnimationIndex.RIGHT] = value;
                _animations[(int)AnimationIndex.RIGHT].Reset();
            }
        }
        public SpriteAnimation animUp
        {
            get
            {
                return _animations[(int)AnimationIndex.UP];
            }
            set
            {
                _animations[(int)AnimationIndex.UP] = value;
                _animations[(int)AnimationIndex.UP].Reset();
            }
        }
        public SpriteAnimation animDown
        {
            get
            {
                return _animations[(int)AnimationIndex.DOWN];
            }
            set
            {
                _animations[(int)AnimationIndex.DOWN] = value;
                _animations[(int)AnimationIndex.DOWN].Reset();
            }
        }

        private SpriteAnimation[] _animations;
        private AnimationIndex currentlyPlaying;

        public GraphicsUnit<Color> p8Graphics;
        private TopDownPhysics _physics;

        public P8TopDownSpr(in GraphicsUnit<Color> p8Graphics, TopDownPhysics physics)
        {
            this.p8Graphics = p8Graphics;
            _physics = physics;

            // Create animations for side, up and down directions.
            _animations = new SpriteAnimation[4];
            currentlyPlaying = AnimationIndex.LEFT;
        }

        public void Update(GameObject gameObject)
        {
            if (_physics.facingDirection.X < 0 && currentlyPlaying != AnimationIndex.LEFT)
            {
                currentlyPlaying = AnimationIndex.LEFT;
                _animations[(int)currentlyPlaying]?.Reset();
            }
            else if (_physics.facingDirection.X > 0 && currentlyPlaying != AnimationIndex.RIGHT)
            {
                currentlyPlaying = AnimationIndex.RIGHT;
                _animations[(int)currentlyPlaying]?.Reset();
            }
            else if (_physics.facingDirection.Y < 0 && currentlyPlaying != AnimationIndex.UP)
            {
                currentlyPlaying = AnimationIndex.UP;
                _animations[(int)currentlyPlaying]?.Reset();
            }
            else if (_physics.facingDirection.Y > 0 && currentlyPlaying != AnimationIndex.DOWN)
            {
                currentlyPlaying = AnimationIndex.DOWN;
                _animations[(int)currentlyPlaying]?.Reset();
            }
            // Add idle animations.


            _animations[(int)currentlyPlaying]?.Update();
        }

        public void Draw(GameObject gameObject)
        {
            _animations[(int)currentlyPlaying]?.GetCurrentSprite()?.Draw(gameObject);
        }
    }
}
