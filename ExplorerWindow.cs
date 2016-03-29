﻿using System;
using UnityEngine;
using System.IO;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

namespace TesseractModLoader.Window
{
	public class Explorer : MonoBehaviour
	{
		public Rect explorerWindowRect = new Rect(20,20,400,600);
		public bool explorerWindow = false;
		public Vector2 explorerScrollBar = new Vector2 ();

		public List<Transform> TList = new List<Transform>();

		public void Start() {
			StartCoroutine (Refresh ());
		}

		public void OnGUI() {
			GUI.skin = TesseractModLoader.Window.UI.UISkin;
			if (explorerWindow) {
				explorerWindowRect = GUI.Window (1, explorerWindowRect, ExplorerWindow, "Object Explorer");
			}
		}

		public IEnumerator Refresh() {
			TList = new List<Transform> ();
			foreach (Transform t in FindObjectsOfType<Transform>()) {
				TList.Add (t);
			}
			yield return new WaitForSeconds (5f);
			StartCoroutine (Refresh ());
		}

		public void ExplorerWindow(int windowID) {
			explorerScrollBar = GUILayout.BeginScrollView (explorerScrollBar);
			foreach (Transform t in TList) {
				//GUILayout.BeginHorizontal ();
				//GUILayout.Label (t.name);
				if (t.parent == null) {
					if (GUILayout.Button (t.name)) {
						foreach (Component c in t.GetComponents<Component>()) {
							UnityEngine.Debug.Log (c.GetType ().Name);
						}
					}
					foreach (Transform child in t.GetComponentsInChildren<Transform>()) {
						GUILayout.BeginHorizontal ();
						GUILayout.Label (" -> ");
						if (GUILayout.Button (child.name)) {
							foreach (Component c in child.GetComponents<Component>()) {
								UnityEngine.Debug.Log (c.GetType ().Name);
							}
						}
						GUILayout.EndHorizontal ();
					}
				}

				//GUILayout.EndHorizontal ();
			}
			GUILayout.EndScrollView ();

			GUI.DragWindow ();
		}

		public void Update() {
			if (Input.GetKey (KeyCode.LeftControl) && Input.GetKeyDown (KeyCode.O) || Input.GetKey (KeyCode.RightControl) && Input.GetKeyDown (KeyCode.O)) {
				explorerWindow = !explorerWindow;
			}

			if (Input.GetKey (KeyCode.DownArrow)&&explorerWindow) {
				explorerScrollBar.Set (explorerScrollBar.x, explorerScrollBar.y + 10);
			}

			if (Input.GetKey (KeyCode.UpArrow)&&explorerWindow) {
				explorerScrollBar.Set (explorerScrollBar.x, explorerScrollBar.y - 10);
			}
		}
	}
}