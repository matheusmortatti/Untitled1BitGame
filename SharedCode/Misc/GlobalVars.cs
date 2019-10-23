using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using NLua;

namespace SharedCode.Misc {
	public static class GlobalVars {
		private static Lua _script;

		public static void LoadScript(string path) {
			_script = new Lua();
			_script.LoadCLRPackage();

			var streamReader = new StreamReader(path);
			_script.DoString(streamReader.ReadToEnd());
		}

		public static object GetVariable(string name) {
			return _script[name];
		}

		public static Dictionary<object, object> GetMessageListAt(int i, int j) {
			LuaTable outerTable = _script.GetTable("messages");
			LuaTable innerTable = (LuaTable)outerTable[i];

			foreach(var key in innerTable.Keys) {
				if (key is long && (long)key == j) {
					return _script.GetTableDict((LuaTable)innerTable[j]);
				}
			}

			return null;
		}

	}
}
