using System;
using UnityEngine;
using System.IO;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

namespace TesseractModLoader.Window
{
	public class Online : MonoBehaviour {
		public Rect onlineWindowRect = new Rect(20,20,375,600);
		public bool onlineWindow = false;

		public void Start() {

		}

		public void OnGUI() {
			GUI.skin = TesseractModLoader.Window.UI.UISkin;
			if (onlineWindow) {
				onlineWindowRect = GUI.Window (0, onlineWindowRect, OnlineWindow, "Online Mod Browser",GUI.skin.GetStyle("window"));
			}
		}

		public void OnlineWindow(int windowID) {


			GUI.DragWindow ();
		}

		public void Update() {
			if (Input.GetKey (KeyCode.LeftControl) && Input.GetKeyDown (KeyCode.U) || Input.GetKey (KeyCode.RightControl) && Input.GetKeyDown (KeyCode.U)) {
				onlineWindow = !onlineWindow;
			}
		}
	}
}

