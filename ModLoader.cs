using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace TesseractModLoader
{
	public class ModLoader : MonoBehaviour
	{

        public void Start()
        {
            Directory.CreateDirectory(Application.dataPath + "/Managed/Mods/");
            foreach (String path in Directory.GetFiles(Application.dataPath + "/Managed/Mods/", "*.dll"))
            {
                LoadMod(path);
            }
        }

        public static void LoadMod(string path)
        {
            if (PlayerPrefs.GetInt(Path.GetFileName(path)) == 0)
            {
                Type type = Assembly.LoadFrom(path).GetType("Mod.Main");
                MethodInfo method = type.GetMethod("Start");
                method.Invoke(Activator.CreateInstance(type), null);
            }
        }
    }
}