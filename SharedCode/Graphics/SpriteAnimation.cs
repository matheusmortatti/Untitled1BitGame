using System;
using System.Collections.Generic;
using System.Text;

namespace SharedCode.Graphics
{
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
