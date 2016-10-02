using System;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

namespace TesseractModLoader.Window
{
	public class Debug : MonoBehaviour
	{
		public Rect debugWindowRect = new Rect(20,20,400,600);
		public bool debugWindow = false;
		public Vector2 debugScrollBar = new Vector2 ();
		List<String> Logs = new List<String>();
		String currentLog = "";

        void Start() {
			Application.logMessageReceived += HandleLog;
		}

		void OnGUI() {
			GUI.skin = TesseractModLoader.Window.UI.UISkin;
			if (debugWindow) {
				debugWindowRect = GUI.Window (2, debugWindowRect, DebugWindow, "Debug");
			}
		}

		public void DebugWindow(int windowID) {
			debugScrollBar = GUILayout.BeginScrollView (debugScrollBar,false,true);

			foreach (String s in Logs) {
				GUILayout.Label (s);
			}

			GUILayout.EndScrollView ();
            
			GUI.DragWindow ();
		}

		public void Update() {
			if (Input.GetKey (KeyCode.LeftControl) && Input.GetKeyDown (KeyCode.P) || Input.GetKey (KeyCode.RightControl) && Input.GetKeyDown (KeyCode.P)) {
				debugWindow = !debugWindow;
			}

		}

		public void HandleLog (String logString, String stackTrace, LogType logType) {
			Logs.Add (logString);
			debugScrollBar.y = Mathf.Infinity;
		}
	}
}