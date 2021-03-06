﻿using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using SharedCode.Misc;

namespace SharedCode {
	public class Map : GameObject {
		private Vector2 currentIndex;
		private List<GameObject> toDestroy;
		private object utils;

		public Map(Vector2 position) : base(position) {
			currentIndex = new Vector2((float)Math.Floor(position.X / 128),
																			(float)Math.Floor(position.Y / 128));
			InstantiateEntities(currentIndex);

			depth = -1000;
		}

		public override void Update(GameTime gameTime) {
			var pi = GameObjectManager.playerInstance;

			if (pi == null)
				return;

			Vector2 nextIndex = util.CorrespondingMapIndex(pi.collisionBox.middle);

			if (currentIndex.X != nextIndex.X || currentIndex.Y != nextIndex.Y) {
				Debug.Log($"Got into map index ( {nextIndex.X}, {nextIndex.Y} )");

				if (toDestroy != null) {
					foreach (var obj in toDestroy) {
						obj.done = true;
					}

					toDestroy = null;
				}

				currentIndex = nextIndex;
				toDestroy = GameObjectManager.FindObjectsWithTag("nonpersistent");
				if (toDestroy != null) {
					foreach (var obj in toDestroy) {
						obj.isPaused = true;
						TaskScheduler.AddTask(() => {
							obj.done = true;
							Debug.Log($"{obj.GetType().FullName} is done");
						}, 0.5, 0.5, this.id);
					}
				}

				TaskScheduler.AddTask(() => toDestroy = null, 0.5, 0.5, this.id);

				InstantiateEntities(currentIndex);
			}
		}

		public override void Draw() {
			GameManager.pico8.Graphics.Map(0, 0, 0, 0, 64, 128, 0x1);
		}

		public void InstantiateEntities(Vector2 screenIndex) {
			Vector2 celPos = new Vector2((int)screenIndex.X, (int)screenIndex.Y) * 16;

			for (int i = 0; i < 16; i += 1) {
				for (int j = 0; j < 16; j += 1) {
					byte val = GameManager.pico8.Memory.Mget((int)celPos.X + i, (int)celPos.Y + j);
					byte flag = (byte)GameManager.pico8.Memory.Fget(val);

					if ((flag & 0b00000010) != 0) {
						GameManager.pico8.Memory.Mset((int)celPos.X + i, (int)celPos.Y + j, 0);
					}

					if ((flag & 0b00001000) != 0) {
						GameObjectFactory.CreateGameObject(val, new Vector2((int)celPos.X + i, (int)celPos.Y + j) * 8);
					}
				}
			}
		}

		public static bool IsSolid(Vector2 celPos) {
			byte val = GameManager.pico8.Memory.Mget((int)celPos.X, (int)celPos.Y);
			byte flag = (byte)GameManager.pico8.Memory.Fget(val);

			return (flag & 0b00000100) != 0;
		}

		public static Vector2 FindPlayerInMapSheet() {
			for (int i = 0; i < 128; ++i) {
				for (int j = 0; j < 64; ++j) {
					byte val = GameManager.pico8.Memory.Mget(i, j);
					if (val == Player.spriteIndex) {
						return new Vector2(i, j) * 8;
					}
				}
			}

			return Vector2.Zero;
		}
	}
}
