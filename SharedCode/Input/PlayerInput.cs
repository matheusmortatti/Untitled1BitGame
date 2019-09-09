using System;
using System.Collections.Generic;
using System.Text;

using Pico8_Emulator;
using Microsoft.Xna.Framework;

namespace SharedCode.Input
{
    class PlayerInput : IInput
    {
        public Pico8<Color> pico8 { get; private set; }
        public PlayerInput(in Pico8<Color> pico8)
        {
            this.pico8 = pico8;
        }

        public void Update(GameObject gameObject)
        {
            Vector2 direction = new Vector2();
            if ((bool)pico8.Btn(0))
            {
                direction.X -= 1;
            }
            if ((bool)pico8.Btn(1))
            {
                direction.X += 1;
            }

            if ((bool)pico8.Btn(2))
            {
                direction.Y -= 1;
            }
            if ((bool)pico8.Btn(3))
            {
                direction.Y += 1;
            }

            gameObject.transform.direction = direction;
        }
    }
}
