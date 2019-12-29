using System;
using System.Collections.Generic;
using System.Text;
using IndependentResolutionRendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
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

			GameManager.Pico8.CartridgeLoader.Load("untitled1bitgame.p8");

			Vector2 playerPosition = Map.FindPlayerInMapSheet();
			GameObjectManager.AddObject(new Camera(playerPosition));
			GameObjectManager.AddObject(new Map(playerPosition, "Maps/OverworldMap"));
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

		public static Emulator Pico8 { get; private set; }
		public static SpriteBatch SpriteBatch { get; private set; }
		public static GraphicsDevice GraphicsDevice { get; private set; }
		public static ContentManager Content { get; private set; }

		public static double framerate;

		public static Random random = new Random();
		private static RasterizerState _rasterizerState = new RasterizerState { MultiSampleAntiAlias = true };

		public static void InitGameState(Emulator pico8, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, ContentManager content) {
			Pico8 = pico8;
			SpriteBatch = spriteBatch;
			GraphicsDevice = graphicsDevice;
			Content = content;

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

			DrawUtility.Init(spriteBatch, graphicsDevice, content);

			DrawUtility.Palt(1, true);
			DrawUtility.Palt(0, false);
		}

		public static void Update(GameTime gameTime) {
			GameObjectManager.UpdateObjects(gameTime);
			ParticleManager.Update(gameTime);
			TaskScheduler.Update(gameTime);
			ControllerVibration.Update(gameTime);

			stateMachine.StateDo(gameTime);
		}

		public static void Draw() {
			var cam = ((Camera)GameObjectManager.FindObjectWithTag("camera"));

			SpriteBatch.Begin(
						SpriteSortMode.Immediate,
						null,
						SamplerState.PointClamp,
						null,
						_rasterizerState,
						null,
						Camera.TranslationMatrix * Resolution.getTransformationMatrix());
			GameObjectManager.DrawObjects();
			ParticleManager.Draw();

			cam?.ResetCamera();

			SpriteBatch.End();
			SpriteBatch.Begin(
						SpriteSortMode.Immediate,
						null,
						SamplerState.PointClamp,
						null,
						_rasterizerState,
						null,
						Resolution.getTransformationMatrix());

			if (Debug.debugMode) {
				Pico8.Graphics.Rectfill(3, 119, 64, 125, 0);
				Pico8.Graphics.Print(framerate.ToString("0.##"), 4, 120, 14);
				Pico8.Graphics.Print(GameObjectManager.numberOfObjects, 32, 120, 14);
				Pico8.Graphics.Print(TaskScheduler.numberOfTasks, 42, 120, 14);
				Pico8.Graphics.Print(ParticleManager.numberOfParticles, 52, 120, 14);
			}

			cam?.RestoreCamera();

			SpriteBatch.End();
		}

		public static void ResetOverworld() {
			stateMachine.resetOverworld = true;
		}

		public static void ResetCamera() {
			SpriteBatch.End();
			SpriteBatch.Begin(
						SpriteSortMode.Immediate,
						null,
						SamplerState.PointClamp,
						null,
						_rasterizerState,
						null,
						Resolution.getTransformationMatrix());
		}

		public static void RestoreCamera() {
			SpriteBatch.End();
			SpriteBatch.Begin(
						SpriteSortMode.Immediate,
						null,
						SamplerState.PointClamp,
						null,
						_rasterizerState,
						null,
						Camera.TranslationMatrix * Resolution.getTransformationMatrix());
		}
	}
}
