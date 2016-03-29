using System;
using UnityEngine;
using System.IO;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

namespace TesseractModLoader.Window
{
	public class Console : MonoBehaviour
	{
		public Rect consoleWindowRect = new Rect(20,20,375,600);
		public bool consoleWindow = true;
		public Dictionary<String, int> ModPrefs;

		public void Start() {
			UpdateModPrefs ();
		}

		public void UpdateModPrefs() {

		}

		public void OnGUI() {
			GUI.skin = TesseractModLoader.Window.UI.UISkin;
			if (consoleWindow) {
				consoleWindowRect = GUI.Window (0, consoleWindowRect, ConsoleWindow, "Console",GUI.skin.GetStyle("window"));
			}
		}

		public void ConsoleWindow(int windowID) {

			GUILayout.Label ("Tesseract Mod Loader v0.5 Enabled");
			GUILayout.Label ("Press Ctrl + I to toggle this menu");
			GUILayout.Label ("Press Ctrl + O to toggle object explorer menu (dev)");
			GUILayout.Label ("Press Ctrl + P to toggle debug viewer (dev)");
			GUILayout.Label ("Click on a mod to toggle it on and off (Requires Game Restart)");
			if (PlayerPrefs.GetInt ("Disabled") == 0) {
				if (GUILayout.Button ("☑ | Enable / Disable Modloader (Requires Game Restart)")) {
					PlayerPrefs.SetInt ("Disabled", 1);
				}
			} else {
				if (GUILayout.Button ("☐ | Enable / Disable Modloader (Requires Game Restart)")) {
					PlayerPrefs.SetInt ("Disabled", 0);
				}
			}
			GUILayout.Label ("---");
			GUILayout.Label ("Loaded mods:");
			GUILayout.BeginVertical ();
			foreach (String path in Directory.GetFiles(Application.dataPath+"/Managed/Mods/","*.dll")) {
				if (PlayerPrefs.GetInt (Path.GetFileName (path)) == 0) {
					if (GUILayout.Button ("☑ | " + Path.GetFileName (path))) {
						PlayerPrefs.SetInt (Path.GetFileName (path), 1);
					}
				} else {
					if (GUILayout.Button ("☐ | " + Path.GetFileName (path))) {
						//Toggle
						PlayerPrefs.SetInt (Path.GetFileName (path), 0);
					}
				}
			}
			GUILayout.EndVertical ();

			GUI.DragWindow ();
		}

		public void Update() {
			if (Input.GetKey (KeyCode.LeftControl) && Input.GetKeyDown (KeyCode.I) || Input.GetKey (KeyCode.RightControl) && Input.GetKeyDown (KeyCode.I)) {
				consoleWindow = !consoleWindow;
			}
		}
	}
}

