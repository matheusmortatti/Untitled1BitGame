using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using System.IO;
using System.Threading;

namespace SharedCode {
	public static class Debug {
		public static bool debugMode = false;
		public static string logPath;

		private static Queue<string> logQueue;
		private static Thread logThread;

		public static void Init() {
			if (!Directory.Exists("Debug")) {
				Directory.CreateDirectory("Debug/");
			}
			logPath = "Debug/log_" + Math.Floor(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds).ToString() + ".txt";
			if (!File.Exists(logPath)) {
				File.Create(logPath).Close();
			}

			logQueue = new Queue<string>();

			logThread = new Thread(LogThread);
			logThread.IsBackground = true;
			logThread.Start();
		}

		public static void DrawLine(float x1, float y1, float x2, float y2, int col = 8) {
			GameManager.pico8.Graphics.Line((int)x1, (int)y1, (int)x2, (int)y2, (byte)col);
		}

		public static void DrawRectangle(float x1, float y1, float x2, float y2, int col = 8) {
			GameManager.pico8.Graphics.Rect((int)x1, (int)y1, (int)x2, (int)y2, (byte)col);
		}

		public static void DrawFilledRectangle(float x1, float y1, float x2, float y2, int col = 8) {
			GameManager.pico8.Graphics.Rectfill((int)x1, (int)y1, (int)x2, (int)y2, (byte)col);
		}

		public static void Log(string message) {
#if LOG
            lock (logQueue)
            {
                logQueue.Enqueue(message);
            }
#endif
		}

		private static void LogThread() {
#if LOG
            var s = File.AppendText(logPath);
            while (true)
            {
                if (logQueue.Count == 0)
                    continue;

                lock (logQueue)
                {
                    while (logQueue.Count != 0)
                    {
                        var message = logQueue.Dequeue();

                        s.WriteLine(message);
                    }
                }
            }
            s.Close();
#endif
		}
	}
}
