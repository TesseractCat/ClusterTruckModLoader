using System;
using UnityEngine;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using System.IO;

namespace TesseractModLoader.Window
{
    public class Mod
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public bool Downloaded { get; set; } = false;
        
        public Mod(string name,string author,string description,string link) {
            this.Name = name;
            this.Author = author;
            this.Description = description;
            this.Link = link;
        }
    }

    public class Online : MonoBehaviour
    {
        //TODO: Add some sort of way where the user can choose to replace file or not.
        public bool REPLACE = true;

        public Rect onlineWindowRect = new Rect(20, 20, 375, 600);
        public bool onlineWindow = false;
        public List<Mod> Mods;

        //TODO: Add ability (maybe config) for user to define their own mod list so its not hardcoded.
        public string modListUrl = "https://raw.githubusercontent.com/TesseractCat/ClusterTruckModLoader/dev-noxml/ModList.txt";
        public Vector2 DataBaseModListScrollBar = new Vector2();

        public void Start()
        {
            //Fails to pass certificates when downloading so I had to do this.
            //TODO: Find and implement the correct way to do this
            try
            {
                //Change SSL checks so that all checks pass
                ServicePointManager.ServerCertificateValidationCallback =
                   new RemoteCertificateValidationCallback(
                        delegate
                        { return true; }
                    );
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log("Error disabling certificates.");
            }
            string data;
            using (WebClient wc = new WebClient())
            {
                data = wc.DownloadString(modListUrl);
            }
            Mods = new List<Mod>();
            using (StringReader reader = new StringReader(data))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] Vals = line.Split('|');
                    Mods.Add(new Mod(Vals[0], Vals[1], Vals[2], Vals[3]));
                }
            }
            
        }

        public void OnGUI()
        {
            GUI.skin = TesseractModLoader.Window.UI.UISkin;
            if (onlineWindow)
            {
                onlineWindowRect = GUI.Window(3, onlineWindowRect, OnlineWindow, "Online Mod Browser", GUI.skin.GetStyle("window"));
            }
        }

        public void OnlineWindow(int windowID)
        {
            DataBaseModListScrollBar = GUILayout.BeginScrollView(DataBaseModListScrollBar, false, true);
            if (Mods != null)
            {

                GUILayout.Label("Online mod database.");
                foreach (Mod m in Mods)
                {
                    GUILayout.Label("---");
                    GUILayout.Label("Name: " + m.Name);
                    GUILayout.Label("Author: " + m.Author);
                    GUILayout.Label("Description: " + m.Description);

                    //TODO: Make the layout better
                    //TODO: Show mods on modlist before the restart occurs
                    if (GUILayout.Button("Download (Requires Restart)") && !m.Downloaded)
                    {
                        m.Downloaded = true;
                        Uri uri = new Uri(m.Link);
                        string originalfilename = Path.GetFileNameWithoutExtension(uri.LocalPath);
                        string filename = originalfilename;
                        string extension = Path.GetExtension(uri.LocalPath);
                        bool foundSame = false;
                        if (REPLACE)
                        {
                            if (File.Exists(Application.dataPath + "/Managed/Mods/" + filename + "." + extension))
                            {
                                UnityEngine.Debug.Log("Removing duplicate mod.");
                                File.Delete(Application.dataPath + "/Managed/Mods/" + filename + "." + extension);
                            }
                        }
                        else
                        {
                            int change = 1;
                            while (File.Exists(Application.dataPath + "/Managed/Mods/" + filename + "." + extension))
                            {
                                foundSame = true;
                                filename += originalfilename + $"({change})";
                                change++;
                            }
                        }
                        using (WebClient wc = new WebClient())
                        {
                            UnityEngine.Debug.Log("Downloading mod " + m.Name);
                            wc.DownloadFile(uri, Application.dataPath + "/Managed/Mods/" + filename + extension);
                            UnityEngine.Debug.Log("Downloaded mod " + m.Name);
                        }
                    }
                }
            }
            else
            {
                GUILayout.Label("Error loading mod database.");
            }
            GUILayout.EndScrollView();
            GUI.DragWindow();
        }

        public void Update()
        {
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.U) || Input.GetKey(KeyCode.RightControl) && Input.GetKeyDown(KeyCode.U))
            {
                onlineWindow = !onlineWindow;
            }
        }
    }
}

