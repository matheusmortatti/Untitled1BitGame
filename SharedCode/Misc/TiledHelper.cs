using System;
using System.Collections.Generic;
using System.Text;

using MonoGame.Extended.Tiled;

namespace SharedCode.Misc {
	public static class TiledHelper {

		public static string GetTilePropertyOrNull(TiledMapTilesetTile tile, string property) {
			string result;
			tile.Properties.TryGetValue(property, out result);
			return result;
		}

		public static bool IsTilePropertyEquals(TiledMapTilesetTile tile, string property, string value) {
			string result;
			tile.Properties.TryGetValue(property, out result);
			return result != null && result == value;
		}
	}
}
