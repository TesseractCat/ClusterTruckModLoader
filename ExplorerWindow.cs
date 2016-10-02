using System;
using UnityEngine;
using System.IO;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

namespace TesseractModLoader.Window
{
	public class Explorer : MonoBehaviour
	{
		public Rect explorerWindowRect = new Rect(20,20,800,600);
		public bool explorerWindow = false;
		public Vector2 explorerScrollBar = new Vector2 ();

		public List<Transform> TList = new List<Transform>();

		public void Start() {

		}

		public void OnGUI() {
			GUI.skin = TesseractModLoader.Window.UI.UISkin;
			if (explorerWindow) {
				explorerWindowRect = GUI.Window (1, explorerWindowRect, ExplorerWindow, "Object Explorer");
			}
		}

		public void Refresh() {
			TList = new List<Transform> ();
			foreach (Transform t in FindObjectsOfType<Transform>()) {
				TList.Add (t);
			}
		}

		public void ExplorerWindow(int windowID) {
            GUILayout.Label("Press Ctrl + R to refresh the object list");
			explorerScrollBar = GUILayout.BeginScrollView (explorerScrollBar);
			foreach (Transform t in TList) {
                GUILayout.BeginHorizontal();
				foreach (Transform parent in t.gameObject.GetComponentsInParent<Transform>())
                {
                    if (parent != null)
                    {
                        GUILayout.Label(parent.name + " > ");
                    }
                }

                if (GUILayout.Button(t.name))
                {
                    foreach (Component c in t.GetComponents<Component>())
                    {
                        UnityEngine.Debug.Log(c.GetType().Name);
                    }
                }

                GUILayout.EndHorizontal();
            }
			GUILayout.EndScrollView ();

			GUI.DragWindow ();
		}

		public void Update() {
			if (Input.GetKey (KeyCode.LeftControl) && Input.GetKeyDown (KeyCode.O) || Input.GetKey (KeyCode.RightControl) && Input.GetKeyDown (KeyCode.O)) {
				explorerWindow = !explorerWindow;
			}

            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.R) || Input.GetKey(KeyCode.RightControl) && Input.GetKeyDown(KeyCode.R))
            {
                Refresh();
            }

            if (Input.GetKey (KeyCode.DownArrow)&&explorerWindow) {
				explorerScrollBar.Set (explorerScrollBar.x, explorerScrollBar.y + 100);
			}

			if (Input.GetKey (KeyCode.UpArrow)&&explorerWindow) {
				explorerScrollBar.Set (explorerScrollBar.x, explorerScrollBar.y - 100);
			}
		}
	}
}
