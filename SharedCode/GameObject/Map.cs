using System;
using System.Collections.Generic;
using System.Text;
using IndependentResolutionRendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using SharedCode.Misc;

namespace SharedCode {
	public class Map : GameObject {
		private Vector2 currentIndex;
		private List<GameObject> toDestroy;

		private TiledMap _loadedMap;
		private TiledMapRenderer _mapRenderer;

		private Dictionary<string, TiledMapObjectLayer> _objectLayers;

		private bool[] _isSolid;

		public Map(Vector2 position, string mapPath) : base(position) {
			currentIndex = new Vector2((float)Math.Floor(position.X / 128),
																 (float)Math.Floor(position.Y / 128));
			InstantiateEntities( currentIndex);

			depth = -1000;

			//
			// Load Tiled map, map rendered and tilesets.
			//

			_loadedMap = GameManager.Content.Load<TiledMap>(mapPath);
			_mapRenderer = new TiledMapRenderer(GameManager.GraphicsDevice);
			_mapRenderer.LoadMap(_loadedMap);

			//
			// Load tilemap information to an easier accessible format.
			//

			var mapWidth = _loadedMap.Width;
			var mapHeight = _loadedMap.Height;
			var mapSize = mapWidth * mapHeight;

			// Init data structures.
			_isSolid = new bool[mapSize];
			_objectLayers = new Dictionary<string, TiledMapObjectLayer>();

			// Fill in object layers.
			foreach (var ol in _loadedMap.ObjectLayers) {
				_objectLayers[ol.Name] = ol;
			}

			// Init tile collision data.
			TiledMapObjectLayer colObjLayer;
			_objectLayers.TryGetValue("CollisionObjects", out colObjLayer);

			if (colObjLayer == null)
				return; 

			foreach (TiledMapTileObject obj in colObjLayer.Objects) {
				var objPos = new Vector2(
					obj.Position.X / _loadedMap.TileWidth, 
					obj.Position.Y / _loadedMap.TileHeight - 1);

				string solid;
				obj.Tile.Properties.TryGetValue("IsSolid", out solid);

				// Is solid if the "IsSolid" property is not null.
				_isSolid[(int)objPos.Y * mapWidth + (int)objPos.X] = solid.Length != 0;
			}
		}

		public override void Update(GameTime gameTime) {
			_mapRenderer.Update(gameTime);

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
			//GameManager.Pico8.Graphics.Map(0, 0, 0, 0, 64, 128, 0x1);

			var cam = GameObjectManager.FindObjectOfType<Camera>() as Camera;
			var layer = _loadedMap.GetLayer("Grass") as TiledMapTileLayer;

			if (cam != null) {
				_mapRenderer.Draw(
					layer,
					cam.TranslationMatrix * Resolution.getTransformationMatrix());
			}
		}

		public void InstantiateEntities(Vector2 screenIndex) {
			Vector2 celPos = new Vector2((int)screenIndex.X, (int)screenIndex.Y) * 16;

			for (int i = 0; i < 16; i += 1) {
				for (int j = 0; j < 16; j += 1) {
					byte val = GameManager.Pico8.Memory.Mget((int)celPos.X + i, (int)celPos.Y + j);
					byte flag = (byte)GameManager.Pico8.Memory.Fget(val);

					if ((flag & 0b00000010) != 0) {
						GameManager.Pico8.Memory.Mset((int)celPos.X + i, (int)celPos.Y + j, 0);
					}

					if ((flag & 0b00001000) != 0) {
						GameObjectFactory.CreateGameObject(val, new Vector2((int)celPos.X + i, (int)celPos.Y + j) * 8);
					}
				}
			}
		}

		public bool IsSolid(Vector2 celPos) {
			return _isSolid[(int)celPos.X + (int)celPos.Y * 128];
			//byte val = GameManager.Pico8.Memory.Mget((int)celPos.X, (int)celPos.Y);
			//byte flag = (byte)GameManager.Pico8.Memory.Fget(val);

			//return (flag & 0b00000100) != 0;
		}

		public static Vector2 FindPlayerInMapSheet() {
			for (int i = 0; i < 128; ++i) {
				for (int j = 0; j < 64; ++j) {
					byte val = GameManager.Pico8.Memory.Mget(i, j);
					if (val == Player.spriteIndex) {
						return new Vector2(i, j) * 8;
					}
				}
			}

			return Vector2.Zero;
		}
	}
}
