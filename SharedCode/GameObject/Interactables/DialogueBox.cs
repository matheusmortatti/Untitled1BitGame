using Microsoft.Xna.Framework;
using SharedCode.Physics;
using System;
using System.Collections.Generic;
using System.Text;
using SharedCode.Misc;
using SharedCode.Input;

namespace SharedCode {
	public class DialogueBox : Interactable {

		private List<string> _messages;
		private int _currentMessage;
		private int _currentLength;
		private int _maxTextSize;
		private Action _endAction;
		private double _letterTime = 0.01;

		public DialogueBox(List<string> messages) : base(Vector2.Zero, null) {
			_messages = messages;

			// Task to advance each letter in the message.
			TaskScheduler.AddTask(() => {
				if (_currentLength < _messages[_currentMessage].Length) {
					_currentLength += 1;
				}
			}, _letterTime, -1, this.id);

			AddComponent(new InteractableInput());

			depth = 10000;
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);
		}

		public override void Draw() {
			base.Draw();

			var cam = (Camera)GameObjectManager.FindObjectOfType<Camera>();
			
			
			cam?.ResetCamera();
			//GameManager.Pico8.Graphics.Rectfill(0, 96, 127, 127, 0);
			//GameManager.Pico8.Graphics.Rect(0, 96, 127, 127, 7);

			DrawUtility.Rectfill(0, 96, 127, 127, 0);
			DrawUtility.Rect(0, 96, 127, 127, 7);

			//GameManager.Pico8.Graphics.Print(_messages[_currentMessage].Substring(0, _currentLength), 4, 100, 7);
			cam?.RestoreCamera();
		}

		public override void Interact() {
			if (_currentLength >= _messages[_currentMessage].Length) {
				_currentMessage += 1;
				_currentLength = 0;
				if (_currentMessage >= _messages.Count) {
					done = true;
					_currentMessage = _messages.Count - 1;
				}
			} else {
				_currentLength = _messages[_currentMessage].Length;
			}
		}

		public override void SetEndAction(Action endAction) {
			_endAction = endAction;
		}

		public override void OnDestroy() {
			base.OnDestroy();

			_endAction?.Invoke();
		}
	}
}
