using System;
using System.Collections.Generic;
using System.Text;

using Pico8_Emulator;
using Microsoft.Xna.Framework;

namespace SharedCode.Input
{
    class PlayerInput : AInput
    {
        public PlayerInput()
        {
        }

        public override void Update(GameObject gameObject, GameTime gameTime)
        {
            Vector2 direction = new Vector2();
            if ((bool)GameManager.pico8.Btn(0))
            {
                direction.X -= 1;
            }
            if ((bool)GameManager.pico8.Btn(1))
            {
                direction.X += 1;
            }

            if ((bool)GameManager.pico8.Btn(2))
            {
                direction.Y -= 1;
            }
            if ((bool)GameManager.pico8.Btn(3))
            {
                direction.Y += 1;
            }

            if ((bool)GameManager.pico8.Btnp(4))
            {
                // Change to attacking state.
                ((Player)gameObject).stateMachine.Init(PlayerStates.Attacking);
                return;
            }

            gameObject.transform.direction = direction;
        }
    }
}
