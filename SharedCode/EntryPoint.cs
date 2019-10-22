#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Audio;

using IndependentResolutionRendering;
using Pico8_Emulator;
using System.Diagnostics;

#endregion

namespace SharedCode {
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Untitled1BitGame : Game {
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		/// <summary>
		/// Defines the rasterizerState
		/// </summary>
		internal RasterizerState rasterizerState;

		/// <summary>
		/// Defines the pico8
		/// </summary>
		internal Pico8<Color> pico8;

		/// <summary>
		/// Defines the screenColorData
		/// </summary>
		internal Color[] screenColorData;

		/// <summary>
		/// Defines the screenTexture
		/// </summary>
		internal Texture2D screenTexture;

		internal DynamicSoundEffectInstance soundEffectInstance;
		internal byte[] audioBuffer;

		public Color[] pico8Palette = {
            // Black
            new Color( 0, 0, 0 ),
            // Dark-blue
            new Color( 29, 43, 83 ),
            // Dark-purple
            new Color( 126, 37, 83 ),
            // Dark-green
            new Color( 0, 135, 81 ),
            // Brown
            new Color( 171, 82, 54 ),
            // Dark-gray
            new Color( 95, 87, 79 ),
            // Light-gray
            new Color( 194, 195, 199 ),
            // White
            new Color( 255, 241, 232 ),
            // Red
            new Color( 255, 0, 77 ),
            // Orange
            new Color( 255, 163, 0 ),
            // Yellow
            new Color( 255, 236, 39 ),
            // Green
            new Color( 0, 228, 54 ),
            // Blue
            new Color( 41, 173, 255 ),
            // Indigo
            new Color( 131, 118, 156 ),
            // Pink
            new Color( 255, 119, 168 ),
            // Peach
            new Color( 255, 204, 170 ),

            // Alternative Palette:

            new Color( 42, 24, 22 ),

						new Color( 17, 29, 53 ),

						new Color( 66, 33, 54 ),

						new Color( 15, 84, 91 ),

						new Color( 116, 47, 40 ),

						new Color( 72, 50, 63 ),

						new Color( 162, 136, 121 ),

						new Color( 242, 239, 124 ),

						new Color( 190, 17, 80 ),

						new Color( 255, 109, 36 ),

						new Color( 169, 231, 46 ),

						new Color( 0, 181, 68 ),

						new Color( 6, 89, 181 ),

						new Color( 117, 70, 102 ),

						new Color( 255, 110, 89 ),

						new Color( 255, 157, 128 )

				};

		internal FrameCounter frameCounter;

		private const double updateTime = 1.0 / 60.0;
		private double _deltaUpdate, _deltaDraw;

		public Untitled1BitGame() {
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";


			Resolution.Init(ref graphics);
			Resolution.SetResolution(1000, 1000, false);
			Resolution.SetVirtualResolution(128, 128);

			//this.TargetElapsedTime = TimeSpan.FromSeconds(1d / 60d); //60);
			this.IsFixedTimeStep = false;
			
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize() {
			// TODO: Add your initialization logic here
			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent() {
			frameCounter = new FrameCounter();

			spriteBatch = new SpriteBatch(GraphicsDevice);

			screenColorData = new Color[128 * 128];
			screenTexture = new Texture2D(GraphicsDevice, 128, 128, false, SurfaceFormat.Color);

			rasterizerState = new RasterizerState { MultiSampleAntiAlias = true };

			pico8 = new Pico8<Color>();
			pico8.screenColorData = screenColorData;
			pico8.indexToColor = (i) => pico8Palette[i];

#if WINDOWS

			//
			// Keyboard input
			//

			pico8.AddLeftButtonDownFunction(() => { return Keyboard.GetState().IsKeyDown(Keys.Left); }, 0);
			pico8.AddDownButtonDownFunction(() => { return Keyboard.GetState().IsKeyDown(Keys.Down); }, 0);
			pico8.AddUpButtonDownFunction(() => { return Keyboard.GetState().IsKeyDown(Keys.Up); }, 0);
			pico8.AddRightButtonDownFunction(() => { return Keyboard.GetState().IsKeyDown(Keys.Right); }, 0);
			pico8.AddOButtonDownFunction(() => { return Keyboard.GetState().IsKeyDown(Keys.Z); }, 0);
			pico8.AddXButtonDownFunction(() => { return Keyboard.GetState().IsKeyDown(Keys.X); }, 0);

#endif

			//
			// Gamepad input
			//

			pico8.AddLeftButtonDownFunction(() => { return GamePad.GetState(PlayerIndex.One).DPad.Left == ButtonState.Pressed; }, 0);
			pico8.AddDownButtonDownFunction(() => { return GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed; }, 0);
			pico8.AddUpButtonDownFunction(() => { return GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed; }, 0);
			pico8.AddRightButtonDownFunction(() => { return GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Pressed; }, 0);

			pico8.AddLeftButtonDownFunction(() => { return GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X < -0.5f; }, 0);
			pico8.AddRightButtonDownFunction(() => { return GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X > 0.5f; }, 0);
			pico8.AddDownButtonDownFunction(() => { return GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y < -0.5f; }, 0);
			pico8.AddUpButtonDownFunction(() => { return GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y > 0.5f; }, 0);

			pico8.AddOButtonDownFunction(() => { return GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed; }, 0);
			pico8.AddXButtonDownFunction(() => { return GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed; }, 0);

			pico8.AddOButtonDownFunction(() => { return GamePad.GetState(PlayerIndex.One).Triggers.Right > 0.5f; }, 1);
			pico8.AddXButtonDownFunction(() => { return GamePad.GetState(PlayerIndex.One).Triggers.Left > 0.5f; }, 1);

			

			GameManager.InitGameState(pico8);

			soundEffectInstance = new DynamicSoundEffectInstance(pico8.audio.sampleRate, AudioChannels.Mono);
			audioBuffer = new byte[pico8.audio.samplesPerBuffer * 2];

			soundEffectInstance.Play();
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime) {
			// For Mobile devices, this logic will close the Game when the Back button is pressed
			// Exit() is obsolete on iOS
#if !__IOS__ && !__TVOS__
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
					Keyboard.GetState().IsKeyDown(Keys.Escape)) {
				Exit();
			}

			if (Keyboard.GetState().IsKeyDown(Keys.D)) {
				Debug.debugMode = !Debug.debugMode;
			}
#endif

			double dt = gameTime.ElapsedGameTime.TotalSeconds;

			_deltaUpdate += dt;
			while (_deltaUpdate > updateTime) {
				_deltaUpdate -= updateTime;

				var gt = new GameTime(gameTime.TotalGameTime, new TimeSpan(0, 0, 0, 0, (int)(updateTime * 1000)));
				GameManager.Update(gt);
				pico8.Update();

				while (soundEffectInstance.PendingBufferCount < 3) {
					float[] p8Buffer = pico8.audio.RequestBuffer();
					int samplesPerBuffer = p8Buffer.Length;

					for (int i = 0; i < samplesPerBuffer; i += 1) {
						float floatSample = p8Buffer[i];

						// Convert it to the 16 bit [short.MinValue..short.MaxValue] range
						short shortSample = (short)(floatSample >= 0.0f ? floatSample * short.MaxValue : floatSample * short.MinValue * -1);

						// Store the 16 bit sample as two consecutive 8 bit values in the buffer with regard to endian-ness
						if (!BitConverter.IsLittleEndian) {
							audioBuffer[i * 2] = (byte)(shortSample >> 8);
							audioBuffer[i * 2 + 1] = (byte)shortSample;
						}
						else {
							audioBuffer[i * 2] = (byte)shortSample;
							audioBuffer[i * 2 + 1] = (byte)(shortSample >> 8);
						}
					}
					soundEffectInstance.SubmitBuffer(audioBuffer);
				}
			}

			frameCounter.Update((float)dt);
			GameManager.framerate = frameCounter.AverageFramesPerSecond;
			Window.Title = $"FPS: {frameCounter.AverageFramesPerSecond}";


			// TODO: Add your update logic here			
			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime) {

			double dt = gameTime.ElapsedGameTime.TotalSeconds;

			_deltaDraw += dt;
			while (_deltaDraw > updateTime) {
				_deltaDraw -= updateTime;

				Resolution.BeginDraw();
				GraphicsDevice.Clear(Color.Black);

				spriteBatch.Begin(
						SpriteSortMode.Immediate,
						null,
						SamplerState.PointClamp,
						null,
						rasterizerState,
						null,
						Resolution.getTransformationMatrix());

				GameManager.Draw();
				pico8.Draw();
				pico8.memory.Cls();
				screenTexture.SetData(screenColorData);
				spriteBatch.Draw(screenTexture, new Rectangle(0, 0, 128, 128), Color.White);

				spriteBatch.End();
			}

			base.Draw(gameTime);
		}
	}
}
