using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedCode {
	public class ScreenTransition : GameObject {

		public enum TransitionEffect {
			FadeIn, FadeOut
		}

		private TransitionEffect _effect;

		private Action _endAction;
		private double _duration, _timePassed;

		public ScreenTransition(double duration, Action endAction, TransitionEffect effect) : base(Vector2.Zero) {
			_duration = duration;
			_endAction = endAction;
			_effect = effect;
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);

			_timePassed += gameTime.ElapsedGameTime.TotalSeconds;
			if (_timePassed > _duration) {
				_endAction.Invoke();

				GameObjectManager.GlobalFillPattern = 0;

				done = true;
			}
		}

		public override void Draw() {
			var ratio = _timePassed / _duration;

			switch (_effect) {
				case TransitionEffect.FadeOut:
					if (ratio < 0.3) {
						GameObjectManager.GlobalFillPattern = 0b0000101000001010;
					} else if (ratio < 0.6) {
						GameObjectManager.GlobalFillPattern = 0b1010010110100101;
					} else if (ratio < 1) {
						GameObjectManager.GlobalFillPattern = 0b1111111111111111;
					}
					break;
				case TransitionEffect.FadeIn:
					if (ratio < 0.2) {
						GameObjectManager.GlobalFillPattern = 0b1111111111111111;
					}
					else if (ratio < 0.6) {
						GameObjectManager.GlobalFillPattern = 0b1010010110100101;
					} else if (ratio < 1) {
						GameObjectManager.GlobalFillPattern = 0b0000101000001010;
					}
					break;
			}
		}
	}
}
