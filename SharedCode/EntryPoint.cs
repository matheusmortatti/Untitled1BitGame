#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

using IndependentResolutionRendering;
using Pico8_Emulator;

#endregion

namespace SharedCode
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Untitled1BitGame : Game
    {
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

        GameObject mainCharacter;

        public Untitled1BitGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";


            Resolution.Init(ref graphics);
            Resolution.SetVirtualResolution(128, 128);
            Resolution.SetResolution(600, 600, false);

            this.IsFixedTimeStep = true;//false;
            this.TargetElapsedTime = TimeSpan.FromSeconds(1d / 30d); //60);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            screenColorData = new Color[128 * 128];
            screenTexture = new Texture2D(GraphicsDevice, 128, 128, false, SurfaceFormat.Color);

            rasterizerState = new RasterizerState { MultiSampleAntiAlias = true };

            pico8 = new Pico8<Color>();
            pico8.screenColorData = screenColorData;
            pico8.rgbToColor = (r, g, b) => new Color(r, g, b);
            pico8.AddLeftButtonDownFunction(() => { return Keyboard.GetState().IsKeyDown(Keys.Left); }, 0);
            pico8.AddDownButtonDownFunction(() => { return Keyboard.GetState().IsKeyDown(Keys.Down); }, 0);
            pico8.AddUpButtonDownFunction(() => { return Keyboard.GetState().IsKeyDown(Keys.Up); }, 0);
            pico8.AddRightButtonDownFunction(() => { return Keyboard.GetState().IsKeyDown(Keys.Right); }, 0);
            pico8.AddOButtonDownFunction(() => { return Keyboard.GetState().IsKeyDown(Keys.Z); }, 0);
            pico8.AddXButtonDownFunction(() => { return Keyboard.GetState().IsKeyDown(Keys.X); }, 0);

            soundEffectInstance = new DynamicSoundEffectInstance(pico8.audio.sampleRate, AudioChannels.Mono);
            audioBuffer = new byte[pico8.audio.samplesPerBuffer * 2];

            soundEffectInstance.Play();

            pico8.LoadGame("resources.lua", new NLuaInterpreter());

            Physics.TopDownPhysics physics = new Physics.TopDownPhysics();
            Graphics.P8TopDownSpr sprs = new Graphics.P8TopDownSpr(pico8.graphics, physics);
            sprs.animLeft = new Graphics.SpriteAnimation(new Graphics.P8Sprite(pico8.graphics, 33), 4, 15);
            mainCharacter = new GameObject(physics, sprs, null);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // For Mobile devices, this logic will close the Game when the Back button is pressed
            // Exit() is obsolete on iOS
#if !__IOS__ && !__TVOS__
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }
#endif

            mainCharacter.Update(gameTime);
            pico8.Update();

            while (soundEffectInstance.PendingBufferCount < 3)
            {
                float[] p8Buffer = pico8.audio.RequestBuffer();
                int samplesPerBuffer = p8Buffer.Length;

                for (int i = 0; i < samplesPerBuffer; i += 1)
                {
                    float floatSample = p8Buffer[i];

                    // Convert it to the 16 bit [short.MinValue..short.MaxValue] range
                    short shortSample = (short)(floatSample >= 0.0f ? floatSample * short.MaxValue : floatSample * short.MinValue * -1);

                    // Store the 16 bit sample as two consecutive 8 bit values in the buffer with regard to endian-ness
                    if (!BitConverter.IsLittleEndian)
                    {
                        audioBuffer[i * 2] = (byte)(shortSample >> 8);
                        audioBuffer[i * 2 + 1] = (byte)shortSample;
                    }
                    else
                    {
                        audioBuffer[i * 2] = (byte)shortSample;
                        audioBuffer[i * 2 + 1] = (byte)(shortSample >> 8);
                    }
                }
                soundEffectInstance.SubmitBuffer(audioBuffer);
            }


            // TODO: Add your update logic here			
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            Resolution.BeginDraw();

            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, rasterizerState, null, Resolution.getTransformationMatrix());
            pico8.memory.Cls();
            mainCharacter.Draw();
            pico8.Draw();
            screenTexture.SetData(screenColorData);
            spriteBatch.Draw(screenTexture, new Rectangle(0, 0, 128, 128), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
