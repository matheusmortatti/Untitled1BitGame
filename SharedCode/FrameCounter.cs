using System;
using System.Collections.Generic;
using System.Linq;

namespace SharedCode {
	public class FrameCounter {
		public const int MaximumSamples = 30;

		private Queue<float> _buffer = new Queue<float>();

		public long TotalFrames { get; private set; }
		public float TotalSeconds { get; private set; }
		public int AverageFramesPerSecond { get; private set; }
		public int CurrentFramesPerSecond { get; private set; }

		public void Update(float deltaTime) {
			CurrentFramesPerSecond = (int) Math.Round(1.0f / deltaTime);

			_buffer.Enqueue(CurrentFramesPerSecond);

			if (_buffer.Count > MaximumSamples) {
				_buffer.Dequeue();
				AverageFramesPerSecond = (int) Math.Round(_buffer.Average(i => i));
			} else {
				AverageFramesPerSecond = CurrentFramesPerSecond;
			}

			TotalFrames++;
			TotalSeconds += deltaTime;
		}
	}
}