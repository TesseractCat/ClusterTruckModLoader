using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.IO.Compression;
using UnityEngine.UI;

namespace TesseractModLoader.Window
{
	public class UI : MonoBehaviour
	{

		public static GUISkin UISkin;

		private static int num = 0;

		public bool Enabled = false;

		public void Start() {
			StartCoroutine (LoadSkin ());
			if (PlayerPrefs.GetInt ("Disabled") == 0) {
				Enabled = true;
			}
		}

		public static int GetWindowID() {
		num--;
			return int.MaxValue - num;
		}

		IEnumerator LoadSkin()
		{
			
			WWW www = WWW.LoadFromCacheOrDownload ("file:///"+Application.dataPath + "/Managed/metalguibundle", 1);

			yield return www;

			AssetBundle bundle = www.assetBundle;

			AssetBundleRequest request = bundle.LoadAssetAsync ("MetalGUISkin", typeof(GUISkin));

			yield return request;

			UISkin = request.asset as GUISkin;

			GUI.skin = UISkin;

			//bundle.Unload (false);

			//www.Dispose();
		}

		public void Update() {
			if (GameObject.Find ("ALPHA") && !GameObject.Find("ALPHA").GetComponent<Text>().text.Contains("MODDED") && Enabled) {
				GameObject.Find ("ALPHA").GetComponent<Text> ().text = "MODDED " + GameObject.Find ("ALPHA").GetComponent<Text> ().text;
				GameObject.Find ("ALPHA").GetComponent<Text> ().color = Color.black;
			}
		}
	}
}