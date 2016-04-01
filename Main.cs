using System;
using UnityEngine;

namespace TesseractModLoader
{
	public class Main : MonoBehaviour
	{
		GameObject modObject;

		public Main()
		{
		}

		public void Start()
		{
			if (!GameObject.Find ("Tesseract Mod Object")) {
				modObject = new GameObject ();
				if (PlayerPrefs.GetInt ("Disabled") == 0) {
					modObject.AddComponent<ModLoader> ();
				}
				modObject.AddComponent<TesseractModLoader.Window.Debug> ();
				modObject.AddComponent<TesseractModLoader.Window.Explorer> ();
				modObject.AddComponent<TesseractModLoader.Window.Console> ();
				modObject.AddComponent<TesseractModLoader.Window.Online> ();
				modObject.AddComponent<TesseractModLoader.Window.UI> ();
				GameObject.DontDestroyOnLoad (modObject);
				modObject.name = "Tesseract Mod Object";
			}
		}
	}
}