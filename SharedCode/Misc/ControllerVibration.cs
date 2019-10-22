using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedCode.Misc {
	public static class ControllerVibration {

		private class VibrationTask {
			public int playerIndex;
			public float rightMotor, leftMotor;
			public double duration;
			public double timePassed;
		}

		private static List<VibrationTask> _tasks = new List<VibrationTask>();
		private static float _lastLM, _lastRM;

		public static void Update(GameTime gameTime) {
			var dt = gameTime.ElapsedGameTime.TotalSeconds;
			float rm = 0, lm = 0;

			for(int i = _tasks.Count - 1; i >= 0; --i) {
				var t = _tasks[i];

				t.timePassed += dt;
				if (t.duration < t.timePassed) {
					_tasks.RemoveAt(i);
				}

				//
				// Take max values for left and right motors.
				//

				if (rm < t.rightMotor) {
					rm = t.rightMotor;
				}
				if (lm < t.leftMotor) {
					lm = t.leftMotor;
				}
			}

			if (_lastRM != rm || _lastLM != lm) {
				GamePad.SetVibration(0, lm, rm);
			}

			_lastRM = rm;
			_lastLM = lm;
		}

		public static void SetVibration(int playerIndex, float leftMotor, float rightMotor, double duration) {
			_tasks.Add(new VibrationTask { playerIndex = playerIndex, rightMotor = rightMotor, leftMotor = leftMotor, duration = duration });
		}

	}
}
