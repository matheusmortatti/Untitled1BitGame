using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Pico8_Emulator;

using SharedCode.Misc;
using SharedCode.Particles;

namespace SharedCode
{

    internal enum GameStates { TitleScreen, Overworld }

    internal class GameStateMachine : StateMachine<GameStates>
    {
        public GameStateMachine() : base(GameStates.Overworld) {  }

        void OverworldInit (GameStates previous)
        {
            GameManager.pico8.LoadGame("untitled1bitgame.p8", new NLuaInterpreter());

             Vector2 playerPosition = Map.FindPlayerInMapSheet();
            GameObjectManager.AddObject(new Camera(playerPosition));
            GameObjectManager.AddObject(new Map(playerPosition));
            GameObjectManager.AddObject(new UI());
        }

        void OverworldState(GameTime gameTime)
        {

        }
    }

    public static class GameManager
    {
        private static GameStateMachine stateMachine;

        public static Pico8<Color> pico8 { get; private set; }

        private static double framerate;

        public static Random random = new Random();

        public static void InitGameState(Pico8<Color> pico8)
        {
            GameManager.pico8 = pico8;

            //
            // Start PICO-8 stuff.
            //

            Debug.Init(pico8);
            GameObjectManager.Init(pico8);

            //
            // Start state machine.
            //

            stateMachine = new GameStateMachine();
            stateMachine.Init(GameStates.Overworld);
        }

        public static void Update(GameTime gameTime)
        {
            GameObjectManager.UpdateObjects(gameTime);
            ParticleManager.Update(gameTime);
            TaskScheduler.Update(gameTime);

            stateMachine.StateDo(gameTime);

            framerate = 1 / gameTime.ElapsedGameTime.TotalSeconds;
        }

        public static void Draw()
        {
            GameObjectManager.DrawObjects();
            ParticleManager.Draw();

            if (Debug.debugMode)
            {
                //pico8.graphics.Rectfill(0, 0, 64, 16, 0);
                pico8.Print(framerate.ToString("0.##"), 0, 0, 10);
            }
        }
    }
}
