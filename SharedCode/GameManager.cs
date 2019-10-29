using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Pico8Emulator;
using SharedCode.Misc;
using SharedCode.Particles;

namespace SharedCode {

	internal enum GameStates { TitleScreen, Overworld }

	internal class GameStateMachine : StateMachine<GameStates> {
		public GameStateMachine() : base(GameStates.Overworld) { }
		public bool resetOverworld;

		void OverworldInit(GameStates previous) {
			GameObjectManager.RemoveAllObjects();
			ParticleManager.RemoveAllParticles();

			GameManager.pico8.CartridgeLoader.Load("untitled1bitgame.p8");

			Vector2 playerPosition = Map.FindPlayerInMapSheet();
			GameObjectManager.AddObject(new Camera(playerPosition));
			GameObjectManager.AddObject(new Map(playerPosition));
			GameObjectManager.AddObject(new UI());
			GameObjectManager.AddObject(new ScreenTransition(1, () => { }, ScreenTransition.TransitionEffect.FadeIn));

			GameObjectManager.GlobalFillPattern = 0;
		}

		void OverworldState(GameTime gameTime) {
			if (resetOverworld) {
				Init(GameStates.Overworld);
				resetOverworld = false;
			}
		}
	}

	public static class GameManager {
		private static GameStateMachine stateMachine;

		public static Emulator pico8 { get; private set; }

		public static double framerate;

		public static Random random = new Random();

		public static void InitGameState(Emulator pico8) {
			GameManager.pico8 = pico8;

			//
			// Start PICO-8 stuff.
			//

			Debug.Init();
			GameObjectManager.Init();
			GlobalVars.LoadScript("GlobalVariables.lua");

			//
			// Start state machine.
			//

			stateMachine = new GameStateMachine();
			stateMachine.Init(GameStates.Overworld);
		}

		public static void Update(GameTime gameTime) {
			GameObjectManager.UpdateObjects(gameTime);
			ParticleManager.Update(gameTime);
			TaskScheduler.Update(gameTime);
			ControllerVibration.Update(gameTime);

			stateMachine.StateDo(gameTime);
		}

		public static void Draw() {
			GameObjectManager.DrawObjects();
			ParticleManager.Draw();

			((Camera)GameObjectManager.FindObjectWithTag("camera"))?.ResetCamera();

			if (Debug.debugMode) {
				pico8.Graphics.Rectfill(3, 119, 64, 125, 0);
				pico8.Graphics.Print(framerate.ToString("0.##"), 4, 120, 14);
				pico8.Graphics.Print(GameObjectManager.numberOfObjects, 32, 120, 14);
				pico8.Graphics.Print(TaskScheduler.numberOfTasks, 42, 120, 14);
				pico8.Graphics.Print(ParticleManager.numberOfParticles, 52, 120, 14);
			}

				((Camera)GameObjectManager.FindObjectWithTag("camera"))?.RestoreCamera();
		}

		public static void ResetOverworld() {
			stateMachine.resetOverworld = true;
		}
	}
}
