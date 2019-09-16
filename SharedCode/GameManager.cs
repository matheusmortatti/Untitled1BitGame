using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Pico8_Emulator;

using SharedCode.Misc;

namespace SharedCode
{

    internal enum GameStates { TitleScreen, Overworld }

    internal class GameStateMachine : StateMachine<GameStates>
    {
        private static Pico8<Color> _pico8;
        public GameStateMachine(Pico8<Color> pico8) : base(GameStates.Overworld) { _pico8 = pico8; }

        void OverworldInit (GameStates previous)
        {
            _pico8.LoadGame("untitled1bitgame.p8", new NLuaInterpreter());

            GameObjectManager.AddObject(new Camera(Vector2.Zero));
            GameObjectManager.AddObject(new Map(Vector2.Zero));
        }

        void OverworldState(GameTime gameTime)
        {

        }
    }

    public static class GameManager
    {
        private static GameStateMachine stateMachine;

        public static Pico8<Color> pico8 { get; private set; }

        public static void InitGameState(Pico8<Color> pico8)
        {
            GameManager.pico8 = pico8;

            //
            // Start PICO-8 stuff.
            //

            Debug.SetPico8(pico8);
            GameObjectManager.Init(pico8);

            //
            // Start state machine.
            //

            stateMachine = new GameStateMachine(pico8);
            stateMachine.Init(GameStates.Overworld);
        }

        public static void Update(GameTime gameTime)
        {
            GameObjectManager.UpdateObjects(gameTime);

            stateMachine.StateDo(gameTime);
        }

        public static void Draw()
        {
            GameObjectManager.DrawObjects();
        }
    }
}
